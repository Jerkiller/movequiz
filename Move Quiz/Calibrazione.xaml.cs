using System.Windows;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System;
using System.Windows.Navigation;

namespace Move_Quiz
{
    public partial class Calibrazione : PhoneApplicationPage
    {
        // VAR: Isolated storage per caricare/salvare
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        
        protected double x_calib=0;
        protected double y_calib=0;

        private string liv;


        public Calibrazione()
        {
            InitializeComponent();
            // Creo un'istanza di AccelerometerHelper con singleton
            AccelerometerHelper.Instance.ReadingChanged += new EventHandler<AccelerometerHelperReadingEventArgs>(OnAccelerometerHelperReadingChanged);
            AccelerometerHelper.Instance.Active = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //Calibra telefono
            Calibra();
            MessageBox.Show("Hai ricalibrato il telefono correttamente.");
            if (liv==null)
                NavigationService.Navigate(new Uri("/PagLivelli.xaml", UriKind.Relative));
            else {
                string uri = "/Question.xaml?id=" + liv;
                NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }
        }

        private void OnAccelerometerHelperReadingChanged(object sender, AccelerometerHelperReadingEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => UpdateImagePos(e));
        }

        void UpdateImagePos(AccelerometerHelperReadingEventArgs e)
        {
            /* Vado a fare data smoothing dei dati presi dall'accelerometro */
            x_calib = Math.Round(e.LowPassFilteredAcceleration.X, 3);
            y_calib = Math.Round(e.LowPassFilteredAcceleration.Y, 3);
        }

        public void Calibra()
        {
            if (appSettings.Contains("x_calib")) appSettings["x_calib"] = x_calib;
            if (appSettings.Contains("y_calib")) appSettings["y_calib"] = y_calib;
            else
            {
                appSettings.Add("x_calib", x_calib);
                appSettings.Add("y_calib", y_calib);
            }
            App.calibrato = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            /// IF: riesco a prendere il livello sul quale sto navigando
            NavigationContext.QueryString.TryGetValue("id", out liv);
        }
    }
}