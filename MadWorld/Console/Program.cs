using Console.CoPilotTest;
using MadMachineLearning;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            bool startCoPilot = true;
            bool startInsert = false;
            bool startMachineLearning = false;
            bool startStorage = false;

            if (startCoPilot)
            {
                CoPilotTester.Start();
            }

            if (startInsert) {
                Startup startup = Startup.Create();
                startup.Load();

                System.Console.WriteLine("Start: insert resume insert!");
                startup.Inserter.Insert();
                System.Console.WriteLine("Finished: insert resume insert!");
            }

            if (startMachineLearning)
            {
                Learner learner = new();
                learner.Start(false);
            }

            if (startStorage)
            {
                StorageExample.Start();
            }
        }
    }
}
