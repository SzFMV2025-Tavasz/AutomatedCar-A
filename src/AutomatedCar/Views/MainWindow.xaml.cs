namespace AutomatedCar.Views
{
    using AutomatedCar.Models;
    using AutomatedCar.ViewModels;
    using Avalonia.Controls;
    using Avalonia.Input;
    using Avalonia.Markup.Xaml;

    public class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Keyboard.Keys.Add(e.Key);
            base.OnKeyDown(e);

            MainWindowViewModel viewModel = (MainWindowViewModel)this.DataContext;

            if (Keyboard.IsKeyDown(Key.Left))
            {
                viewModel.CourseDisplay.SetSteeringLeft(true);
            }

            if (Keyboard.IsKeyDown(Key.Right))
            {
                viewModel.CourseDisplay.SetSteeringRight(true);
            }

            if (Keyboard.IsKeyDown(Key.Up))
            {
                viewModel.CourseDisplay.ThrottleOnSet(true);
            }

            if (Keyboard.IsKeyDown(Key.Down))
            {
                viewModel.CourseDisplay.BrakeOnSet(true);
            }
            
            if (Keyboard.IsKeyDown(Key.D1))
            {
                viewModel.CourseDisplay.ToggleDebug();
            }

            if (Keyboard.IsKeyDown(Key.D2))
            {
                viewModel.CourseDisplay.ToggleCamera();
            }

            if (Keyboard.IsKeyDown(Key.D3))
            {
                viewModel.CourseDisplay.ToggleRadar();
            }

            if (Keyboard.IsKeyDown(Key.D4))
            {
                viewModel.CourseDisplay.ToggleUltrasonic();
            }

            if (Keyboard.IsKeyDown(Key.D5))
            {
                viewModel.CourseDisplay.ToggleRotation();
            }

            if (Keyboard.IsKeyDown(Key.F1))
            {
                new HelpWindow().Show();
                Keyboard.Keys.Remove(Key.F1);
            }

            if (Keyboard.IsKeyDown(Key.F5))
            {
                viewModel.NextControlledCar();
                Keyboard.Keys.Remove(Key.F5);
            }

            if (Keyboard.IsKeyDown(Key.F6))
            {
                viewModel.PrevControlledCar();
                Keyboard.Keys.Remove(Key.F5);
            }

            var scrollViewer = this.Get<CourseDisplayView>("courseDisplay").Get<ScrollViewer>("scrollViewer");
            viewModel.CourseDisplay.FocusCar(scrollViewer);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            Keyboard.Keys.Remove(e.Key);
            base.OnKeyUp(e);

            MainWindowViewModel viewModel = (MainWindowViewModel)this.DataContext;

            if (!Keyboard.IsKeyDown(Key.Left))
            {
                viewModel.CourseDisplay.SetSteeringLeft(false);
            }

            if (!Keyboard.IsKeyDown(Key.Right))
            {
                viewModel.CourseDisplay.SetSteeringRight(false);
            }


            if (!Keyboard.IsKeyDown(Key.Up))
            {
                viewModel.CourseDisplay.ThrottleOnSet(false);
            }

            if (!Keyboard.IsKeyDown(Key.Down))
            {
                viewModel.CourseDisplay.BrakeOnSet(false);
            }

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}