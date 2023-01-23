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
        static Mezo kijelolt = null;
        static Point kezdoHely = new Point(50, 50);
        static int babuszam = 12;
        static List<Babu> player1 = new List<Babu>();
        static List<Babu> player2 = new List<Babu>();
        public Form1()
        {
            MatrixGeneralas();
            MezoPictureboxBeallitas();

            InitializeComponent();
        }

        private void MezoPictureboxBeallitas()
        {/*
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
            }*/
        }

        private void Klikkeles(object sender, EventArgs e)
        {
            Mezo klikkelt = sender as Mezo;

            if (klikkelt.Babu != null)
            {
                if (kijelolt == null)
                {
                    kijelolt = klikkelt;
                    if (HovaLephet() < 1)
                    {
                        kijelolt = null;
                    }
                }
            }
            else
            {
                if (kijelolt != null)
                {
                    Mozgas();
                }
            }
        }

        private void Mozgas()
        {
            throw new NotImplementedException();
        }

        private int HovaLephet()
        {
            int lehetoseg = 0;



            for (int i = 1; i <= kijelolt.Babu.MozgasMennyiseg; i++)
            {
                tabla[Index_Kezeles(kijelolt.Koordinatak.X - kijelolt.Babu.Irany * i), kijelolt.Koordinatak.Y + i].Image = Properties.Resources.sotetzoldpotyivelakozepen;
                tabla[kijelolt.Koordinatak.X - kijelolt.Babu.Irany * i, kijelolt.Koordinatak.Y + i].Jelolt = true;

                tabla[kijelolt.Koordinatak.X - kijelolt.Babu.Irany * i, kijelolt.Koordinatak.Y - i].Image = Properties.Resources.sotetzoldpotyivelakozepen;
                tabla[kijelolt.Koordinatak.X - kijelolt.Babu.Irany * i, kijelolt.Koordinatak.Y - i].Jelolt = true;
                lehetoseg++;
            }
            return lehetoseg;

        }

        private int Index_Kezeles(int szam)
        {
            return szam < 0 ? 0 : szam;
        }

        private void MatrixGeneralas()
        {
            for (int sor = 0; sor < meret; sor++)
            {
                for (int oszlop = 0; oszlop < meret; oszlop++)
                {
                    tabla[sor, oszlop] = GenerateMezo(sor, oszlop);
                    tabla[sor, oszlop].Location = new Point(kezdoHely.X + oszlop * mezomeret, kezdoHely.Y + sor * mezomeret);
                    tabla[sor, oszlop].Size = new Size(mezomeret, mezomeret);
                    tabla[sor, oszlop].SizeMode = PictureBoxSizeMode.StretchImage;
                    this.Controls.Add(tabla[sor, oszlop]);
                }
            }

            GenerateBabuk(Properties.Resources.sotetmezoFeketeAmogaval, 0, meret, 1, player1);
            GenerateBabuk(Properties.Resources.sotetmezoFeherAmogaval, meret-1, -1, -1, player2);
            EventHozzadas();
        }

        private void EventHozzadas()
        {
            for (int sor = 0; sor < meret; sor++)
            {
                for (int oszlop = 0; oszlop < meret; oszlop++)
                {
                    if (Sotet_e(sor, oszlop))
                    {
                        tabla[sor, oszlop].MouseDown += new MouseEventHandler(Klikkeles);
                    }
                }
            }
        }

        private void GenerateBabuk(Image kep, int honnan, int hova, int leptek, List<Babu> player)
        {
            for (int sor = honnan; leptek > 0 ? sor < hova : sor > hova; sor+=leptek)
            {
                for (int oszlop = honnan; leptek > 0 ? oszlop < hova : oszlop > hova; oszlop+=leptek)
                {
                    if (Sotet_e(sor, oszlop))
                    {
                        tabla[sor, oszlop].Image = kep;
                        tabla[sor, oszlop].Babu = new Babu(-1 * leptek);
                        player.Add(tabla[sor, oszlop].Babu);
                        if (player.Count >= babuszam)
                        {
                            return;
                        }
                    }
                }
            }
        }

        private Mezo GenerateMezo(int sor, int oszlop)
        {
            return new Mezo(new Point(sor, oszlop), Sotet_e(sor, oszlop) ? Properties.Resources.sotet : Properties.Resources.vilagos);
        }

        private bool Sotet_e(int sor, int oszlop)
        {
            return sor % 2 == 0 && oszlop % 2 == 1 || sor % 2 == 1 && oszlop % 2 == 0 ? true : false;
        }
    }
}
