namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Current Thread: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Run(() =>
            {
                Console.WriteLine($"Current Thread: {Thread.CurrentThread.ManagedThreadId}");
                for (int i = 0; i < 15; i++)
                {
                    Console.WriteLine(i);
                }
            });

            Console.WriteLine($"Current Thread: {Thread.CurrentThread.ManagedThreadId}");
        }

        static void Method1()
        {
            try
            {
                Console.WriteLine("Method 1 started");
                Method2();
                Console.WriteLine("Method 1 finished");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception caught in method 1");
            }
        }

        static void Method2()
        {
            Console.WriteLine("Method 2 started");
            Method3();
            Console.WriteLine("Method 2 finished");
        }

        static void Method3()
        {
            Console.WriteLine("Method 3 started");
            throw new Exception();
            Console.WriteLine("Method 3 finished");
        }
    }
}