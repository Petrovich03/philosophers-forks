using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfApp;

namespace WpfApp
{
    class filosov
    {
        private int id;
        private Semaphore fork1, fork2;
        private Semaphore check;
        private Random random;
        private Ellipse[] pp;
        private TextBox[] ff;
        int think1;
        int think2;
        int eat1;
        int eat2;
        int num_eat;
        TextBox info;
        private Stopwatch stopwatch;
        int time;

        public filosov(int id, Semaphore fork1, Semaphore fork2, Ellipse[] pp, int think1, int think2, int eat1, int eat2, TextBox[] ff, Semaphore check, int num_eat, TextBox info, int time)
        {
            this.id = id;
            this.fork1 = fork1;
            this.fork2 = fork2;
            this.random = new Random();
            this.pp = pp;
            this.ff = ff;
            this.think1 = think1;
            this.think2 = think2;
            this.eat1 = eat1;
            this.eat2 = eat2;
            this.check = check;
            this.num_eat = num_eat;
            this.info = info;
            this.time = time;
        }

        public void Think()
        {
            Console.WriteLine($"Философ {id} думает.");
            pp[id].Dispatcher.Invoke(() =>
            {
                pp[id].Fill = new SolidColorBrush(Colors.Green); 
            });


            Thread.Sleep(random.Next(think1, think2));
        }

        public void Eat()
        {
            Console.WriteLine($"Философ {id} голоден и пытается взять вилки {id} и {(id + 1) % 5}.");

            pp[id].Dispatcher.Invoke(() =>
            {
                pp[id].Fill = new SolidColorBrush(Colors.Aqua);
            });


            check.WaitOne();



            fork1.WaitOne();
            pp[id].Dispatcher.Invoke(() =>
            {
                pp[id].Fill = new SolidColorBrush(Colors.Yellow);
            });
            ff[id].Dispatcher.Invoke(() =>
            {
                ff[id].Background = new SolidColorBrush(Colors.Yellow);
            });
            fork2.WaitOne();
            ff[(id + 1) % 5].Dispatcher.Invoke(() =>
            {
                ff[(id + 1) % 5].Background = new SolidColorBrush(Colors.Red);
            });
            ff[id].Dispatcher.Invoke(() =>
            {
                ff[id].Background = new SolidColorBrush(Colors.Red);
            });

            Console.WriteLine($"Философ {id} ест.");

            pp[id].Dispatcher.Invoke(() =>
            {
                pp[id].Fill = new SolidColorBrush(Colors.Red);
            });

            Thread.Sleep(random.Next(eat1, eat2));


            ff[id].Dispatcher.Invoke(() =>
            {
                ff[id].Background = new SolidColorBrush(Colors.White);
            });
            fork1.Release();


            ff[(id + 1) % 5].Dispatcher.Invoke(() =>
            {
                ff[(id + 1) % 5].Background = new SolidColorBrush(Colors.White);
            });

            fork2.Release();


            Console.WriteLine($"Философ {id} закончил есть и кладет вилки обратно на стол.");
            pp[id].Dispatcher.Invoke(() =>
            {
                pp[id].Fill = new SolidColorBrush(Colors.Green);
            });

            check.Release();
            num_eat++;


            info.Dispatcher.Invoke(() =>
            {
                info.Text += $"Филосов {id+1} поел: {num_eat} \n";
            });
        }

        public void Run()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.ElapsedMilliseconds < time) 
            {
                Think();
                Eat();
            }
        }
    }
}
