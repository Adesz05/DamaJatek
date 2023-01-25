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
            KijelolesekTorlese();
            if (klikkelt.MelyikBabu!="üres")
            {
                if (klikkelt.MelyikBabu == kijon || klikkelt.MelyikBabu==kijon+"dáma")
                {
                    if (UtoKenyszerVizsgalat(kijon))
                    {
                        if (uthetomezok.Count(x => x.Uto == klikkelt) > 0)
                        {
                            UthetoMezokMegjelenitese(klikkelt);
                        }
                    }
                    //Nincs ütéskényszer
                    else
                    {
                        MessageBox.Show("lépjé paraszt");
                    }
                }
            }

            /*if (!vanekijeloltbabu)
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

                            uthetomezok.Add(tabla[klikkelt.Koordinatak.X-2,klikkelt.Koordinatak.Y-2],tabla[klikkelt.Koordinatak.X - 1, klikkelt.Koordinatak.Y - 1]);
                            kijeloltbabu = klikkelt;
                            uteskenyszer = true;
                        }
                    }

                    //jobbra lehet mozogni
                    if (klikkelt.Koordinatak.X != 7)
                    {
                        //jobbra 1db átló mélység vizsgálat
                        if (tabla[klikkelt.Koordinatak.X + 1, klikkelt.Koordinatak.Y - 1].MelyikBabu == "üres" && !uteskenyszer)
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
                            uthetomezok.Add(tabla[klikkelt.Koordinatak.X + 2, klikkelt.Koordinatak.Y - 2],tabla[klikkelt.Koordinatak.X + 1, klikkelt.Koordinatak.Y - 1]);
                            kijeloltbabu = klikkelt;
                            uteskenyszer = true;
                        }
                       

                    }
                    if (uteskenyszer && tabla[klikkelt.Koordinatak.X - 1, klikkelt.Koordinatak.Y - 1].MelyikBabu == "üres")
                    {
                        tabla[klikkelt.Koordinatak.X - 1, klikkelt.Koordinatak.Y - 1].Image = Properties.Resources.sotet;
                    }
                    uteskenyszer = false;
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
                        if (kijon=="fehér")
                        {
                            tabla[mezokahovalepnilehet[i].Koordinatak.X, mezokahovalepnilehet[i].Koordinatak.Y].Image =Properties.Resources.sotetmezoFeherAmogaval;
                        }
                        else
                        {
                            tabla[mezokahovalepnilehet[i].Koordinatak.X, mezokahovalepnilehet[i].Koordinatak.Y].Image =Properties.Resources.sotetmezoFeketeAmogaval;

                        }
                        tabla[mezokahovalepnilehet[i].Koordinatak.X, mezokahovalepnilehet[i].Koordinatak.Y].MelyikBabu=kijeloltbabu.MelyikBabu;
                        tabla[kijeloltbabu.Koordinatak.X, kijeloltbabu.Koordinatak.Y].Image = Properties.Resources.sotet;
                        tabla[kijeloltbabu.Koordinatak.X, kijeloltbabu.Koordinatak.Y].MelyikBabu="üres";
                        foreach (KeyValuePair<Mezo, Mezo> item in uthetomezok)
                        {
                            if (item.Key.Koordinatak==klikkelt.Koordinatak)
                            {
                                item.Value.Image = Properties.Resources.sotet;
                                item.Value.MelyikBabu= "üres";

                            }
                        }
                    }
                }
                mezokahovalepnilehet.Clear();
                uthetomezok.Clear();
            }
            

            //dáma if
            // || klikkelt.MelyikBabu == kijon + "dáma"
            */
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
                        for (int irany1 = -1; irany1 <= 1; irany1+=2)
                        {
                            for (int irany2 = -1; irany2 <= 1; irany2+=2)
                            {
                                for (int k = 1; k <= (tabla[i, j].MelyikBabu.Contains("dáma") ? 8 : 1); k++)
                                {
                                    try
                                    {
                                        if (!tabla[i + irany1 * k, j + irany2 * k].MelyikBabu.Contains("üres") && !tabla[i + irany1 * k, j + irany2 * k].MelyikBabu.Contains(kijon))
                                        {
                                            if (tabla[i + irany1 * (k+1), j + irany2 * (k+1)].MelyikBabu.Contains("üres"))
                                            {
                                                UthetoMezoMentese(i, j, i+irany1*k, j+irany2*k, i + irany1 * (k+1), j + irany2 * (k+1));
                                            }
                                            break;
                                        }
                                    }
                                    catch (Exception) { }
                                }
                            }
                        }
                        
                    }
                }
            }
            return uthetomezok.Count != 0;
        }

        private void UthetoMezoMentese(int utoX, int utoY, int uthetoX, int uthetoY, int lephetoX, int lephetoY)
        {
            uthetomezok.Add(new UtoUthetoLepheto(tabla[utoX,utoY], tabla[uthetoX, uthetoY], tabla[lephetoX, lephetoY]));


        }

        private void BalOldaliKijelolesVisszaAllitasa()
        {
            
        }

        private void MatrixGeneralas()
        {
            int futasokszama = 0;
            int feherbabuk = 0;
            bool vilagos = true;
            string melyikbabu = "üres";

/*
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
            }*/

            //ütéses mozagasos
            
             for (int i = 0; i < meret; i++)
            {
                for (int j = 0; j < meret; j++)
                {
                    if (!vilagos && feherbabuk < 8 && i!=1 && futasokszama>15)
                    {
                        melyikbabu = "feketedáma";
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
             
        }
    }
}
