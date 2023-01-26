﻿using System;
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
        static List<UtoUthetoLepheto> uthetomezok = new List<UtoUthetoLepheto>(); // key=lepheto val=utheto
        static bool uteskenyszer = false;
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
                        else if (tabla[i, j].MelyikBabu == "fehérdáma")
                        {
                            tabla[i, j].Image = Properties.Resources.sotetmezoFeherKoronasAmogaval;
                        }
                        else if (tabla[i, j].MelyikBabu == "feketedáma")
                        {
                            tabla[i, j].Image = Properties.Resources.sotetmezoFeketeKoronasAmogaval;
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
            if (klikkelt.MelyikBabu!="üres")
            {
                if (klikkelt.MelyikBabu == kijon || klikkelt.MelyikBabu==kijon+"dáma")
                {
                    if (UtoKenyszerVizsgalat(kijon))
                    {
                        if (uthetomezok.Count(x => x.Uto == klikkelt) > 0)
                        {
                            KijelolesekTorlese();
                            UthetoMezokMegjelenitese(klikkelt);
                            kijeloltbabu = klikkelt;
                        }
                    }
                    //Nincs ütéskényszer
                    else
                    {
                        //a kijelölt bábu tud-e mozogni
                        if (TudeMozogni(klikkelt))
                        {
                            KijelolesekTorlese();
                            LephetoMezoKijeloles(klikkelt);
                            kijeloltbabu = klikkelt;
                        }
                    }
                }
            }
            else
            {
                //üres mezőre kattint
                if (kijeloltbabu!=null)
                {
                    if (klikkelt.MelyikSzin == "kijelölt")
                    {
                        if (uthetomezok.Count(x => x.Lepheto==klikkelt && x.Uto == kijeloltbabu) > 0)
                        {
                            uthetomezok.Find(x => x.Lepheto == klikkelt && x.Uto == kijeloltbabu).Lepheto.MelyikBabu = kijeloltbabu.MelyikBabu;
                            uthetomezok.Find(x => x.Lepheto == klikkelt && x.Uto == kijeloltbabu).Lepheto.Image = kijeloltbabu.Image;
                            uthetomezok.Find(x => x.Lepheto == klikkelt && x.Uto == kijeloltbabu).Lepheto.MelyikSzin="sötét";
                            uthetomezok.Find(x => x.Lepheto == klikkelt && x.Uto == kijeloltbabu).Utheto.MelyikBabu="üres";
                            uthetomezok.Find(x => x.Lepheto == klikkelt && x.Uto == kijeloltbabu).Utheto.Image = Properties.Resources.sotet;
                            uthetomezok.Find(x => x.Lepheto == klikkelt && x.Uto == kijeloltbabu).Uto.Image = Properties.Resources.sotet;
                            uthetomezok.Find(x => x.Lepheto == klikkelt && x.Uto == kijeloltbabu).Uto.MelyikBabu="üres";
                            kijeloltbabu = null;
                            KijelolesekTorlese();
                            uthetomezok.Clear();
                            EgyBabuUtoKenyszer(klikkelt.Koordinatak.X, klikkelt.Koordinatak.Y);
                            if (uthetomezok.Count > 0)
                            {
                                //ujra ütünk
                            }


                        }
                        //Mozgunk oda
                    }
                    else
                    {
                        //levesszük a kijelölést
                    }
                }
            }
        }

        private void LephetoMezoKijeloles(Mezo klikkelt)
        {
            //int irany = klikkelt.MelyikBabu.Contains("fehér") ? -1 : 1;
            int hanyatlephet = klikkelt.MelyikBabu.Contains("dáma") ? meret - 1 : 1;
            for (int i = -1; i <= 1; i += 2)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    for (int k = 1; k <= hanyatlephet; k++)
                    {
                        try
                        {
                            if (klikkelt.MelyikBabu.Contains("dáma") || klikkelt.MelyikBabu.Contains("fehér") && j*k < 0)
                            {
                                if (tabla[klikkelt.Koordinatak.X + i*k, klikkelt.Koordinatak.Y + j*k].MelyikBabu == "üres")
                                {
                                    tabla[klikkelt.Koordinatak.X + i *k, klikkelt.Koordinatak.Y + j*k].Image = Properties.Resources.sotetzoldpotyivelakozepen;
                                    tabla[klikkelt.Koordinatak.X + i * k, klikkelt.Koordinatak.Y + j*k].MelyikSzin = "kijelölt";
                                }
                                else { break; }
                            }
                            if (klikkelt.MelyikBabu.Contains("dáma") || klikkelt.MelyikBabu.Contains("fekete") && j*k > 0)
                            {
                                if (tabla[klikkelt.Koordinatak.X + i*k, klikkelt.Koordinatak.Y + j*k].MelyikBabu == "üres")
                                {
                                    tabla[klikkelt.Koordinatak.X + i*k, klikkelt.Koordinatak.Y + j*k].Image = Properties.Resources.sotetzoldpotyivelakozepen;
                                    tabla[klikkelt.Koordinatak.X + i*k, klikkelt.Koordinatak.Y + j*k].MelyikSzin = "kijelölt";
                                }
                                else { break; }
                            }
                        }
                        catch (Exception) { break; }
                    }
                }
            }
        }

        private bool TudeMozogni(Mezo klikkelt)
        {
            for (int i = -1; i <= 1; i+=2)
            {
                for (int j = -1; j <= 1; j+=2)
                {
                    try
                    {
                        if (klikkelt.MelyikBabu.Contains("dáma") || klikkelt.MelyikBabu.Contains("fehér") && j<0)
                        {
                            if (tabla[klikkelt.Koordinatak.X + i, klikkelt.Koordinatak.Y + j].MelyikBabu == "üres")
                            {
                                return true;
                            }
                        }
                        if (klikkelt.MelyikBabu.Contains("dáma") || klikkelt.MelyikBabu.Contains("fekete") && j > 0)
                        {
                            if (tabla[klikkelt.Koordinatak.X + i, klikkelt.Koordinatak.Y + j].MelyikBabu == "üres")
                            {
                                return true;
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
            return false;
        }

        private void KijelolesekTorlese()
        {
            for (int i = 0; i < meret; i++)
            {
                for (int j = 0; j < meret; j++)
                {
                    if (tabla[i,j].MelyikSzin=="kijelölt")
                    {
                        tabla[i, j].MelyikSzin = "sötét";
                        tabla[i, j].Image=Properties.Resources.sotet;
                    }
                }
            }
        }

        private void UthetoMezokMegjelenitese(Mezo klikkelt)
        {
            foreach (UtoUthetoLepheto item in uthetomezok)
            {
                if (item.Uto==klikkelt)
                {
                    item.Lepheto.Image = Properties.Resources.sotetzoldpotyivelakozepen;
                    item.Lepheto.MelyikSzin = "kijelölt";
                }
            }
        }

        private bool UtoKenyszerVizsgalat(string kijon)
        {
            uthetomezok.Clear();
            for (int i = 0; i < meret; i++)
            {
                for (int j = 0; j < meret; j++)
                {
                    if (tabla[i,j].MelyikBabu.Contains(kijon))
                    {
                        EgyBabuUtoKenyszer(i, j);
                    }
                }
            }
            return uthetomezok.Count != 0;
        }

        private void EgyBabuUtoKenyszer(int i, int j)
        {
            for (int irany1 = -1; irany1 <= 1; irany1 += 2)
            {
                for (int irany2 = -1; irany2 <= 1; irany2 += 2)
                {
                    for (int k = 1; k <= (tabla[i, j].MelyikBabu.Contains("dáma") ? meret - 1 : 1); k++)
                    {
                        try
                        {
                            if (tabla[i + irany1 * k, j + irany2 * k].MelyikBabu.Contains("üres"))
                            {
                                continue;
                            }
                            if (!tabla[i + irany1 * k, j + irany2 * k].MelyikBabu.Contains(kijon))
                            {
                                if (tabla[i + irany1 * (k + 1), j + irany2 * (k + 1)].MelyikBabu.Contains("üres"))
                                {
                                    UthetoMezoMentese(i, j, i + irany1 * k, j + irany2 * k, i + irany1 * (k + 1), j + irany2 * (k + 1));
                                }
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch (Exception) { break; }
                    }
                }
            }
        }

        private void UthetoMezoMentese(int utoX, int utoY, int uthetoX, int uthetoY, int lephetoX, int lephetoY)
        {
            uthetomezok.Add(new UtoUthetoLepheto(tabla[utoX,utoY], tabla[uthetoX, uthetoY], tabla[lephetoX, lephetoY]));


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
                        melyikbabu = "feketedáma";
                        feherbabuk++;
                    }
                    else melyikbabu = "üres";
                    if (futasokszama > 39 && !vilagos && futasokszama%2==0)
                    {
                        melyikbabu = "fehérdáma";
                    }

                    tabla[j,i] = new Mezo((vilagos) ? "világos" : "sötét",melyikbabu, new Point());
                    if (j!=7) vilagos = !vilagos;
                    futasokszama++;
                }
            }

            //ütéses mozagasos
            /*
             for (int i = 0; i < meret; i++)
            {
                for (int j = 0; j < meret; j++)
                {
                    if (!vilagos && feherbabuk < 8 && i!=1 && futasokszama>15)
                    {
                        melyikbabu = "fekete";
                        feherbabuk++;
                    }
                    else melyikbabu = "üres";
                    if (futasokszama > 23 && !vilagos)
                    {
                        melyikbabu = "fehérdáma";
                    }

                    tabla[j,i] = new Mezo((vilagos) ? "világos" : "sötét",melyikbabu, new Point());
                    if (j!=7) vilagos = !vilagos;
                    futasokszama++;
                }
            }
            */
             
        }
    }
}
