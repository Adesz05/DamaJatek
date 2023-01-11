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
    class Mezo
    {
        public PictureBox mezo;
        public string MelyikSzin;
        public string MelyikBabu;

        public Mezo(PictureBox mezo, string melyikszin, string melyikbabu)
        {
            this.mezo = mezo;
            MelyikSzin = melyikszin;
            MelyikBabu = melyikbabu;
        }
    }
}
