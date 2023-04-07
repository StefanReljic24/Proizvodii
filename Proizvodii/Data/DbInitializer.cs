using Proizvodii.Entity;

namespace Proizvodii.Data
{
    public class DbInitializer
    {
        public static void Initialize(EFDataContext context)
        {
            var created = context.Database.EnsureCreated();

            if (!created)
                return;

            var categories = new Category[]
            {
                    new Category {  Name = "Tv", Code = "01", Active = true },
                    new Category {  Name = "Bela tehnika", Code = "02", Active = true },
                    new Category {  Name = "Racunari", Code = "03", Active = true },
                    new Category {  Name = "Mobilni", Code = "04", Active = true }
            };

            context.Category.AddRange(categories);
            context.SaveChanges();

            var products = new Product[]
            {
                    new Product {
                        Name = "Sony televzor 50''",
                        Code = "01",
                        Active = true,
                        Category = context.Category.FirstOrDefault(p => p.CategoryId == 1),
                        Description = "LED TV",
                        Price = 850,
                        ImageName = "sony_50_nd.jpg"
                    },
                    new Product {
                        Name = "Samsung tv 40''",
                        Code = "02",
                        Active = true,
                        Category = context.Category.FirstOrDefault(p => p.CategoryId == 1),
                        Description = "LED TV",
                        Price = 540,
                        ImageName = "samsung_40_nd.jpg"
                    },
                    new Product {
                        Name = "Ves masina LG",
                        Code = "03",
                        Active = true,
                        Category = context.Category.FirstOrDefault(p => p.CategoryId == 2),
                        Description = "Ves masina akcija",
                        Price = 620,
                        ImageName = "vesmasina_lg_nd.jpg"
                    },
                    new Product {
                        Name = "Dell monitor 24''",
                        Code = "04",
                        Active = true,
                        Category = context.Category.FirstOrDefault(p => p.CategoryId == 3),
                        Description = "LED TV",
                        Price = 240,
                        ImageName = "dell_montor_24_nd.jpg"
                    }
            };
            context.Product.AddRange(products);
            context.SaveChanges();

            var adminRole = new Role { RoleId = 1, Name = "Admin", Active = true };
            var userRole = new Role { RoleId = 2, Name = "User", Active = true };
            var roles = new Role[]
            {
                    adminRole,
                    userRole
            };
            context.Role.AddRange(roles);
            context.SaveChanges();

            var users = new User[]
            {
                    new User{
                        FirstName = "Pera",
                        LastName = "Peric",
                        Username = "admin",
                        Password = "admin",
                        Email = "admin@nesto.com",
                        Active = true,
                        UserRole = new List<UserRole>{ new UserRole { Role = adminRole }  }
                    },
                     new User{
                        FirstName = "Nikola",
                        LastName = "Nikolic",
                        Username = "korisnik",
                        Password = "korisnik",
                        Email = "korisnik@nesto.com",
                        Active = true,
                        UserRole = new List<UserRole>{ new UserRole { Role = userRole }  }
                    }
            };
            context.User.AddRange(users);
            context.SaveChanges();
        }
    }
}
