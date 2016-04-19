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
        public Random globalTime = new Random();
        private static List<Patient> patientList = new List<Patient>();
        private static int position = -1;
    


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
                //Patient_list.Add(new Patient(get_random_time()));
            }
        }


        public int get_random_time()
        {
            int random_time = globalTime.Next(1, 400);
            return random_time;
        }
        public bool add_Patient(List<Patient> plist, int random_time)
        {
            plist.Add(new Patient(random_time));

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
    }


    class Patient
    {
        private int catId = PatientGenerator.get_new_id();
        private int hospitalArriveTime;
        private int timeToLive = 600; //Is the time the Patient has left to live
        private int triageNr;
        public Patient()
        {
            Console.WriteLine("Patient with default life expectancy(600) created");
        }
        public Patient(int assignTime) //Overloaded Constructor
        {
            timeToLive = assignTime;
            Console.WriteLine("Patient with life expectancy:" + timeToLive + "created");
        }

        public int getCatId()
        {
            return catId;
        }

        public int getTimeToLive()
        {
            return timeToLive;
        }
        public void setTimeToLive(int TTL)
        {
            timeToLive = TTL;
        }

        public int getTriageNr()
        {
            return triageNr;
        }

        public void setTriageNr(int trnr)
        {
            triageNr = trnr;
        }


        public void triagePatient(double TTL)
        // 1 slightly injured, 2 severely injured, 3 hopeless, 4 dead
        // TTL in seconds
        {
            if (TTL <= 0) triageNr = 4; //dead
            else if (TTL > 0 && TTL <= 900) triageNr = 3; //TTL <= 15min -> hopeless
            else if (TTL > 900 && TTL <= 36000) triageNr = 2; //TTL > 15min and <= 10h -> serverely injured
            else triageNr = 1; //TTL > 10h -> slightly injured
        }

        public void withdrawTTL(int subtrahend)
        {
            timeToLive -= subtrahend;
        }
    }   

 


class Program
    {
        static void Main()
        {
            Patient Patient1 = new Patient(400);

            PatientGenerator catastrophe = new PatientGenerator(10);

            PatientGenerator.getNextPatient();
            PatientGenerator.getNextPatient();
            PatientGenerator.getNextPatient();
        }
    }


}
