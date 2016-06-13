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
            patientManager.getInstance();//initializing PatientManager

            new K4().RunSimulation(20,42);//Amount of Patients as Parameter
            //new K4().RunSimulation(20);
            eventLog.getLog().writeToFile("C:/Users/Aich/MBI/4.sem/projekt/log/log11305.csv");
            eventLog.getLog().fromFileToConsole("C:/Users/Aich/MBI/4.sem/projekt/log/log11305.csv", "\t");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}