using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace EmguCVAllTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateImageClick(object sender, RoutedEventArgs e)
        {
            Image<Gray, Byte> img = new Image<Gray, Byte>(480, 320, new Gray(0);

            img[0, 0] = new Gray(1234.00);
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    img[i, j] = new Gray(new Random().Next(0, 255));
                }
            }
            img.Save(@"C:\bitTest.bmp");

        }
    }
}
