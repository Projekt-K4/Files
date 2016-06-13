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
  
        static IEnumerable<Event> Steuerprozess(Environment env)    //Simulator start, Timer starts!
        {
            //Resources:

            OPWaiting = new Resource(env, Parameter.getInstance().OPWaiting);   //where do they wait? How many can wait for OP?
            Mortuary = new Resource(env, Parameter.getInstance().mortuary);
            Ward = new Resource(env, Parameter.getInstance().ward);

            while (patientManager.getInstance().stillPatientsLeft())
            {
                //timestop in seconds until new patient arrives
                yield return env.TimeoutUniform(TimeSpan.FromSeconds(Parameter.getInstance().arriveMin), TimeSpan.FromSeconds(Parameter.getInstance().arriveMax));

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
        static IEnumerable<Event> Triage(Environment env, Patient pat, int from=0)
        {
            if (from == 0)
            {
                //patients arrive at the triage
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), "---", "2");

                //timestop for the duration of the triage process
                yield return env.Timeout(TimeSpan.FromSeconds(Parameter.getInstance().triageTime));
            }
            //TTL Calculation
            var support = env.Now.Subtract(pat.arrivalTime);//Timespan between now and arrival
            var TTL = pat.getTimeToLive().Subtract(support);
            pat.setTimeToLive(TTL - support);//subtract Timespan

            if (from == 0)
            {
                //Patient gets KID
                pat.setKID();
            }
            //Patient gets new TTL
            pat.triagePatient(pat.getTimeToLive());
            if (from == 0)
            {
                //patient finally printed to log with triage number
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "3" + pat.getTriageNr().ToString());
            }
            if (pat.getTriageNr()==1 && from != 4)
            {
                
                yield return env.Timeout(TimeSpan.FromSeconds(0));//Placeholder
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "9");
            }
            else if(pat.getTriageNr()==2&&from!=4)
            {
                
                env.Process(OP_waiting(env, pat));
            }
            else if(pat.getTriageNr()==3 && from != 4)
            {
                yield return env.Timeout(TimeSpan.FromSeconds(0));
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "10");
            }
            else if(pat.getTriageNr() == 4)
            {
                env.Process(MortuaryProcess(env, pat));
            }
            else if ((pat.getTriageNr() == 1|| pat.getTriageNr() == 3) && from==4)
            {
                env.Process(WardProcess(env, pat,"4"));
            }
        }
            

        static IEnumerable<Event> OP_waiting(Environment env, Patient pat)
        {
            //wating resource
            using (var reqW = OPWaiting.Request())
            {
                //patients arrive at the waitingRoom
                yield return reqW;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "12");

                //OP Resources:
                OP op = RSStore.getInstance().GetOP();
                var ch = RSStore.getInstance().ChirurgStore.Get();
                var ane = RSStore.getInstance().AnesthesistStore.Get();
                var nurse1 = RSStore.getInstance().NurseStore.Get();
                var nurse2 = RSStore.getInstance().NurseStore.Get();
                var anen = RSStore.getInstance().AnesthesistNurseStore.Get();
                var support = RSStore.getInstance().SupportStore.Get();
                var rta = RSStore.getInstance().RTAStore.Get();
                
                yield return ch;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "1" + op);
                yield return ane;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "5" + op);
                yield return nurse1;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "2" + op);
                yield return nurse2;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "3" + op);
                yield return anen;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "6" + op);
                yield return support;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "4" + op);
                yield return rta;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "7" + op);
                yield return op;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "4" + op);
                //System for new TTL if patient dies or survives the OP Process!!!!
                //Has to be changed in the near future!!!!!!!!
                if (survive(env, pat))
                {
                    pat.setTimeToLive(pat.getTimeToLive().Add(TimeSpan.FromDays(Parameter.getInstance().OPWinTime)));
                }
               //it´t possible that the patient is still alive without the if....has to be changed
                yield return env.TimeoutUniform(TimeSpan.FromSeconds(Parameter.getInstance().OPMin), TimeSpan.FromSeconds(Parameter.getInstance().OPMax));
                //re triage process
                env.Process(Triage(env, pat, 4));
                // env.Process(WardProcess(env, pat, Mortuary, op.Value.ToString()));
                RSStore.getInstance().PutOP(op); RSStore.getInstance().ChirurgStore.Put(ch.Value); RSStore.getInstance().AnesthesistStore.Put(ane.Value); RSStore.getInstance().NurseStore.Put(nurse1.Value); RSStore.getInstance().NurseStore.Put(nurse2.Value); RSStore.getInstance().AnesthesistNurseStore.Put(anen.Value); RSStore.getInstance().SupportStore.Put(support.Value);
                // Ops.Put(new OP(obj.Value.ToString()));
            }

        }
        static bool survive(Environment env,Patient p)
        {
            TimeSpan timeDifference = p.getTimeToLive().Subtract(DateTime.Now);
            double TTLInSec = timeDifference.TotalSeconds;
            if (TTLInSec > TTLGlobal.TTL_DEAD)
            {
                double live = env.RandUniform(0, 100);
                if (live > Parameter.getInstance().OPDyingRate)
                {
                    return true;
                }
            }
            return false;
        }

        static IEnumerable<Event> MortuaryProcess (Environment env, Patient pat) //also pass code to know WHERE the patient died (to write in Log - Visualization wants that)
        {
            //wating resource
            using (var reqM = Mortuary.Request())
            {
                //dead patient brought into mortuary
                yield return reqM;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "11");
            }

        }

        static IEnumerable<Event> WardProcess(Environment env, Patient pat, String comeFrom)
        {
            //wating resource
            using (var reqM = Ward.Request())
            {
                //dead patient brought into mortuary
                yield return reqM;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "6" + comeFrom);
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
            //initialize Stores
            RSStore.getInstance().initStores(env, 4, 4, 4, 4, 4, 4, 4);
            //Parameter for waitingTimes and RoomSpace
            Parameter.getInstance().initialize(50, 50, 100, 30, 0, 3600, 1000, 1200, 7200, 10);

            //Simulation starts
            env.Process(Steuerprozess(env));
            env.RunD();
        }
        public void RunSimulation(int amount)//overload without seed
        {
            //creating Environment of Simulation
            var env = new Environment();//is responsible for every randomNumber in the simulation
            //creating Patients
            PatientGenerator patientGen = new PatientGenerator(amount, env);//Patients get generated
            //PatientManager receives Patient list:
            patientManager.getInstance().createPatients(patientGen.getPatientList());//outside of process!!!!!!!     
            //initialize Stores
            RSStore.getInstance().initStores(env, 4, 4, 4, 4, 4, 4, 4);
            //Parameter for waitingTimes and RoomSpace
            Parameter.getInstance().initialize(50, 50, 100, 30, 0, 3600, 1000, 1200, 7200, 10);

            //Simulation starts
            env.Process(Steuerprozess(env));
            env.RunD();
        }
    }
}
