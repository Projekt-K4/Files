using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimSharp.Samples
{
    class RunAllSamples
    {
        public static void Main()
        {
            Patient Patient1 = new Patient(400);

            PatientGenerator catastrophe = new PatientGenerator(10);

            Console.WriteLine(PatientGenerator.getNextPatient();
            PatientGenerator.getNextPatient();
            PatientGenerator.getNextPatient();
        }
    }
}
