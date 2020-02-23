using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Move_Quiz.ViewModel;

namespace Move_Quiz
{
    public partial class PagLivelli : PhoneApplicationPage
    {
        public PagLivelli()
        {
            InitializeComponent();
            this.DataContext = App.livelliVM();
        }


        //Premendo il Back button voglio tornare alla main page e non nella pagina prima
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Calibrazione.xaml", UriKind.Relative));

        }
        
    }
}