using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProjectBilling.DataAccess;
using Wpf_MVC;

namespace ProjectBilling.UI.MVC
{
    public partial class ProjectsView : Window
    {
        private readonly IProjectsModel _model;
        private readonly IProjectsController _controller
            = null;
        private const int NONE_SELECTED = -1;

        public ProjectsView(IProjectsController projectsController, IProjectsModel projectsModel)
        {
            InitializeComponent();
            _controller = projectsController;
            _model = projectsModel;

            _model.ProjectUpdated += model_ProjectUpdated;
            ProjectsComboBox.ItemsSource = _model.Projects;
            ProjectsComboBox.DisplayMemberPath = "Name";
            ProjectsComboBox.SelectedValuePath = "ID";
        }


        #region Event Handlers

        void model_ProjectUpdated(object sender, ProjectEventArgs e)
        {
            int selectedProjectId = GetSelectedProjectId();

            if (selectedProjectId > NONE_SELECTED)
            {
                ProjectsComboBox.SelectedValue
                    = selectedProjectId;
                if (selectedProjectId == e.Project.ID)
                {
                    UpdateDetails(e.Project);
                }
            }
        }

        private void ProjectsComboBox_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            Project project = GetSelectedProject();
            if (project != null)
            {
                EstimatedTextBox.Text = project.Estimate.ToString();
                EstimatedTextBox.IsEnabled = true;
                ActualTextBox.Text
                    = project.Actual.ToString();
                ActualTextBox.IsEnabled = true;
                updateButton.IsEnabled = true;
                UpdateEstimatedColor();
            }
        }

        private void UpdateButton_Click(object sender,
            RoutedEventArgs e)
        {
            Project project = new Project()
            {
                ID = (int)ProjectsComboBox.SelectedValue,
                Name = ProjectsComboBox.Text,
                Estimate = GetDouble(
                    EstimatedTextBox.Text),
                Actual = GetDouble(ActualTextBox.Text)
            };
            _controller.Update(project);
        }

        #endregion Event Handlers

        #region Helpers

        private void UpdateEstimatedColor()
        {
            double actual
                = GetDouble(ActualTextBox.Text);
            double estimated
                = GetDouble(EstimatedTextBox.Text);
            if (actual == 0)
            {
                EstimatedTextBox.Foreground
                    = ActualTextBox.Foreground;
            }
            else if (actual > estimated)
            {
                EstimatedTextBox.Foreground
                    = Brushes.Red;
            }
            else
            {
                EstimatedTextBox.Foreground
                    = Brushes.Green;
            }
        }

        private void UpdateDetails(Project project)
        {
            EstimatedTextBox.Text
                = project.Estimate.ToString();
            ActualTextBox.Text
                = project.Actual.ToString();
            UpdateEstimatedColor();
        }

        private double GetDouble(string text)
        {
            return string.IsNullOrEmpty(text) ?
                0 : double.Parse(text);
        }

        private Project GetSelectedProject()
        {
            return ProjectsComboBox.SelectedItem
                as Project;
        }

        private int GetSelectedProjectId()
        {
            Project project = GetSelectedProject();
            return (project == null)
                ? NONE_SELECTED : project.ID;
        }

        #endregion Helpers
    }
}

