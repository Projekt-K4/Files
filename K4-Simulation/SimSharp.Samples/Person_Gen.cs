using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace SimSharp.Samples
{


    class PatientGenerator : IEnumerable, IEnumerator //PatientGenerator
    {
        private static int sequence = 0; //Pool aus dem fortlaufende Katastrophen Nummer für Patient erstellt wird
        public SystemRandom globalTime = new SystemRandom();
        private static List<Patient> patientList = new List<Patient>();
        private static int position = -1;
        private Random random_int = new Random();


        public PatientGenerator()
        {
            Console.WriteLine("Catastrophe engages");

        }
        public PatientGenerator(int number_of_causualities)
        {
            generate_causualities(number_of_causualities);
            Console.WriteLine(number_of_causualities + "involved in catastrophe");
        }
        private void generate_causualities(int number_of_Patients)
        {
            for (int i = 0; i < number_of_Patients; i++)
            {
                add_Patient(patientList, get_random_time());
                patientList.Add(new Patient(get_random_int(0,11), get_random_int(0, 59), get_random_int(0, 59)));
            }
        }
        public int get_random_int(int min, int max)
        {
            
            return random_int.Next(min,max);
        }

        public TimeSpan get_random_time()
        {
            TimeSpan random_time = new TimeSpan(globalTime.Next(0,12));
            return random_time;
        }
        public bool add_Patient(List<Patient> plist, TimeSpan random_time)
        {
            plist.Add(new Patient(get_random_int(0,11), get_random_int(0,59), get_random_int(0, 59)));
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
    }


    class Patient
    {
        /* public string KID;
         public DateTime waitingTime;
         public DateTime TTL;
         public DateTime arrivalTime;*/
        private int KID = PatientGenerator.get_new_id();
        public DateTime arrivalTime;
        private DateTime TTL = new DateTime(); //Is the time the Patient has left to live
        private int triageNr;
        //public Patient()
        //{
        //    Console.WriteLine("Patient with default life expectancy(600) created");
        //}
        //public Patient(DateTime assignTime) //Overloaded Constructor
        //{
        //    TTL = assignTime;
        //    Console.WriteLine("Patient with life expectancy:" + TTL + "created");
        //}
        public Patient(int hours, int minutes, int seconds) //Overloaded Constructor
        {
            DateTime thisDay = DateTime.Now;
            setTimeToLive(new DateTime(thisDay.Year, thisDay.Month, thisDay.Day, hours, minutes, seconds));
            //Console.WriteLine("Patient with life expectancy:" + TTL + "created");
        }

        public int getKID()
        {
            return KID;
        }

        public DateTime getTimeToLive()
        {
            return TTL;
        }
        public String getTimeToLiveString()
        {
            return TTL.ToShortTimeString();
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
            
            int TTLInSec =TTL.Hour*3600 + TTL.Minute*60 + TTL.Second;
            if (TTLInSec <= 0) triageNr = 4; //dead
            else if (TTLInSec > 0 && TTLInSec <= 3600) triageNr = 3; //TTL <= 60min -> hopeless
            else if (TTLInSec > 3600 && TTLInSec <= 36000) triageNr = 2; //TTL > 15min and <= 10h -> serverely injured
            else triageNr = 1; //TTL > 10h -> slightly injured

        }

        public void withdrawTTL(TimeSpan subtrahend)
        {
            TTL -= subtrahend;
        }
    }   

 


class Program
    {
        static void Main()
        {
            Patient Patient1 = new Patient();

            PatientGenerator catastrophe = new PatientGenerator(10);

            PatientGenerator.getNextPatient();
            PatientGenerator.getNextPatient();
            PatientGenerator.getNextPatient();
        }
    }


}
