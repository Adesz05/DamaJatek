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
        public Babu Babu;
        public Point Koordinatak;
        public bool Jelolt;

        public Mezo(Point koordinatak, Image kep)
        {
            Babu = null;
            Koordinatak = koordinatak;
            Image = kep;
            Jelolt = false;
        }
    }
}
