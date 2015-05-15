using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ProjectBilling.DataAccess;
using System.Windows.Media;

namespace ProjectBilling.UI.Monolithic
{
    sealed class ProjectsView : Window
    {
        [STAThread]
        static void Main(string[] args)
        {
            ProjectsView mainWindow = new ProjectsView();
            new Application().Run(mainWindow);
        }

        private static readonly Thickness _margin = new Thickness(5);
        private readonly ComboBox _projectsComboBox = new ComboBox() { Margin = _margin };
        private readonly TextBox _estimateTextBox = new TextBox() { IsEnabled = false, Margin = _margin };
        private readonly TextBox _actualTextBox = new TextBox() { IsEnabled = false, Margin = _margin };

        private readonly Button _updateButton = new Button()
        {
            IsEnabled = false,
            Content = "Update",
            Margin = _margin
        };

        public ProjectsView()
        {
            Title = "Project";
            Width = 250;
            MinWidth = 250;
            Height = 180;
            MinHeight = 180;

            LoadProjects();
            AddControlsToWindow();

            _updateButton.Click += updateButton_Click;
        }

        private void projectsListBox_SelectionChanged(
       object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            // Проверяем был ли выбран элемент
            if (comboBox != null && comboBox.SelectedIndex > -1)
            {
                UpdateDetails();
            }
            else
            {
                DisableDetails();
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            Project selectedProject = _projectsComboBox.SelectedItem as Project;
            if (selectedProject != null)
            {
                selectedProject.Estimate = double.Parse(_estimateTextBox.Text);
                if (!string.IsNullOrEmpty(_actualTextBox.Text))
                {
                    selectedProject.Actual = double.Parse(_actualTextBox.Text);
                }
                SetEstimateColor(selectedProject);
            }
        }

        // Добавляем коллекцию проектов в ComboBox, используя класс доступа к данным
        // Привязываем видимую часть списка к свойству Name
        // Добавляем обработчик событий SelectionChangedEventHandler
        private void LoadProjects()
        {
            foreach (Project project in new DataServiceStub().GetProjects())
            {
                _projectsComboBox.Items.Add(project);
            }

            _projectsComboBox.DisplayMemberPath = "Name";
            _projectsComboBox.SelectionChanged += new SelectionChangedEventHandler(
                projectsListBox_SelectionChanged);
        }

        // Добавляем в окно элемент UniformGrid
        // и размещаем в нем дочерние элементы
        private void AddControlsToWindow()
        {
            UniformGrid grid = new UniformGrid() { Columns = 2 };

            grid.Children.Add(new Label() { Content = "Проект:" });
            grid.Children.Add(_projectsComboBox);

            Label label = new Label() { Content = "Сметная стоимость:" };
            grid.Children.Add(label);
            grid.Children.Add(_estimateTextBox);

            label = new Label() { Content = "Фактическая стоимость:" };
            grid.Children.Add(label);
            grid.Children.Add(_actualTextBox);
            grid.Children.Add(_updateButton);

            Content = grid;
        }

        // Создает таблицу размером 2x3
        private Grid GetGrid()
        {
            Grid grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            return grid;
        }

        // При выборе элемента в ComboBox вызывается этот метод
        // Сначала извлекаем выбранный элемент, преобразуя его к классу Project,
        // затем активируем текстовые поля и указываем в них значения
        // Вызываем SetEstimateColor() для сравнения сметной и фактической стоимостей
        private void UpdateDetails()
        {
            Project selectedProject = _projectsComboBox.SelectedItem as Project;

            _estimateTextBox.IsEnabled = true;
            _estimateTextBox.Text = selectedProject.Estimate.ToString();
            _actualTextBox.IsEnabled = true;
            _actualTextBox.Text = (selectedProject.Actual == 0) ? "" : selectedProject.Actual.ToString();
            SetEstimateColor(selectedProject);
            _updateButton.IsEnabled = true;
        }

        // Если в ComboBox ничего не выбрано отключаем доступ к элементам управления
        private void DisableDetails()
        {
            _estimateTextBox.IsEnabled = false;
            _actualTextBox.IsEnabled = false;
            _updateButton.IsEnabled = false;
        }

        // Подсвечиваем текстовые поля в зависимости от сметной и фактической стоимостей
        private void SetEstimateColor(Project selectedProject)
        {
            if (selectedProject.Actual == 0)
            {
                this._estimateTextBox.Foreground = _actualTextBox.Foreground;
            }
            else if (selectedProject.Actual <= selectedProject.Estimate)
            {
                this._estimateTextBox.Foreground = Brushes.Green;
            }
            else
            {
                this._estimateTextBox.Foreground = Brushes.Red;
            }
        }
    }
}
