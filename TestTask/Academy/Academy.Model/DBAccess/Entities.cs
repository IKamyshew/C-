using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Academy.Model.Entities;
using System.Data.Entity;

namespace Academy.Model.DBAccess
{
    public class Entities
    {
        AcademyContext db = new AcademyContext();

        //Users
        public List<User> GetAllUsers()
        {
            return db.Users.Include(r => r.Role).Include(g => g.Group).ToList();
        }

        public List<User> GetUsersFilteredByRole(int roleId)
        {
            return db.Users.Include(r => r.Role).Include(g => g.Group).Where(r => r.RoleId == roleId).ToList();
        }

        public List<User> GetUsersFilteredByGroup(int groupId)
        {
            return db.Users.Include(r => r.Role).Include(g => g.Group).Where(g => g.GroupId == groupId).ToList();
        }

        public List<User> GetUsersFilteredByRoleAndGroup(int roleId, int groupId)
        {
            return db.Users.Include(r => r.Role).Include(g => g.Group).Where(r => r.RoleId == roleId).Where(g => g.GroupId == groupId).ToList();
        }

        public User GetUserByID(int id)
        {
            return db.Users.Find(id);
        }

        public bool IsUserExists(int id)
        {
            using (AcademyContext db = new AcademyContext())
            {
                try
                {
                    db.Users.Where(u => u.Id == id).FirstOrDefault();
                    return true;
                } catch {
                    return false;
                }
            }
        }

        public bool IsUserExists(string userLogin)
        {
            using (AcademyContext db = new AcademyContext())
            {
                try
                {
                    User user = db.Users.Where(u => u.Login == userLogin)
                                        .FirstOrDefault();

                    if (user != null)
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
                return false;
            }
        }

        public bool AddUser(User user)
        {
            if (!IsUserExists(user.Login))
            {
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            } else {
                return false;
            }
        }

        public bool UpdateUser(User user)
        {
            if (IsUserExists(user.Id))
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteUser(User user)
        {
            if (IsUserExists(user.Id)) { 
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        //Roles
        public List<Role> GetAllRoles()
        {
            return db.Roles.ToList();
        }

        //Groups
        public List<Group> GetAllGroups()
        {
            return db.Groups.ToList();
        }
    }


}
