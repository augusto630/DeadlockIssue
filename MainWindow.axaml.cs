using System;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;

namespace DeadlockIssue
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OpenWindow();
        }

        void OpenWindow()
        {
            new MyWindow(OpenWindow).Show();
        }

        class MyWindow : Window
        {
            readonly Action mThreadLoop;
            public MyWindow(Action threadLoop)
            {
                mThreadLoop = threadLoop;

                // funky colors, not related to the issue
                double r = new Random().NextDouble();
                Background =
                    r > 0.9 ? Brushes.Firebrick :
                    r > 0.8 ? Brushes.Aqua :
                    r > 0.7 ? Brushes.Azure :
                    r > 0.6 ? Brushes.Beige :
                    r > 0.5 ? Brushes.Brown :
                    r > 0.4 ? Brushes.Gold :
                    r > 0.3 ? Brushes.Chocolate :
                    r > 0.2 ? Brushes.Khaki :
                    r > 0.1 ? Brushes.SkyBlue :
                    Brushes.Red;

                Task.Run(
                    () =>
                    {
                        Thread.Sleep(300);
                        Dispatcher.UIThread.InvokeAsync(Close).Wait();
                    });
            }

            protected override void OnClosed(EventArgs e)
            {
                mThreadLoop.Invoke();
                base.OnClosed(e);
            }
        }
    }
}