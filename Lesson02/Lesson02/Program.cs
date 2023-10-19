namespace Lesson02
{
    internal class Program
    {
        static int counter = 0;
        static List<int> list = new List<int>();
        static object obj = new object();
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter your number");

            //int number = int.Parse(Console.ReadLine());

            //Thread thread = new Thread(Calculate);
            //thread.Name = $"Thread for number {number}";

            //if (number % 2 == 0)
            //{
            //    thread.Priority = ThreadPriority.Highest;
            //}
            //else
            //{
            //    thread.Priority = ThreadPriority.Lowest;
            //}

            //thread.Start(number);

            //Main(args);

            //counter = 0;
            //list = new List<int>();

            //Thread thread1 = new Thread(Print100);
            //// thread1.Priority = ThreadPriority.Highest;
            //thread1.Name = "Thread 1";

            //Thread thread2 = new Thread(Print100);
            //// thread2.Priority = ThreadPriority.Lowest;
            //thread2.Name = "Thread 2";

            //Thread thread3 = new Thread(Print100);
            //// thread3.Priority = ThreadPriority.Lowest;
            //thread3.Name = "Thread 3";

            //thread1.Start();
            //thread2.Start();
            //thread3.Start();

            //Console.ReadKey();
            //Console.WriteLine();
            //Console.WriteLine(counter);

            //list.ForEach(el => Console.WriteLine(el));

            //Main(args);

            counter = 0;

            // запускаем пять потоков
            for (int i = 1; i < 5; i++)
            {
                Thread myThread = new(Print);
                myThread.Name = $"Thread {i}";   // устанавливаем имя для каждого потока
                myThread.Start();
            }

            Console.ReadKey();
            Console.WriteLine(counter);

            Main(args);
        }

        static void Calculate(object obj)
        {
            int number = (int)obj;

            Console.WriteLine($"{Thread.CurrentThread.Name} Started task.");
            // Thread.Sleep(number * 1000);

            try
            {
                string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                Directory.CreateDirectory(path + "\\files");
                path += $"\\files\\data-{number}.txt";

                using var fs = new FileStream(path, FileMode.OpenOrCreate);
                using var sw = new StreamWriter(fs);

                for (int i = 1; i < 100_000_000; i++)
                {
                    sw.WriteLine(number * i);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} finished writing {number}");
            }
        }

        static void Print()
        {
            counter = 1;
            for (int i = 1; i < 10; i++)
            {
                lock (obj)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name}: {counter}");
                    counter++;
                    Thread.Sleep(100);
                }
            }
        }
    }
}