using Microsoft.Phone.Controls;
using Move_Quiz.ViewModel;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Windows.Input;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;



namespace Move_Quiz
{
    public partial class Question : PhoneApplicationPage
    {

        #region campi
        int tempo_rimanente;
        DispatcherTimer dt = new DispatcherTimer();
        int punti = 50;
        int idLivello;
        bool correct;
        // VAR: Isolated storage per caricare/salvare
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

        DispatcherTimer timer;
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
        protected bool nord = false;
        protected bool sud = false;
        protected bool est = false;
        protected bool ovest = false;
        protected bool riposo = false;
        protected bool risposta = false;
        #endregion

        // Constructor
        public Question()
        {
            InitializeComponent();
            /*risposta = false;
            punti = 50;
            dt.Interval = new TimeSpan(0,0,0,1,0); // 1 sec
            dt.Tick += new EventHandler(dt_Tick);
            avviaTimer(50);
            RecuperaDatiDaCalibrazione();
            timer = new DispatcherTimer();
            // Intervallo ottimo perché l'occhio umano veda qualcosa di fluido
            timer.Interval = TimeSpan.FromMilliseconds(40);
            timer.Tick += new EventHandler(timer_Tick);

            // Creo un'istanza di AccelerometerHelper con singleton
            AccelerometerHelper.Instance.ReadingChanged += new EventHandler<AccelerometerHelperReadingEventArgs>(OnAccelerometerHelperReadingChanged);
            AccelerometerHelper.Instance.Active = true;

            timer.Start();*/

        }

        /// METODO: durante il passaggio alla pagina con id x caricami i dati del livello con id x e setta il DataContext
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string liv = string.Empty;

            /// IF: riesco a prendere il livello sul quale sto navigando
            if (NavigationContext.QueryString.TryGetValue("id", out liv))
            {
                /// converti la stringa dell id del livello in intero
                idLivello = Convert.ToInt32(liv);
                /// crea un nuovo DataContext con il LivelloVM(id) per il binding
                this.DataContext = new QuestionsVM(idLivello, Cursore);
                QuestionsVM vm = (QuestionsVM)this.DataContext;
                ///associo all'evento PlaySounds il play dei vari media elements
                vm.PlayCorrettaSound += (sender, ev) =>
                {
                    correctding.Play();
                };

                vm.PlayErrataSound += (sender, ev) =>
                {
                    wrong.Play();
                };

                vm.PlayVittoriaSound += (sender, ev) =>
                {
                    win.Play();
                };

                vm.PlayContatoreSound += (sender, ev) =>
                {
                    beep.Play();
                };

                vm.PlayContatoreLastSound += (sender, ev) =>
                {
                    lastbeep.Play();
                };
            }

        }

        #region definizione suoni
        static Stream beep_stream = TitleContainer.OpenStream("Sounds/beep.wav");
        static Stream correct_stream = TitleContainer.OpenStream("Sounds/correct.wav");
        static Stream lastbeep_stream = TitleContainer.OpenStream("Sounds/lastbeep.wav");
        static Stream win_stream = TitleContainer.OpenStream("Sounds/win.wav");
        static Stream wrong_stream = TitleContainer.OpenStream("Sounds/wrong.wav");

        static SoundEffect beep_sound = SoundEffect.FromStream(beep_stream);
        static SoundEffect correct_sound = SoundEffect.FromStream(correct_stream);
        static SoundEffect lastbeep_sound = SoundEffect.FromStream(lastbeep_stream);
        static SoundEffect win_sound = SoundEffect.FromStream(win_stream);
        static SoundEffect wrong_sound = SoundEffect.FromStream(wrong_stream);

        static SoundEffectInstance beep = beep_sound.CreateInstance();
        static SoundEffectInstance correctding = correct_sound.CreateInstance();
        static SoundEffectInstance lastbeep = lastbeep_sound.CreateInstance();
        static SoundEffectInstance win = win_sound.CreateInstance();
        static SoundEffectInstance wrong = wrong_sound.CreateInstance();
        #endregion


