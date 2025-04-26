namespace AutomatedCar.Views
{
    using AutomatedCar.ViewModels;
    using Avalonia.Controls;
    using Avalonia.Input;
    using Avalonia.Markup.Xaml;

    public class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.Loaded += (sender, e) =>
            {
                var viewModel = (MainWindowViewModel)this.DataContext;
                var scrollViewer = this.Get<CourseDisplayView>("courseDisplay").Get<ScrollViewer>("scrollViewer");
                viewModel.CourseDisplay.ScrollViewer = scrollViewer;
            };
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Keyboard.Keys.Add(e.Key);
            base.OnKeyDown(e);

            MainWindowViewModel viewModel = (MainWindowViewModel)this.DataContext;

            // váltó:
            if (Keyboard.IsKeyDown(Key.Q))
            {
                viewModel.CourseDisplay.ShiftGearRequestSet(true, false);
                Keyboard.Keys.Remove(Key.Q);
            }
            
            if (Keyboard.IsKeyDown(Key.A))
            {
                viewModel.CourseDisplay.ShiftGearRequestSet(false, true);
                Keyboard.Keys.Remove(Key.A);
            }

            // gáz és fék:
            if (Keyboard.IsKeyDown(Key.Up))
            {
                viewModel.CourseDisplay.ThrottleOnSet(true);
            }

            if (Keyboard.IsKeyDown(Key.Down))
            {
                viewModel.CourseDisplay.BrakeOnSet(true);
            }

            //forgatás:
            if (Keyboard.IsKeyDown(Key.Left))
            {
                viewModel.CourseDisplay.KeyLeft();
                Keyboard.Keys.Remove(Key.Left);
            }

            if (Keyboard.IsKeyDown(Key.Right))
            {
                viewModel.CourseDisplay.KeyRight();
                Keyboard.Keys.Remove(Key.Right);
            }

            //egyéb:
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
                Keyboard.Keys.Remove(Key.F6);
            }

        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            Keyboard.Keys.Remove(e.Key);
            base.OnKeyUp(e);

            MainWindowViewModel viewModel = (MainWindowViewModel)this.DataContext;

            //gáz és fék:
            if (e.Key == Key.Up)
            {
                viewModel.CourseDisplay.ThrottleOnSet(false);
            }

            if (e.Key == Key.Down)
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