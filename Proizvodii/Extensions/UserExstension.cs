using Proizvodii.Entity;
using Proizvodii.Models;

namespace Proizvodii.Extensions
{
    public static class UserExstension
    {
        public static UserModel ToModel(this User model)
        {
            return new UserModel
            {
                Active = model.Active,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password,
                UserId = model.UserId,
                Username = model.Username,
                Roles = model.UserRole.Select(p => new RoleModel
                {
                    RoleId = p.RoleId,
                    RoleName = p.Role.Name
                }).ToList()
            };
        }
        public static IEnumerable<UserModel> ToModel(this IEnumerable<User> model)
        {
            return model.Select(p => p.ToModel());
        }
    }
}
