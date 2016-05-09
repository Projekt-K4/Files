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
        private static string eventLogText = "";

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
        public delegate void number_triage_class_delegate(int i);
        public number_triage_class_delegate my_number_triage_class_delegate;
        public delegate void number_triage_class2_delegate();
        public delegate void number_triage_class3_delegate();
        public delegate void number_triage_class4_delegate();
        public delegate void add_eventLog_text_delegate(int i);
        public add_eventLog_text_delegate my_add_eventLog_text_delegate;


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
                    Invoke(my_add_eventLog_text_delegate, new Object[] { 0 });
                }
                if (i==1)
                {
                    Invoke(new patient_waiting_delegate(patient_waiting));
                    Invoke(my_add_eventLog_text_delegate, new Object[] { i });
                }
                else if (i==2)
                {
                    PatientTriage.Invoke(new triage_delegate(triage));
                    Invoke(my_add_eventLog_text_delegate, new Object[] { i });
                }
                else if (s.StartsWith("3"))
                {
                    int j = i - 30;
                    Invoke(my_triage_number_delegate, new Object[] { j });
                    Invoke(my_add_eventLog_text_delegate, new Object[] { i});
                }
                else if (s.StartsWith("4"))
                {
                    int j = i - 40;
                    MessageBox.Show("OP");
                    Invoke(my_add_eventLog_text_delegate, new Object[] { i });
                }
                else if (s.StartsWith("5"))
                {
                    int j = i - 50;
                    MessageBox.Show("Bettenstation");
                    Invoke(my_add_eventLog_text_delegate, new Object[] {5 });
                }
                else if (s.Equals("6"))
                {
                    int j = i - 60;
                    MessageBox.Show("Kirche");
                    Invoke(my_add_eventLog_text_delegate, new Object[] { 6 });
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

        private void add_eventLog_text(int i)
        {
            string s = i.ToString();
            if (i == 0)
            {
                EventLogFeld.Text = "???.\n" + eventLogText;
            }else if (i == 1)
            {
                EventLogFeld.Text = "Patient wartet vor Triage.\n" + eventLogText;
            }
            else if (i == 2)
            {
                EventLogFeld.Text = "Patient wird triagiert.\n" + eventLogText;
            }
            else if (s.StartsWith("3"))
            {
                int j = i - 30;
                EventLogFeld.Text = "Patient bekommt Triagenummer " + j + ".\n" + eventLogText;
            }
            else if (s.StartsWith("4"))
            {
                int j = i - 40;
                EventLogFeld.Text = "Patient wird in OP" + j + " operiert.\n" + eventLogText;
            }
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

        private void number_triage_class(int i)
        {
            if (i == 1)
            {
                class1.Text = "Klasse 1\nLeichtverletzte: " + LV;
            }
            else if (i == 2)
            {
                class2.Text = "Klasse 2\nSchwerverletzte: " + SV;
            }
            else if (i == 3)
            {
                class3.Text = "Klasse 3\nHoffnungslose: " + H;
            }
            else if (i == 4)
            {
                class4.Text = "Klasse 4\nTote: " + T;
            }
            else
            {
                throw new Exception("Error in triage class text!");
            }
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
            }
            else if (i == 2)
            {
                triage_number_sv();
            }
            else if (i == 3)
            {
                triage_number_h();
            }
            else if (i == 4)
            {
                triage_number_t();
            }
            else
            {
                throw new Exception("Triagenumber doesn'T exist!");
            }

            Invoke(my_number_triage_class_delegate, new Object[] { i });

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
     
    }
    
}
