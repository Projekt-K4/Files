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
        static int dead = 0;
        static int station = 0;
        //fill this Queue please!!
        static int QueueOPRoom = 0;
       

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


        //OP - Functions

        //OP background

        //Event Code 4
        public void operate(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OP1.BackColor = Color.Red;
                    QueueOPRoom--;
                    break;
                case 2:
                    OP2.BackColor = Color.Red;
                    QueueOPRoom--;
                    break;
                case 3:
                    OP3.BackColor = Color.Red;
                    QueueOPRoom--;
                    break;
                case 4:
                    OP4.BackColor = Color.Red;
                    QueueOPRoom--;
                    break;
                default: break;
            }
        }


        //Event Code 5
        public void diedInOP(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OP1.BackColor = Color.Green;
                    dead++;
                    break;
                case 2:
                    OP2.BackColor = Color.Green;
                    dead++;
                    break;
                case 3:
                    OP3.BackColor = Color.Green;
                    dead++;
                    break;
                case 4:
                    OP4.BackColor = Color.Green;
                    dead++;
                    break;
                default: break;
            }
        }

        //Event Code 6
        private void aliveAfterOP(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OP1.BackColor = Color.Green;
                    station++;
                    break;
                case 2:
                    OP2.BackColor = Color.Green;
                    station++;
                    break;
                case 3:
                    OP3.BackColor = Color.Green;
                    station++;
                    break;
                case 4:
                    OP4.BackColor = Color.Green;
                    station++;
                    break;
                default: break;
            }
        }

        //OP staff
        //Event Code 7
        //surgeon
        public void staffOPC (int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPOPC1Label.BackColor = Color.Green;
                    break;
                case 2:
                    OPOPC2Label.BackColor = Color.Green;
                    break;
                case 3:
                    OPOPC3Label.BackColor = Color.Green;
                    break;
                case 4:
                    OPOPC4Label.BackColor = Color.Green;
                    break;
                default: break;
            }

            //check_staff(OPRoom);
            //Checks if needed staff has arrived
            //useless because we get Code 4 if somebody gets operated
        }

        //op schwester1
        //event code8
        public void staffOPS1(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPOPS11Label.BackColor = Color.Green;
                    break;
                case 2:
                    OPOPS21Label.BackColor = Color.Green;
                    break;
                case 3:
                    OPOPS31Label.BackColor = Color.Green;
                    break;
                case 4:
                    OPOPS41Label.BackColor = Color.Green;
                    break;
                default: break;
            }
        }

        //OPS2
        //Event Code 9
        public void staffOPS2(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPOPS12Label.BackColor = Color.Green;
                    break;
                case 2:
                    OPOPS22Label.BackColor = Color.Green;
                    break;
                case 3:
                    OPOPS32Label.BackColor = Color.Green;
                    break;
                case 4:
                    OPOPS42Label.BackColor = Color.Green;
                    break;
                default: break;
            }
        }

        //OPB
        //Event Code 10
        public void staffOPB(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPOPB1Label.BackColor = Color.Green;
                    break;
                case 2:
                    OPOPB2Label.BackColor = Color.Green;
                    break;
                case 3:
                    OPOPB3Label.BackColor = Color.Green;
                    break;
                case 4:
                    OPOPB4Label.BackColor = Color.Green;
                    break;
                default: break;
            }
        }

        //Anä
        //Event Code 11
        public void staffAnä(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPAnä1Label.BackColor = Color.Green;
                    break;
                case 2:
                    OPAnä2Label.BackColor = Color.Green;
                    break;
                case 3:
                    OPAnä3Label.BackColor = Color.Green;
                    break;
                case 4:
                    OPAnä4Label.BackColor = Color.Green;
                    break;
                default: break;
            }
        }

        //AnäS
        //Event Code 12
        public void staffAnäS(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPAnäS1Label.BackColor = Color.Green;
                    break;
                case 2:
                    OPAnäS2Label.BackColor = Color.Green;
                    break;
                case 3:
                    OPAnäS3Label.BackColor = Color.Green;
                    break;
                case 4:
                    OPAnäS4Label.BackColor = Color.Green;
                    break;
                default: break;
            }
        }

        //RTA
        //Event Code 13
        public void staffRTA(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPRTA1Label.BackColor = Color.Green;
                    break;
                case 2:
                    OPRTA2Label.BackColor = Color.Green;
                    break;
                case 3:
                    OPRTA3Label.BackColor = Color.Green;
                    break;
                case 4:
                    OPRTA4Label.BackColor = Color.Green;
                    break;
                default: break;
            }
        }

    }
    
}
