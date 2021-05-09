namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Startup startup = Startup.Create();
            startup.Load();

            System.Console.WriteLine("Start: insert resume insert!");
            startup.Inserter.Insert();
            System.Console.WriteLine("Finished: insert resume insert!");
        }
    }
}
