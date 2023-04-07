using Proizvodii.Entity;
using Proizvodii.Models;

namespace Proizvodii.Extensions
{
    public static class UserModelExstension
    {
        public static User ToModel(this UserModel model)
        {
            return new User
            {
                Active = model.Active,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password,
                UserId = model.UserId,
                Username = model.Username,
                UserRole = model.Roles.Select(p => new UserRole
                {
                    RoleId = p.RoleId,
                    UserId = model.UserId
                }).ToList()
            };
        }
    }
}
