using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccelerometerWP7
{
    class Formica
    {
        //Numero 1-8 stabilisce la posizione della formica
        public int numero;
        //stabilisce se la formica è visibile o no
        public bool isVisible;

        public void cammina(){}
        public bool inCollision(){ return false;}
        public double posizX(){ return 0.0;}
        public double posizY(){ return 0.0;}
    }
}
