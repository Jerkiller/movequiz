using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Move_Quiz
{
    public partial class Tutorial : PhoneApplicationPage
    {
        public bool riproducendo = false;

        public Tutorial()
        {
            InitializeComponent();
            mediaElement1.Stop();
            mediaElement1.Source = new Uri("video.wmv", UriKind.RelativeOrAbsolute);
        }



        void MediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var errorException = e.ErrorException;
        }



 

        private void StopButt_Click_1(object sender, EventArgs e)
        {
                    mediaElement1.Stop();
            riproducendo = false;


        
        }

        private void PlayButt_Click_1(object sender, EventArgs e)
        {
            if (!riproducendo)
            {
                mediaElement1.Play();
                mediaElement1.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(MediaElement_MediaFailed);
                riproducendo = true;


            }

        }

        private void PauseButt_Click_1(object sender, EventArgs e)
        {
            if (riproducendo)
            {
                mediaElement1.Pause();
                riproducendo = false;
            }
        }
    }
}