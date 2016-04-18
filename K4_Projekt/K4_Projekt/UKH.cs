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
            Dictionary<string, string> test = stat_file.file_to_dictionary("test.csv");

            int i = 0;
            foreach (KeyValuePair<string, string> pair in test)
            {
                i+= Int32.Parse(pair.Value);
                //Console.WriteLine(pair.Key + "\t" + pair.Value);
            }
            Queue.Text = ("Wartende Patienten: " + i.ToString());

            triage(i);

        }

        private void triage(int i)
        {
            int c1 = 0;
            int c2 = 0;
            int c3 = 0;
            int c4 = 0;

            while (i != 0)
            {
                System.Threading.Thread.Sleep(2);
                int triage_number = triage_numb();

                if (triage_number == 1)
                {
                    ++c1;
                }
                else if (triage_number == 2)
                {
                    ++c2;
                }
                else if (triage_number == 3)
                {
                    ++c3;
                }
                else if (triage_number == 4)
                {
                    ++c4;
                }

                class1.Text = ("Klasse 1 \nSchwerverletzte: " + c1.ToString());

                class2.Text = ("Klasse 2\nLeichtverletzte: " + c2.ToString());

                class3.Text = ("Klasse 3\nHoffnungslose: " + c3.ToString());

                class4.Text = ("Klasse 4\nTote: " + c4.ToString());

                --i;

                Queue.Text = ("Wartende Patienten: " + i.ToString());

            }
        }

        private int triage_numb()
        {
            Random random = new Random();
            int random_number = random.Next(1, 4);
            return random_number;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        { 
        
        }

        private void Triage_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void UKH_Load(object sender, EventArgs e)
        {

        }

        private void class1_Click(object sender, EventArgs e)
        {

        }
    }
}
