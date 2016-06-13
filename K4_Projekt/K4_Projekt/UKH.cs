using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace K4_Projekt {
    public partial class UKH : Form {

        //Waiting
        private static int PW = 0; //waiting for triage
        private static int QueueOPRoom = 0;


        //triageKlasse
        private static int LV = 0;
        private static int SV = 0;
        private static int H = 0;
        private static int T = 0;
        private static int LVWaiting = 0;
        private static int AmountOfPatients = 0;

        //end stations
        private static int ChurchCount = 0;
        private static int MortuaryCount = 0;
        private static int StationCount = 0;

        //Time
        DateTime startTime = DateTime.ParseExact("00:32:00", "hh:mm:ss", new CultureInfo("de-DE"));
        DateTime timeVisi = DateTime.ParseExact("00:32:00", "hh:mm:ss", new CultureInfo("de-DE"));
        private static int speed = 10;

        //eventlog
        private static int patientWaitingEventLogNumber = 0;
        private static string KID;

        public UKH() {
            InitializeComponent();
        }

        public void UKH_Load(object sender, EventArgs e) {
            Thread triageThread = new Thread(new ThreadStart(read_puffer));
            triageThread.Start();

            timer.Interval = 1000/speed;
            timer.Tick += new EventHandler(this.timer_Tick);
            timer.Start();
        }

        public delegate void patient_waiting_delegate();
        public delegate void triage_delegate();
        public delegate void triage_number_delegate(int i);
        public triage_number_delegate my_triage_number_delegate;
        public delegate void number_waiting_delegate();
        public delegate void number_triage_class_delegate(int i);
        public number_triage_class_delegate my_number_triage_class_delegate;
        public delegate void operate_delegate(int i);
        public operate_delegate my_operate_delegate;
        public delegate void Bettenstation_delegate();
        public delegate void Church_delegate();
        public delegate void Mortuary_delegate();
        public delegate void add_eventLog_text_delegate(int i);
        public add_eventLog_text_delegate my_add_eventLog_text_delegate;
        public delegate void patientInWaitingarea_delegate();
        public delegate void aliveAfterOP_delegate(int i);
        public add_eventLog_text_delegate my_aliveAfterOP_delegate;
        public delegate void patientWaitingForOP_delegate();
        public delegate void diedInOP_delegate(int i);
        public diedInOP_delegate my_diedInOP_delegate;
        public delegate void updateAmountOfPatients_delegate();

        
            //DateTime time = DateTime.ParseExact("00:08:00", "hh:mm:ss", new CultureInfo("de-DE"));
            //while (true) {
            //    Thread.Sleep(1000/speed );
            //    timer.AddSeconds(1);
            //    if (InvokeRequired) {
            //        Invoke((MethodInvoker)delegate { textBoxTimer.Text = timer.Now.ToString("hh:mm:ss"); });
            //    } else {
            //        textBoxTimer.Text = timer.ToString("hh:mm:ss");
            //    }
            //}
        private void timer_Tick(object sender, EventArgs e) {

            
            timeVisi.AddSeconds(1);
            int hh = timeVisi.Hour;
            int mm = timeVisi.Minute;
            int ss = timeVisi.Second;

            //time
            string time = "";

            //padding leading zero
            if (hh < 10) {
                time += "0" + hh;
            } else {
                time += hh;
            }
            time += ":";

            if (mm < 10) {
                time += "0" + mm;
            } else {
                time += mm;
            }
            time += ":";

            if (ss < 10) {
                time += "0" + ss;
            } else {
                time += ss;
            }

            //update label
            if (InvokeRequired) {
                Invoke((MethodInvoker)delegate { textBoxTimer.Text = time; });
            } else {
                textBoxTimer.Text = time; 
            }
            
        }
    


        public void read_puffer() {
            eventLog.getLog().fromFileToList("file.csv");
            for (int e = 0; e < eventLog.eventList.Count; ++e) {
                KID = eventLog.KIDList.ElementAt(e);
                my_triage_number_delegate = new triage_number_delegate(triage_number);
                int i = 0;
                i = Int32.Parse(eventLog.eventList.ElementAt(e));

                string s = i.ToString();
                DateTime oldTime = DateTime.ParseExact(eventLog.timeStampList.ElementAt(e), "hh:mm:ss", new CultureInfo("de-DE"));
                TimeSpan difference = oldTime - startTime;
                int duration = difference.Hours * 60 * 60 * 1000 + difference.Minutes * 60 * 1000 + difference.Seconds * 1000;
                if (duration == 0) {
                    duration = 1000;
                }
                Thread.Sleep(duration / speed);


                if (i == 1) {
                    if (InvokeRequired) {
                        Invoke(new patient_waiting_delegate(patient_waiting));
                    } else {
                        patient_waiting();
                    }

                } else if (i == 2) {
                    if (InvokeRequired) {
                        Invoke(new triage_delegate(triage));
                    } else {
                        triage();
                    }
                } else if (s.StartsWith("3")) {
                    int j = i - 30;
                    if (InvokeRequired) {
                        Invoke(my_triage_number_delegate, new Object[] { j });
                    } else {
                        triage_number(j);
                    }
                } else if (s.StartsWith("4")) {

                    int j = i - 40;
                    operate(j);

                } else if (s.StartsWith("5")) {

                    int j = i - 50;
                    diedInOP(j);


                } else if (s.StartsWith("6")) {

                    int j = i - 60;
                    aliveAfterOP(j);

                } else if (s.StartsWith("7")) //values from 711 to 774
                {
                    int j = i - 700;

                    int staff = (s.ElementAt(1)) - '0';
                    int OP = (s.ElementAt(2)) - '0';

                    get_personalOP(staff, OP);
                } else if (s.StartsWith("8")) {
                    Console.Write("Code not existing");
                } else if (s.StartsWith("9")) //wird weggeschickt?? lt. EventCodierung auf Straße
                  {
                    if (InvokeRequired) {
                        Invoke(new patientInWaitingarea_delegate(patientInWaitingarea));
                    } else {
                        patientInWaitingarea();
                    }
                    
                } else if (s.StartsWith("10"))
                  //if was classified as "hoffnungslos" the patient is transported into church
                  {
                    int j = i - 100;
                    SettleToChurch(j);
                } else if (s.StartsWith("11"))
                  //patient dead and comes in Mortuary when died somewhere or was classified as dead
                  {
                    int j = i - 110;
                    DiedAt(j);
                } else if (s.StartsWith("12")) {
                    if (InvokeRequired) {
                        Invoke(new patientInWaitingarea_delegate(patientWaitingForOP));
                    } else {
                        patientWaitingForOP();
                    }

                } else {
                    throw new Exception("Event doesn't exist!");
                }
                //AmountOfPatients aktuaisieren
                if (InvokeRequired) {
                    Invoke(new updateAmountOfPatients_delegate(updateAmountOfPatients));
                } else {
                    updateAmountOfPatients();
                }
                startTime = oldTime;
            }
            MessageBox.Show("FERTIG!");
        }


        private void updateAmountOfPatients() {
            AmountOfPatients = T + H + SV + LV;
            labelPatientAmount.Text = ("Patienten \ninsgesamt: " + AmountOfPatients.ToString());
        }


        private void patient_waiting() {
            ++PW;
            if (PW == 1) {
                Patient1.Visible = true;
            } else if (PW == 2) {
                Patient2.Visible = true;
            } else if (PW == 3) {
                Patient3.Visible = true;
            } else if (PW == 4) {
                Patient4.Visible = true;
            } else if (PW == 5) {
                Patient5.Visible = true;
            } else if (PW >= 6) {
                Patient6.Visible = true;
            } else {
                throw new Exception("Error in patient waiting queue!");
            }
            number_waiting();
            add_eventLog_text(1);
        }

        private void add_eventLog_text(int i) {
            if (i == 1) {
                ++patientWaitingEventLogNumber;
                textBox_eventlog.AppendText("undefined\t\t" + patientWaitingEventLogNumber + ". Patient wartet vor Triage\n");
            } else if (i == 2) {
                textBox_eventlog.AppendText(KID + "\t\tPatient wird triagiert\n");
            } else if (i == 9) {
                textBox_eventlog.AppendText(KID + "\tPatient wird in Wartebereich geschickt\n");
            } else if (i == 10) {
                textBox_eventlog.AppendText(KID + "\tPatient wird in Kirche verlegt\n");
            } else if (i == 11) {
                textBox_eventlog.AppendText(KID + "\tPatient kommt in Leichenhalle\n");
            } else if (i == 12) {
                textBox_eventlog.AppendText(KID + "\tPatient wartet auf OP\n");
            }
        }


        private void add_eventLog_text(int i, int j) {
            if (i == 3) {
                if (InvokeRequired) {
                    Invoke((MethodInvoker)delegate { textBox_eventlog.AppendText(KID + "\tPatient bekommt Triagenummer " + j + "\n"); });
                } else {
                    textBox_eventlog.AppendText(KID + "\tPatient bekommt Triagenummer " + j + "\n");
                }
            } else if (i == 4) {
                if (InvokeRequired) {
                    Invoke((MethodInvoker)delegate { textBox_eventlog.AppendText(KID + "\tPatient wird in OP" + j + " operiert\n"); });
                } else {
                    textBox_eventlog.AppendText(KID + "\tPatient wird in OP" + j + " operiert\n");
                }
            } else if (i == 5) {
                if (InvokeRequired) {
                    Invoke((MethodInvoker)delegate { textBox_eventlog.AppendText(KID + "\tPatient ist in OP" + j + " verstorben\n"); });
                } else {
                    textBox_eventlog.AppendText(KID + "\tPatient ist in OP" + j + " verstorben\n");
                }
            } else if (i == 6) {
                if (InvokeRequired) {
                    Invoke((MethodInvoker)delegate { textBox_eventlog.AppendText(KID + "\tPatient aus OP" + j + " auf die Bettenstation verlegt\n"); });
                } else {
                    textBox_eventlog.AppendText(KID + "\tPatient aus OP" + j + " auf die Bettenstation verlegt\n");
                }
            } else if (i == 11) {
                if (j == 1) {
                    if (InvokeRequired) {
                        Invoke((MethodInvoker)delegate { textBox_eventlog.AppendText(KID + "\tPatient ist auf der Bettenstation verstorben\n"); });
                    } else {
                        textBox_eventlog.AppendText(KID + "\tPatient ist auf der Bettenstation verstorben\n");
                    }
                } else if (j == 2) {
                    if (InvokeRequired) {
                        Invoke((MethodInvoker)delegate { textBox_eventlog.AppendText(KID + "\tPatient ist in der Kirche verstorben\n"); });
                    } else {
                        textBox_eventlog.AppendText(KID + "\tPatient ist in der Kirche verstorben\n");
                    }
                }
            }
        }

        private void number_waiting() {
            if (PW == 1) {
                Queue.Text = PW + " Patient wartet";             
            } else if (PW == 0 || PW > 1) {
               Queue.Text = PW + " Patienten warten";
            } else {
                throw new Exception("Error in number waiting!");
            }
        }

        private void triage() {
            --PW;
            PatientTriage.Visible = true;
            if (PW == 5) {
                Patient6.Visible = false;
            } else if (PW == 4) {
                Patient5.Visible = false;
            } else if (PW == 3) {
                Patient4.Visible = false;
            } else if (PW == 2) {
                Patient3.Visible = false;
            }else if (PW == 1) {
                Patient2.Visible = false;
            } else if (PW == 0) {
                Patient1.Visible = false;
            }
            number_waiting();
            add_eventLog_text(2);
        }

        private void triage_number(int i) {
            PatientTriage.Visible = false;
            if (i == 1) {
                //  triage_number_lv();
                ++LV;
            } else if (i == 2) {
                // triage_number_sv();
                ++SV;
            } else if (i == 3) {
                //  triage_number_h();
                ++H;
            } else if (i == 4) {
                //  triage_number_t();
                ++T;
            } else {
                throw new Exception("Triagenumber doesn't exist!");
            }
            add_eventLog_text(3, i);
            number_triage_class(i);
        }

        private void number_triage_class(int i) {
            if (i == 1) {
                class1.Text = "Klasse 1\nLeichtverletzte: " + LV;
            } else if (i == 2) {
                class2.Text = "Klasse 2\nSchwerverletzte: " + SV;
            } else if (i == 3) {
                class3.Text = "Klasse 3\nHoffnungslose: " + H;
            } else if (i == 4) {
                class4.Text = "Klasse 4\nTote: " + T;
            } else {
                throw new Exception("Error in triage class text!");
            }
        }
        /*
        private void triage_number_lv() {
            if (LV == 0) {
                p_lv1.Visible = true;
                ++LV;
            } else if (LV == 1) {
                p_lv2.Visible = true;
                ++LV;
            } else if (LV == 2) {
                p_lv3.Visible = true;
                ++LV;
            } else if (LV == 3) {
                p_lv4.Visible = true;
                ++LV;
            } else if (LV == 4) {
                p_lv5.Visible = true;
                ++LV;
            } else if (LV == 5) {
                p_lv6.Visible = true;
                ++LV;
            } else if (LV >= 6) {
                ++LV;
            } else {
                throw new Exception("Error at the LV triage!");
            }
        }

        private void triage_number_sv() {
            if (SV == 0) {
                p_sv1.Visible = true;
                ++SV;
            } else if (SV == 1) {
                p_sv2.Visible = true;
                ++SV;
            } else if (SV == 2) {
                p_sv3.Visible = true;
                ++SV;
            } else if (SV == 3) {
                p_sv4.Visible = true;
                ++SV;
            } else if (SV == 4) {
                p_sv5.Visible = true;
                ++SV;
            } else if (SV == 5) {
                p_sv6.Visible = true;
                ++SV;
            } else if (SV >= 6) {
                ++SV;
            } else {
                throw new Exception("Error at the SV triage!");
            }
        }

        private void triage_number_h() {
            if (H == 0) {
                p_h1.Visible = true;
                ++H;
            } else if (H == 1) {
                p_h2.Visible = true;
                ++H;
            } else if (H == 2) {
                p_h3.Visible = true;
                ++H;
            } else if (H == 3) {
                p_h4.Visible = true;
                ++H;
            } else if (SV == 4) {
                p_h5.Visible = true;
                ++H;
            } else if (H == 5) {
                p_h6.Visible = true;
                ++H;
            } else if (H >= 6) {
                ++H;
            } else {
                throw new Exception("Error at the H triage!");
            }

        }

        private void triage_number_t() {
            if (T == 0) {
                p_t1.Visible = true;
                ++T;
            } else if (T == 1) {
                p_t2.Visible = true;
                ++T;
            } else if (T == 2) {
                p_t3.Visible = true;
                ++T;
            } else if (T == 3) {
                p_t4.Visible = true;
                ++T;
            } else if (T == 4) {
                p_t5.Visible = true;
                ++T;
            } else if (T == 5) {
                p_t6.Visible = true;
                ++T;
            } else if (T >= 6) {
                ++T;
            } else {
                throw new Exception("Error at the T triage!");
            }
        }

    */



        //OP - Functions

        //OP background

        //Event Code 4
        public void operate(int OPRoom) {
            switch (OPRoom) {
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
            add_eventLog_text(4, OPRoom);
        }


        //Event Code 5
        public void diedInOP(int OPRoom) {
            switch (OPRoom) {
                case 1:
                    OP1.BackColor = Color.Green;
                    T++;
                    break;
                case 2:
                    OP2.BackColor = Color.Green;
                    T++;
                    break;
                case 3:
                    OP3.BackColor = Color.Green;
                    T++;
                    break;
                case 4:
                    OP4.BackColor = Color.Green;
                    T++;
                    break;
                default: break;
            }
            add_eventLog_text(5, OPRoom);
        }

        //Event Code 6
        private void aliveAfterOP(int OPRoom)
        {

            switch (OPRoom) {
                case 1:
                    OP1.BackColor = Color.Green;
                    StationCount++;
                    if (InvokeRequired) {
                        Invoke(new Bettenstation_delegate(Bettenstation));
                        // Bettenstation_delegate d = new Bettenstation_delegate(Bettenstation);
                        //this.Invoke(d);
                    } else {
                        Bettenstation();
                    }
                    //Bettenstation()
                    break;
                case 2:
                    OP2.BackColor = Color.Green;
                    StationCount++;
                    if (InvokeRequired) {
                        Invoke(new Bettenstation_delegate(Bettenstation));
                    } else {
                        Bettenstation();
                    }
                    break;
                case 3:
                    OP3.BackColor = Color.Green;
                    StationCount++;
                    if (InvokeRequired) {
                        Invoke(new Bettenstation_delegate(Bettenstation));
                    } else {
                        Bettenstation();
                    }
                    break;
                case 4:
                    OP4.BackColor = Color.Green;
                    StationCount++;
                    if (InvokeRequired) {
                        Invoke(new Bettenstation_delegate(Bettenstation));
                    } else {
                        Bettenstation();
                    }
                    break;
                default: break;
            }
            add_eventLog_text(6, OPRoom);
        }

        //event code 7 XX
        //get personal
        public void get_personalOP(int personalCode, int OP)
        {
            switch (personalCode)
            {
                case 1: staffOPC(OP); break;
                case 2: staffOPS1(OP); break;
                case 3: staffOPS2(OP); break;
                case 4: staffOPB(OP); break;
                case 5: staffAnä(OP); break;
                case 6: staffAnäS(OP); break;
                case 7: staffRTA(OP); break;
                default: break;
            }
        }

        //OP staff 
        //Event Code 7 1 X
        //surgeon
        public void staffOPC(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPOPC1Label.BackColor = Color.LimeGreen;
                    break;
                case 2:
                    OPOPC2Label.BackColor = Color.LimeGreen;
                    break;
                case 3:
                    OPOPC3Label.BackColor = Color.LimeGreen;
                    break;
                case 4:
                    OPOPC4Label.BackColor = Color.LimeGreen;
                    break;
                default: break;
            }

            //check_staff(OPRoom);
            //Checks if needed staff has arrived
            //useless because we get Code 4 if somebody gets operated
        }

        //op schwester1
        //event code 7 2 X
        public void staffOPS1(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPOPS11Label.BackColor = Color.LimeGreen;
                    break;
                case 2:
                    OPOPS21Label.BackColor = Color.LimeGreen;
                    break;
                case 3:
                    OPOPS31Label.BackColor = Color.LimeGreen;
                    break;
                case 4:
                    OPOPS41Label.BackColor = Color.LimeGreen;
                    break;
                default: break;
            }
        }

        //OPS2
        //Event Code 73X
        public void staffOPS2(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPOPS12Label.BackColor = Color.LimeGreen;
                    break;
                case 2:
                    OPOPS22Label.BackColor = Color.LimeGreen;
                    break;
                case 3:
                    OPOPS32Label.BackColor = Color.LimeGreen;
                    break;
                case 4:
                    OPOPS42Label.BackColor = Color.LimeGreen;
                    break;
                default: break;
            }
        }

        //OPB
        //Event Code 74X
        public void staffOPB(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPOPB1Label.BackColor = Color.LimeGreen;
                    break;
                case 2:
                    OPOPB2Label.BackColor = Color.LimeGreen;
                    break;
                case 3:
                    OPOPB3Label.BackColor = Color.LimeGreen;
                    break;
                case 4:
                    OPOPB4Label.BackColor = Color.LimeGreen;
                    break;
                default: break;
            }
        }

        //Anä
        //Event Code 75X
        public void staffAnä(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPAnä1Label.BackColor = Color.LimeGreen;
                    break;
                case 2:
                    OPAnä2Label.BackColor = Color.LimeGreen;
                    break;
                case 3:
                    OPAnä3Label.BackColor = Color.LimeGreen;
                    break;
                case 4:
                    OPAnä4Label.BackColor = Color.LimeGreen;
                    break;
                default: break;
            }
        }

        //AnäS
        //Event Code 76X
        public void staffAnäS(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPAnäS1Label.BackColor = Color.LimeGreen;
                    break;
                case 2:
                    OPAnäS2Label.BackColor = Color.LimeGreen;
                    break;
                case 3:
                    OPAnäS3Label.BackColor = Color.LimeGreen;
                    break;
                case 4:
                    OPAnäS4Label.BackColor = Color.LimeGreen;
                    break;
                default: break;
            }
        }

        //RTA
        //Event Code 77X
        public void staffRTA(int OPRoom)
        {
            switch (OPRoom)
            {
                case 1:
                    OPRTA1Label.BackColor = Color.LimeGreen;
                    break;
                case 2:
                    OPRTA2Label.BackColor = Color.LimeGreen;
                    break;
                case 3:
                    OPRTA3Label.BackColor = Color.LimeGreen;
                    break;
                case 4:
                    OPRTA4Label.BackColor = Color.LimeGreen;
                    break;
                default: break;
            }
        }
        

        //runterzählen implementieren!
        private void Bettenstation()
        {
            labelBS.Text = "Bettenstation: " + StationCount;
            switch (StationCount)
            {
                case 0: break; //just to be safe
                case 1:
                    pictureBoxBS1.Visible = true;
                    break;
                case 2:
                    pictureBoxBS2.Visible = true;
                    break;
                case 3:
                    pictureBoxBS3.Visible = true;
                    break;
                case 4:
                    pictureBoxBS4.Visible = true;
                    break;
                case 5:
                    pictureBoxBS5.Visible = true;
                    break;
                case 6:
                    pictureBoxBS6.Visible = true;
                    break;
                default: break;

            }
        }

        private void Church()
        {
            labelChurch.Text = "Kirche: " + ChurchCount;
            switch (ChurchCount)
            {
                case 0: break; //just to be safe
                case 1:
                    pictureBoxChurch1.Visible = true;
                    break;
                case 2:
                    pictureBoxChurch2.Visible = true;
                    break;
                case 3:
                    pictureBoxChurch3.Visible = true;
                    break;
                case 4:
                    pictureBoxChurch4.Visible = true;
                    break;
                case 5:
                    pictureBoxChurch5.Visible = true;
                    break;
                case 6:
                    pictureBoxChurch6.Visible = true;
                    break;
                default: break;

            }
            add_eventLog_text(10);
        }

        private void Mortuary()
        {
            labelMortuary.Text = "Leichenhalle: " + MortuaryCount;
            switch (MortuaryCount)
            {
                case 0: break; //just to be safe
                case 1:
                    pictureBoxMortuary1.Visible = true;
                    break;
                case 2:
                    pictureBoxMortuary2.Visible = true;
                    break;
                case 3:
                    pictureBoxMortuary3.Visible = true;
                    break;
                case 4:
                    pictureBoxMortuary4.Visible = true;
                    break;
                case 5:
                    pictureBoxMortuary5.Visible = true;
                    break;
                case 6:
                    pictureBoxMortuary6.Visible = true;
                    break;
                default: break;
            }
            add_eventLog_text(11);
        }

        //EventCode 10
        private void SettleToChurch(int from)
        {
            ChurchCount++;
            if (InvokeRequired)
            {
                Invoke(new Church_delegate(Church));
            }
            else
            {
                Church();
            }
            switch (from)
            {
                case 1: StationCount--; break;
                case 2: Console.Write("Not implemented yet."); break;
                default: break;
            }
            add_eventLog_text(10, from);

        }

        //EventCode 11
        private void DiedAt(int from)
        {
            MortuaryCount++;
            if (InvokeRequired)
            {
                Invoke(new Mortuary_delegate(Mortuary));
            }
            else
            {
                Mortuary();
            }
            switch (from)
            {
                case 1: StationCount--;
                    if (InvokeRequired)
                    {
                        Invoke(new Bettenstation_delegate(Bettenstation));
                    }
                    else
                    {
                        Bettenstation(); 
                    }
                    add_eventLog_text(11,from);
                   
                    break;
                case 2: ChurchCount--;
                    if (InvokeRequired)
                    {
                        Invoke(new Church_delegate(Church));
                    }
                    else
                    {
                        Church();
                    }
                    add_eventLog_text(11, from);
                    break;


                default: break;
            }
        }
        //Event Code 12



        private void patientInWaitingarea() {
            LVWaiting++;
            add_eventLog_text(9);
        }

        private void patientWaitingForOP() {
            QueueOPRoom++;
            add_eventLog_text(12);
        }

        private void trackBar1_Scroll(object sender, EventArgs e) {
            speed = trackBarSpeed.Value;
        }
    }
    
}
