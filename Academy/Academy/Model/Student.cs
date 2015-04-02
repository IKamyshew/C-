using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy
{
    class Student
    {
        public static int StudentsCounter { get; private set; }
        private int StudentId;
        private string FirstName;
        private string LastName;
        private int GroupId;

        public Student(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            if (StudentsCounter != null)
                StudentsCounter++;
            else
                StudentsCounter = 1;

            StudentId = StudentsCounter;
        }

        private void ShowProfile()
        {
            Console.WriteLine("First Name: " + FirstName + "; Last Name: " + LastName + ";");
            Console.WriteLine("Group ID: " + GroupId + ";" + "Teacher: " );
            Console.WriteLine("Subjects: ");
        }

        private void ShowMarks()
        {

        }
    }
}
