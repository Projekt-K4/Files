using System;
using System.Collections.Generic;
namespace SimSharp.Samples
{
    class RunAllSamples
    {
        public static void Main(string[] args)
        {
            ////patientManagerTest:
            //Console.WriteLine("before:");
            //List<Patient> before = patientManager.getInstance().createPatients(10);
            //foreach(Patient p in before)
            //{
            //    Console.WriteLine(p.getTimeToLiveString());
            //}
            //Console.WriteLine();
            //Console.WriteLine("after:");
            //List<Patient> after = patientManager.getInstance().getAllPatients(true);
            //foreach (Patient p in after)
            //{
            //    Console.WriteLine(p.getTimeToLiveString());
            //}


            //Simulation
            eventLog.getLog();//for initalizing and creating file
            //patientManager.getInstance(); //initializing PatientManager //Überflüssig, da PatientGenerator in RunSimulation erneut erstellt wird

            new K4().RunSimulation(10);//Amount of Patients as Parameter
            eventLog.getLog().writeToFile("C:/Users/Aich/Desktop/log.csv");
            eventLog.getLog().fromFileToConsole("C:/Users/Aich/Desktop/log.csv", "\t");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}