using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using System.Windows.Threading;


namespace AccelerometerWP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        Accelerometer accel = new Accelerometer();
        DispatcherTimer timer;
        protected double CursorCenter;
        protected double accelY = 0;
        protected double accelX = 0;
        protected double xdiff;
        protected double ydiff;
        protected double width = 480;
        protected double height = 800;
        protected double centerX = 480 / 2;
        protected double centerY = 800 / 2;
        protected double timerX;
        protected double timerY;
        protected bool nord=false;
        protected bool sud=false;
        protected bool est=false;
        protected bool ovest=false;
        protected bool riposo=false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(40);
            timer.Tick += new EventHandler(timer_Tick);

            accel.ReadingChanged += new EventHandler<AccelerometerReadingEventArgs>(AccelerometerReadingChanged);
            accel.Start();
            timer.Start();
          
        }
        void timer_Tick(object sender, EventArgs e)
        {
            CursorCenter = Cursor.Width / 2;

            xdiff = timerX - accelX;
            ydiff = timerY - accelY;

                accelX = -timerX ;
                accelY = timerY;

                if ((timerY < -0.5)&&(!sud))
                {
                    sud = true;
                    est = false; nord = false; ovest = false; riposo = false;
                    MessageBox.Show("Sotto!");
                }
                else if ((timerY > 0.5)&&(!nord))
                {
                    nord = true;
                    sud = false; ovest = false; est = false; riposo = false;
                    MessageBox.Show("Sopra!");
                }
                else if ((timerX < -0.52)&&(!ovest))
                {
                    ovest = true;
                    sud = false; nord = false; est = false; riposo = false;
                    MessageBox.Show("Sinistra!");
                }
                else if ((timerX > 0.52) && (!est))
                {
                    est = true;
                    sud = false; ovest = false; nord = false; riposo = false;
                    MessageBox.Show("Destra!");
                }
                else if((!riposo)&&(timerX>=-0.5)&&(timerX<=0.5)&&(timerY<=0.5)&&(timerY>=-0.5))
                {
                    riposo = true;
                    sud = false; ovest = false; nord = false; est = false;
                    
                }
        }

        void AccelerometerReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => UpdateImagePos(e));
        }

        void UpdateImagePos(AccelerometerReadingEventArgs e)
        {
            timerX = Math.Round(e.X,3);
            timerY = Math.Round(e.Y,3);
            Cursor.Margin = new Thickness(getX(), getY(), (width - (getX() + Cursor.Width)), (height - (getY() + Cursor.Height)));
        }

        double getX()
        {
            var newX = centerX + (-accelX *1.5* centerX);
            if ((newX - CursorCenter) < 0)
            {
                return 0;
            }
            else if ((newX + CursorCenter) > width)
            {
                return width - 2 * CursorCenter;
            }
            return newX - CursorCenter;
        }
        double getY()
        {
            var newY = centerY + (-accelY*1.5 * centerY);
           
            if ((newY - CursorCenter) < 0)
            {
                return 0;
            }
            else if ((newY + CursorCenter) > height)
            {
                return height - 2 * CursorCenter;
            }
            return newY - CursorCenter;
        }


     

    }
}