        /*/// METODO: invoca il movimento sul bottone selezionato
        private void Go(int risp)
        {
            //stopTimer();
            correct = ((QuestionsVM)this.DataContext).Verify(risp);

            if (correct)
            {
                answer_correct.Volume = 0.8;
                answer_correct.Play(); //suono risposta corretta
                punti += 3;
            }
            else
            {
                answer_wrong.Volume = 0.8;
                answer_wrong.Play(); //suono risposta errata
                punti -= 5;
            }
            
            risposta = true;
            
            

            
        }

        public void avviaTimer(int tempo) {
            tempo_rimanente = tempo;
            dt.Start();
        }

        public void stopTimer()
        {
            dt.Stop();
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            tempo_rimanente--;
            punti--;
            switch (tempo_rimanente)
            {
                // cambio il colore degli ultimi secondi del cronometro
                case 6: { last_beep.Volume = 0; last_beep.Play(); break; }
                case 5: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(80, 255, 0, 0)); time_beep.Volume=0; time_beep.Play(); break; }
                case 4: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0)); time_beep.Volume = 0.6; time_beep.Play(); break; }
                case 3: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(120, 255, 0, 0)); time_beep.Volume = 0.8; time_beep.Play(); break; }
                case 2: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(140, 255, 0, 0)); time_beep.Volume = 0.9; time_beep.Play(); break; }
                case 1: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(160, 255, 0, 0)); time_beep.Volume = 0.9; time_beep.Play(); break; }
                case 0: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(180, 255, 0, 0));
                last_beep.Volume = 1.0;
                    last_beep.Play();
                    dt.Stop();
                    timer.Stop();
                    sconfittaPopUp.IsOpen = true;

                    break; }

                default: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(80, 255, 255, 255)); break; }
            }
            Counter.Text = tempo_rimanente.ToString();
        }



        #region definizione metodi nord sud est ovest riposo


        public void Nord() {
            this.Go(1);
            RispostaNord.FontSize = 40;
            RispostaSud.FontSize = 30;
            RispostaOvest.FontSize = 30;
            RispostaEst.FontSize = 30;
        }

        public void Sud() {
            this.Go(3);
            RispostaSud.FontSize = 40;
            RispostaNord.FontSize = 30;
            RispostaOvest.FontSize = 30;
            RispostaEst.FontSize = 30;

        }

        public void Est() {
                this.Go(2);
                RispostaOvest.FontSize = 30;
                RispostaEst.FontSize = 40;
                RispostaNord.FontSize = 30;
                RispostaSud.FontSize = 30;
        }

        public void Ovest() {
                this.Go(4);
                RispostaOvest.FontSize = 40;
                RispostaEst.FontSize = 30;
                RispostaNord.FontSize = 30;
                RispostaSud.FontSize = 30;

        }

        public void Riposo()
        {
            RispostaNord.FontSize = 30;
            RispostaSud.FontSize = 30;
            RispostaOvest.FontSize = 30;
            RispostaEst.FontSize = 30;
                if (correct)
                {
                    bool exist_next = ((QuestionsVM)this.DataContext).nextQuestion(punti);
                    if (!exist_next)
                    {
                        win_sound.Play();

                        int best=0;
                        if (appSettings.Contains("bestscore" + idLivello))
                        {
                            //recupero bestscore
                            
                            best = Int32.Parse((appSettings["bestscore" + idLivello]).ToString());
                            if (punti > best)
                            {
                                //aggiorno bestscore
                                appSettings["bestscore" + idLivello] = punti;
                                best = punti;
                            }
                        }

                        //Aggiorno campi popup
                        bestscore.Text = best.ToString();
                        punteggio.Text = punti.ToString();
                        //Apro il popup
                        vittoria.IsOpen = true;
                        timer.Stop();
                        stopTimer();
                    }
                }
                else { ((QuestionsVM)this.DataContext).Ricomincia(); }
        }
        #endregion

        //ATTENZIONE! NON VA QUI
        private void ricomincia(object sender, RoutedEventArgs e)
        {
            sconfittaPopUp.IsOpen = false;
            
            //suono inizio
            punti = 50;
            timerPunt.Start();
            avviaTimer(50);
            //Ricomincia dalla prima domanda!!!
            ((QuestionsVM)this.DataContext).Ricomincia();
        }
        */
        private void esci(object sender, RoutedEventArgs e)
        {
            sconfittaPopUp.IsOpen = false;
            string uri = "/PagLivelli.xaml?Refresh=true";
            //MessageBox.Show("passo alla prossima pagina " + uri);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));

        }




        /*
        /// <summary>
        /// Ogni tick del timer vado ad invocare i metodi in caso di movimento verso nord, est, sud, ovest...
        /// </summary>
        void timer_Tick(object sender, EventArgs e)
        {
            CursoreCenter = Cursore.Width / 2;

            xdiff = timerX - accelX;
            ydiff = timerY - accelY;

                accelX = -timerX ;
                accelY = timerY;
                #region invocazione metodi nord sud est ovest riposo

                double left = Cursore.Margin.Left;
                double right = Cursore.Margin.Right;
                double up = Cursore.Margin.Top;
                double down = Cursore.Margin.Bottom;

                if ((down < 100) && (!nord) && (!est) && (!ovest) && (!sud))
                {
                    sud = true;
                    est = false; nord = false; ovest = false; riposo = false; risposta = true;
                    Sud();
                }
                else if ((up <100) && (!nord) && (!est) && (!ovest) && (!sud))
                {
                    nord = true;
                    sud = false; ovest = false; est = false; riposo = false; risposta = true;
                    Nord();
                }
                else if ((left < 60) && (up>120) && (down>120) && (!nord) && (!est) && (!ovest) && (!sud))
                {
                    ovest = true;
                    sud = false; nord = false; est = false; riposo = false; risposta = true;
                    Ovest();
                }
                else if ((right < 60) && (up > 120) && (down > 120) && (!nord) && (!est) && (!ovest) && (!sud))
                {
                    est = true;
                    sud = false; ovest = false; nord = false; riposo = false; risposta = true;
                    Est();
                }
                else if((!riposo)&&(up>150)&&(down>150)&&(left>150)&&(right>150))
                {
                    riposo = true;
                    sud = false; ovest = false; nord = false; est = false;
                    if (risposta)
                    {
                        Riposo();
                        risposta = false;
                    }
                }
                #endregion
        }*/

        //Al click del back button chiedo se si è sicuri di uscire. Se si torno alla pagina dei livelli a cui applico un refresh
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            sconfittaPopUp.IsOpen = false;
            ((QuestionsVM)DataContext).Esci.Execute(null);
                string uri = "/PagLivelli.xaml?Refresh=true";

                NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }


        /* #region Modifica posizione del cursore

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
             timerX = Math.Round(e.LowPassFilteredAcceleration.X, 3)-x_calib;
             timerY = Math.Round(e.LowPassFilteredAcceleration.Y, 3)-y_calib;
             Cursore.Margin = new Thickness(getX(), getY(), (width - (getX() + Cursore.Width)), (height - (getY() + Cursore.Height)));
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

         #endregion*/

     }
    }
