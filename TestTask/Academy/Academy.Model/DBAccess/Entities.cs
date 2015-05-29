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
        public User GetUserByLastName(string lastName)
        {
            return db.Users.Where(ln => ln.LastName == lastName).Include(r => r.Role).Include(g => g.Group).FirstOrDefault();
        }

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

        public User GetUserByLogin(string userLogin)
        {
            return db.Users.Where(ln => ln.Login == userLogin).Include(r => r.Role).Include(g => g.Group).FirstOrDefault();
        }
        

        public bool IsUserExists(int id)
        {
            using (AcademyContext db = new AcademyContext())
            {
                try
                {
                    User user = db.Users.Where(u => u.Id == id).FirstOrDefault();

                    if (user != null)
                    {
                        return true;
                    }
                } catch {
                    return false;
                }
                return false;
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
                try
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Entry(user).State = EntityState.Detached;
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                {
                    return false;
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    db.Entry(user).State = EntityState.Detached;
                    db.SaveChanges();
                    return false;
                }
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

        public Role GetRole(string roleName)
        {
            return db.Roles.Where(r => r.Name == roleName).FirstOrDefault();
        }

        //Groups
        public List<Group> GetAllGroups()
        {
            return db.Groups.ToList();
        }

        public Group GetGroupByName(string groupName)
        {
            return db.Groups.Where(g => g.Name == groupName).FirstOrDefault();
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

        //Subjects
        public List<Subject> GetAllSubjects()
        {
            return db.Subjects.ToList();
        }

        public Subject GetSubjectByID(int subjectID)
        {
            Subject subj = db.Subjects.Find(subjectID);
            if (subj != null)
                return subj;

            return null;
        }

        public bool IsSubjectExist(string subjectName)
        {
            if (!(db.Subjects.Where(s => s.Name == subjectName).FirstOrDefault() == null))
                return true;
            else
                return false;
        }

        public bool AddSubject(Subject newSubject)
        {
            if (!IsSubjectExist(newSubject.Name))
            { 
                db.Subjects.Add(newSubject);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteSubject(int subjectID)
        {
            Subject subj = GetSubjectByID(subjectID);
            if (subj != null)
            {
                db.Subjects.Remove(subj);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        //Marks
        public bool AddMark(Mark mark)
        {
            if (!IsMarkExist(mark))
            { 
                db.Marks.Add(mark);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool IsMarkExist(Mark mark)
        {
            if (db.Marks.Where(s => s.SubjectId == mark.SubjectId).Where(s => s.StudentId == mark.StudentId).Where(d => d.Date == mark.Date).FirstOrDefault() != null)
                return true;
            return false;
        }

        public List<Mark> GetMarksOrderedByDate() {
            return db.Marks.OrderBy(d => d.Date)
                           .ToList();
        }

        public List<Mark> GetMarksOrderedByDateForStudent(int studentID)
        {
            return db.Marks.Where(u => u.StudentId == studentID)
                           .OrderBy(d => d.Date)
                           .ToList();
        }

        public List<Mark> GetMarksWithDistinctDate() {
            return db.Marks.GroupBy(d => d.Date)
                           .Select(date => date.FirstOrDefault())
                           .ToList();
        }

        public List<Mark> GetMarksWithDistinctDateForStudent(int studentID)
        {
            return db.Marks.Where(s => s.StudentId == studentID)
                           .GroupBy(d => d.Date)
                           .Select(date => date.FirstOrDefault())
                           .ToList();
        }

        public List<Mark> GetMarksForSubject(int subjectID) {
            return db.Marks.Where(s => s.SubjectId == subjectID).ToList();
        }

        public List<Mark> GetMarksWithDistinctDateForSubject(int subjectID)
        {
            return db.Marks.Where(s => s.SubjectId == subjectID)
                           .GroupBy(d => d.Date)
                           .Select(date => date.FirstOrDefault())
                           .ToList();
        }

        public List<Mark> GetMarksBetweenDates(DateTime startDate, DateTime endDate) {
            return db.Marks.Where(d => d.Date >= startDate && d.Date <= endDate)
                           .OrderBy(d => d.Date)
                           .ToList();
        }

        public List<Mark> GetMarksBetweenDatesForStudent(int studentID, DateTime startDate, DateTime endDate)
        {
            return db.Marks.Where(s => s.StudentId == studentID)
                           .Where(d => d.Date >= startDate && d.Date <= endDate)
                           .OrderBy(d => d.Date)
                           .ToList();
        }

        public List<Mark> GetMarksWithDistinctDatesForStudentInPeriod(int studentID, DateTime startDate, DateTime endDate)
        {
            return db.Marks.Where(s => s.StudentId == studentID)
                           .Where(d => d.Date >= startDate && d.Date <= endDate)
                           .GroupBy(d => d.Date)
                           .Select(date => date.FirstOrDefault())
                           .ToList();
        }


        //Schedule
        public bool AddSchedule(Schedule schedule)
        {
            if (!IsScheduleExist(schedule.GroupId, schedule.Date, schedule.Lesson)) { 
                db.Schedule.Add(schedule);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateSchedule(Schedule schedule)
        {
            if (IsScheduleExist(schedule.GroupId, schedule.Date, schedule.Lesson))
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool IsScheduleExist(int groupID, DateTime date, int lesson)
        {
            bool exist = db.Schedule.Where(g => g.GroupId == groupID).Where(d => d.Date == date).Where(l => l.Lesson == lesson).FirstOrDefault() != null;
            if (exist)
                return true;
            return false;
        }

        public List<Schedule> GetSchedulesForGroup(int groupID) {
            return db.Schedule.Include(s => s.Subject).Where(s => s.GroupId == groupID).ToList();
        }

        public List<Schedule> GetSchedulesDistinctByDate()
        {
            return db.Schedule.Include(s => s.Subject)
                                .GroupBy(d => d.Date)
                                .Select(day => day.FirstOrDefault())
                                .ToList();
        }

        public List<Schedule> GetSchedulesForGroupDistinctByDate(int groupID) {
            return db.Schedule.Include(s => s.Subject)
                                .Where(g => g.GroupId == groupID)
                                .GroupBy(d => d.Date)
                                .Select(day => day.FirstOrDefault())
                                .ToList();
        }

        public List<Schedule> GetSchedulesForGroupAndDate(int groupID, DateTime date)
        {
            return db.Schedule.Include(u => u.Subject).Where(sch => sch.Date == date).Where(gr => gr.GroupId == groupID).OrderBy(l => l.Lesson).ToList();
        }

        public bool IsSchedulesContainsDateForGroup(DateTime date, int groupID)
        {
            if (db.Schedule.Where(d => d.Date == date).Where(g => g.GroupId == groupID).FirstOrDefault() != null)
                return true;
            else
                return false;
        }

        public bool IsSchedulesContainsDate(DateTime date)
        {
            if (db.Schedule.Where(d => d.Date == date).FirstOrDefault() != null)
                return true;
            else
                return false;
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
