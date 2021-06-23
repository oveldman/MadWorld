namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            bool startInsert = false;
            bool startStorage = true;

            if (startInsert) {
                Startup startup = Startup.Create();
                startup.Load();

                System.Console.WriteLine("Start: insert resume insert!");
                startup.Inserter.Insert();
                System.Console.WriteLine("Finished: insert resume insert!");
            }

            if (startStorage)
            {
                StorageExample.Start();
            }
        }
    }
}
