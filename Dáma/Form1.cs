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
        static bool vanekijeloltbabu = false;
        static List<Mezo> mezokahovalepnilehet = new List<Mezo>();
        static Mezo kijeloltbabu;
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
            //MessageBox.Show(klikkelt.Koordinatak.X + " meg " + klikkelt.Koordinatak.Y);
            //fehér------------------------------------------------------------------------------------------------------------------------------------------------------
            if (!vanekijeloltbabu)
            {
                if (klikkelt.MelyikBabu == kijon)
                {
                    //balra lehet mozogni
                    if (klikkelt.Koordinatak.X != 0)
                    {
                        //balra 1db átló mélység vizsgálat
                        if (tabla[klikkelt.Koordinatak.X - 1, klikkelt.Koordinatak.Y - 1].MelyikBabu == "üres")
                        {
                            tabla[klikkelt.Koordinatak.X - 1, klikkelt.Koordinatak.Y - 1].Image = Properties.Resources.sotetzoldpotyivelakozepen;
                            mezokahovalepnilehet.Add(tabla[klikkelt.Koordinatak.X - 1, klikkelt.Koordinatak.Y - 1]);
                            vanekijeloltbabu = true;
                            kijeloltbabu = klikkelt;
                        }
                        //ha balra az ellenfél bábuja amit ütni szeretnénk nem a fal mellett áll
                        //a sajat babut ne lehessen leutni TODO
                        else if (klikkelt.Koordinatak.X - 1 != 0 && tabla[klikkelt.Koordinatak.X - 2, klikkelt.Koordinatak.Y - 2].MelyikBabu == "üres" && tabla[klikkelt.Koordinatak.X - 1, klikkelt.Koordinatak.Y - 1].MelyikBabu != "fehér") //vagy dáma
                        {
                            tabla[klikkelt.Koordinatak.X - 2, klikkelt.Koordinatak.Y - 2].Image = Properties.Resources.sotetzoldpotyivelakozepen;
                            mezokahovalepnilehet.Add(tabla[klikkelt.Koordinatak.X - 2, klikkelt.Koordinatak.Y - 2]);
                            vanekijeloltbabu = true;
                            kijeloltbabu = klikkelt;
                        }
                    }

                    //jobbra lehet mozogni
                    if (klikkelt.Koordinatak.X != 7)
                    {
                        //jobbra 1db átló mélység vizsgálat
                        if (tabla[klikkelt.Koordinatak.X + 1, klikkelt.Koordinatak.Y - 1].MelyikBabu == "üres")
                        {
                            tabla[klikkelt.Koordinatak.X + 1, klikkelt.Koordinatak.Y - 1].Image = Properties.Resources.sotetzoldpotyivelakozepen;
                            mezokahovalepnilehet.Add(tabla[klikkelt.Koordinatak.X + 1, klikkelt.Koordinatak.Y - 1]);
                            vanekijeloltbabu = true;
                            kijeloltbabu = klikkelt;
                        }
                        //ha balra az ellenfél bábuja amit ütni szeretnénk nem a fal mellett áll
                        else if(klikkelt.Koordinatak.X + 1 !=7 && tabla[klikkelt.Koordinatak.X + 2, klikkelt.Koordinatak.Y - 2].MelyikBabu == "üres" && tabla[klikkelt.Koordinatak.X + 1, klikkelt.Koordinatak.Y - 1].MelyikBabu != "fehér") // dáma //fekete meg nem jo
                        {
                            tabla[klikkelt.Koordinatak.X + 2, klikkelt.Koordinatak.Y - 2].Image = Properties.Resources.sotetzoldpotyivelakozepen;
                            mezokahovalepnilehet.Add(tabla[klikkelt.Koordinatak.X + 2, klikkelt.Koordinatak.Y - 2]);
                            vanekijeloltbabu = true;
                            kijeloltbabu = klikkelt;
                        }
                       

                    }
                }
            }
            //van már egy kijelölt bábu
            else
            {
                //MessageBox.Show("vankijeloltbabu");
                vanekijeloltbabu = false;
                for (int i = 0; i < mezokahovalepnilehet.Count; i++)
                {
                    tabla[mezokahovalepnilehet[i].Koordinatak.X, mezokahovalepnilehet[i].Koordinatak.Y].Image = Properties.Resources.sotet;
                    if (klikkelt== mezokahovalepnilehet[i])
                    {
                        //egyenlőre csak sima amogus bábu léphet oda
                        tabla[mezokahovalepnilehet[i].Koordinatak.X, mezokahovalepnilehet[i].Koordinatak.Y].Image =Properties.Resources.sotetmezoFeherAmogaval;
                        tabla[mezokahovalepnilehet[i].Koordinatak.X, mezokahovalepnilehet[i].Koordinatak.Y].MelyikBabu=kijeloltbabu.MelyikBabu;
                        tabla[kijeloltbabu.Koordinatak.X, kijeloltbabu.Koordinatak.Y].Image = Properties.Resources.sotet;
                        tabla[kijeloltbabu.Koordinatak.X, kijeloltbabu.Koordinatak.Y].MelyikBabu="üres";
                    }
                }
                mezokahovalepnilehet.Clear();
            }
            

            //dáma if
            // || klikkelt.MelyikBabu == kijon + "dáma"
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
                    if (!vilagos && feherbabuk < 8 && i!=1)
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
