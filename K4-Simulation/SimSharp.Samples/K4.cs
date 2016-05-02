
using System;
using System.Collections.Generic;


namespace SimSharp.Samples
{
    public class K4
    {
        //Timevariables, not used in current Simulation
        //TimeSpan ARRIVAL_TIME = TimeSpan.FromSeconds(360);
        //TimeSpan PROCESSING_TIME = TimeSpan.FromSeconds(30);
        //TimeSpan SIMULATION_TIME = TimeSpan.FromHours(1000);

        static IEnumerable<Event> Steuerprozess(Environment env, List<Patient> patients)//Simulator start, Timer starts!
        {

            //each patient arrives at the hospital after a random timestop
            foreach (Patient pat in patients)
            {
                pat.arrivalTime = env.Now;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID().ToString(), "---", "arrived");
                //yield return env.TimeoutUniform(TimeSpan.FromSeconds(360), TimeSpan.FromSeconds(1000));//timestop in seconds till Triage
                yield return env.Process(Triage(env, pat));



            }
        }
        static IEnumerable<Event> Triage(Environment env, Patient pat)
        {
            //patients arrive at the triage
            eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID().ToString(), "---", "at triage");


            //timestop for the duration of the triage process
            yield return env.Timeout(TimeSpan.FromSeconds(30));

            //getTimeToLive() calculation
            var support = env.Now.Subtract(pat.arrivalTime);//Timespan between now and arrival
            var TTL = pat.getTimeToLive().Subtract(support);
            pat.setTimeToLive(TTL - support);//subtract Timespan


            pat.triagePatient(pat.getTimeToLive());

            //patient finaly printed to log with triage number
            eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID().ToString(), pat.getTriageNr().ToString(), "get number");

        }




        public void RunSimulation(int amount)
        {
            var env = new Environment(randomSeed: 41, defaultStep: TimeSpan.FromMinutes(1));

            //Patients start to live
            /*List<Patient> patients = new List<Patient>();
            Random rand = new Random(41);*/
            PatientGenerator patientGen = new PatientGenerator(amount);//Patients get generated


            //Patients gets generated
            /* for (int i = 1; i <= 50; ++i)
             {
                 int j = rand.Next(0, 10000);
                 patients.Add(new Patient("Patient_" + i, new DateTime(), new DateTime(1970, 1, 1).AddSeconds(j), env.Now));
             }*/
            env.Process(Steuerprozess(env, patientGen.getPatientList()));//Simulation starts with generated Patientlist
            env.RunD();
           



        }

    }
}
