using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Academy.Model.Entities;
using Academy.Model.ScreenModels;
using System.Data.Entity;

namespace Academy.Model.DBAccess
{
    public class CurrentUser
    {
        public User GetCurrentUserByLogin(string userLogin)
        {
            using (AcademyContext db = new AcademyContext())
            {
                try
                {
                    User user = db.Users.Include(r => r.Role)
                                        .Include(g => g.Group)
                                        .Where(u => u.Login == userLogin)
                                        .FirstOrDefault();
                    
                    if (user != null)
                    {
                        return user;
                    }
                }
                catch
                {
                    return null;
                }
                return null;
            }
        }

        public void LogOff()
        {
            FormsAuthentication.SignOut();
        }

        public bool ValidateUser(string login, string password)
        {
            bool isValid = false;

            using (AcademyContext _db = new AcademyContext())
            {
                try
                {
                    User user = (from u in _db.Users
                                 where u.Login == login && u.Password == password
                                 select u).FirstOrDefault();

                    if (user != null)
                    {
                        isValid = true;
                    }
                }
                catch
                {
                    isValid = false;
                }
            }
            return isValid;
        }
    }
}
