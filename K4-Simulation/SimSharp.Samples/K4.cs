
using System;
using System.Collections.Generic;


namespace SimSharp.Samples
{
    public class K4
    {

        static Resource Op = null;
        //Timevariables, not used in current Simulation
        //TimeSpan ARRIVAL_TIME = TimeSpan.FromSeconds(360);
        //TimeSpan PROCESSING_TIME = TimeSpan.FromSeconds(30);
        //TimeSpan SIMULATION_TIME = TimeSpan.FromHours(1000);

        static IEnumerable<Event> Steuerprozess(Environment env, List<Patient> patients)//Simulator start, Timer starts!
        {
            //PatientManager receives Patient list:
            patientManager.getInstance().createPatients(patients);//outside of process!!!!!!!
            //PatientManager sends Patients from Apocalyps
            List<Patient> arriving = null;
            Op = new Resource(env, 2);
            while (patientManager.getInstance().stillPatientsLeft())
            {
                yield return env.TimeoutUniform(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(600));//timestop in seconds till new Group arrives
                arriving = patientManager.getInstance().getRandomPatients(1, 1, true);
                
                //each patient finds his way to the triage
                foreach (Patient pat in arriving)
                {
                    pat.arrivalTime = env.Now;
                    eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), "---", "arrived");
                    yield return env.Process(Triage(env, pat));
                }
            }
          
        }
        static IEnumerable<Event> Triage(Environment env, Patient pat)
        {
            //patients arrive at the triage
            eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), "---", "at triage");


            //timestop for the duration of the triage process
            yield return env.Timeout(TimeSpan.FromSeconds(15));//15->30 Seconds->I don`t know why
            //using resource:
            //Console.WriteLine(priority.getInstance().getPriority(pat.getTimeToLive()));
            //getTimeToLive() calculation
            var support = env.Now.Subtract(pat.arrivalTime);//Timespan between now and arrival
            var TTL = pat.getTimeToLive().Subtract(support);
            pat.setTimeToLive(TTL - support);//subtract Timespan


            pat.triagePatient(pat.getTimeToLive());

            //patient finaly printed to log with triage number
            eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "got number");
            if(pat.getTriageNr()==1)
            {
                yield return env.Timeout(TimeSpan.FromSeconds(15));
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "->Street");
            }
            else if(pat.getTriageNr()==2)
            {
                
                env.Process(OP_waiting(env, pat,Op));
            }
            else if(pat.getTriageNr()==3)
            {
                yield return env.Timeout(TimeSpan.FromSeconds(15));
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "->Church");
            }
            else
            {
                yield return env.Timeout(TimeSpan.FromSeconds(15));
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "->Cellar");
            }
            

        }
        static IEnumerable<Event> OP_waiting(Environment env, Patient pat, Resource Op)
        {
            //wating resource.......

            //patients arrive at the triage
            eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "waiting for OP");

            yield return env.Timeout(TimeSpan.FromSeconds(60));

            //OP Resources:
            eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "OP requested");
            using (var req = Op.Request())
            {
                yield return req;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), Op.Remaining+"OP assigned");
                yield return env.Timeout(TimeSpan.FromSeconds(3600));
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "->Station");
            } 

        }
        //static IEnumerable<Event> OP(Environment env, Patient pat)
        //{
        //    //patients arrive at the triage
        //    eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "at OP");


        //    //timestop for the duration of the OP process
        //    yield return env.TimeoutUniform(TimeSpan.FromSeconds(1200), TimeSpan.FromSeconds(10800));//timespan of one OP
        //    //using resource:
        //    //-----------------
           
        //    var support = env.Now.Subtract(pat.arrivalTime);//Timespan between now and arrival
        //    var TTL = pat.getTimeToLive().Subtract(support);
        //    pat.setTimeToLive(TTL - support);//subtract Timespan
        //    pat.triagePatient(pat.getTimeToLive());

        //    //patient finaly printed to log with triage number
        //    eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "operiert");

        //}




        public void RunSimulation(int amount)
        {
            //creating Environment of Simulation
            var env = new Environment(randomSeed: 41);
  
            //creating Patients
            PatientGenerator patientGen = new PatientGenerator(amount);//Patients get generated

            //pushing the patients to the Simulationprocess
            env.Process(Steuerprozess(env, patientGen.getPatientList()));//Simulation starts with generated Patientlist
            env.RunD();
           



        }

    }
}
