using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Academy.Model.Models;
using Academy.Model.Access;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Methods model = new Methods();

            var users = model.GetAllUsers();

            foreach (var user in users)
            {
                Console.WriteLine(user.FirstName + " " + user.LastName);

            }

            Console.ReadKey();
        }
    }
}
