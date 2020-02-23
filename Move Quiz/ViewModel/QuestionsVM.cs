using Move_Quiz.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Move_Quiz.ViewModel
{
    public class QuestionsVM : INotifyPropertyChanged
    {
        /// VAR: Isolate storage per salvare/caricare
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        private Livello actLiv;
        private List<Question> domande;
        private Question actQuestion;
        private int num_actQuestion;
        private GestorePuntatore gestorePunt;
        private DispatcherTimer timerPunt;
        private DispatcherTimer timerContatore;
        private Image Cursore;

        protected bool nord = false;
        protected bool sud = false;
        protected bool est = false;
        protected bool ovest = false;
        protected bool riposo = false;
        protected bool risposta = false;

        private bool correct;

        private ICommand livelloSuccessivo;
        public ICommand LivelloSuccessivo
        {
            get
            {
                return livelloSuccessivo;
            }
        }

        private ICommand esci;
        public ICommand Esci
        {
            get
            {
                return esci;
            }
        }

        private ICommand ricomincia;
        public ICommand Ricomincia
        {
            get
            {
                return ricomincia;
            }
        }

     

        private SolidColorBrush coloreContatore;
        public SolidColorBrush ColoreContatore
        {
            get
            {
                return coloreContatore;
            }

            set
            {
                if (value != null)
                {
                    coloreContatore = value;
                    RaisePropertyChanged("ColoreContatore");
                }
            }
        }

        private Thickness marginePuntatore;
        public Thickness MarginePuntatore
        {
            get
            {
                return marginePuntatore;
            }

            set
            {
                if (value != null)
                {
                    marginePuntatore = value;
                    RaisePropertyChanged("MarginePuntatore");
                    //MessageBox.Show("Magine ccccccurrssore cambiato");
                }
            }
        }

        public int dimensioneRispostaNord;
        public int DimensioneRispostaNord {
            get {
                return dimensioneRispostaNord;
            }

            set {
                if (value != dimensioneRispostaNord) {
                    dimensioneRispostaNord = value;
                    RaisePropertyChanged("DimensioneRispostaNord");
                }
            }
        }

        public int dimensioneRispostaEst;
        public int DimensioneRispostaEst
        {
            get
            {
                return dimensioneRispostaEst;
            }

            set
            {
                if (value != dimensioneRispostaEst)
                {
                    dimensioneRispostaEst = value;
                    RaisePropertyChanged("DimensioneRispostaEst");
                }
            }
        }

        public int dimensioneRispostaSud;
        public int DimensioneRispostaSud
        {
            get
            {
                return dimensioneRispostaSud;
            }

            set
            {
                if (value != dimensioneRispostaSud)
                {
                    dimensioneRispostaSud = value;
                    RaisePropertyChanged("DimensioneRispostaSud");
                }
            }
        }

        public int dimensioneRispostaOvest;
        public int DimensioneRispostaOvest
        {
            get
            {
                return dimensioneRispostaOvest;
            }

            set
            {
                if (value != dimensioneRispostaOvest)
                {
                    dimensioneRispostaOvest = value;
                    RaisePropertyChanged("DimensioneRispostaOvest");
                }
            }
        }

        private bool vittoria;
        public bool Vittoria
        {
            get
            {
                return vittoria;
            }

            set
            {
                if (value != vittoria)
                {
                    vittoria = value;
                    RaisePropertyChanged("Vittoria");
                }
            }
        }

        private bool sconfitta;
        public bool Sconfitta
        {
            get
            {
                return sconfitta;
            }

            set
            {
                if (value != sconfitta)
                {
                    sconfitta = value;
                    RaisePropertyChanged("Sconfitta");
                }
            }
        }

        private int bestScore;
        public int BestScore
        {
            get
            {
                return bestScore;
            }

            set
            {
                if (value != bestScore)
                {
                    bestScore = value;
                    RaisePropertyChanged("BestScore");
                }
            }
        }

        private int punteggio;
        public int Punteggio
        {
            get
            {
                return punteggio;
            }

            set
            {
                if (value != punteggio)
                {
                    punteggio = value;
                    RaisePropertyChanged("Punteggio");
                }
            }
        }

        private int tempoRimanente;
        public int TempoRimanente
        {
            get
            {
                return tempoRimanente;
            }

            set
            {
                if (value != tempoRimanente)
                {
                    tempoRimanente = value;
                    RaisePropertyChanged("TempoRimanente");
                }
            }
        }

        //COSTRUTTORE
        public QuestionsVM(int liv, Image img)
        {
            actLiv = App.getLivello(liv);
            domande = actLiv.Domande;
            actQuestion = domande[0];
            num_actQuestion = 1;
            livelloSuccessivo = new DelegateCommand(_livelloSuccessivo);
            ricomincia = new DelegateCommand(_ricomincia);
            esci = new DelegateCommand(_esci);
            coloreContatore = new SolidColorBrush(Color.FromArgb(80, 255, 255, 255));
            marginePuntatore = new Thickness(291, 335, 139, 353);
            gestorePunt = new GestorePuntatore(img);
            Cursore = img;

            timerPunt = new DispatcherTimer();
            // Intervallo ottimo perché l'occhio umano veda qualcosa di fluido
            timerPunt.Interval = TimeSpan.FromMilliseconds(40);
            timerPunt.Tick += new EventHandler(timer_Tick);

            dimensioneRispostaNord = 30;
            dimensioneRispostaEst = 30;
            dimensioneRispostaSud = 30;
            dimensioneRispostaOvest = 30;

            correct=false;
            vittoria = false;
            sconfitta = false;

            risposta = false;
            punteggio = 50;
            timerContatore = new DispatcherTimer();
            timerContatore.Interval = new TimeSpan(0, 0, 0, 1, 0); // 1 sec
            timerContatore.Tick += new EventHandler(dt_Tick);
            avviaTimer(90);
            timerPunt.Start();
           
        }

        private void _esci(object o)
        {
            timerPunt.Stop();
            timerContatore.Stop();
        }


        private void _ricomincia(object o)
        {
            Sconfitta = false;
            punteggio = 90;
            timerPunt.Start();
            avviaTimer(90);
            TornaAllaPrima();
        }

        public void avviaTimer(int tempo)
        {
            TempoRimanente = tempo;
            timerContatore.Start();
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            TempoRimanente=tempoRimanente-1;
            Punteggio=punteggio-1;
            switch (tempoRimanente)
            {
                // cambio il colore degli ultimi secondi del cronometro
                case 6: {  this.PlayContatoreSound(this, EventArgs.Empty); break; }
                case 5: { ColoreContatore = new SolidColorBrush(Color.FromArgb(80, 255, 0, 0)); this.PlayContatoreSound(this, EventArgs.Empty); break; }
                case 4: { ColoreContatore = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0)); this.PlayContatoreSound(this, EventArgs.Empty); break; }
                case 3: { ColoreContatore = new SolidColorBrush(Color.FromArgb(120, 255, 0, 0));  this.PlayContatoreSound(this, EventArgs.Empty); break; }
                case 2: { ColoreContatore = new SolidColorBrush(Color.FromArgb(140, 255, 0, 0)); this.PlayContatoreSound(this, EventArgs.Empty); break; }
                case 1: { ColoreContatore = new SolidColorBrush(Color.FromArgb(160, 255, 0, 0));  this.PlayContatoreSound(this, EventArgs.Empty); break; }
                case 0:
                    {
                        ColoreContatore = new SolidColorBrush(Color.FromArgb(180, 255, 0, 0));
                        
                        this.PlayContatoreLastSound(this, EventArgs.Empty);
                        timerContatore.Stop();
                        timerPunt.Stop();
                        Sconfitta = true;

                        break;
                    }

                default: { ColoreContatore = new SolidColorBrush(Color.FromArgb(80, 255, 255, 255)); break; }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            gestorePunt.Ricalcola();
            MarginePuntatore = gestorePunt.MargineCursore;
            double left = Cursore.Margin.Left;
            double right = Cursore.Margin.Right;
            double up = Cursore.Margin.Top;
            double down = Cursore.Margin.Bottom;



            if ((down < 55) && (!nord) && (!est) && (!ovest) && (!sud) && (left>100) && (right>100))
            {
                sud = true;
                est = false; nord = false; ovest = false; riposo = false; risposta = true;
                Sud();
            }
            else if ((up < 55) && (!nord) && (!est) && (!ovest) && (!sud) && (left>100) && (right>100))
            {
                nord = true;
                sud = false; ovest = false; est = false; riposo = false; risposta = true;
                Nord();
            }
            else if ((left < 55)&& (!nord) && (!est) && (!ovest) && (!sud) && (up>260) && (down > 260))
            {
                ovest = true;
                sud = false; nord = false; est = false; riposo = false; risposta = true;
                Ovest();
            }
            else if ((right < 55) && (!nord) && (!est) && (!ovest) && (!sud) && (up > 260) && (down > 260))
            {
                est = true;
                sud = false; ovest = false; nord = false; riposo = false; risposta = true;
                Est();
            }
            else if ((!riposo) && (  ((up > 55) && (down > 55) && (left > 55) & (right > 55))  || ((up<260)&&(left<100)) || ((up<260)&&(right<100))  || ((down<260)&&(left<100))  || ((down<260)&&(right<100)) ))
            {
                riposo = true;
                sud = false; ovest = false; nord = false; est = false;
                if (risposta)
                {
                    Riposo();
                    risposta = false;
                }
            }
            
        }

        /// eventi da lanciare per il play dei suoni (ascoltati poi nella view)
        public event EventHandler PlayCorrettaSound;
        public event EventHandler PlayErrataSound;
        public event EventHandler PlayVittoriaSound;
        public event EventHandler PlayContatoreSound;
        public event EventHandler PlayContatoreLastSound;


         /// METODO: invoca il movimento
        private void movimentoRisposta(int risp)
        {
            //stopTimer();
            correct = actQuestion.isCorrect(risp);

            if (correct)
            {
                this.PlayCorrettaSound(this, EventArgs.Empty); //suono risposta corretta
                Punteggio =punteggio+ 3;
            }
            else
            {
                this.PlayErrataSound(this, EventArgs.Empty); //suono risposta errata
                Punteggio = punteggio- 5;
            }
            
            risposta = true;
            
            

            
        }


        public void Nord()
        {
            this.movimentoRisposta(1);
            /*Cambio dimensione testo*/
            DimensioneRispostaNord= 40;
            /*Reimposto la dimensione per gli altri testi*/
            DimensioneRispostaSud= 30;
            DimensioneRispostaOvest= 30;
            DimensioneRispostaEst= 30;
        }

        public void Sud()
        {
            this.movimentoRisposta(3);
            DimensioneRispostaSud= 40;
            DimensioneRispostaNord= 30;
            DimensioneRispostaOvest= 30;
            DimensioneRispostaEst= 30;

        }

        public void Est()
        {
            this.movimentoRisposta(2);
            DimensioneRispostaOvest= 30;
            DimensioneRispostaEst= 40;
            DimensioneRispostaNord= 30;
            DimensioneRispostaSud= 30;
        }

        public void Ovest()
        {
            this.movimentoRisposta(4);
            DimensioneRispostaOvest= 40;
            DimensioneRispostaEst= 30;
            DimensioneRispostaNord= 30;
            DimensioneRispostaSud= 30;

        }

        public void Riposo()
        {
            DimensioneRispostaNord= 30;
            DimensioneRispostaSud= 30;
            DimensioneRispostaOvest= 30;
            DimensioneRispostaEst= 30;
            if (correct)
            {
                bool exist_next = nextQuestion(punteggio);
                if (!exist_next)
                {
                    timerContatore.Stop();
                    this.PlayVittoriaSound(this, EventArgs.Empty); //suono vittoria
                    timerPunt.Stop();
                    int best = 0;
                    Punteggio = punteggio;
                    if (appSettings.Contains("bestscore" + actLiv.Id))
                    {
                        //recupero bestscore

                        best = Int32.Parse((appSettings["bestscore" + actLiv.Id]).ToString());
                        if (punteggio > best)
                        {
                            //aggiorno bestscore
                            appSettings["bestscore" + actLiv.Id] = punteggio;
                            BestScore = punteggio;
                        }
                    }
                    //Apro il popup
                    Vittoria = true;
                    
                }
            }
            else { TornaAllaPrima(); }
        }

        public int Num_actQuestion
        {
            get
            {
                return num_actQuestion;
            }
            set
            {
                if (value != num_actQuestion)
                {
                    num_actQuestion = value;
                    RaisePropertyChanged("Num_actQuestion");
                }
            }
        }

        private void _livelloSuccessivo(object o)
        {
            int next = actLiv.Id + 1;
            string uri;
            if (next <= 16)
                uri = "/Question.xaml?Refresh=true&id=" + next;
            else uri = "/MainPage.xaml";
            var rootFrame = (App.Current as App).RootFrame;
            rootFrame.Navigate(new Uri(uri, UriKind.Relative));
        }

        public bool nextQuestion(int punti)
        {
            int i = domande.IndexOf(actQuestion);
            if (i < (domande.Count - 1))
            {
                ActQuestion = domande[i + 1];
                int a = num_actQuestion + 1;
                Num_actQuestion = a;
                return true;
            }
            else
            {
                actLiv.Best_Score = punti.ToString();
                Num_actQuestion = 1;

                /// Aggiungo alle settings bestscore+id
                if (appSettings.Contains("bestscore" + actLiv.Id))
                {
                    /// Rimuovi vecchio best score
                    appSettings.Remove("bestscore" + actLiv.Id);
                }
                /// Aggiungi nuovo best score
                appSettings.Add("bestscore" + actLiv.Id, actLiv.Best_Score);
                return false;
            }
        }

        public void TornaAllaPrima()
        {
            ActQuestion = domande[0];
            Num_actQuestion = 1;
        }

        

        public Question ActQuestion
        {
            get
            {
                return actQuestion;
            }

            set
            {
                if (value != actQuestion)
                {
                    actQuestion = value;
                    RaisePropertyChanged("ActQuestion");
                    for (int i = 1; i < 5; i++)
                    {
                        RaisePropertyChanged("Risposta_" + i);
                    }

                }

            }
        }

        /// METODO: Implementa interfaccia Notify    
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string PropName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropName));
        }
    }
}
