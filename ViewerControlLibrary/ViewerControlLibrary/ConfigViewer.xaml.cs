using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.UI;
using System.Windows.Interop;
using System.Windows.Controls;


namespace ViewerControlLibrary
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigViewer : UserControl
    {
        public ConfigViewer()
        {
            InitializeComponent();
            _pictureBox = new PanAndZoomPictureBox();
            _regionsWindow = new RegionsWindow();




        }
        public ConfigViewer(double _parentWidth, double _parentHeight)
        {
            wholeWidth = _parentWidth;
            wholeHeight = _parentHeight;
        }

        PanAndZoomPictureBox _pictureBox;
        double wholeWidth = 0;
        double wholeHeight = 0;
        RegionsWindow _regionsWindow;

        private void ConfigViewerControl_Initialized(object sender, EventArgs e)
        {


        }

        private void mainCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _regionsWindow.Show();
        }
    }
}
