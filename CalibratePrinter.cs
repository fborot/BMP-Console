using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nini.Config;

namespace BMP_Console
{
    public partial class CalibratePrinter : Form
    {
        public CalibratePrinter()
        {
            InitializeComponent();
        }

        private void CalibratePrinter_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.CPrinter = null;
        }

        private void CalibratePrinter_Load(object sender, EventArgs e)
        {
            IniConfigSource source = new IniConfigSource("./Settings/settings.ini");
            try
            {
               tbX.Text = source.Configs["Printer"].Get("X_Offset", "5");
               tbY.Text = source.Configs["Printer"].Get("Y_Offset", "5");
               tbFSize.Text = source.Configs["Printer"].Get("FontSize", "12");

               Form1.X_Offset = Int32.Parse(tbX.Text);
               Form1.Y_Offset = Int32.Parse(tbY.Text);
               Form1.Font_Size = float.Parse(tbFSize.Text); 

            }
            catch (Nini.Ini.IniException NINI_ex)
            {
                source = null; //I got a nini exception
                MessageBox.Show("Error loading keywords from ini file. {0}", NINI_ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IniConfigSource source = new IniConfigSource("./Settings/settings.ini");
            try
            {
                source.Configs["Printer"].Set("X_Offset", tbX.Text);
                source.Configs["Printer"].Set("Y_Offset", tbY.Text);
                source.Configs["Printer"].Set("FontSize", tbFSize.Text);
                source.Save();
            }
            catch (Nini.Ini.IniException NINI_ex)
            {
                source = null; //I got a nini exception
                MessageBox.Show("Error loading keywords from ini file. {0}", NINI_ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //printDialog1.Document = printDocument1;
            //if (printDialog1.ShowDialog() == DialogResult.OK) {
            //    printDocument1.Print();
            //}

            printPreviewDialog1.Document = printDocument1;
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            Pen p1 = new Pen(Brushes.Black);
            p1.Width = 0.5F;
            Font font = new Font(FontFamily.GenericSansSerif, 12);
            e.Graphics.DrawRectangle(p1, new Rectangle(new Point(0, 0), new Size(100, 50)));

            e.Graphics.DrawString("BMP Printer Test Page", font, Brushes.Black, 10, 10);

            e.HasMorePages =  false;
        }
    }
}
