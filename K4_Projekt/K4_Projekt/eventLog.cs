using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace K4_Projekt
{
    class eventLog
    {
        private static eventLog log = null;
        private static bool logFile = true;
        public static List<String> puffer;
        public static List<String> timeStampList;
        public static List<String> TTLList;
        public static List<String> KIDList;
        public static List<String> triageNumberList;
        public static List<String> eventList;
        

        public static eventLog getLog()
        {

            if (log == null)
            {
                puffer = new List<String>();
                timeStampList = new List<String>();
                TTLList = new List<String>();
                KIDList = new List<String>();
                triageNumberList = new List<String>();
                eventList = new List<String>();

                log = new eventLog();
            }
            return log;
        }

        public List<string> fromFileToList(String _file)
        {
            List<string> log = new List<string>();
            if (logFile)//checking if file exists
            {
                //Open file Stream
                System.IO.StreamReader file = new System.IO.StreamReader(_file);
                string support;
                while ((support = file.ReadLine()) != null)//while not end of file
                {
                    if (support.Length != 0)//checking if file is not empty, only for important for the first iteration
                    {
                        splitLine(support, ';');
                        puffer.Add(support);
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
        public List<string> fromFileToConsole(String _file)
        {
            List<string> log = new List<string>();
            if (logFile)//checking if file exists
            {
                //Open file Stream
                System.IO.StreamReader file = new System.IO.StreamReader(_file);
                string support;
                while ((support = file.ReadLine()) != null)//while not end of file
                {
                    if (support.Length != 0)//checking if file is not empty, only for important for the first iteration
                    {
                        Console.WriteLine(support);
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

        private void splitLine(string line, char delimiter)
        {
            string[] coloumns = line.Split(';');
            string sysTime = coloumns[0];
            string TTL = coloumns[1];
            string KID = coloumns[2];
            string triageNumber = coloumns[3];
            string eventNumber = coloumns[4];

            timeStampList.Add(sysTime);
            TTLList.Add(TTL);
            KIDList.Add(KID);
            triageNumberList.Add(triageNumber);
            eventList.Add(eventNumber);
        }
    }
}
