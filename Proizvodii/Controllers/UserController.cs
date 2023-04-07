using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proizvodii.Data;
using Proizvodii.Extensions;
using Proizvodii.Models;

namespace Proizvodii.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly EFDataContext _context;

        public UserController(EFDataContext context)
        {
            _context = context;
        }

        public IActionResult Index([FromQuery] string filter)
        {
            ViewData["filter"] = filter;

            if (string.IsNullOrEmpty(filter))
            {
                return View(_context.User.ToModel());
            }
            else
            {
                var users = _context.User
                    .Where(p => p.FirstName.ToLower().Contains(filter.ToLower())
                    || p.LastName.ToLower().Contains(filter.ToLower()));
                return View(users.ToModel());
            }
        }

        public IActionResult Create()
        {
            var user = new UserModel
            {
                Roles = _context.Role
                    .Select(p => new RoleModel
                    {
                        RoleId = p.RoleId,
                        RoleName = p.Name
                    })
                    .OrderBy(p => p.RoleName)
                    .ToList()
            };
            return View("Edit", user);
        }

        public IActionResult Edit(int id)
        {
            var user = _context.User
                .Include(p => p.UserRole)
                .ThenInclude(p => p.Role)
                .SingleOrDefault(p => p.UserId == id);


            if (user == null)
                return RedirectToAction(nameof(Index));

            var userModel = user.ToModel();

            userModel.Roles.ForEach(p => { p.Selected = true; });


            var roles = _context.Role
                .Where(p => !user.UserRole.Select(r => r.RoleId).Contains(p.RoleId));

            userModel.Roles
                .AddRange(roles.Select(p => new RoleModel
                {
                    RoleId = p.RoleId,
                    RoleName = p.Name
                }));


            userModel.Roles = userModel.Roles
                .OrderBy(p => p.RoleName)
                .ToList();

            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserModel userModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(userModel);

                if (userModel.UserId == 0)
                {

                    userModel.Roles.RemoveAll(p => !p.Selected);
                    var user = userModel.ToModel();


                    _context.User.Add(user);
                }
                else
                {

                    var user = userModel.ToModel();
                    _context.Entry(user).State = EntityState.Modified;


                    if (_context.User.FirstOrDefault(p => p.UserId == user.UserId) == null)
                        return RedirectToAction(nameof(Index));


                    if (string.IsNullOrEmpty(user.Password))
                    {
                        var usr = _context.Entry(user);
                        usr.Property(x => x.Password).IsModified = false;
                    }


                    var currentRoles = _context.UserRole
                         .Where(p => p.UserId == user.UserId)
                         .Select(p => p.RoleId);


                    user.UserRole
                        .Where(p => !currentRoles.Contains(p.RoleId))
                        .ToList()
                        .ForEach(p => _context.Entry(p).State = EntityState.Added);


                    user.UserRole
                        .Where(p => userModel.Roles.Any(r => r.RoleId == p.RoleId && !r.Selected)
                            && currentRoles.Contains(p.RoleId))
                        .ToList()
                        .ForEach(p => _context.Entry(p).State = EntityState.Deleted);
                }

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(userModel);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var c = _context.User.SingleOrDefault(p => p.UserId == id);
                if (c != null)
                {
                    _context.User.Remove(c);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
