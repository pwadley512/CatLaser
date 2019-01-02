using CatLaser;
using System;
using System.ComponentModel;
using System.Drawing;
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
        private int latestXradio = 1;
        private int latestYradio = 2;

        public int packetNumber = 0;
        private int latestUpDownServo = 3;
        private int latestRightLeftServo = 4;
        private int latestUpDownServoOLD = 5;
        private int latestRightLeftServoOLD = 6;

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
            //GlobalMouseHandler gmh = new GlobalMouseHandler();
            //gmh.TheMouseMoved += new MouseMovedEvent(gmh_TheMouseMoved);
            //Application.AddMessageFilter(gmh);
            InitializeComponent();
            endPoint = new IPEndPoint(serverAddr, 80);
            myController = new XBOXONE();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            packetNumber = 0;
            myController.Update();
            curLocationRadio = new System.Drawing.Point(Width / 3, Height / 3);
            radioButton1.Location = curLocationRadio;
            backgroundWorker1.RunWorkerAsync(status);
        }



        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs statuse)
        {
            myController.Update();
            latestXradio = Clamp(curLocationRadio.X + (int)(myController.leftThumbX * 0.09f), 3, (Width - 35));
            latestYradio = Clamp(curLocationRadio.Y - (int)(myController.leftThumbY * 0.09f), 3, (Height - 55));
            latestUpDownServo = Clamp(latestXradio * 180 / (Width - 35), 0, 180);
            latestRightLeftServo = Clamp(latestYradio * 180 / (Height - 55), 0, 180);

            if (this.myController.leftTrigger > 0.0f)
            {
                if (!locked)
                {
                    locked = true;
                    ZigZag();
                    locked = false;
                }
            }
            if (latestUpDownServo != latestUpDownServoOLD || latestRightLeftServo != latestRightLeftServoOLD)
            {

                curLocationRadio = new Point(latestXradio, latestYradio);
                radioButton1.Location = curLocationRadio;

                latestUpDownServoOLD = latestUpDownServo;
                latestRightLeftServoOLD = latestRightLeftServo;
                this.packetNumber++;
                string packetNumber = String.Format("{0, 0:D5}", (this.packetNumber));
                string servoUpDownValue = String.Format("{0, 0:D3}", (latestUpDownServo));
                string servoRightLeftValue = String.Format("{0, 0:D3}", (latestRightLeftServo));
                byte[] send_buffer1 = Encoding.ASCII.GetBytes(servoUpDownValue + servoRightLeftValue);
                sock.SendTo(send_buffer1, endPoint);
                locked = false;
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
                Thread.Sleep(16); // 1000 fps

            }

        }
        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        void ZigZag()
        {


            // oscillate shrinking since wave
            //100 point zag:
            var counter = 0;
            while (counter < 100)
            {
                counter++;
                // latestUpDownServo += 1;
                if ((counter % 2) == 0)
                {
                    latestRightLeftServo -= 20;
                }
                else
                {
                    latestRightLeftServo += 20;

                }


                string servoUpDownValue = String.Format("{0, 0:D3}", (latestUpDownServo));
                string servoRightLeftValue = String.Format("{0, 0:D3}", (latestRightLeftServo));
                byte[] send_buffer1 = Encoding.ASCII.GetBytes(servoUpDownValue + servoRightLeftValue);

                sock.SendTo(send_buffer1, endPoint);

            }

            locked = false;

        }


        //void gmh_TheMouseMoved()
        //{
        //    Point cur_pos = new Point(Cursor.Position.X - this.Location.X - 8, Cursor.Position.Y - this.Location.Y - 31);
        //    radioButton1.Location = cur_pos;
        //    Console.WriteLine(cur_pos);
        //}


        //public delegate void MouseMovedEvent();

        //public class GlobalMouseHandler : IMessageFilter
        //{
        //    private const int WM_MOUSEMOVE = 0x0200;

        //    public event MouseMovedEvent TheMouseMoved;



        //    public bool PreFilterMessage(ref Message m)
        //    {
        //        if (m.Msg == WM_MOUSEMOVE)
        //        {
        //            if (TheMouseMoved != null)
        //            {
        //                TheMouseMoved();
        //            }
        //        }
        //        // Always allow message to continue to the next filter control
        //        return false;
        //    }

        //}














































        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(553, 543);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(857, 543);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.BackColor = System.Drawing.Color.DarkRed;
            this.radioButton1.Checked = true;
            this.radioButton1.ForeColor = System.Drawing.Color.DarkRed;
            this.radioButton1.Location = new System.Drawing.Point(527, 265);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(17, 16);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1162, 574);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cat Laser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }



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
        private Label label1;
        private Label label2;
        private RadioButton radioButton1;

        public object Integer { get; private set; }
    }
}