using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using WpfApp;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Start();      
        }

        void Start()
        {
            int num_eat = 0;

            int think1 = Convert.ToInt32(thinkot.Text);
            int think2 = Convert.ToInt32(thinkdo.Text);
            int eat1 = Convert.ToInt32(eatot.Text);
            int eat2 = Convert.ToInt32(eatdo.Text);
            int time = Convert.ToInt32(time_work.Text);

            int numPhilosophers = 5;
            Semaphore[] forks = new Semaphore[numPhilosophers];
            filosov[] philosophers = new filosov[numPhilosophers];
            Semaphore check = new Semaphore(numPhilosophers - 1, numPhilosophers - 1);

            Ellipse[] pp = { p1, p2, p3, p4, p5 };
            TextBox[] ff = { f1, f2, f3, f4, f5 };
            TextBox info_all = info;
            info.Text = null;



            for (int i = 0; i < numPhilosophers; i++)
            {
                forks[i] = new Semaphore(1, 1);
            }

            for (int i = 0; i < numPhilosophers; i++)
            {
                philosophers[i] = new filosov(i, forks[i], forks[(i + 1) % numPhilosophers], pp, think1, think2, eat1, eat2, ff, check, num_eat, info_all, time);
                Thread philosopherThread = new Thread(philosophers[i].Run);
                philosopherThread.Start();
            }
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
