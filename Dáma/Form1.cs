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
            InitializeComponent();
            MatrixGeneralas();
        }



        private void Klikkeles(object sender, EventArgs e)
        {
            Mezo klikkelt = sender as Mezo;
            if (!klikkelt.Jelolt)
            {
                VisszaAllit();
            }
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
                    Mozgas(klikkelt);
                }
            }
        }

        private void Mozgas(Mezo klikkelt)
        {
            if (klikkelt.Jelolt)
            {
                klikkelt.Jelolt = false;
                klikkelt.Image = kijelolt.Image;
                klikkelt.Babu = kijelolt.Babu;
                kijelolt.Image = Properties.Resources.sotet;
                kijelolt.Babu = null;
                VisszaAllit();
            }
        }

        private void VisszaAllit()
        {
            for (int sor = 0; sor < tabla.GetLength(0); sor++)
            {
                for (int oszlop = 0; oszlop < tabla.GetLength(1); oszlop++)
                {
                    if (tabla[sor, oszlop].Jelolt)
                    {
                        tabla[sor, oszlop].Jelolt = false;
                        tabla[sor, oszlop].Image = Properties.Resources.sotet;
                    }
                }
            }
            kijelolt = null;
        }

        private int HovaLephet()
        {
            int lehetoseg = 0;
            int meddig = kijelolt.Babu.MozgasMennyiseg;

            for (int i = 1; i <= kijelolt.Babu.MozgasMennyiseg; i++)
            {
                for (int j = -1; j <= 1; j+=2)
                {
                    try
                    {
                        if (!UtesKotelezettseg().Contains(kijelolt.Koordinatak))
                        {
                            if (tabla[kijelolt.Koordinatak.X - kijelolt.Babu.Irany * i, kijelolt.Koordinatak.Y + i * j].Babu == null)
                            {
                                tabla[kijelolt.Koordinatak.X - kijelolt.Babu.Irany * i, kijelolt.Koordinatak.Y + i*j].Image = Properties.Resources.sotetzoldpotyivelakozepen;
                                tabla[kijelolt.Koordinatak.X - kijelolt.Babu.Irany * i, kijelolt.Koordinatak.Y + i*j].Jelolt = true;
                                lehetoseg++;
                            }
                            else if (tabla[kijelolt.Koordinatak.X - kijelolt.Babu.Irany * i, kijelolt.Koordinatak.Y + i * j].Babu.Szin != kijelolt.Babu.Szin)
                            {
                                if ()
                                {

                                }
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
            return lehetoseg;

        }

        private List<Point> UtesKotelezettseg()
        {
            List<Point> lista = new List<Point>();
            for (int sor = 0; sor < tabla.GetLength(0); sor++)
            {
                for (int oszlop = 0; oszlop < tabla.GetLength(1); oszlop++)
                {
                    try
                    {
                        if (tabla[sor, oszlop].Babu != null && tabla[sor, oszlop].Babu.Szin == kijelolt.Babu.Szin)
                        {
                            for (int i = -1; i <= 1; i+=2)
                            {
                                if (tabla[sor - kijelolt.Babu.Irany * i, oszlop + i].Babu.Szin == null)
                                {
                                    lista.Add(tabla[sor - kijelolt.Babu.Irany * i, oszlop + i].Koordinatak);
                                }
                            }
                        }
                    }
                    catch(Exception) { }
                }
            }
            return lista;
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

            GenerateBabuk(Properties.Resources.sotetmezoFeketeAmogaval, 0, meret, 1, player1, "Fehér");
            GenerateBabuk(Properties.Resources.sotetmezoFeherAmogaval, meret-1, -1, -1, player2, "Fekete");
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
                        tabla[sor, oszlop].Click += new EventHandler(Klikkeles);
                    }
                    else
                    {
                        tabla[sor, oszlop].Click += delegate (object sender, EventArgs e) { VisszaAllit(); };
                    }

                }
            }
        }

        private void GenerateBabuk(Image kep, int honnan, int hova, int leptek, List<Babu> player, string szin)
        {
            for (int sor = honnan; leptek > 0 ? sor < hova : sor > hova; sor+=leptek)
            {
                for (int oszlop = honnan; leptek > 0 ? oszlop < hova : oszlop > hova; oszlop+=leptek)
                {
                    if (Sotet_e(sor, oszlop))
                    {
                        tabla[sor, oszlop].Image = kep;
                        tabla[sor, oszlop].Babu = new Babu(-1 * leptek, szin);
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
