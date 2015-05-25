using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Academy.Model.DBAccess;
using Academy.Model.Entities;

namespace Academy.WPF
{
    public partial class Profile : Window
    {
        public Profile(string Login)
        {
            CurrentUser CurUser = new CurrentUser();
            User user = CurUser.GetCurrentUserByLogin(Login);

            InitializeComponent();

            BlockFirstName.Text = "First Name: " + user.FirstName;
            BlockLastName.Text = "Last Name: " + user.LastName;
            BlockRole.Text = "Role: " + user.Role.Name;
            BlockGroup.Text = "Group: " + user.Group.Name;

        }
    }
}
