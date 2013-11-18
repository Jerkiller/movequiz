using System;
using System.Collections.Generic;

namespace AccelerometerWP7
{
    /// <summary>
    /// crea formiche a manetta anche contemporaneamente da piu parti
    /// </summary>
    class AntGenerator
    {
        protected List<int> formiche;






        public AntGenerator() {
            formiche = new List<int>();
            formiche.Add(1);
        }




        /// <summary>
        /// verrà invocato ad ogni tick
        /// </summary>
        public void tick() {
            //foreach(int i in formiche)
           //avanza verso il centro
        }


    }
}
