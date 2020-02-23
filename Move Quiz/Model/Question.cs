using System;
using System.Collections.Generic;

namespace Move_Quiz
{
    public partial class Question
    {
        public string testo;
        public List<string> risposte;
        protected int corretta;

        //CLASSE INTERNA
        public class TetraLista {
            public List<int> lista;
            public TetraLista(int a, int b, int c, int d) {
                lista = new List<int>();
                lista.Add(a); lista.Add(b); lista.Add(c); lista.Add(d);
            }
        }

        public Question(string testo, string a, string b, string c, string d, int j){
            //generare risposte random
            this.testo = testo;
            risposte = new List<string>();
            Random rando = new Random();
            List<List<int>> lista = new List<List<int>>();

            #region popolo lista
            lista.Add(new TetraLista(1, 0, 3, 2).lista);
            lista.Add(new TetraLista(0, 1, 3, 2).lista);
            lista.Add(new TetraLista(0, 2, 3, 1).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(0, 2, 3, 1).lista);
            lista.Add(new TetraLista(3, 2, 0, 1).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(0, 2, 3, 1).lista);
            lista.Add(new TetraLista(3, 2, 0, 1).lista);
            lista.Add(new TetraLista(2, 0, 3, 1).lista);
            lista.Add(new TetraLista(1, 3, 2, 0).lista);
            lista.Add(new TetraLista(2, 1, 0, 3).lista);
            lista.Add(new TetraLista(3, 2, 0, 1).lista);
            lista.Add(new TetraLista(0, 1, 3, 2).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(3, 0, 2, 1).lista);
            lista.Add(new TetraLista(0, 1, 3, 2).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(3, 0, 2, 1).lista); 
            lista.Add(new TetraLista(3, 0, 2, 1).lista);
            lista.Add(new TetraLista(1, 3, 0, 2).lista);
            lista.Add(new TetraLista(1, 0, 2, 3).lista);
            lista.Add(new TetraLista(2, 1, 0, 3).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(1, 0, 3, 2).lista);
            lista.Add(new TetraLista(0, 1, 3, 2).lista);
            lista.Add(new TetraLista(3, 2, 0, 1).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(3, 0, 2, 1).lista);
            lista.Add(new TetraLista(2, 0, 3, 1).lista);
            lista.Add(new TetraLista(0, 2, 3, 1).lista);
            lista.Add(new TetraLista(3, 2, 0, 1).lista);
            lista.Add(new TetraLista(2, 0, 3, 1).lista);
            lista.Add(new TetraLista(3, 0, 2, 1).lista); 
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(0, 2, 3, 1).lista);
            lista.Add(new TetraLista(3, 2, 0, 1).lista);
            lista.Add(new TetraLista(1, 0, 3, 2).lista); 
            lista.Add(new TetraLista(0, 1, 3, 2).lista);
            lista.Add(new TetraLista(0, 2, 3, 1).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(3, 0, 2, 1).lista);
            lista.Add(new TetraLista(1, 3, 0, 2).lista);
            lista.Add(new TetraLista(1, 0, 2, 3).lista);
            lista.Add(new TetraLista(2, 1, 0, 3).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(2, 0, 3, 1).lista);
            lista.Add(new TetraLista(0, 1, 3, 2).lista);
            lista.Add(new TetraLista(1, 0, 3, 2).lista);
            lista.Add(new TetraLista(2, 0, 3, 1).lista);
            lista.Add(new TetraLista(1, 3, 2, 0).lista);
            lista.Add(new TetraLista(2, 1, 0, 3).lista);
            lista.Add(new TetraLista(3, 2, 0, 1).lista);
            lista.Add(new TetraLista(0, 1, 3, 2).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(3, 0, 2, 1).lista);
            lista.Add(new TetraLista(0, 1, 3, 2).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(3, 0, 2, 1).lista);
            lista.Add(new TetraLista(2, 0, 3, 1).lista);
            lista.Add(new TetraLista(0, 1, 3, 2).lista);
            lista.Add(new TetraLista(3, 2, 0, 1).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(1, 0, 3, 2).lista);
            lista.Add(new TetraLista(1, 0, 3, 2).lista);
            lista.Add(new TetraLista(0, 1, 3, 2).lista);
            lista.Add(new TetraLista(3, 2, 0, 1).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            lista.Add(new TetraLista(3, 2, 0, 1).lista);
            lista.Add(new TetraLista(3, 2, 1, 0).lista);
            /* //DEBUG
            int i = 0;
            while(i<lista.Count){
                    MessageBox.Show("lista " + i + ": " + lista[i][0] + " " + lista[i][1] + " " + lista[i][2] + " " + lista[i][3] + " ");
                    i++;
                }
             * */
            #endregion

            //z è un numero sorteggiato tra le permutazioni possibili (lista) in modo che abbia di sicuro altri 9 successori
            int z = rando.Next(0, (lista.Count-17));

            // j indica il numero della domanda caricata e va da 0 a 9
            //ora inserisco in lista le 4 domande secondo la permutazione 2013, 3021, 0312, ...
            for (int s = 0; s < 4; s++)
            {
                if (lista[z+j][s] == 0) { risposte.Add(a); corretta = s+1; }
                if (lista[z+j][s] == 1) risposte.Add(b);
                if (lista[z+j][s] == 2) risposte.Add(c);
                if (lista[z+j][s] == 3) risposte.Add(d);
            }

        }

        public string Risposta_1
        {
            get
            {
                return risposte[0];
            }
        }

        public string Risposta_2
        {
            get
            {
                return risposte[1];
            }
        }

        public string Risposta_3
        {
            get
            {
                    return risposte[2];
            }
        }

        public string Risposta_4
        {
            get
            {
                    return risposte[3];
            }
        }

        public string Testo
        {
            get
            {
                return testo;
            }
        }
        
       
       
        /// <summary>
        /// un verificatore che dice se è corretta la risposta
        /// </summary>
        /// <param name="answer">il testo della risposta che passo al verificatore</param>
        /// <returns>un bool che dice se la risposta è giusta</returns>
        public bool isCorrect(int answer) {
            if (answer == corretta) return true;
            else return false;
        }

    }
}