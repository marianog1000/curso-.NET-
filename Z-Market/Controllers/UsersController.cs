using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Z_Market.Models;
using Z_Market.ViewModels;

namespace Z_Market.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var usersView = new List<UserView>();
            foreach (var user in users)
            {
                var userView = new UserView
                {
                    EMail = user.Email,
                    Name = user.UserName,
                    UserID = user.Id
                };
                usersView.Add(userView);
            }
            return View(usersView);
        }

        public ActionResult Roles(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userID);

            if (user == null)
            {
                return HttpNotFound();
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var roles = roleManager.Roles.ToList();
            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                var role = roles.Find(r => r.Id == item.RoleId);

                var roleView = new RoleView
                {
                    RoleID = role.Id,
                    Name = role.Name
                };
                rolesView.Add(roleView);
            }

            var userView = new UserView
            {
                EMail = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView
            };

            return View(userView);
        }


        public ActionResult AddRole(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userID);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userView = new UserView
            {
                EMail = user.Email,
                Name = user.UserName,
                UserID = user.Id,
            };

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var lista = roleManager.Roles.ToList();
            lista.Add(new IdentityRole { Id = "", Name = "[Seleccione un Role ...]" });
            lista = lista.OrderBy(r => r.Name).ToList();
            ViewBag.RoleID = new SelectList(lista, "Id", "Name");

            return View(userView);
        }

        [HttpPost]
        public ActionResult AddRole(string userID, FormCollection form)
        {
            var roleID = Request["RoleID"];

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userID);
            var userView = new UserView
            {
                EMail = user.Email,
                Name = user.UserName,
                UserID = user.Id,
            };

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            if (string.IsNullOrEmpty(roleID))
            {
                ViewBag.Error = "Debes seleccionar un Role";
                var lista = roleManager.Roles.ToList();
                lista.Add(new IdentityRole { Id = "", Name = "[Seleccione un Role ...]" });
                lista = lista.OrderBy(r => r.Name).ToList();
                ViewBag.RoleID = new SelectList(lista, "Id", "Name");
                return View(userView);
            }

            var roles = roleManager.Roles.ToList();
            var role = roles.Find(r => r.Id == roleID);

            if (!userManager.IsInRole(userID, role.Name))
            {
                userManager.AddToRole(userID, role.Name);
            }

            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                var rol = roles.Find(r => r.Id == item.RoleId);

                var roleView = new RoleView
                {
                    RoleID = rol.Id,
                    Name = rol.Name
                };
                rolesView.Add(roleView);
            }

            userView = new UserView
            {
                EMail = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView
            };
            return View("Roles", userView);
        }

        public ActionResult Delete(string userID, string roleID)
        {
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(roleID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userID);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var roles = roleManager.Roles.ToList();
            var role = roles.Find(r => r.Id == roleID);

            if (userManager.IsInRole(userID, role.Name))
            {
                userManager.RemoveFromRole(userID, role.Name);
            }
            
            var rolesView = new List<RoleView>();
            foreach (var item in user.Roles)
            {
                var rol = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    RoleID = rol.Id,
                    Name = rol.Name
                };
                rolesView.Add(roleView);
            }

            var userView = new UserView
            {
                EMail = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView
            };
            return View("Roles", userView);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}