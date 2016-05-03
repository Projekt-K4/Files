using System;

namespace SimSharp.Samples
{
    class RunAllSamples
    {
        public static void Main(string[] args)
        {
            eventLog.getLog();//for initalizing and creating file
                       
            new K4().RunSimulation(10);//Amount of Patients as Parameter
            eventLog.getLog().writeToFile("C:/Users/Christina/Desktop/log.csv");
            eventLog.getLog().fromFileToConsole("C:/Users/Christina/Desktop/log.csv", "\t");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}