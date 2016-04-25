
using System;
using System.Collections.Generic;


namespace SimSharp.Samples
{
    public class K4
    {
        /*public class Patient
        {
            public Patient(string _KID, DateTime _waitingTime, DateTime _getTimeToLive(), DateTime _arrivalTime)//placeholder for patient class
            {
                KID = _KID;
                waitingTime = _waitingTime;
                getTimeToLive() = _getTimeToLive();
                arrivalTime = _arrivalTime;
            }
            public string KID;
            public DateTime waitingTime;
            public DateTime getTimeToLive();
            public DateTime arrivalTime;
        }*/

        //Timevariables, not used in current Simulation
        TimeSpan ARRIVAL_TIME = TimeSpan.FromSeconds(360);
        TimeSpan PROCESSING_TIME = TimeSpan.FromSeconds(30);
        TimeSpan SIMULATION_TIME = TimeSpan.FromHours(1000);

        static IEnumerable<Event> Steuerprozess(Environment env, List<Patient> patients)//Simulator start, Timer starts!
        {

            //each patient arrives at the hospital after a random timestop
            foreach (Patient pat in patients)
            {
                pat.hospitalArriveTime = env.Now;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLive().ToLongTimeString(), pat.KID, "---", "arrived");
                //yield return env.TimeoutUniform(TimeSpan.FromSeconds(360), TimeSpan.FromSeconds(1000));//timestop in seconds till Triage
                yield return env.Process(Triage(env, pat));



            }
        }
        static IEnumerable<Event> Triage(Environment env, Patient pat)
        {
            //patients arrive at the triage
            eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLive().ToLongTimeString(), pat.KID, "---", "at triage");


            //timestop for the duration of the triage process
            yield return env.Timeout(TimeSpan.FromSeconds(30));

            //getTimeToLive() calculation
            var support = env.Now.Subtract(pat.arrivalTime);//Timespan between now and arrival
            pat.getTimeToLive() = pat.getTimeToLive() - support;//subtract Timespan


            //Triage number part should be insertet here....................................

            //patient finaly printed to log with triage number
            eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLive().ToLongTimeString(), pat.KID, "generated number", "get number");

        }




        public void RunSimulation()
        {
            var env = new Environment(randomSeed: 41, defaultStep: TimeSpan.FromMinutes(1));

            //Patients start to live
            /*List<Patient> patients = new List<Patient>();
            Random rand = new Random(41);*/
            PatientGenerator patientGen = new PatientGenerator(50);


            //Patients gets generated
           /* for (int i = 1; i <= 50; ++i)
            {
                int j = rand.Next(0, 10000);
                patients.Add(new Patient("Patient_" + i, new DateTime(), new DateTime(1970, 1, 1).AddSeconds(j), env.Now));
            }*/
            env.Process(Steuerprozess(env, patientGen.getPatientList()));
            env.RunD();




        }

    }
}
