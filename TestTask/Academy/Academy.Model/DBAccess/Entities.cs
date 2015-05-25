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

        public List<User> GetAllManagers()
        {
            return db.Users.Include(r => r.Role).Include(g => g.Group).Where(r => r.RoleId == 1).ToList();
        }

        public List<User> GetAllTeachers()
        {
            return db.Users.Include(r => r.Role).Include(g => g.Group).Where(r => r.RoleId == 2).ToList();
        }

        public List<User> GetAllStudents()
        {
            return db.Users.Include(r => r.Role).Include(g => g.Group).Where(r => r.RoleId == 3).ToList();
        }

        public List<User> GetAllStudentsForGroup(int groupID)
        {
            return db.Users.Include(r => r.Role).Include(g => g.Group).Where(r => r.RoleId == 3).Where(g => g.GroupId == groupID).ToList();
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

        public List<Group> GetAllStudentGroups()
        {
            return db.Groups.Where(gr => gr.Name != "Managers" && gr.Name != "Teachers").ToList();
        }

        public bool AddGroup(Group newGroup)
        {
            if (!IsGroupExist(newGroup.Name))
            {
                db.Groups.Add(newGroup);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteGroup(int groupID)
        {
            Group group = GetGroupByID(groupID);
            if (group != null)
            {
                db.Groups.Remove(group);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool IsGroupExist(int id)
        {
            Group gr = db.Groups.Find(id);
            if (gr != null)
                return true;

            return false;
        }

        public bool IsGroupExist(string name)
        {
            Group gr = db.Groups.Where(g => g.Name == name).FirstOrDefault();
            if (gr != null)
                return true;

            return false;
        }

        public Group GetGroupByID(int id)
        {
            Group gr = db.Groups.Find(id);
            if (gr != null)
                return gr;

            return null;
        }

        //TeachersGroups
        public List<TeachersGroups> GetTeachersForGroup(int groupID)
        {
            return db.TeachersGroups.Include(t => t.Teacher).Include(g => g.Group).Where(gr => gr.GroupId == groupID).ToList();
        }

        public bool IsTeacherRelatesToGroup(int groupID, int teacherID)
        {
            return (db.TeachersGroups.Include(t => t.Teacher).Include(g => g.Group).Where(gr => gr.GroupId == groupID).Where(t => t.TeacherId == teacherID).FirstOrDefault() == null) ? false : true;
        }

        public bool AddTeacherToGroup(int groupID, int teacherID)
        {
            if (!IsTeacherRelatesToGroup(groupID, teacherID)) { 
                TeachersGroups tg = new TeachersGroups();
                tg.TeacherId = teacherID;
                tg.GroupId = groupID;
                db.Entry(tg).State = EntityState.Added;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoveTeacherToGroup(int groupID, int teacherID)
        {
            if (IsTeacherRelatesToGroup(groupID, teacherID)) { 
                TeachersGroups removeTeacher = db.TeachersGroups.Where(t => t.TeacherId == teacherID).Where(gr => gr.GroupId == groupID).FirstOrDefault();
                db.Entry(removeTeacher).State = EntityState.Deleted;
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }


}
