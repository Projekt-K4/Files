using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K4_Projekt
{
    public partial class UKH : Form
    {
        public UKH()
        {
            InitializeComponent();

        }

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
                visualize_patient_queue(i);

            }
        }




        private void visualize_patient_queue(int patient_number)
        {
            if (patient_number == 0)
            {
                Patient1.Visible = false;
                Patient2.Visible = false;
                Patient3.Visible = false;
                Patient4.Visible = false;
                Patient5.Visible = false;
                Patient6.Visible = false;
            }
            else if (patient_number == 1)
            {
                Patient1.Visible = true;
            }
            else if (patient_number == 2)
            {
                Patient1.Visible = true;
                Patient2.Visible = true;
            }
            else if (patient_number == 3)
            {
                Patient1.Visible = true;
                Patient2.Visible = true;
                Patient3.Visible = true;
            }
            else if (patient_number == 4)
            {
                Patient1.Visible = true;
                Patient2.Visible = true;
                Patient3.Visible = true;
                Patient4.Visible = true;
            }
            else if (patient_number == 5)
            {
                Patient1.Visible = true;
                Patient2.Visible = true;
                Patient3.Visible = true;
                Patient4.Visible = true;
                Patient5.Visible = true;
            }
            else
            {
                Patient1.Visible = true;
                Patient2.Visible = true;
                Patient3.Visible = true;
                Patient4.Visible = true;
                Patient5.Visible = true;
                Patient6.Visible = true;
            }
        }

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
