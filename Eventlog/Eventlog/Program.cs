using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace eventLog
{
    class Program
    {
        static void Main(string[] args)
        {
            eventLog.getLog().addLog("01.02.2014", "00:04:00", "patient_1", "triagiert");
            eventLog.getLog().addLog("01.02.2014", "00:04:00", "patient_2", "triagiert");
            eventLog.getLog().addLog("01.02.2014", "00:04:00", "patient_3", "triagiert");
            eventLog.getLog().addLog("01.02.2014", "00:04:00", "patient_4", "triagiert");
            eventLog.getLog().addLog("01.02.2014", "00:04:00", "patient_5", "triagiert");
            eventLog.getLog().addLog("01.02.2014", "00:04:00", "patient_6", "triagiert");
            eventLog.getLog().addLog("01.02.2014", "00:04:00", "patient_7", "triagiert");
            eventLog.getLog().addLog("01.02.2014", "00:04:00", "patient_8", "triagiert");
            eventLog.getLog().addLog("01.02.2014", "00:04:00", "patient_9", "triagiert");
            eventLog.getLog().addLog("01.02.2014", "00:04:00", "patient_10", "triagiert");
            eventLog.getLog().addLog("01.02.2014", "00:04:00", "patient_11", "triagiert");
            List<string> StringList = eventLog.getLog().toList();

            foreach (string s in StringList)
            {
                Console.WriteLine(s);
            }


        }
    }

    class eventLog
    {
        private static eventLog log = null;
        private static bool logFile = false;
        
        public static eventLog getLog()
        {
           
            if (log == null)
            {
                log = new eventLog();
            }
            return log;
        }
        public void addLog(string _date,string _time, string _patient, string _action)
        {
             if (!logFile)
             {
              File.WriteAllText("log.csv", "");
              logFile = true;
             }
            System.IO.StreamWriter file = new System.IO.StreamWriter("log.csv",true);
            file.WriteLine(_date+ ";"+_time + ";"+_patient + ";"+_action);//write to file
                 file.Close();
           }
        
       
        
        public List<string> toList()
        {
            List<string> log = new List<string>();
            if (logFile)//checking if file exists
            {
                //Open file Stream
                System.IO.StreamReader file = new System.IO.StreamReader("log.csv");
                string support;
                while ((support = file.ReadLine()) != null)//while not end of file
                {
                    if (support.Length != 0)//checking if file is not empty, only for important for the first iteration
                    {
                        log.Add(support.Replace(";", "\t"));
                    }
                    else
                    {
                        throw new Exception("File empty");
                    }

                }


                file.Close();
            }
            else
            {
                throw new Exception("File not found");
            }

            return log;


        }

    }
    

}

