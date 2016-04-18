using System.Windows.Forms;
namespace K4_Projekt
{
    partial class UKH
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AUVA_LOGO = new System.Windows.Forms.PictureBox();
            this.Triageplatzbeschriftung = new System.Windows.Forms.Label();
            this.Triageplatz = new System.Windows.Forms.PictureBox();
            this.Queue = new System.Windows.Forms.Label();
            this.Triagewagen = new System.Windows.Forms.Label();
            this.class1 = new System.Windows.Forms.Label();
            this.class2 = new System.Windows.Forms.Label();
            this.class3 = new System.Windows.Forms.Label();
            this.class4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.AUVA_LOGO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Triageplatz)).BeginInit();
            this.SuspendLayout();
            // 
            // AUVA_LOGO
            // 
            this.AUVA_LOGO.AccessibleDescription = "AUVA_LOGO";
            this.AUVA_LOGO.AccessibleName = "AUVA_LOGO";
            this.AUVA_LOGO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AUVA_LOGO.Image = global::K4_Projekt.Properties.Resources.AUVA_svg;
            this.AUVA_LOGO.InitialImage = null;
            this.AUVA_LOGO.Location = new System.Drawing.Point(1120, 38);
            this.AUVA_LOGO.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AUVA_LOGO.Name = "AUVA_LOGO";
            this.AUVA_LOGO.Size = new System.Drawing.Size(192, 156);
            this.AUVA_LOGO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.AUVA_LOGO.TabIndex = 0;
            this.AUVA_LOGO.TabStop = false;
            this.AUVA_LOGO.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Triageplatzbeschriftung
            // 
            this.Triageplatzbeschriftung.AccessibleName = "";
            this.Triageplatzbeschriftung.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Triageplatzbeschriftung.AutoSize = true;
            this.Triageplatzbeschriftung.BackColor = System.Drawing.Color.Transparent;
            this.Triageplatzbeschriftung.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Triageplatzbeschriftung.Location = new System.Drawing.Point(390, 131);
            this.Triageplatzbeschriftung.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Triageplatzbeschriftung.Name = "Triageplatzbeschriftung";
            this.Triageplatzbeschriftung.Size = new System.Drawing.Size(119, 25);
            this.Triageplatzbeschriftung.TabIndex = 6;
            this.Triageplatzbeschriftung.Text = "Triageplatz";
            // 
            // Triageplatz
            // 
            this.Triageplatz.AccessibleName = "";
            this.Triageplatz.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Triageplatz.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Triageplatz.Location = new System.Drawing.Point(396, 162);
            this.Triageplatz.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Triageplatz.Name = "Triageplatz";
            this.Triageplatz.Size = new System.Drawing.Size(608, 406);
            this.Triageplatz.TabIndex = 5;
            this.Triageplatz.TabStop = false;
            this.Triageplatz.Click += new System.EventHandler(this.Triage_Click);
            // 
            // Queue
            // 
            this.Queue.AccessibleDescription = "Queue2";
            this.Queue.AccessibleName = "Queue2";
            this.Queue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Queue.AutoSize = true;
            this.Queue.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Queue.Location = new System.Drawing.Point(390, 88);
            this.Queue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Queue.Name = "Queue";
            this.Queue.Size = new System.Drawing.Size(0, 25);
            this.Queue.TabIndex = 8;
            // 
            // Triagewagen
            // 
            this.Triagewagen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Triagewagen.BackColor = System.Drawing.Color.LightCoral;
            this.Triagewagen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Triagewagen.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Triagewagen.Location = new System.Drawing.Point(800, 185);
            this.Triagewagen.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.Triagewagen.Name = "Triagewagen";
            this.Triagewagen.Size = new System.Drawing.Size(186, 112);
            this.Triagewagen.TabIndex = 10;
            this.Triagewagen.Text = "Triagewagen";
            this.Triagewagen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // class1
            // 
            this.class1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.class1.BackColor = System.Drawing.Color.Yellow;
            this.class1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.class1.Location = new System.Drawing.Point(426, 208);
            this.class1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.class1.MaximumSize = new System.Drawing.Size(200, 55);
            this.class1.Name = "class1";
            this.class1.Size = new System.Drawing.Size(190, 55);
            this.class1.TabIndex = 11;
            this.class1.Text = "Klasse 1\r\nSchwerverletzte:";
            this.class1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.class1.Click += new System.EventHandler(this.class1_Click);
            // 
            // class2
            // 
            this.class2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.class2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.class2.ForeColor = System.Drawing.Color.White;
            this.class2.Location = new System.Drawing.Point(426, 300);
            this.class2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.class2.MaximumSize = new System.Drawing.Size(200, 55);
            this.class2.Name = "class2";
            this.class2.Size = new System.Drawing.Size(190, 55);
            this.class2.TabIndex = 12;
            this.class2.Text = "Klasse 2\r\nLeichtverletzte:";
            this.class2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // class3
            // 
            this.class3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.class3.BackColor = System.Drawing.Color.Blue;
            this.class3.ForeColor = System.Drawing.Color.White;
            this.class3.Location = new System.Drawing.Point(426, 392);
            this.class3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.class3.MaximumSize = new System.Drawing.Size(200, 55);
            this.class3.Name = "class3";
            this.class3.Size = new System.Drawing.Size(190, 55);
            this.class3.TabIndex = 13;
            this.class3.Text = "Klasse 3\r\nHoffnungslose:";
            this.class3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // class4
            // 
            this.class4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.class4.BackColor = System.Drawing.Color.Black;
            this.class4.ForeColor = System.Drawing.Color.White;
            this.class4.Location = new System.Drawing.Point(426, 490);
            this.class4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.class4.MaximumSize = new System.Drawing.Size(200, 55);
            this.class4.Name = "class4";
            this.class4.Size = new System.Drawing.Size(190, 55);
            this.class4.TabIndex = 14;
            this.class4.Text = "Klasse 4\r\nTote:";
            this.class4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UKH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1356, 833);
            this.Controls.Add(this.class4);
            this.Controls.Add(this.class3);
            this.Controls.Add(this.class2);
            this.Controls.Add(this.class1);
            this.Controls.Add(this.Triagewagen);
            this.Controls.Add(this.Queue);
            this.Controls.Add(this.Triageplatzbeschriftung);
            this.Controls.Add(this.Triageplatz);
            this.Controls.Add(this.AUVA_LOGO);
            this.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UKH";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Triage-Simulator UKH Linz";
            this.Load += new System.EventHandler(this.UKH_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AUVA_LOGO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Triageplatz)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void createPerson()
        {

        }
           
        

        #endregion

        private PictureBox AUVA_LOGO;
        private Label Triageplatzbeschriftung;
        private PictureBox Triageplatz;
        private Label Queue;
        private Label Triagewagen;
        private Label class1;
        private Label class2;
        private Label class3;
        private Label class4;
    }
}

