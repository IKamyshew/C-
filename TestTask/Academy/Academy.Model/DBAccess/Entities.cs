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

        public List<Role> GetAllRoles()
        {
            return db.Roles.ToList();
        }
    }
}
