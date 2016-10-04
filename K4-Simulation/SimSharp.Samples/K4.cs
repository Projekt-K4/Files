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
        static Resource SilentRoom = null;

        static IEnumerable<Event> Steuerprozess(Environment env)    //Simulator start, Timer starts!
        {
            //Resources:

            OPWaiting = new Resource(env, Parameter.getInstance().OPWaiting);   //where do they wait? How many can wait for OP?
            Mortuary = new Resource(env, Parameter.getInstance().mortuary);
            Ward = new Resource(env, Parameter.getInstance().ward);
            SilentRoom = new Resource(env, Parameter.getInstance().silentRoom);

            for(int i=0;i<RSStore.getInstance().opsReady();i++)
            {
                //probability for available ops to be blocked at the beginning
                if(env.RandUniform(0,100)<Parameter.getInstance().OPBlockedRate)
                {
                    env.Process(Block_OP(env));
                }
            }
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
        static IEnumerable<Event> Triage(Environment env, Patient pat, int from = 0, int detail = 0)
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
                env.Process(SilentRoomProcess(env, pat, from.ToString()));
            }
            else if(pat.getTriageNr() == 4)
            {
                if (from == 4)
                {
                    eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "5"+detail);
                }
                env.Process(MortuaryProcess(env, pat));
            }
            else if ((pat.getTriageNr() == 1|| pat.getTriageNr() == 3) && from==4)
            {
                env.Process(WardProcess(env, pat,detail.ToString()));
            }
        }
        
        static IEnumerable<Event>Block_OP(Environment env)
        {
            //OP Resources:
            var op = RSStore.getInstance().OPStore.Get();
            var ch = RSStore.getInstance().ChirurgStore.Get();
            var ane = RSStore.getInstance().AnesthesistStore.Get();
            var nurse1 = RSStore.getInstance().NurseStore.Get();
            var nurse2 = RSStore.getInstance().NurseStore.Get();
            var anen = RSStore.getInstance().AnesthesistNurseStore.Get();
            var support = RSStore.getInstance().SupportStore.Get();
            var rta = RSStore.getInstance().RTAStore.Get();

            yield return nurse1;
            yield return nurse2;
            yield return anen;
            yield return support;
            yield return rta;
            yield return ch;
            yield return ane;
            yield return op;

            //Blocked OP
            eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "13" + op.Value);
            yield return env.TimeoutUniform(TimeSpan.FromSeconds(Parameter.getInstance().OPMin), TimeSpan.FromSeconds(Parameter.getInstance().OPMax));
            RSStore.getInstance().OPStore.Put(op.Value);
            RSStore.getInstance().ChirurgStore.Put(ch.Value);
            RSStore.getInstance().AnesthesistStore.Put(ane.Value);
            RSStore.getInstance().NurseStore.Put(nurse1.Value);
            RSStore.getInstance().NurseStore.Put(nurse2.Value);
            RSStore.getInstance().AnesthesistNurseStore.Put(anen.Value);
            RSStore.getInstance().SupportStore.Put(support.Value);
            RSStore.getInstance().RTAStore.Put(rta.Value);
            eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "13" + op.Value);
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
                var op = RSStore.getInstance().OPStore.Get();
                var ch = RSStore.getInstance().ChirurgStore.Get();
                var ane = RSStore.getInstance().AnesthesistStore.Get();
                var nurse1 = RSStore.getInstance().NurseStore.Get();
                var nurse2 = RSStore.getInstance().NurseStore.Get();
                var anen = RSStore.getInstance().AnesthesistNurseStore.Get();
                var support = RSStore.getInstance().SupportStore.Get();
                var rta = RSStore.getInstance().RTAStore.Get();

              
                
                yield return nurse1;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "2" + op.Value);
                yield return nurse2;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "3" + op.Value);
                yield return anen;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "6" + op.Value);
                yield return support;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "4" + op.Value);
                yield return rta;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "7" + op.Value);
                yield return ch;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "1" + op.Value);
                yield return ane;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "7" + "5" + op.Value);
                yield return op;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "4" + op.Value);

                //Probability for surviving the OP
                if (survive(env, pat))
                {
                    pat.setTimeToLive(pat.getTimeToLive().Add(TimeSpan.FromDays(Parameter.getInstance().OPWinTime)));
                }
                else
                {
                    pat.setTimeToLive(env.Now);
                }
                int OPNr = Int32.Parse(op.Value.ToString());
               //it´t possible that the patient is still alive without the if....has to be changed
                yield return env.TimeoutUniform(TimeSpan.FromSeconds(Parameter.getInstance().OPBlockedMin), TimeSpan.FromSeconds(Parameter.getInstance().OPBlockedMax));
                //OP Freigabe
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), "", "", "", "" + "8" + op.Value);
                //re triage process
                env.Process(Triage(env, pat, 4,OPNr));
                // env.Process(WardProcess(env, pat, Mortuary, op.Value.ToString()));
                RSStore.getInstance().OPStore.Put(op.Value);
                RSStore.getInstance().ChirurgStore.Put(ch.Value);
                RSStore.getInstance().AnesthesistStore.Put(ane.Value);
                RSStore.getInstance().NurseStore.Put(nurse1.Value);
                RSStore.getInstance().NurseStore.Put(nurse2.Value);
                RSStore.getInstance().AnesthesistNurseStore.Put(anen.Value);
                RSStore.getInstance().SupportStore.Put(support.Value);
                RSStore.getInstance().RTAStore.Put(rta.Value);
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

        static IEnumerable<Event> SilentRoomProcess(Environment env, Patient pat, String comeFrom)
        {
            //wating resource
            using (var reqM = SilentRoom.Request())
            {
                //dead patient brought into mortuary
                yield return reqM;
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "10");
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
            RSStore.getInstance().initStores(env,chirurgs: 4, nurses: 4, support: 4, anesthesists: 4, anesthesistsNurse: 4, rtas: 4, ops: 4);
            
            //Parameter for waitingTimes and RoomSpace
            Parameter.getInstance().initialize(_OPWaiting: 50, _mortuary: 50, _ward: 100, _silentRoom: 100, _triageTime: 30, _arriveMin: 0, _arriveMax: 3600, _OPWinTime: 1000, _OPMin: 1800, _OPMax: 3600, _OPDyingRate: 25, _OPBlockedMin: 600, _OPBlockedMax: 1800, _OPBlockedRate: 50);

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
            RSStore.getInstance().initStores(env, chirurgs: 4, nurses: 4, support: 4, anesthesists: 4, anesthesistsNurse: 4, rtas: 4, ops: 4);
            //Parameter for waitingTimes and RoomSpace
            Parameter.getInstance().initialize(_OPWaiting: 50, _mortuary: 50, _ward: 100, _silentRoom: 100, _triageTime: 30, _arriveMin: 0, _arriveMax: 3600, _OPWinTime: 1000, _OPMin: 1800, _OPMax: 3600, _OPDyingRate: 25, _OPBlockedMin: 600, _OPBlockedMax: 1800, _OPBlockedRate: 50);
        
            //Simulation starts
            env.Process(Steuerprozess(env));
            env.RunD();
        }
    }
}
