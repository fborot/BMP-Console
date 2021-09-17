using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace BMP_Console
{
    public partial class DrawingTest : Form
    {
        public DrawingTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(500, 500);
            Graphics g = Graphics.FromImage(bmp);
            //g.PageUnit = GraphicsUnit.Millimeter;

            g.FillRectangle(Brushes.Green, 10, 10, 50, 50);

            Pen blackPen = new Pen(Color.Black, 1);

            // Create rectangle.
            Rectangle rect = new Rectangle(10, 10, 20, 20);

            g.DrawRectangle(blackPen, rect);

            // Create string to draw.
            String drawString = "Sample Text Test test Best medical Plan";

            GraphicsPath blackfont = new GraphicsPath();
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            Font df = new Font("Arial", 11, FontStyle.Regular);
            blackfont.AddString("Sample Text Test test Best medical Plan", new FontFamily("Tahoma"), (int)FontStyle.Regular, 12,new Point(60,30), StringFormat.GenericDefault);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.FillPath(drawBrush, blackfont);    //Fill the font with White brush

                                            // Create font and brush.
                                            //Font drawFont = new Font("Arial", 6);



            // Set format of string.
            StringFormat drawFormat = new StringFormat();
            //drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            // Draw string to screen.
            g.DrawString(drawString, df, drawBrush, 60, 60, drawFormat);

            
            bmp.Save(@"bango.png", System.Drawing.Imaging.ImageFormat.Png);
            bmp.Dispose();

            g.Dispose();



            //FileInfo fi = new FileInfo(@"bango.png");
            //FileStream fstr = fi.Create();
            //Bitmap bmp2 = new Bitmap(50, 50);
            //bmp2.Save(fstr, ImageFormat.Png);
            //bmp2.Dispose();

            //fstr.Close();


        }

        private void DrawingTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.DTest = null;
        }
    }
}
