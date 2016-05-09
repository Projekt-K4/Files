using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K4_Projekt
{
    public partial class UKH : Form
    {

        private static int PW=0;
        private static int LV=0;
        private static int SV=0;
        private static int H=0;
        private static int T=0;

        public UKH()
        {
            InitializeComponent();

        }

        public void UKH_Load(object sender, EventArgs e)
        {
            var t = new Thread(new ThreadStart(read_puffer));
            t.Start();
        }

        public delegate void patient_waiting_delegate();
        public delegate void triage_delegate();
        public delegate void triage_number_delegate(int i);
        public triage_number_delegate my_triage_number_delegate;
        public delegate void triage_number_lv_delegate();
        public delegate void number_waiting_delegate();
        public delegate void number_triage_class1_delegate();
        public delegate void number_triage_class2_delegate();
        public delegate void number_triage_class3_delegate();
        public delegate void number_triage_class4_delegate();

        public void read_puffer()
        {
            eventLog.getLog().fromFileToList("file.csv");
            while (eventLog.puffer.Count > 0)
            {
                my_triage_number_delegate = new triage_number_delegate(triage_number);
                int i = Int32.Parse(eventLog.puffer.ElementAt(0));
                string s = i.ToString();
                Thread.Sleep(1000);
                if (PatientTriage.Visible == true)
                {
                    Invoke(new triage_delegate(triage));
                }
      
                if (i==1)
                {
                    Invoke(new patient_waiting_delegate(patient_waiting));
                }
                else if (i==2)
                {
                    PatientTriage.Invoke(new triage_delegate(triage));
                }
                else if (s.StartsWith("3"))
                {
                    int j = i - 30;
                    Invoke(my_triage_number_delegate, new Object[] { j });
                }
                else if (s.StartsWith("4"))
                {
                    int j = i - 40;
                    MessageBox.Show("OP");
                }
                else if (s.StartsWith("5"))
                {
                    int j = i - 50;
                    MessageBox.Show("Bettenstation");
                }
                else if (s.Equals("6"))
                {
                    MessageBox.Show("Kirche");
                }
                else
                {
                    throw new Exception("Event doesn't exist!");
                }
                eventLog.puffer.RemoveAt(0);
            }
        }
       
        private void patient_waiting()
        {
            if (Patient1.Visible == false)
            {
                Patient1.Visible = true;
                ++PW;
            }
            else if (Patient1.Visible == true && Patient2.Visible == false)
            {
                Patient2.Visible = true;
                ++PW;
            }
            else if (Patient2.Visible == true && Patient3.Visible == false)
            {
                Patient3.Visible = true;
                ++PW;
            }
            else if (Patient3.Visible == true && Patient4.Visible == false)
            {
                Patient4.Visible = true;
                ++PW;
            }
            else if (Patient4.Visible == true && Patient5.Visible == false)
            {
                Patient5.Visible = true;
                ++PW;
            }
            else if (Patient5.Visible == true && Patient6.Visible == false)
            {
                Patient6.Visible = true;
                ++PW;
            }
            else if (Patient5.Visible == true && Patient6.Visible == true)
            {
                ++PW;
            }
            else
            {
                throw new Exception("Error in patient waiting queue!");
            }

            Invoke(new number_waiting_delegate(number_waiting));
        }

        private void number_waiting()
        {
            if (PW == 1)
            {
                Queue.Text = PW + " Patient wartet";
            }
            else if (PW ==0 || PW>1)
            {
                Queue.Text = PW + " Patienten warten";
            }
            else
            {
                throw new Exception("Error in number waiting!");
            }
        }

        private void number_triage_class1()
        {
            class1.Text = "Klasse 1\nLeichtverletzte: " +LV;
        }

        private void number_triage_class2()
        {
            class2.Text = "Klasse 2\nSchwerverletzte: " + SV;
        }

        private void number_triage_class3()
        {
            class3.Text = "Klasse 3\nHoffnungslose: " + H;
        }

        private void number_triage_class4()
        {
            class4.Text = "Klasse 4\nTote: " + T;
        }

        private void triage()
        {
            if (PatientTriage.Visible == false)
            {
                PatientTriage.Visible = true;
                if (PW == 6)
                {
                    Patient6.Visible = false;
                    --PW;
                }
                else if (PW == 5)
                {
                    Patient5.Visible = false;
                    --PW;
                }
                else if (PW == 4)
                {
                    Patient4.Visible = false;
                    --PW;
                }
                else if (PW == 3)
                {
                    Patient3.Visible = false;
                    --PW;
                }
                else if (PW == 2)
                {
                    Patient2.Visible = false;
                    --PW;
                }
                else if (PW == 1)
                {
                    Patient1.Visible = false;
                    --PW;
                }
                else
                {
                    --PW;
                }
            }
            else
            {
                PatientTriage.Visible = false;
            }

            Invoke(new number_waiting_delegate(number_waiting));

        }

        private void triage_number(int i)
        {
            if (i == 1)
            {
                triage_number_lv();
                Invoke(new number_triage_class1_delegate(number_triage_class1));
            }
            else if (i == 2)
            {
                triage_number_sv();
                Invoke(new number_triage_class1_delegate(number_triage_class2));
            }
            else if (i == 3)
            {
                triage_number_h();
                Invoke(new number_triage_class1_delegate(number_triage_class3));
            }
            else if (i == 4)
            {
                triage_number_t();
                Invoke(new number_triage_class1_delegate(number_triage_class4));
            }
            else
            {
                throw new Exception("Triagenumber doesn'T exist!");
            }

        }

        private void triage_number_lv()
        {
            if (LV == 0)
            {
                p_lv1.Visible = true;
                ++LV;
            }
            else if (LV == 1)
            {
                p_lv2.Visible = true;
                ++LV;
            }
            else if (LV == 2)
            {
                p_lv3.Visible = true;
                ++LV;
            }
            else if (LV == 3)
            {
                p_lv4.Visible = true;
                ++LV;
            }
            else if (LV == 4)
            {
                p_lv5.Visible = true;
                ++LV;
            }
            else if (LV == 5)
            {
                p_lv6.Visible = true;
                ++LV;
            }
            else if (LV >= 6)
            {
                ++LV;
            }
            else
            {
                throw new Exception("Error at the LV triage!");
            }
        }

        private void triage_number_sv()
        {
            if (SV == 0)
            {
                p_sv1.Visible = true;
                ++SV;
            }
            else if (SV == 1)
            {
                p_sv2.Visible = true;
                ++SV;
            }
            else if (SV == 2)
            {
                p_sv3.Visible = true;
                ++SV;
            }
            else if (SV == 3)
            {
                p_sv4.Visible = true;
                ++SV;
            }
            else if (SV == 4)
            {
                p_sv5.Visible = true;
                ++SV;
            }
            else if (SV == 5)
            {
                p_sv6.Visible = true;
                ++SV;
            }
            else if (SV >= 6)
            {
                ++SV;
            }
            else
            {
                throw new Exception("Error at the SV triage!");
            }
        }

        private void triage_number_h()
        {
            if (H == 0)
            {
                p_h1.Visible = true;
                ++H;
            }
            else if (H == 1)
            {
                p_h2.Visible = true;
                ++H;
            }
            else if (H == 2)
            {
                p_h3.Visible = true;
                ++H;
            }
            else if (H == 3)
            {
                p_h4.Visible = true;
                ++H;
            }
            else if (SV == 4)
            {
                p_h5.Visible = true;
                ++SV;
            }
            else if (H == 5)
            {
                p_h6.Visible = true;
                ++H;
            }
            else if (H >= 6)
            {
                ++H;
            }
            else
            {
                throw new Exception("Error at the H triage!");
            }
        }

        private void triage_number_t()
        {
            if (T == 0)
            {
                p_t1.Visible = true;
                ++T;
            }
            else if (T == 1)
            {
                p_t2.Visible = true;
                ++T;
            }
            else if (T == 2)
            {
                p_t3.Visible = true;
                ++T;
            }
            else if (T == 3)
            {
                p_t4.Visible = true;
                ++SV;
            }
            else if (T == 4)
            {
                p_t5.Visible = true;
                ++T;
            }
            else if (T == 5)
            {
                p_t6.Visible = true;
                ++T;
            }
            else if (T >= 6)
            {
                ++T;
            }
            else
            {
                throw new Exception("Error at the T triage!");
            }
        }


        /*
        private int create_Queue()
        {
            Dictionary<string, string> test = stat_file.file_to_dictionary("test.csv");
            int i = 0;
            foreach (KeyValuePair<string, string> pair in test)
            {
                i += Int32.Parse(pair.Value);
                //Console.WriteLine(pair.Key + "\t" + pair.Value);
            }
            return i;
        }
        
        public int write_Queue()
        {
            int queue_length = create_Queue();
            Queue.Text = ("Wartende Patienten: " + queue_length.ToString());
            visualize_patient_queue(queue_length);
            return queue_length;
        }

        public void triage(int i)
        {
            int c1 = 0;
            int c2 = 0;
            int c3 = 0;
            int c4 = 0;

            Random random = new Random();
            
            while (i != 0)
            {
                int triage_number = random.Next(1, 4);

                if (triage_number == 1)
                {
                    ++c1;
                    visualize_patient_triage(1, c1);
                }
                else if (triage_number == 2)
                {
                    ++c2;
                    visualize_patient_triage(2, c2);
                }
                else if (triage_number == 3)
                {
                    ++c3;
                    visualize_patient_triage(3, c3);
                }
                else if (triage_number == 4)
                {
                    ++c4;
                    visualize_patient_triage(4, c4);
                }

                class1.Text = ("Klasse 1\nLeichtverletzte: " + c1.ToString());

                class2.Text = ("Klasse 2\nSchwerverletzte: " + c2.ToString());

                class3.Text = ("Klasse 3\nHoffnungslose: " + c3.ToString());

                class4.Text = ("Klasse 4\nTote: " + c4.ToString());

                --i;

                Queue.Text = ("Wartende Patienten: " + i.ToString());


            }
        }

    */



        private void visualize_patient_triage(int triage_class, int patient_number)
        {
            if (triage_class == 1)
            {
                if (patient_number == 0)
                {
                    p_lv1.Visible = false;
                    p_lv2.Visible = false;
                    p_lv3.Visible = false;
                    p_lv4.Visible = false;
                    p_lv5.Visible = false;
                    p_lv6.Visible = false;
                }
                else if (patient_number == 1)
                {
                    p_lv1.Visible = true;
                }
                else if (patient_number == 2)
                {
                    p_lv1.Visible = true;
                    p_lv2.Visible = true;
                }
                else if (patient_number == 3)
                {
                    p_lv1.Visible = true;
                    p_lv2.Visible = true;
                    p_lv3.Visible = true;
                }
                else if (patient_number == 4)
                {
                    p_lv1.Visible = true;
                    p_lv2.Visible = true;
                    p_lv3.Visible = true;
                    p_lv4.Visible = true;
                }
                else if (patient_number == 5)
                {
                    p_lv1.Visible = true;
                    p_lv2.Visible = true;
                    p_lv3.Visible = true;
                    p_lv4.Visible = true;
                    p_lv5.Visible = true;
                }
                else
                {
                    p_lv1.Visible = true;
                    p_lv2.Visible = true;
                    p_lv3.Visible = true;
                    p_lv4.Visible = true;
                    p_lv5.Visible = true;
                    p_lv6.Visible = true;
                }
            }
            else if (triage_class == 2)
            {
                if (patient_number == 0)
                {
                    p_sv1.Visible = false;
                    p_sv2.Visible = false;
                    p_sv3.Visible = false;
                    p_sv4.Visible = false;
                    p_sv5.Visible = false;
                    p_sv6.Visible = false;
                }
                else if (patient_number == 1)
                {
                    p_sv1.Visible = true;
                }
                else if (patient_number == 2)
                {
                    p_sv1.Visible = true;
                    p_sv2.Visible = true;
                }
                else if (patient_number == 3)
                {
                    p_sv1.Visible = true;
                    p_sv2.Visible = true;
                    p_sv3.Visible = true;
                }
                else if (patient_number == 4)
                {
                    p_sv1.Visible = true;
                    p_sv2.Visible = true;
                    p_sv3.Visible = true;
                    p_sv4.Visible = true;
                }
                else if (patient_number == 5)
                {
                    p_sv1.Visible = true;
                    p_sv2.Visible = true;
                    p_sv3.Visible = true;
                    p_sv4.Visible = true;
                    p_sv5.Visible = true;
                }
                else
                {
                    p_sv1.Visible = true;
                    p_sv2.Visible = true;
                    p_sv3.Visible = true;
                    p_sv4.Visible = true;
                    p_sv5.Visible = true;
                    p_sv6.Visible = true;
                }
            }
            else if (triage_class == 3)
            {
                if (patient_number == 0)
                {
                    p_h1.Visible = false;
                    p_h2.Visible = false;
                    p_h3.Visible = false;
                    p_h4.Visible = false;
                    p_h5.Visible = false;
                    p_h6.Visible = false;
                }
                else if (patient_number == 1)
                {
                    p_h1.Visible = true;
                }
                else if (patient_number == 2)
                {
                    p_h1.Visible = true;
                    p_h2.Visible = true;
                }
                else if (patient_number == 3)
                {
                    p_h1.Visible = true;
                    p_h2.Visible = true;
                    p_h3.Visible = true;
                }
                else if (patient_number == 4)
                {
                    p_h1.Visible = true;
                    p_h2.Visible = true;
                    p_h3.Visible = true;
                    p_h4.Visible = true;
                }
                else if (patient_number == 5)
                {
                    p_h1.Visible = true;
                    p_h2.Visible = true;
                    p_h3.Visible = true;
                    p_h4.Visible = true;
                    p_h5.Visible = true;
                }
                else
                {
                    p_h1.Visible = true;
                    p_h2.Visible = true;
                    p_h3.Visible = true;
                    p_h4.Visible = true;
                    p_h5.Visible = true;
                    p_h6.Visible = true;
                }
            }
            else if (triage_class == 4)
            {
                if (patient_number == 0)
                {
                    p_t1.Visible = false;
                    p_t2.Visible = false;
                    p_t3.Visible = false;
                    p_t4.Visible = false;
                    p_t5.Visible = false;
                    p_t6.Visible = false;
                }
                else if (patient_number == 1)
                {
                    p_t1.Visible = true;
                }
                else if (patient_number == 2)
                {
                    p_t1.Visible = true;
                    p_t2.Visible = true;
                }
                else if (patient_number == 3)
                {
                    p_t1.Visible = true;
                    p_t2.Visible = true;
                    p_t3.Visible = true;
                }
                else if (patient_number == 4)
                {
                    p_t1.Visible = true;
                    p_t2.Visible = true;
                    p_t3.Visible = true;
                    p_t4.Visible = true;
                }
                else if (patient_number == 5)
                {
                    p_t1.Visible = true;
                    p_t2.Visible = true;
                    p_t3.Visible = true;
                    p_t4.Visible = true;
                    p_t5.Visible = true;
                }
                else
                {
                    p_t1.Visible = true;
                    p_t2.Visible = true;
                    p_t3.Visible = true;
                    p_t4.Visible = true;
                    p_t5.Visible = true;
                    p_t6.Visible = true;
                }
            }
        }

     
    }
    
}
