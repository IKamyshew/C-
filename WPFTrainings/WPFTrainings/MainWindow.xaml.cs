using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Markup;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Помещаем базовую разметку
            if (File.Exists(System.Environment.CurrentDirectory + "\\YourXaml.xaml"))
            {
                txtCodeXAML.Text = File.ReadAllText("YourXaml.xaml");
            }
            else
            {
                string s = "<Window xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\n" +
                    "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n" +
                    "Title=\"XAML Editor\" Height=\"350\" Width=\"525\" WindowStartupLocation=\"CenterScreen\">\n" +
                    "<StackPanel>\n</StackPanel>\n</Window>";
                txtCodeXAML.Text = s;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Записать данные из текстового блока в локальный файл
            File.WriteAllText("YourXaml.xaml", txtCodeXAML.Text);
        }

        private void btnViewXAML_Click(object sender, RoutedEventArgs e)
        {
            // Запись данных из текстового блока в файл YourXaml.xaml
            File.WriteAllText("YourXaml.xaml", txtCodeXAML.Text);
            Window myWindow = null;
            try
            {
                using (Stream sr = File.Open("YourXaml.xaml", FileMode.Open))
                {
                    myWindow = (Window)XamlReader.Load(sr);
                    myWindow.ShowDialog();
                    myWindow.Close();
                    myWindow = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
