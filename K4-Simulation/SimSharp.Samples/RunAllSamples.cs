using System;
using System.Collections.Generic;
namespace SimSharp.Samples
{
    class RunAllSamples
    {
        public static void Main(string[] args)
        {
            //Simulation
            eventLog.getLog();//for initalizing and creating file

            new K4().RunSimulation(50,42);//Amount of Patients as Parameter
            //new K4().RunSimulation(20);
            eventLog.getLog().writeToFile("C:/Users/Andreas/Desktop/log.csv");
            //eventLog.getLog().fromFileToConsole("C:/Users/Andreas/Desktop/log.csv", "\t");
            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}