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
    /// Interaction logic for Marks.xaml
    /// </summary>
    public partial class Marks : Window
    {
        public Marks()
        {
            InitializeComponent();
            WindowMark.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
