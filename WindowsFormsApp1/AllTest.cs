using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WindowsFormsApp1
{
    public partial class AllTest : Form
    {
        public AllTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            imageBoxEx1.MouseClick += ImageBoxEx1_MouseClick;
            //imageBoxEx1.SizeMode = LMI.Gocator.Tools.ImageBoxSizeMode.Fit;


        }

        private void ImageBoxEx1_MouseClick(object sender, MouseEventArgs e)
        {
            if(imageBoxEx1.IsPointInImage(e.Location))
            {
                Point p;
                p = imageBoxEx1.PointToImage(e.Location);
                button1.Text = p.ToString();

            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            imageBoxEx1.SelectionColor = Color.Blue;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            imageBoxEx1.SelectionMode = LMI.Gocator.Tools.ImageBoxSelectionMode.None;
            imageBoxEx1.SelectionRegion = RectangleF.Empty;
    
        }

        bool ShowFlag = false;

        private void button3_Click(object sender, EventArgs e)
        {

            //imageBoxEx1.AddRectToGraphics()
            //imageBoxEx1.Invalidate();

            imageBoxEx1.SelectionMode = LMI.Gocator.Tools.ImageBoxSelectionMode.Rectangle;
            imageBoxEx1.SelectionRegion = new RectangleF(10, 10, 20, 20);

        }

        private void imageBoxEx1_Paint(object sender, PaintEventArgs e)
        {

            if(ShowCheck.Checked)
            {
                e.Graphics.SetClip(imageBoxEx1.GetInsideViewPort(true));
                imageBoxEx1.Invalidate();
                imageBoxEx1.AddRectToGraphics(imageBoxEx1.GetOffsetRectangle(new RectangleF(0, 0, 100, 100)), new Pen(Color.Red), new SolidBrush(Color.FromArgb(128,122,33,44)));
                imageBoxEx1.AddRectToGraphics(imageBoxEx1.GetOffsetRectangle(new RectangleF(100, 100, 100, 100)), new Pen(Color.Black), new SolidBrush(Color.FromArgb(128,33,44,123)));
                imageBoxEx1.AddRectToGraphics(imageBoxEx1.GetOffsetRectangle(new RectangleF(200, 200, 100, 100)), new Pen(Color.BlueViolet), new SolidBrush(Color.FromArgb(128,11,222,45)));
                imageBoxEx1.AddLineToGraphics(imageBoxEx1.GetOffsetPoint(new Point(0, 0)), imageBoxEx1.GetOffsetPoint( new PointF(300, 300)), new Pen(Color.Red,4));
                imageBoxEx1.DrawGraphics(e.Graphics);
                imageBoxEx1.ClearGraphicsGroup();
                e.Graphics.ResetClip();
            }


  

        }

        private void ShowCheck_CheckedChanged(object sender, EventArgs e)
        {

                imageBoxEx1.Invalidate();

        }
    }
}
