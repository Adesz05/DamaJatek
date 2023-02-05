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
        static Mezo kijeloltbabu = null;
        static List<UtoUthetoLepheto> uthetomezok = new List<UtoUthetoLepheto>(); // key=lepheto val=utheto
        static bool uteskenyszer = false;
        static bool utessorozat = false;
        static bool szerkesztes = false;
        static Image szerkesztoKep;
        static string szerkesztoBabu;
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
                    tabla[i, j].Location = new Point(50+i*mezomeret,j*mezomeret);
                    tabla[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    tabla[i, j].MouseClick += new MouseEventHandler(Klikkeles);
                    tabla[i, j].MouseEnter += Form1_MouseEnter;
                    tabla[i, j].MouseLeave += Form1_MouseLeave;
                    tabla[i, j].Koordinatak = new Point(i, j);
                    this.Controls.Add(tabla[i, j]);

                }
            }
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            Mezo mezo = sender as Mezo;
            if (mezo.MelyikSzin == "kijelölt")
            {
                mezo.Image = Properties.Resources.sotetzoldpotyivelakozepen;
            }
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            Mezo mezo = sender as Mezo;
            if (mezo.MelyikSzin == "kijelölt")
            {
                mezo.Image = Properties.Resources.sotetzoldmezo;
            }
        }

        private void Klikkeles(object sender, MouseEventArgs e)
        {
            Mezo klikkelt = sender as Mezo;

            if (szerkesztes)
            {
                Szerkesztes(klikkelt, e);
                return;
            }

            //MessageBox.Show(klikkelt.Koordinatak.X + " meg " + klikkelt.Koordinatak.Y);
            //fehér------------------------------------------------------------------------------------------------------------------------------------------------------
            if (klikkelt.MelyikBabu!="üres")
            {
                if (utessorozat)
                {
                    return;
                }
                if (klikkelt.MelyikBabu == kijon || klikkelt.MelyikBabu==kijon+"dáma")
                {
                    if (UtoKenyszerVizsgalat(kijon))
                    {
                        if (uthetomezok.Count(x => x.Uto == klikkelt) > 0)
                        {
                            KijelolesekTorlese();
                            UthetoMezokMegjelenitese(klikkelt);
                            kijeloltbabu = klikkelt;
                            kijeloltbabu.BorderStyle = BorderStyle.Fixed3D;
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
                            kijeloltbabu.BorderStyle = BorderStyle.Fixed3D;
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
                            UtoUthetoLepheto utesmezo = uthetomezok.Find(x => x.Lepheto == klikkelt && x.Uto == kijeloltbabu);
                            
                            Mozgas(utesmezo.Uto, utesmezo.Lepheto);
                            Utes(utesmezo.Utheto);
                            if (Gyozelem_e())
                            {
                                if (DialogResult.Yes == MessageBox.Show($"A Győztes a {kijon} bábu Játékosa\nAkarsz új játékot kezdeni?", "Győzelem", MessageBoxButtons.YesNo))
                                {
                                    Application.Restart();
                                }
                                Application.Exit();
                            }
                            KijelolesekTorlese();
                            uthetomezok.Clear();
                            EgyBabuUtoKenyszer(klikkelt.Koordinatak.X, klikkelt.Koordinatak.Y);
                            utessorozat = uthetomezok.Count() > 0;
                            if (utessorozat)
                            {
                                kijeloltbabu = klikkelt;
                                kijeloltbabu.BorderStyle = BorderStyle.Fixed3D;
                                UthetoMezokMegjelenitese(klikkelt);
                            }
                            if (DamaLett_e(klikkelt) || !(uthetomezok.Count > 0))
                            {
                                JatekosCsere();
                            }
                            if (DamaLett_e(klikkelt))
                            {
                                klikkelt.MelyikBabu += "dáma";
                                klikkelt.Image = kijon == "fehér" ? Properties.Resources.sotetmezoFeketeKoronasAmogaval : Properties.Resources.sotetmezoFeherKoronasAmogaval;
                            }
                        }
                        else
                        {
                            //Mozgunk oda
                            Mozgas(kijeloltbabu, klikkelt);
                            kijeloltbabu.BorderStyle = BorderStyle.None;
                            kijeloltbabu = null;
                            KijelolesekTorlese();
                            uthetomezok.Clear();
                            if (DamaLett_e(klikkelt))
                            {
                                klikkelt.MelyikBabu += "dáma";
                                klikkelt.Image = kijon == "fehér" ? Properties.Resources.sotetmezoFeherKoronasAmogaval : Properties.Resources.sotetmezoFeketeKoronasAmogaval; 
                            }
                            JatekosCsere();
                        }
                    }
                    else if (!utessorozat)
                    {
                        KijelolesekTorlese();
                    }
                }
            }
        }

        private void Szerkesztes(Mezo klikkelt, MouseEventArgs e)
        {
            if (klikkelt.MelyikSzin != "világos")
            {
                if (e.Button == MouseButtons.Right)
                {
                    klikkelt.MelyikBabu = "üres";
                    klikkelt.Image = Properties.Resources.sotet;
                }
                else if(e.Button == MouseButtons.Left && szerkesztoKep != null)
                {
                    klikkelt.Image = szerkesztoKep;
                    klikkelt.MelyikBabu = szerkesztoBabu;
                }
            }
        }

        private void JatekosCsere()
        {
            kijon = new List<string>() { "fekete", "fehér" }.Find(x => x != kijon);
            this.Text = $"Dáma, Következik: {kijon}";
            pictureBox1.Image = kijon == "fehér" ? Properties.Resources.FeherKoronasAmoga : Properties.Resources.FeketeKoronasAmoga;
        }

        private bool Gyozelem_e()
        {
            string ellenfel = new List<string>() { "fekete", "fehér" }.Find(x => x != kijon);
            for (int i = 0; i < meret; i++)
            {
                for (int j = 0; j < meret; j++)
                {
                    if (tabla[i, j].MelyikBabu.Contains(ellenfel))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void Utes(Mezo utheto)
        {
            utheto.MelyikBabu = "üres";
            utheto.Image = Properties.Resources.sotet;
        }

        private void Mozgas(Mezo honnan, Mezo hova)
        {
            hova.MelyikBabu = honnan.MelyikBabu;
            hova.MelyikSzin = "sötét";
            hova.Image = honnan.Image;
            honnan.MelyikBabu = "üres";
            honnan.MelyikSzin = "sötét";
            honnan.Image = Properties.Resources.sotet;
            if (Beszoritott_e())
            {
                if (DialogResult.Yes == MessageBox.Show($"A Győztes a {kijon} bábu Játékosa\nAkarsz új játékot kezdeni?", "Győzelem", MessageBoxButtons.YesNo))
                {
                    Application.Restart();
                }
                Application.Exit();
            }
            
        }

        private bool Beszoritott_e()
        {
            for (int sor = 0; sor < meret; sor++)
            {
                for (int oszlop = 0; oszlop < meret; oszlop++)
                {
                    if (!tabla[sor, oszlop].MelyikBabu.Contains(kijon) && tabla[sor, oszlop].MelyikBabu != "üres")
                    {
                        if (TudeMozogni(tabla[sor, oszlop]))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool DamaLett_e(Mezo klikkelt)
        {
            if (klikkelt.Koordinatak.Y == 0 && klikkelt.MelyikBabu == "fehér" || klikkelt.Koordinatak.Y == meret-1 && klikkelt.MelyikBabu == "fekete")
            {
                return true;
            }
            return false;
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
            if (kijeloltbabu != null)
            {
                kijeloltbabu.BorderStyle = BorderStyle.None;
            }
            kijeloltbabu = null;
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

        private void HowTo_MouseEnter(object sender, EventArgs e)
        {
            groupBox1.BringToFront();
            groupBox1.Visible = true;
        }

        private void HowTo_MouseLeave(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void HowTo_MouseMove(object sender, MouseEventArgs e)
        {
            groupBox1.Location = new Point(RelMousPoz().X - groupBox1.Width - 10, RelMousPoz().Y);
        }

        private Point RelMousPoz()
        {
            return new Point(MousePosition.X - this.Location.X - 8, MousePosition.Y - this.Location.Y - 38);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                szerkesztes = true;
                szerkesztesGbox.Visible = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                szerkesztes = false;
                szerkesztesGbox.Visible = false;
            }

        }

        private void ClearMapBtn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.Enabled = false;
            for (int sor = 0; sor < meret; sor++)
            {
                for (int oszlop = 0; oszlop < meret; oszlop++)
                {
                    if (tabla[sor, oszlop].MelyikSzin != "világos")
                    {
                        tabla[sor, oszlop].MelyikBabu = "üres";
                        tabla[sor, oszlop].Image = Properties.Resources.sotet;
                    }
                }
            }
            btn.Enabled = true;
        }

        

        private void SzerkesztoKepKivalasztas(object sender, string melyikbabu, Image kep)
        {
            PictureBox pBox = sender as PictureBox;
            szerkesztoKep = kep;
            szerkesztoBabu = melyikbabu;


            foreach (PictureBox item in new List<PictureBox>() { pictureBox2, pictureBox3, pictureBox4, pictureBox5 })
            {
                item.BorderStyle = BorderStyle.None;
            }
            pBox.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            SzerkesztoKepKivalasztas(sender, "feketedáma", Properties.Resources.sotetmezoFeketeKoronasAmogaval);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SzerkesztoKepKivalasztas(sender, "fehér", Properties.Resources.sotetmezoFeherAmogaval);
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            SzerkesztoKepKivalasztas(sender, "fehérdáma", Properties.Resources.sotetmezoFeherKoronasAmogaval);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            SzerkesztoKepKivalasztas(sender, "fekete", Properties.Resources.sotetmezoFeketeAmogaval);
        }
    }
}
