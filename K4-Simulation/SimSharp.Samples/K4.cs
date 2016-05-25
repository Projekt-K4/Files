
using SimSharp.Core.Resources.StoreObjects;
using System;
using System.Collections.Generic;


namespace SimSharp.Samples
{
    public class K4
    {

        //static Resource Op = null;
        static Resource OPWaiting = null;
        static Resource Mortuary = null;
        static Resource Ward = null;

        static Store OPStore = null;
        
        static IEnumerable<Event> Steuerprozess(Environment env)    //Simulator start, Timer starts!
        {
            //Resources:

            OPWaiting = new Resource(env, 50);   //where do they wait? How many can wait for OP?
            Mortuary = new Resource(env, 50);
            Ward = new Resource(env, 150);
            OPStore = new Store(env, 4);

            for (int i=1; i<=4; i++)
            {
                OP op = new OP(i.ToString());
                OPStore.Put(op);
            }


            while (patientManager.getInstance().stillPatientsLeft())
            {
                //timestop in seconds until new patient arrives
                yield return env.TimeoutUniform(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(3600));

                //each patient finds his way to the triage
                Patient pat = null;
                while((pat=patientManager.getInstance().getPatient(env))!=null)
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
            yield return env.Timeout(TimeSpan.FromSeconds(30));

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
                
                yield return env.Timeout(TimeSpan.FromSeconds(0));//Placeholder
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "9");
            }
            else if(pat.getTriageNr()==2)
            {
                
                env.Process(OP_waiting(env, pat, OPStore, OPWaiting));
            }
            else if(pat.getTriageNr()==3)
            {
                yield return env.Timeout(TimeSpan.FromSeconds(0));
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "10");
            }
            else
            {
                env.Process(MortuaryProcess(env, pat, Mortuary));
            }
            
            }
            

        static IEnumerable<Event> OP_waiting(Environment env, Patient pat, Store Ops, Resource Waiting)
        {
            //wating resource
            using (var reqW = Waiting.Request())
            {
                //patients arrive at the waitingRoom
                yield return reqW;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "12");

                //OP Resources:
                var obj = Ops.Get();
                
                    yield return obj;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "4" + obj.Value);
                    yield return env.TimeoutUniform(TimeSpan.FromSeconds(1200), TimeSpan.FromSeconds(7200));
                env.Process(WardProcess(env, pat, Mortuary, new OP(obj.Value.ToString())));
                Ops.Put(new OP(obj.Value.ToString()));
            }

        }


        static IEnumerable<Event> MortuaryProcess (Environment env, Patient pat, Resource Mortuary) //also pass code to know WHERE the patient died (to write in Log - Visualization wants that)
        {
            //wating resource
            using (var reqM = Mortuary.Request())
            {
                //dead patient brought into mortuary
                yield return reqM;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "11");
            }

        }

        static IEnumerable<Event> WardProcess(Environment env, Patient pat, Resource ward, OP op)
        {
            //wating resource
            using (var reqM = ward.Request())
            {
                //dead patient brought into mortuary
                yield return reqM;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "6" + op);
            }

        }


        public void RunSimulation(int amount,int seed)
        {
            //creating Environment of Simulation
            var env = new Environment(seed);//is responsible for every randomNumber in the simulation
            env.Reset(seed);
            
            //creating Patients
            PatientGenerator patientGen = new PatientGenerator(amount,env);//Patients get generated
            //PatientManager receives Patient list:
            patientManager.getInstance().createPatients(patientGen.getPatientList());//outside of process!!!!!!!     

            //Simulation starts
            env.Process(Steuerprozess(env));
            env.RunD();
            
           



        }

    }
}
