
using System;
using System.Collections.Generic;


namespace SimSharp.Samples
{
    public class K4
    {

        static Resource Op = null;
        static Resource OPWaiting = null;
        static IEnumerable<Event> Steuerprozess(Environment env)//Simulator start, Timer starts!
        {
            //Resources:
            Op = new Resource(env, 4);
            OPWaiting = new Resource(env,50);//where do they wait? How many can wait for OP?
            while (patientManager.getInstance().stillPatientsLeft())
            {
                //timestop in seconds until new patient arrives
                yield return env.TimeoutUniform(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(3600));

                //each patient finds his way to the triage
                Patient pat = null;
                while((pat=patientManager.getInstance().getPatient())!=null)
                {
                    pat.arrivalTime = env.Now;
                    eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), "---", "1");
                    yield return env.Process(Triage(env, pat));
                }
            }
          
        }
        static IEnumerable<Event> Triage(Environment env, Patient pat)
        {
            //patients arrive at the triage
            eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), "---", "2");

            //timestop for the duration of the triage process
            yield return env.Timeout(TimeSpan.FromSeconds(15));//15->30 Seconds->I don`t know why

            //TTL Calculation
            var support = env.Now.Subtract(pat.arrivalTime);//Timespan between now and arrival
            var TTL = pat.getTimeToLive().Subtract(support);
            pat.setTimeToLive(TTL - support);//subtract Timespan

            //Patient gets KID
            pat.setKID();

            //Patient gets new TTL
            pat.triagePatient(pat.getTimeToLive());
       
            //patient finally printed to log with triage number
            eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "3"+ pat.getTriageNr().ToString());
            if (pat.getTriageNr()==1)
            {

                yield return env.Timeout(TimeSpan.FromSeconds(0));
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "9");
            }
            else if(pat.getTriageNr()==2)
            {
                
                env.Process(OP_waiting(env, pat,Op,OPWaiting));
            }
            else if(pat.getTriageNr()==3)
            {
                yield return env.Timeout(TimeSpan.FromSeconds(0));
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "10");
            }
            else
            {
                yield return env.Timeout(TimeSpan.FromSeconds(0));
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "11");
            }
            

        }
        static IEnumerable<Event> OP_waiting(Environment env, Patient pat, Resource Op,Resource Waiting)
        {
            //wating resource
            using (var reqW = Waiting.Request())
            {
                //patients arrive at the triage
                yield return reqW;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "12");

                //OP Resources:
                using (var reqOP = Op.Request())
                {
                    yield return reqOP;
                    eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "4" + "numberOfOP");
                    yield return env.TimeoutUniform(TimeSpan.FromSeconds(1200), TimeSpan.FromSeconds(7200));
                    eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "6" + "numberOfOP");
                }
            }

        }


        public void RunSimulation(int amount)
        {
            //creating Environment of Simulation
            var env = new Environment(randomSeed: 41);

            //creating Patients
            PatientGenerator patientGen = new PatientGenerator(amount);//Patients get generated
            //PatientManager receives Patient list:
            patientManager.getInstance().createPatients(patientGen.getPatientList());//outside of process!!!!!!!     

            //Simulation starts
            env.Process(Steuerprozess(env));
            env.RunD();
           



        }

    }
}
