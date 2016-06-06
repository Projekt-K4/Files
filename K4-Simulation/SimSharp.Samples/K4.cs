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
   
        /*static Store OPStore = null;
        static Store  ChirurgStore= null;
        static Store NurseStore = null;
        static Store SupportStore = null;
        static Store AnesthesistStore = null;
        static Store AnesthesistNurseStore = null;
        static Store RTAStore = null;

        static void initStores(Environment env,int chirurgs, int nurses, int support, int anesthesists, int anesthesistsNurse, int rtas,int ops)
        {
             int i = 0;
            OPStore = new Store(env, ops);
            ChirurgStore = new Store(env, chirurgs);
            NurseStore = new Store(env, nurses);
            SupportStore = new Store(env, support);
            AnesthesistStore = new Store(env, anesthesists);
            AnesthesistNurseStore = new Store(env, anesthesistsNurse);
            RTAStore = new Store(env, rtas);
            while (i<chirurgs||i<nurses||i<support||i<anesthesists||i<anesthesistsNurse||i<rtas||i<ops)
            {
                if(i<chirurgs)
                {
                    ChirurgStore.Put(new Staff(i));
                }
               if (i < nurses)
                {
                    NurseStore.Put(new Staff(i));
                }
                if (i < support)
                {
                    SupportStore.Put(new Staff(i));
                }
                if (i < anesthesists)
                {
                    AnesthesistStore.Put(new Staff(i));
                }
                if (i < anesthesistsNurse)
                {
                    AnesthesistNurseStore.Put(new Staff(i));
                }
                if (i < rtas)
                {
                    RTAStore.Put(new Staff(i));
                }
                if (i < ops)
                {
                    OPStore.Put(new Staff(i));
                }
                ++i;
            }   
        }*/
        static IEnumerable<Event> Steuerprozess(Environment env)    //Simulator start, Timer starts!
        {
            //Resources:

            OPWaiting = new Resource(env, 50);   //where do they wait? How many can wait for OP?
            Mortuary = new Resource(env, 50);
            Ward = new Resource(env, 150);

            /*OPStore = new Store(env, 4);

            for (int i=1; i<=4; i++)
            {
                OP op = new OP(i.ToString());
                OPStore.Put(op);
            }*/

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
                
                env.Process(OP_waiting(env, pat));
            }
            else if(pat.getTriageNr()==3)
            {
                yield return env.Timeout(TimeSpan.FromSeconds(0));
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "10");
            }
            else
            {
                env.Process(MortuaryProcess(env, pat));
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
                var op = RSStore.getInstance().OPStore.Get(); var ane = RSStore.getInstance().AnesthesistStore.Get(); var nurse1 = RSStore.getInstance().NurseStore.Get(); var nurse2 = RSStore.getInstance().NurseStore.Get();var anen = RSStore.getInstance().AnesthesistNurseStore.Get(); var support = RSStore.getInstance().SupportStore.Get();

                yield return op;yield return ane; yield return nurse1; yield return nurse2; yield return anen; yield return support; 
                eventLog.getLog().addLog(env.Now.ToLongTimeString(), pat.getTimeToLiveString(), pat.getKID(), pat.getTriageNr().ToString(), "4" + op.Value);
                yield return env.TimeoutUniform(TimeSpan.FromSeconds(1200), TimeSpan.FromSeconds(7200));
                env.Process(WardProcess(env, pat, Mortuary, op.Value.ToString()));
                RSStore.getInstance().OPStore.Put(op.Value); RSStore.getInstance().AnesthesistStore.Put(ane.Value); RSStore.getInstance().NurseStore.Put(nurse1.Value); RSStore.getInstance().NurseStore.Put(nurse2.Value); RSStore.getInstance().AnesthesistNurseStore.Put(anen.Value); RSStore.getInstance().SupportStore.Put(support.Value);
                // Ops.Put(new OP(obj.Value.ToString()));
            }

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

        static IEnumerable<Event> WardProcess(Environment env, Patient pat, Resource ward, String comeFrom)
        {
            //wating resource
            using (var reqM = ward.Request())
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


            //Simulation starts
            env.Process(Steuerprozess(env));
            env.RunD();
        }
    }
}
