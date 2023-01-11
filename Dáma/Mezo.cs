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
        public bool VaneRajta;
        public string MelyikSzin;

        public Mezo(PictureBox mezo, bool vaneRajta, string melyikszin)
        {
            this.mezo = mezo;
            VaneRajta = vaneRajta;
            MelyikSzin = melyikszin;
        }
    }
}
