using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace Move_Quiz
{
    public class QuestionLoader
    {
        private XDocument doc;
        private int livello;


        /// <returns>una domanda casuale dato un livello e una categoria (proprietà del questionLoader)</returns>
        public List<Question> getQuestions(int livello)
        {
            
            this.doc = XDocument.Load("domande.xml");
            this.livello = livello;
            List<Question> questions = new List<Question>();
            //seloeziona livello corrente
            var livellocorrente = from query in doc.Descendants("livello")
                      where query.Attribute("id").Value == livello.ToString()
                      select query;
            var domande = from query in livellocorrente.Descendants("domanda")
                          select query;
            
            List<XElement> i = domande.ToList();
            for (int j = 0; j < 10; j++)
            {
                
                //seleziono il testo della domanda
                var testo = from query in i[j].Descendants("testo")
                            select query;
                string testodomanda = testo.ToList()[0].Value;
                //MessageBox.Show("testo: " + testodomanda);
                //seleziono risposta_a
                var risposta_a = from query in i[j].Descendants("risposta_a")
                                 select query;
                string testorisposta_a = risposta_a.ToList()[0].Value;
                //MessageBox.Show("risposta a: " + testorisposta_a);

                //seleziono risposta_b
                var risposta_b = from query in i[j].Descendants("risposta_b")
                                 select query;
                string testorisposta_b = risposta_b.ToList()[0].Value;

                    //seleziono risposta_c
                    var risposta_c = from query in i[j].Descendants("risposta_c")
                                     select query;
                    string testorisposta_c = risposta_c.ToList()[0].Value;

                    //seleziono risposta_d
                    var risposta_d = from query in i[j].Descendants("risposta_d")
                                     select query;
                    string testorisposta_d = risposta_d.ToList()[0].Value;

                    questions.Add(new Question(testodomanda, testorisposta_a, testorisposta_b, testorisposta_c, testorisposta_d, j));
                
            }
            return questions;
        }

         public int caricaLivelli()
        {
            //MessageBox.Show("hanno invocato il loader");
            /// apertura file livelli.xml
            XDocument doc = XDocument.Load("domande.xml");

            /// creazione lista di interi
            List<int> lista = new List<int>();

            /// configurazione dei livelli
            string conf = ritornaLivelli(doc);
            int c = Convert.ToInt32(conf);
            //MessageBox.Show("sono stati trovati " + c+" livelli");
            return c;

        }

        /// METODO: Ritorna id dell'ultimo livello (stesso numero di livelli per ogni categoria)
        private string ritornaLivelli(XDocument doc)
        {
            var pos = from query in doc.Descendants("livello")
                      select query.Attribute("id").Value;
            return pos.Last();
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

