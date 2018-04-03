using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            imageBoxEx1.MouseClick += ImageBoxEx1_MouseClick;
            
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

        private void button3_Click(object sender, EventArgs e)
        {
            imageBoxEx1.SelectionMode = LMI.Gocator.Tools.ImageBoxSelectionMode.Rectangle;
         
        }
    }
}
