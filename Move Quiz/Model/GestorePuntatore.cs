using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Move_Quiz.Model
{
    public class GestorePuntatore
    {
        
        protected double CursoreCenter;
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
        protected double x_calib;
        protected double y_calib;

        private Image Cursore;
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

        private Thickness margineCursore;
        public Thickness MargineCursore {
            get {
                return margineCursore;
            }

            set {
                if (value != null && !value.Equals(margineCursore)) {
                    margineCursore = value;
                   // MessageBox.Show("Margine cursore cambiato " + margineCursore.ToString());
                }
            }
        }



        public GestorePuntatore(Image c) {
            Cursore = c;
            // Creo un'istanza di AccelerometerHelper con singleton
            AccelerometerHelper.Instance.ReadingChanged += new EventHandler<AccelerometerHelperReadingEventArgs>(OnAccelerometerHelperReadingChanged);
            AccelerometerHelper.Instance.Active = true;
            RecuperaDatiDaCalibrazione();
        }


        /// <summary>
        /// Ogni tick del timer vado ad invocare i metodi in caso di movimento verso nord, est, sud, ovest...
        /// </summary>
        public void Ricalcola()
        {
            CursoreCenter = Cursore.Width / 2;

            xdiff = timerX - accelX;
            ydiff = timerY - accelY;

            accelX = -timerX;
            accelY = timerY;

           
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
            //MessageBox.Show("cambio posizione immagine");
            /* Vado a fare data smoothing dei dati presi dall'accelerometro */
            timerX = Math.Round(e.LowPassFilteredAcceleration.X, 3) - x_calib;
            timerY = Math.Round(e.LowPassFilteredAcceleration.Y, 3) - y_calib;
           // Cursore.Margin = new Thickness(getX(), getY(), (width - (getX() + Cursore.Width)), (height - (getY() + Cursore.Height)));
            MargineCursore = new Thickness(getX(), getY(), (width - (getX() + Cursore.Width)), (height - (getY() + Cursore.Height)));
            //MessageBox.Show("cambio posizione immagine" + new Thickness(getX(), getY(), (width - (getX() + Cursore.Width)), (height - (getY() + Cursore.Height))).ToString());
        }


        /// <returns> La nuova posizione del margine sinistro del cursore in modo che non esca dal rettangolo</returns>
        double getX()
        {
            var newX = centerX + (-accelX * 3 * centerX);
            if ((newX - CursoreCenter) < 0)
            {
                return 0;
            }
            else if ((newX + CursoreCenter) > width)
            {
                return width - 2 * CursoreCenter;
            }
            return newX - CursoreCenter;
        }

        /// <returns> La nuova posizione del margine superiore del cursore in modo che non esca dal rettangolo</returns>
        double getY()
        {
            var newY = centerY + (-accelY * 4 * centerY);

            if ((newY - CursoreCenter) < 0)
            {
                return 0;
            }
            else if ((newY + CursoreCenter) > height)
            {
                return height - 2 * CursoreCenter;
            }
            return newY - CursoreCenter;
        }


        public void RecuperaDatiDaCalibrazione()
        {
            if (appSettings.Contains("x_calib") && appSettings.Contains("y_calib"))
            {
                x_calib = Double.Parse((appSettings["x_calib"]).ToString());
                y_calib = Double.Parse((appSettings["y_calib"]).ToString());
            }
            else
            {
                x_calib = 0;
                y_calib = 0;
            }
        }

        #endregion

      
    }
}
