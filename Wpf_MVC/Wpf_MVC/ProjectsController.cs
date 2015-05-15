using System;
using ProjectBilling;
using ProjectBilling.DataAccess;
using System.Windows;
using ProjectBilling.UI.MVC;

namespace Wpf_MVC
{
    public interface IProjectsController
    {
        void ShowProjectsView(Window owner);
        void Update(Project project);
    }

    public class ProjectsController : IProjectsController
    {
        private readonly IProjectsModel _model;

        public ProjectsController(IProjectsModel projectModel)
        {
            if (projectModel == null)
                throw new ArgumentNullException(
                    "projectModel");
            _model = projectModel;
        }

        public void ShowProjectsView(Window owner)
        {
            ProjectsView view = new ProjectsView(this, _model);
            view.Owner = owner;
            view.Show();
        }

        public void Update(Project project)
        {
            _model.UpdateProject(project);
        }
    }
}