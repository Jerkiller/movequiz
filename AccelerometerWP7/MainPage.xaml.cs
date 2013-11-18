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
using System.Windows.Media.Imaging;


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
        protected double _CursorCenter;
        protected double _accelY = 0;
        protected double _accelX = 0;
        protected double _xdiff;
        protected double _ydiff;
        protected double _width = 480;
        protected double _height = 800;
        protected double _centerX = 480 / 2;
        protected double _centerY = 800 / 2;
        protected double _timerX = 0;
        protected double _timerY = 0;
        protected bool ant = true;

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
            UpdateImagePos2();
            xdiff = timerX - accelX; //differenziale tra x vecchia e x nuova
            ydiff = timerY - accelY;    //differenziale tra y vecchia e y nuova

                accelX = -timerX ;//setta la nuoava posizione (invertita, ruotando a destra freccia a destra)
                accelY = timerY;

                Formica1.Source = cambiaTextureFormica();
                Formica2.Source = cambiaTextureFormica();
                Formica3.Source = cambiaTextureFormica();
                Formica4.Source = cambiaTextureFormica();
                Formica5.Source = cambiaTextureFormica();
                Formica6.Source = cambiaTextureFormica();
                Formica7.Source = cambiaTextureFormica();
               /* Formica8.Source = cambiaTextureFormica();*/
        }

        /// <summary>
        /// aggiorna l'immagine formica per il movimento delle zampette
        /// </summary>
        private ImageSource cambiaTextureFormica()
        {
            if (ant) { ant = false; return new BitmapImage(new Uri("Images/ant2.png", UriKind.Relative)); }
            else { ant = true; return new BitmapImage(new Uri("Images/ant.png", UriKind.Relative)); }
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
            if (collisione())
            {
                timer.Stop();
                Formica1.Source = null;
                //MessageBox.Show("formica distrutta" + "getX=" + getX() + " getX+Cursor.Width=" + (getX() + Cursor.Width) + " _timerX=" + _timerX + " _timerX+Formica.Width=" + (_timerX + Formica.Width));
            }

        }

        void UpdateImagePos2()
        {
            if (controlla())
                Formica1.Margin = new Thickness(_timerX, _timerY, (_width - (_timerX + Cursor.Width)), (_height - (_timerY + Cursor.Height)));

        }

        bool collisione() {
            if (((_timerX <= getX() && getX() <= (_timerX + Formica1.Width)) || (_timerX <= (getX() + Cursor.Width) && (getX() + Cursor.Width) <= (_timerX + Formica1.Width))) && ((_timerY <= getY() && getY() <= (_timerY + Formica1.Height)) || (_timerY <= (getY() + Cursor.Height) && (getY() + Cursor.Height) <= (_timerY + Formica1.Height))))
                return true;
            else return false;
        }

        bool controlla()
        {
            _timerX += 1;
            _timerY += 1.5;
            if ((_timerX + (_CursorCenter * 2) >= 190) && (_timerY - (_CursorCenter * 2) >= 300))
            {
                MessageBox.Show("ti ga perso");
                timer.Stop();
                return false;
            }
            else return true;
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