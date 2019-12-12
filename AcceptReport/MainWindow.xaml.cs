using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AcceptReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string AppPath = @"C:\Users\home\source\repos\ReportCreater\ReportCreater\bin\Debug\netcoreapp3.0\ReportCreater.dll";
        public string Email { get; set; }
        public TimeSpan Time { get; set; }
        public DispatcherTimer Timer;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            Email = textBox_Email.Text;
            GoTimer();
        }

        static void ProcessCreateAndSendReport(string arg)
        {
            var context = new ReportAssemblyLoadContext();
            var assemblyPath = AppPath;
            Assembly assembly = context.LoadFromAssemblyPath(assemblyPath);
            var type = assembly.GetType("ReportCreater.Program");
            var greetMethod = type.GetMethod("CreateAndSendReport");
            var instance = Activator.CreateInstance(type);
            greetMethod.Invoke(instance, new object[] { arg });
            context.Unload();
            MessageBox.Show("Успешно отправлено!!!");
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void GoTimer()
        {
            Time = TimeSpan.FromSeconds(double.Parse(textBox_Timer.Text));
            Timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                textB_Timer.Text = Time.ToString("c");
                if (Time == TimeSpan.Zero)
                {
                    ProcessCreateAndSendReport(Email);
                    Time = TimeSpan.FromSeconds(double.Parse(textBox_Timer.Text));
                    Timer.Start();
                }
                Time = Time.Add(TimeSpan.FromSeconds(-1));

            }, Application.Current.Dispatcher);

            Timer.Start();

        }
    }
}
