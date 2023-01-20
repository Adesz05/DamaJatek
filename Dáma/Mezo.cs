using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dáma
{
    public partial class Mezo : PictureBox
    {
        public string MelyikSzin;
        public string MelyikBabu;
        public Point Koordinatak;

        public Mezo(string melyikszin, string melyikbabu, Point koordinatak)
        {
            MelyikSzin = melyikszin; //mezo szine: sotet v vilagos
            MelyikBabu = melyikbabu; // babu: fehér v fekete v fehérdama v feketedama v üres
            Koordinatak = koordinatak; // X, Y
        }
    }
}
