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
    public partial class Form1 : Form
    {
        static int meret = 8;
        static int mezomeret = 80;
        static Mezo[,] tabla = new Mezo[meret,meret];
        public Form1()
        {
            MatrixGeneralas();
            MezoPictureboxBeallitas();

            InitializeComponent();
        }

        private void MezoPictureboxBeallitas()
        {
            for (int i = 0; i < meret; i++)
            {
                for (int j = 0; j < meret; j++)
                {
                    //MessageBox.Show(tabla[i, j].MelyikSzin + " , " + tabla[i, j].MelyikBabu);
                    tabla[i, j].mezo.Size = new Size(mezomeret, mezomeret);
                    if (tabla[i, j].MelyikSzin == "világos")
                    {
                        tabla[i, j].mezo.Image = Properties.Resources.vilagos;
                    }
                    else
                    {
                        if (tabla[i, j].MelyikBabu == "fekete")
                        {
                            tabla[i, j].mezo.Image = Properties.Resources.sotetmezoFeketeAmogaval;
                        }
                        else if (tabla[i, j].MelyikBabu == "fehér")
                        {
                            tabla[i, j].mezo.Image = Properties.Resources.sotetmezoFeherAmogaval;
                        }
                        else
                        {
                            tabla[i, j].mezo.Image = Properties.Resources.sotet;
                        }
                    }
                    tabla[i, j].mezo.Location = new Point(50+i*mezomeret,50+j*mezomeret);
                    tabla[i, j].mezo.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.Controls.Add(tabla[i, j].mezo);

                }
            }
        }

        private void MatrixGeneralas()
        {
            int futasokszama = 0;
            int feherbabuk = 0;
            bool vilagos = true;
            string melyikbabu = "üres";


            for (int i = 0; i < meret; i++)
            {
                for (int j = 0; j < meret; j++)
                {
                    if (!vilagos && feherbabuk < 12)
                    {
                        melyikbabu = "fekete";
                        feherbabuk++;
                    }
                    else melyikbabu = "üres";
                    if (futasokszama > 39 && !vilagos)
                    {
                        melyikbabu = "fehér";
                    }

                    tabla[j,i] = new Mezo(new PictureBox(), (vilagos) ? "világos" : "sötét",melyikbabu);
                    if (j!=7) vilagos = !vilagos;
                    futasokszama++;
                }
            }
        }
    }
}
