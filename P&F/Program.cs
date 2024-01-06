using System;
using System.Threading;
using System.Timers;

class Philosopher
{
    private int id;
    private Semaphore fork1, fork2;
    private Random random;

    public Philosopher(int id, Semaphore fork1, Semaphore fork2)
    {
        this.id = id;
        this.fork1 = fork1;
        this.fork2 = fork2;
        this.random = new Random();
    }

    public void Think()
    {
        Console.WriteLine($"Философ {id} думает.");
        Thread.Sleep(random.Next(0, 5000)); // Философ думает случайное время (0-5 секунд).
    }

    public void Eat()
    {
        Console.WriteLine($"Философ {id} голоден и пытается взять вилки {id} и {(id + 1) % 5}.");

        fork1.WaitOne();
        fork2.WaitOne();

        Console.WriteLine($"Философ {id} ест.");
        Thread.Sleep(random.Next(0, 5000)); // Философ ест случайное время (0-5 секунд).

        fork1.Release();
        fork2.Release();

        Console.WriteLine($"Философ {id} закончил есть и кладет вилки обратно на стол.");
    }

    public void Run()
    {
        var timer = new System.Timers.Timer(10000); // Запускаем программу на 10 секунд.
        timer.Elapsed += (sender, e) => Environment.Exit(0);
        timer.Start();

        while (true)
        {
            Think();
            Eat();
        }
    }
}

class Program
{
    static void Main()
    {
        int numPhilosophers = 5;
        Semaphore[] forks = new Semaphore[numPhilosophers];
        Philosopher[] philosophers = new Philosopher[numPhilosophers];

        for (int i = 0; i < numPhilosophers; i++)
        {
            forks[i] = new Semaphore(1, 1);
        }

        for (int i = 0; i < numPhilosophers; i++)
        {
            philosophers[i] = new Philosopher(i, forks[i], forks[(i + 1) % numPhilosophers]);
            Thread philosopherThread = new Thread(philosophers[i].Run);
            philosopherThread.Start();
        }

        Thread.Sleep(10000); // Ожидаем 10 секунд.
    }
}
