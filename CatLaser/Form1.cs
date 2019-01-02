using CatLaser;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CrossThreadDemo
{
    public class Form1 : Form
    {
        private BackgroundWorker backgroundWorker1;
        private TextBox textBox1;
        private int latestXradio = 1;
        private int latestYradio = 2;
        private int latestXradioOLD = 5;
        private int latestYradioOLD = 6;

        private int latestXservo = 3;
        private int latestYservo = 4;
        private int latestXservoOLD = 5;
        private int latestYservoOLD = 6;

        private Boolean locked = false;

        string status = "";
        private IContainer components = null;
        private System.Drawing.Point curLocationRadio;
        private XBOXONE myController;
        private IPAddress serverAddr = IPAddress.Parse("192.168.43.182");
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        private IPEndPoint endPoint;
        public Form1()
        {
            InitializeComponent();
            endPoint = new IPEndPoint(serverAddr, 80);
            myController = new XBOXONE();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;

            myController.Update();
            curLocationRadio = new System.Drawing.Point(Width / 3, Height / 3);
            radioButton1.Location = curLocationRadio;
            backgroundWorker1.RunWorkerAsync(status);
        }



        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs statuse)
        {
            myController.Update();
            latestXradio = Clamp(curLocationRadio.X + (int)(myController.leftThumbX * 0.2f), 3, (Width - 35));
            latestYradio = Clamp(curLocationRadio.Y - (int)(myController.leftThumbY * 0.2f), 3, (Height - 55));
            latestXservo = Clamp(latestXradio * 180 / (Width - 35), 0, 180);
            latestYservo = Clamp(latestYradio * 180 / (Height - 55), 0, 180);

            if (latestXservo != latestXservoOLD || latestYservo != latestYservoOLD)
            {
                if (!locked)
            {
                locked = true;

                    curLocationRadio = new System.Drawing.Point(latestXradio, latestYradio);
                    radioButton1.Location = curLocationRadio;

                latestXservoOLD = latestXservo;
                latestYservoOLD = latestYservo;
                string servo1 = String.Format("%010d", (latestXservo));
                string delimiter = " , ";
                string servo2 = String.Format("%010d", (latestYservo));
                    byte[] send_buffer1 = Encoding.ASCII.GetBytes(servo1 + delimiter + servo2);
                sock.SendTo(send_buffer1, endPoint);
                    locked = false;
                }
            
            }

        }
        private void backgroundWorker1_DoWork(
    object sender,
    DoWorkEventArgs e)
        {

            status = "T";
            while (true)
            {

                backgroundWorker1.ReportProgress(1, status);
                Thread.Sleep(3);

            }

        }
        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            textBox1 = new System.Windows.Forms.TextBox();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            textBox2 = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            radioButton1 = new System.Windows.Forms.RadioButton();
            SuspendLayout();
            textBox1.Location = new System.Drawing.Point(1067, 538);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(83, 22);
            textBox1.TabIndex = 0;
            textBox2.Location = new System.Drawing.Point(12, 538);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(83, 22);
            textBox2.TabIndex = 4;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(553, 543);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(17, 17);
            label1.TabIndex = 5;
            label1.Text = "X";
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(857, 543);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(17, 17);
            label2.TabIndex = 6;
            label2.Text = "Y";
            radioButton1.AutoSize = true;
            radioButton1.BackColor = System.Drawing.Color.DarkRed;
            radioButton1.Checked = true;
            radioButton1.ForeColor = System.Drawing.Color.DarkRed;
            radioButton1.Location = new System.Drawing.Point(527, 265);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(17, 16);
            radioButton1.TabIndex = 7;
            radioButton1.TabStop = true;
            radioButton1.UseVisualStyleBackColor = false;
            BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            ClientSize = new System.Drawing.Size(1162, 574);
            Controls.Add(radioButton1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Cat Laser";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }

        private TextBox textBox2;
        private Label label1;
        private Label label2;
        private RadioButton radioButton1;

        public object Integer { get; private set; }
    }
}