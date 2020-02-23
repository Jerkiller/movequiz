using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace Move_Quiz
{
    public partial class BestScores : PhoneApplicationPage
    {
        // VAR: Isolated storage per caricare/salvare
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        public const int numLivelli = 16;

        public BestScores()
        {
            InitializeComponent();
            CaricaBestScores();
        }

        private void CaricaBestScores()
        {
            if (appSettings.Contains("bestscore1")) Livello1.Text = appSettings["bestscore1"].ToString();
            if (appSettings.Contains("bestscore2")) Livello2.Text = appSettings["bestscore2"].ToString();
            if (appSettings.Contains("bestscore3")) Livello3.Text = appSettings["bestscore3"].ToString();
            if (appSettings.Contains("bestscore4")) Livello4.Text = appSettings["bestscore4"].ToString();
            if (appSettings.Contains("bestscore5")) Livello5.Text = appSettings["bestscore5"].ToString();
            if (appSettings.Contains("bestscore6")) Livello6.Text = appSettings["bestscore6"].ToString();
            if (appSettings.Contains("bestscore7")) Livello7.Text = appSettings["bestscore7"].ToString();
            if (appSettings.Contains("bestscore8")) Livello8.Text = appSettings["bestscore8"].ToString();
            if (appSettings.Contains("bestscore9")) Livello9.Text = appSettings["bestscore9"].ToString();
            if (appSettings.Contains("bestscore10")) Livello10.Text = appSettings["bestscore10"].ToString();
            if (appSettings.Contains("bestscore11")) Livello11.Text = appSettings["bestscore11"].ToString();
            if (appSettings.Contains("bestscore12")) Livello12.Text = appSettings["bestscore12"].ToString();
            if (appSettings.Contains("bestscore13")) Livello13.Text = appSettings["bestscore13"].ToString();
            if (appSettings.Contains("bestscore14")) Livello14.Text = appSettings["bestscore14"].ToString();
            if (appSettings.Contains("bestscore15")) Livello15.Text = appSettings["bestscore15"].ToString();
            if (appSettings.Contains("bestscore16")) Livello16.Text = appSettings["bestscore16"].ToString();
        }


        

        /// <summary>
        /// Cancella tutti i bestscore!
        /// </summary>
        public void CancellaProgressi()
        {
            for (int id = 0; id <= numLivelli; id++)
                if (appSettings.Contains("bestscore" + id))
                { appSettings["bestscore" + id] = "-"; }
            CaricaBestScores();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("Resettare tutti i dati di gioco?", "Cancella", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.OK)
            {
                CancellaProgressi();
                MessageBox.Show("Tutti i progressi nel gioco sono stati cancellati.");
            }
        }
    }
}