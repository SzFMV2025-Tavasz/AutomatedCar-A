namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;
    using ReactiveUI;

    public class MainWindowViewModel : ViewModelBase
    {
        private DashboardViewModel dashboard;
        private CourseDisplayViewModel courseDisplay;

        public MainWindowViewModel(World world)
        {
            this.CourseDisplay = new CourseDisplayViewModel(world);
            this.Dashboard = new DashboardViewModel(world.ControlledCar);
        }

        public CourseDisplayViewModel CourseDisplay
        {
            get => this.courseDisplay;
            private set => this.RaiseAndSetIfChanged(ref this.courseDisplay, value);
        }

        public DashboardViewModel Dashboard
        {
            get => this.dashboard;
            private set => this.RaiseAndSetIfChanged(ref this.dashboard, value);
        }

        public void NextControlledCar()
        {
            World.Instance.NextControlledCar();
            this.Dashboard = new DashboardViewModel(World.Instance.ControlledCar);
        }

        public void PrevControlledCar()
        {
            World.Instance.PrevControlledCar();
            this.Dashboard = new DashboardViewModel(World.Instance.ControlledCar);
        }
    }
}