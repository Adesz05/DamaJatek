using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dáma
{
    public class Babu
    {
        public int MozgasMennyiseg;
        public int Irany;
        public string Szin;
        public bool Dama;


        public Babu(int irany, string szin)
        {
            MozgasMennyiseg = 1;
            Irany = irany;
            Szin = szin;
            Dama = false;
            

        }
    }
}
