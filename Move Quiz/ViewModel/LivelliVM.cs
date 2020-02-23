using Move_Quiz.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Move_Quiz.ViewModel
{
    public class LivelliVM : INotifyPropertyChanged
    {
        /// VAR: lista di livelli
        private ObservableCollection<Livello> listaLiv;
        QuestionLoader ql;

        private ICommand goToGame;
        public ICommand GoToGame
        {
            get
            {
                return goToGame;
            }
        }

        /// COSTRUTTORE: carica dallo xml una lista di id e crea i livelli in base all'id
        public LivelliVM()
        {
            ql = new QuestionLoader();
            listaLiv = new ObservableCollection<Livello>();

            /// assegna una lista di id di livelli
            int livelli = ql.caricaLivelli();

            /// oer ogni id nella lista caricata, crea un livello nuovo ed assegnalo al campo 
            for(int i =1;i<livelli+1;i++)
            {
                listaLiv.Add(new Livello(i));
            }
            goToGame = new DelegateCommand(_goToGame);
        }

        public Livello getLivello(int id)
        {
            return listaLiv[id - 1];
        }

        /// GETTER: listaLiv
        public ObservableCollection<Livello> ListaLiv
        {
            get
            {
                return listaLiv;
            }
        }
  

        public bool Avaiable(int num)
        {
                if (listaLiv[num - 1].isAvaiable())
                    return true;
                else return false;
        }

        public void _goToGame(object o)
        {
            int liv = ((Livello)o).Id;
            bool ris = this.Avaiable(liv);
            string uri;
           
                if (ris)
                {
                    if (App.calibrato)
                    {
                    uri = "/Question.xaml?id=" + liv.ToString();

                    var rootFrame = (App.Current as App).RootFrame;
                    rootFrame.Navigate(new Uri(uri, UriKind.Relative));
                }
                    else
                    {
                        uri = "/Calibrazione.xaml?id=" + liv.ToString();

                        var rootFrame = (App.Current as App).RootFrame;
                        rootFrame.Navigate(new Uri(uri, UriKind.Relative));
                    }
            }
            

        }



        /// METODO: Implementa interfaccia
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string PropName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropName));
        }


    }

}
