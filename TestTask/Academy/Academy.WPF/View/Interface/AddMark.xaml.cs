﻿using System;
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

namespace Academy.WPF.View.Interface
{
    /// <summary>
    /// Interaction logic for AddMark.xaml
    /// </summary>
    public partial class AddMark : Window
    {
        public AddMark()
        {
            InitializeComponent();
            WindowAddMark.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}