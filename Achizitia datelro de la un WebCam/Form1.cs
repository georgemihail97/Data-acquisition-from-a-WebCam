using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Achizitia_datelro_de_la_un_WebCam
{
    public partial class Form1 : Form
    {
        VideoCapture capture;
        Mat frame;
        Bitmap image;
        private Thread camera;
        bool isRunning = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void CaptureCamera()
        {
            camera = new Thread(new ThreadStart(CaptureCallback));
            camera.Start();
        }

        private void CaptureCallback()
        {
            frame = new Mat();
            capture= new VideoCapture(0);
            capture.Open(0);
            if(isRunning==true)
            {
                while(isRunning)
                {
                    try
                    {
                        capture.Read(frame);
                        image = BitmapConverter.ToBitmap(frame);
                       // if (pictureBox1.Image != null)
                          //  pictureBox1.Dispose();
                        pictureBox1.Image = image;
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CaptureCamera();
            isRunning = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(isRunning)
            {
                Bitmap captura = new Bitmap(pictureBox1.Image);
                captura.Save(string.Format(@"D:\imgTest\test.png"), ImageFormat.Png);
                textBox1.Text = captura.Size.Width + "x" + captura.Size.Height;


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isRunning = false;
            capture.Release();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int x, y;
            x = Convert.ToInt16(textBox2.Text);
            y = Convert.ToInt16(textBox3.Text);
            Bitmap bm = new Bitmap(@"D:\imgTest\test.png");
            pictureBox2.Image = bm;
            textBox2.Text = bm.GetPixel(x, y).ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Mat imgM = new Mat(@"D:\imgTest\test.png", ImreadModes.Grayscale);
            image = BitmapConverter.ToBitmap(imgM);
            pictureBox2.Image = image;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Mat imgM = new Mat(@"D:\imgTest\test.png", ImreadModes.Grayscale);
            var srI=new Mat(@"D:\imgTest\test.png");
            var binarI = new Mat(imgM.Size(), MatType.CV_8UC1);
            Cv2.CvtColor(srI, binarI, ColorConversionCodes.BGRA2GRAY);
            Cv2.Threshold(binarI, binarI, thresh:100, maxval:155,type:ThresholdTypes.Binary);
            image = BitmapConverter.ToBitmap(binarI);
            pictureBox2.Image = image;
        }
    }
}
