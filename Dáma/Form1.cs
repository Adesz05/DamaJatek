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
        static Mezo[,] tabla = new Mezo[meret,meret];
        public Form1()
        {
            MatrixGeneralas();

            InitializeComponent();
        }

        private void MatrixGeneralas()
        {
            bool vilagos = true;
            for (int i = 0; i < meret; i++)
            {
                for (int j = 0; j < meret; j++)
                {
                    string szin = (vilagos) ? "vilagos" : "sotet";
                    tabla[i, j] = new Mezo(new PictureBox(), false, szin);
                    vilagos = !vilagos;
                    
                }
            }
            for(int i = 0; i < meret; i++)
            {
                for (int j = 0; j < meret; j++)
                {
                    MessageBox.Show( tabla[i, j].MelyikSzin);

                }
            }
        }
    }
}
