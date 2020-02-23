using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace Move_Quiz
{
    public class Livello: INotifyPropertyChanged
    {
        List<Question> domande;
        int id;
        private QuestionLoader ql;
        string best_score;

        // VAR: Isolated storage per caricare/salvare
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;


        public Livello(int id) {
            ql = new QuestionLoader();
            domande = ql.getQuestions(id);
            this.id = id;
            if (appSettings.Contains("bestscore" + id))
            {
                string content = appSettings["bestscore" + id].ToString();
                best_score = content;
            }
            else best_score = "-";
            
        }

        public List<Question> Domande {
            get {
                return domande;
            }
        }

        // GETTER: ritorna best_score
        // SETTER: setta il nuovo best score del livello
        public string Best_Score
        {
            get
            {
                return best_score;
            }
            set
            {
                // se esiste già un best score imposta il piu grande tra l'attuale e il vecchio
                if (!best_score.Equals("-"))
                {
                    int val = Convert.ToInt32(value);
                    int val2 = Convert.ToInt32(best_score);
                    if (val > val2)
                    {
                        best_score = value;
                        RaisePropertyChanged("Best_Score");
                    }
                }
                // altrimenti se si gioca per la prima volta viene assegnato il valore corrente 
                else
                {
                    best_score = value;
                    RaisePropertyChanged("Best_Score");
                }
            }

        }

        // GETTER: ritorna id
        public int Id
        {
            get
            {
                return id;
            }
        }


        /// METODO: ritorna se un livello è sbloccato o bloccato guardando il precedente (?)
        public bool isAvaiable()
        {

            /// Controlla se il livello precedente è stato vinto
            int livelloprecedente = (Convert.ToInt32(id)) - 1;

            /// Il primo livello e sempre accessibile   
            if (livelloprecedente == 0) { return true; }

            /// IF: il best score del livello precedente è settato (QUESTO è SEMPRE VERO O NO ?)
            if (appSettings.Contains("bestscore" + livelloprecedente))
            {
                /// prendo il best score del livello precedente
                string content = appSettings["bestscore" + livelloprecedente].ToString();

                /// IF: non si è vinto il livello precedento ritorno bloccato
                if (content == "-")
                    return false;

               /// ELSE: torno sbloccato
                else
                    return true;
            }
            /// ELSE: ritorno bloccato
            else
                return false;
        }

        /// METODO: ritorna la stringa del path del background in base se il livello è sbloccato o meno
        public string PathSfondo
        {
            get
            {
                if (isAvaiable())
                    return "Images/unlocked.png";
                else return "Images/locked.png";
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
