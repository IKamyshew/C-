using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy
{
    class Manager
    {
        private string FirstName;
        private string LastName;

        public Manager(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public void AddStudent(string firstName, string lastName)
        {
            new Student(firstName, lastName);
            Console.WriteLine("Student added. Id = " + Student.StudentsCounter + ";");
        }

        public void AddTeacher(string firstName, string lastName)
        {
            new Teacher(firstName, lastName);
            Console.WriteLine("Teacher added;");
        }

        public void AddSubject(string name)
        {
            new Subject(name);
            Console.WriteLine("Subject added;");
        }

    }
}
