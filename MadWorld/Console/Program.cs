﻿using MadMachineLearning;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            bool startInsert = false;
            bool startMachineLearning = true;
            bool startStorage = false;

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
