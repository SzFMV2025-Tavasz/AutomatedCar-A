namespace AutomatedCar.Views
{
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

    public class DashboardView : UserControl
    {
        public DashboardView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}