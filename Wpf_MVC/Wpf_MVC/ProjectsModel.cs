using System;
using System.Collections.Generic;
using System.Linq;
using ProjectBilling.DataAccess;

namespace Wpf_MVC
{
    public interface IProjectsModel
    {
        IEnumerable<Project> Projects { get; set; }
        event EventHandler<ProjectEventArgs> ProjectUpdated;
        void UpdateProject(Project project);
    }

    public class ProjectsModel : IProjectsModel
    {
        public IEnumerable<Project> Projects { get; set; }

        public event EventHandler<ProjectEventArgs>
            ProjectUpdated = delegate { };

        public ProjectsModel()
        {
            Projects = new DataServiceStub().GetProjects();
        }

        private void RaiseProjectUpdated(Project project)
        {
            ProjectUpdated(this, new ProjectEventArgs(project));
        }

        public void UpdateProject(Project project)
        {
            Project selectedProject = Projects.Where(p => p.ID == project.ID)
                                              .FirstOrDefault() as Project;
            selectedProject.Name = project.Name;
            selectedProject.Estimate = project.Estimate;
            selectedProject.Actual = project.Actual;
            RaiseProjectUpdated(selectedProject);
        }
    }

    public class ProjectEventArgs : EventArgs
    {
        public Project Project { get; set; }

        public ProjectEventArgs(Project project)
        {
            Project = project;
        }
    }
}
