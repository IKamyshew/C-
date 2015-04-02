using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy
{
    class Teacher
    {
        private string FirstName;
        private string LastName;

        public Teacher(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        private void FormGroup(int id)
        {
            new Group(id);
        }

        
    }
}
