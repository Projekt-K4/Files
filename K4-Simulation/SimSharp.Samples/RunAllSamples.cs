#region License Information
/* SimSharp - A .NET port of SimPy, discrete event simulation framework
Copyright (C) 2016  Heuristic and Evolutionary Algorithms Laboratory (HEAL)
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.*/
#endregion

using System;

namespace SimSharp.Samples
{
    class RunAllSamples
    {
        public static void Main(string[] args)
        {
            DateTime thisDay = DateTime.Now;
            Console.WriteLine(thisDay.ToLongTimeString());
            eventLog.getLog();//for initalizing and creating file
                              //PatientGenerator patientGen = new PatientGenerator(50);
            new K4().RunSimulation();
            eventLog.getLog().writeToFile("C:/Users/Andreas/Desktop/log.csv");
            eventLog.getLog().fromFileToConsole("C:/Users/Andreas/Desktop/log.csv", "\t");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}