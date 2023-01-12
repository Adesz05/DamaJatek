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
        static string kijon = "fehér";
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
                    tabla[i, j].Size = new Size(mezomeret, mezomeret);
                    if (tabla[i, j].MelyikSzin == "világos")
                    {
                        tabla[i, j].Image = Properties.Resources.vilagos;
                    }
                    else
                    {
                        if (tabla[i, j].MelyikBabu == "fekete")
                        {
                            tabla[i, j].Image = Properties.Resources.sotetmezoFeketeAmogaval;
                        }
                        else if (tabla[i, j].MelyikBabu == "fehér")
                        {
                            tabla[i, j].Image = Properties.Resources.sotetmezoFeherAmogaval;
                        }
                        else
                        {
                            tabla[i, j].Image = Properties.Resources.sotet;
                        }
                    }
                    tabla[i, j].Location = new Point(50+i*mezomeret,50+j*mezomeret);
                    tabla[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    tabla[i, j].Click += new EventHandler(Klikkeles);
                    tabla[i, j].Koordinatak = new Point(i, j);
                    this.Controls.Add(tabla[i, j]);

                }
            }
        }

        private void Klikkeles(object sender, EventArgs e)
        {
            Mezo klikkelt = sender as Mezo;
            if (klikkelt.MelyikBabu==kijon)
            {
                
            }
            //MessageBox.Show($"x: {klikkelt.Koordinatak.X} y: {klikkelt.Koordinatak.Y}");

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

                    tabla[j,i] = new Mezo((vilagos) ? "világos" : "sötét",melyikbabu, new Point());
                    if (j!=7) vilagos = !vilagos;
                    futasokszama++;
                }
            }
        }
    }
}
