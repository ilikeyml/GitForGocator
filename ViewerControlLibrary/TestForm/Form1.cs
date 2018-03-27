using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewerControlLibrary;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ConfigViewer cv = new ConfigViewer(elementHost1);
            ConfigViewer cv = new ConfigViewer();
            this.elementHost1.Child = cv;

            double width = cv.Width;

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
