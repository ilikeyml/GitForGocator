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
using System.Runtime.InteropServices;
using Lmi3d.GoSdk;
using Lmi3d.Zen;
using Lmi3d.Zen.Io;
using Lmi3d.GoSdk.Messages;




namespace TestWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            //System Initial with accelerator
            //InitialSys();
        }

        GoSystem _system;
        //GoAccelerator _goAcc;
        GoSensor _sensor;
        string IPAddress = "127.0.0.1";
        GoDataSet _dataSource;
        GoSetup _setup;
   

        private void OnDataReceived(KObject data)
        {
            //system.Stop();
            _dataSource = (GoDataSet)data;
            for (UInt32 i = 0; i < _dataSource.Count; i++)
            {
                GoDataMsg dataObj = (GoDataMsg)_dataSource.Get(i);
                switch (dataObj.MessageType)
                {
                    case GoDataMessageType.Stamp:
                        {
                            GoStampMsg stampMsg = (GoStampMsg)dataObj;
                            for (UInt32 j = 0; j < stampMsg.Count; j++)
                            {
                                GoStamp stamp = stampMsg.Get(j);
                            }
                        }
                        break;
                    case GoDataMessageType.Surface:
                        {
                            GoSurfaceMsg surface = (GoSurfaceMsg)dataObj;
                            long size = surface.Size;
                            

                        }
                        break;
                    case GoDataMessageType.Profile:
                        {
                            

                        }
                        break;

                }
            }

            // Dispose required to prevent memory leak.
            _dataSource.Dispose();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            //Connect
            goRun();

        }
        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {

            //_system.Stop();
            //_sensor.Disconnect();
            infoTxt.Text += "Stop" + Environment.NewLine;

        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        GoSystem system;
        void goRun()
        {
            try
            {
                KApiLib.Construct();
                GoSdkLib.Construct();
                system = new GoSystem();
                GoSensor sensor;
                KIpAddress ipAddress = KIpAddress.Parse(IPAddress);
                GoDataSet dataSet = new GoDataSet();
                sensor = system.FindSensorByIpAddress(ipAddress);
                sensor.Connect();
                infoTxt.Text = "Conn" + Environment.NewLine;
                GoSetup setup = sensor.Setup;
                setup.ScanMode = GoMode.Surface;

                system.EnableData(true);
                system.SetDataHandler(OnDataReceived);
                infoTxt.Text += "Binding data handler" + Environment.NewLine;

                system.Start();

                infoTxt.Text += "System Started" + Environment.NewLine;
                infoTxt.Text += "Waiting Data" + Environment.NewLine;

                GoActiveAreaConfig goActiveAreaConfig = new GoActiveAreaConfig();

                
                
                sensor.Setup.GetActiveAreaLength(GoRole.Main);
                GoSetup set = sensor.Setup;
                setup.
                
                // wait for Enter key
            }
            catch (KException ex)
            {
   
            }

        }
    }
    
}
