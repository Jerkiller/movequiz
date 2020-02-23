using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace Move_Quiz
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void Gioca_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PagLivelli.xaml", UriKind.Relative));
        }
        private void Info_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Info.xaml", UriKind.Relative));
        }
        private void Altro_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BestScores.xaml", UriKind.Relative));
        }

        //Premendo il Back button chiedo se si è sicuri di uscire. Se si non torno alla pagina di prima ma esco dall'applicazione (cancello la storia delle pagine navigate)
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }

        }

        private void Tutorial_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Tutorial.xaml", UriKind.Relative));
        }
    }
}