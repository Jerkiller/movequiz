using Microsoft.Devices.Sensors;
using Microsoft.Phone.Applications.Common;
using Microsoft.Phone.Controls;
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
using System.Windows.Threading;


namespace AccelerometerWP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Campi della classe
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
        #endregion

        // Constructor
        public MainPage()
        {
            InitializeComponent();


            timer = new DispatcherTimer();
            // Intervallo ottimo perché l'occhio umano veda qualcosa di fluido
            timer.Interval = TimeSpan.FromMilliseconds(40);
            timer.Tick += new EventHandler(timer_Tick);

            // Creo un'istanza di AccelerometerHelper con singleton
            AccelerometerHelper.Instance.ReadingChanged += new EventHandler<AccelerometerHelperReadingEventArgs>(OnAccelerometerHelperReadingChanged);
            AccelerometerHelper.Instance.Active = true;

            timer.Start();
        }

        

        /// <summary>
        /// Ogni tick del timer vado ad invocare i metodi in caso di movimento verso nord, est, sud, ovest...
        /// </summary>
        void timer_Tick(object sender, EventArgs e)
        {
            CursorCenter = Cursor.Width / 2;

            xdiff = timerX - accelX;
            ydiff = timerY - accelY;

                accelX = -timerX ;
                accelY = timerY;

                if ((timerY < -0.45)&&(!sud))
                {
                    sud = true;
                    est = false; nord = false; ovest = false; riposo = false;
                    MessageBox.Show("Sotto!");
                }
                else if ((timerY > 0.45)&&(!nord))
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
                else if((!riposo)&&(timerX>=-0.48)&&(timerX<=0.48)&&(timerY<=0.38)&&(timerY>=-0.38))
                {
                    riposo = true;
                    sud = false; ovest = false; nord = false; est = false;
                    MessageBox.Show("Safe area!");
                }
        }

        #region Modifica posizione del cursore

        /// <summary>
        /// Ogni 50 volte al secondo (frequenza di campionamento dell'accelerometro)
        /// vado a chiamare il metodo UpdateImagePos
        /// </summary>
        private void OnAccelerometerHelperReadingChanged(object sender, AccelerometerHelperReadingEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => UpdateImagePos(e));
        }

        /// <summary>
        /// Aggiorna la posizione del cursore ridefinendone i margini
        /// </summary>
        void UpdateImagePos(AccelerometerHelperReadingEventArgs e)
        {
            /* Vado a fare data smoothing dei dati presi dall'accelerometro */
            timerX = Math.Round(e.OptimalyFilteredAcceleration.X, 3);
            timerY = Math.Round(e.OptimalyFilteredAcceleration.Y, 3);
            Cursor.Margin = new Thickness(getX(), getY(), (width - (getX() + Cursor.Width)), (height - (getY() + Cursor.Height)));
        }


        /// <returns> La nuova posizione del margine sinistro del cursore in modo che non esca dal rettangolo</returns>
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

        /// <returns> La nuova posizione del margine superiore del cursore in modo che non esca dal rettangolo</returns>
        double getY()
        {
            var newY = centerY + (-accelY*1.7 * centerY);
           
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
        #endregion

    }
}