using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace SimSharp.Samples
{
    //Definition of Patient status based on remaining TTL (in Seconds)
    public static class TTLGlobal
    {
        public const int TTL_DEAD = 0;                          //0 seconds left - dead
        public const int TTL_HOPELESS = 3600;                   //3600 seconds left - hopeless
        public const int TTL_SEVERELY_INJURED = 36000;          //36000 seconds left - severely injured
        public const int TTL_SLIGHTLY_INJURED = 300000000;      //>36000 seconds left - slightly injured
    }

    class PatientGenerator : IEnumerable, IEnumerator //PatientGenerator
    {
        /*          Fields          */
        private static int sequence = 0;                                //iterating number for KID
        //public SystemRandom globalTime = new SystemRandom();
        private static List<Patient> patientList = new List<Patient>(); //List of all Patients
        private static int position = -1;                               //position in List for Iterator

        //Distribution Array
        //does not need to be given in percentages.Could also be set as:
        // # {190, 150, 50, 60 }
        // # {0.5 , 0.2, 12, 17} 
        //Chance for generating:         Position:
        private double[] distribution = { 0.15         // # dead Patient                  0
                                        , 0.15           // # hopeless Patient              1
                                        , 0.55           // # severly injured Patient       2
                                        , 0.15 };        // # slightly injured Patient      3

        /*          Constructor          */
        /*public PatientGenerator()
        {
            Console.WriteLine("Catastrophe engages\n");
    
            randGen = new Random();

        }*/
        //Creates PatientGenerator with given number of casualties
        /*public PatientGenerator(int _numberOfCasualties)
        {
            generateCasualties(_numberOfCasualties);
            Console.WriteLine(_numberOfCasualties + " involved in catastrophe\n");
            //randGen = new SystemRandom();
            randGen = new Random();
        }*/
        public PatientGenerator(int _numberOfCasualties, Environment env)
        {
            generateCasualties(_numberOfCasualties, env);
            Console.WriteLine(_numberOfCasualties + " involved in catastrophe");
            //globalTime = new SystemRandom(seed);
    }

        /*          Private         */
        //Produces given number of Patients and appends them to patientList
        private void generateCasualties(int _numberOfPatients, Environment env)
        {
            for (int i = 0; i < _numberOfPatients; i++)
            {
                addPatient(env);
            }
        }
        /*          Public          */

        //Adds a new Patient to the List according to the given distribution
        public bool addPatient(Environment env)
        {
            double x = env.RandUniform(0,1); // determins the Patients state
            double sumDist = distribution.Sum();

            if (x <= distribution[0] / sumDist)
            {
                patientList.Add(new Patient(TTLGlobal.TTL_DEAD));
            }
            else if (x <= (distribution[0] + distribution[1]) / sumDist)
            {
                patientList.Add(new Patient((double)env.RandUniform(TTLGlobal.TTL_DEAD + 1, TTLGlobal.TTL_HOPELESS))); // Creates hopeless patient as distributed
            }
            else if (x <= (distribution[0] + distribution[1] + distribution[2]) / sumDist)
            {
                patientList.Add(new Patient((double)env.RandUniform(TTLGlobal.TTL_HOPELESS + 1, TTLGlobal.TTL_SEVERELY_INJURED)));
            }
            else if (x <= distribution.Sum())
            {
                patientList.Add(new Patient((double)env.RandUniform(TTLGlobal.TTL_SEVERELY_INJURED + 1, TTLGlobal.TTL_SLIGHTLY_INJURED)));
            }
            else
                Console.WriteLine("Something went wrong!");

            return true;
        }

        public static int get_new_id()
        {
            return ++sequence;
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }

        public bool MoveNext()
        {
            if (position < patientList.Count - 1)
            {
                ++position;

                return true;
            }
            return false;
        }


        public void Reset()
        {
            position = -1;
        }

        public object Current
        {
            get
            {
                return patientList[position];
            }
        }

        public static object getNextPatient()
        {
            return patientList[++position];
        }
        public List<Patient> getPatientList()
        {
            return patientList;
        }

        //changes the distribution
        public void setDistribution(double dead, double hopeless, double severe, double slight)
        {
            distribution[0] = dead;
            distribution[1] = hopeless;
            distribution[2] = severe;
            distribution[3] = slight;
        }
    }


    class Patient
    {

        private string KID = "undefined";
        public DateTime arrivalTime;
        private DateTime TTL = new DateTime(); //Is the time the Patient has left to live
        private int triageNr;

        public void setKID()
        {
            KID="4 KH 12 " + PatientGenerator.get_new_id();
        }
        //When creating the patient, TTL will be set to the current time + the given seconds
        public Patient(double seconds)
        {
            DateTime thisDay = DateTime.Now.AddSeconds(seconds);
            setTimeToLive(new DateTime(thisDay.Year, thisDay.Month, thisDay.Day, thisDay.Hour, thisDay.Minute, thisDay.Second));
        }

        public String getKID()
        {
            return KID;
        }

        public DateTime getTimeToLive()
        {
            return TTL;
        }
        public String getTimeToLiveString()
        {
            return TTL.ToLongTimeString();
        }
        public void setTimeToLive(DateTime _TTL)
        {
            TTL = _TTL;
        }

        public int getTriageNr()
        {
            return triageNr;
        }

        public void setTriageNr(int trnr)
        {
            triageNr = trnr;
        }


        public void triagePatient(DateTime TTL)//changes of this method, has to be also changed in Priority class
        // 1 slightly injured, 2 severely injured, 3 hopeless, 4 dead
        // TTL in seconds
        {

            TimeSpan timeDifference = TTL.Subtract(DateTime.Now);
            double TTLInSec = timeDifference.TotalSeconds;
            //Console.WriteLine("calculated time to Live" + TTLInSec + "| given TTL:" + TTL + "| given TTL:" + TTL.Second); //for debuging purposes
            if (TTLInSec <= TTLGlobal.TTL_DEAD) triageNr = 4; //dead
            else if (TTLInSec > TTLGlobal.TTL_DEAD && TTLInSec <= TTLGlobal.TTL_HOPELESS) triageNr = 3; //TTL <= 60min -> hopeless
            else if (TTLInSec > TTLGlobal.TTL_HOPELESS && TTLInSec <= TTLGlobal.TTL_SEVERELY_INJURED) triageNr = 2; //TTL > 15min and <= 10h -> serverely injured
            else triageNr = 1; //TTL > 10h -> slightly injured

        }

        public void withdrawTTL(TimeSpan subtrahend)
        {
            TTL -= subtrahend;
        }
    }


}
