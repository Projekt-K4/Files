﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K4_Visualisierung
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UKH());
        }
    }

    class stat_file
    {
        public static Dictionary<string, string> file_to_dictionary(string _file)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();//temporary dictionary
            if (new FileInfo(_file).Exists)//checking if file exists
            {
                //Open file Stream
                System.IO.StreamReader file = new System.IO.StreamReader(_file);

                string support;

                while ((support = file.ReadLine()) != null)//while not end of file
                {
                    if (support.Length != 0)//checking if file is not empty, only for important for the first iteration
                    {
                        var pair = support.Split(';');//splitt value pair
                        dict.Add(pair[0], pair[1]);//add pair to dictionary
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
            return dict;
        }
    }

}
