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
using Lmi3d.GoSdk;
using Lmi3d.Zen;
using Lmi3d.Zen.Io;
using Lmi3d.Zen.Data;
using Lmi3d.GoSdk.Messages;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace ProfileOps
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 



    public class DataContext
    {
        public Double xResolution;
        public Double zResolution;
        public Double xOffset;
        public Double zOffset;
        public uint serialNumber;
    }


    public class ProfileShape
    {
        public int width;
        public long size;
    }

    public struct ProfilePoint
    {
        public double x;
        public double z;
        byte intensity;
    }


    public struct GoPoints
    {
        public Int16 x;
        public Int16 y;
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        GoSystem _system;
        GoSensor _sensor;
        GoSensorInfo _sensorInfo;
        List<KObject> _dataList;
        static string ipAddr = "127.0.0.1";
        SynchronizationContext _syncContext;
        BackgroundWorker _profileProcessWorker;
        Mutex mutex;
        public static string filePath = @"C:\ProfileData.csv";
        private void Window_Closed(object sender, EventArgs e)
        {




        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            initialApi();
            _syncContext = SynchronizationContext.Current;
            _dataList = new List<KObject>();
            _profileProcessWorker = new BackgroundWorker();
            _profileProcessWorker.DoWork += _profileProcessWorker_DoWork;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }

        private void _profileProcessWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var item in _dataList)
            {
                ProfileProcessingFunc(item, _dataList.IndexOf(item));
            }
        }


        void ProfileProcessingFunc(KObject data, int dataQuene)
        {

            GoDataSet _dataSource = new GoDataSet();
            _dataSource = (GoDataSet)data;
            DataContext _dataContext = new DataContext();
            ProfileShape _profileShape = new ProfileShape();
            StringBuilder sb = new StringBuilder();

            for (UInt32 i = 0; i < _dataSource.Count; i++)
            {
                GoDataMsg dataObj = (GoDataMsg)_dataSource.Get(i);
                switch (dataObj.MessageType)
                {
                    case GoDataMessageType.ResampledProfile:
                        {
                            GoResampledProfileMsg profileMsg = (GoResampledProfileMsg)dataObj;
                            _dataContext.xResolution = (double)profileMsg.XResolution / 1000000;
                            _dataContext.zResolution = (double)profileMsg.ZResolution / 1000000;
                            _dataContext.xOffset = (double)profileMsg.XOffset / 1000;
                            _dataContext.zOffset = (double)profileMsg.ZOffset / 1000;
                            _profileShape.width = profileMsg.Width;
                            _profileShape.size = profileMsg.Size;
                            //generate csv file for point data save
                            _syncContext.Post(new SendOrPostCallback(delegate {
                                updateMsg("x resolution:" + _dataContext.xResolution.ToString());
                                updateMsg("z resolution:" + _dataContext.zResolution.ToString());

                            }), null);
                            short[] points = new short[_profileShape.width];
                            ProfilePoint[] profileBuffer = new ProfilePoint[_profileShape.width];
                            IntPtr pointsPtr = profileMsg.Data;
                            Marshal.Copy(pointsPtr, points, 0, points.Length);

                            for (UInt32 arrayIndex = 0; arrayIndex < _profileShape.width; ++arrayIndex)
                            {

                                if (points[arrayIndex] != -32768)
                                {
                                    profileBuffer[arrayIndex].x = _dataContext.xOffset + _dataContext.xResolution * arrayIndex;
                                    profileBuffer[arrayIndex].z = _dataContext.zOffset + _dataContext.zResolution * points[arrayIndex];

                                }
                                else
                                {
                                    profileBuffer[arrayIndex].x = _dataContext.xOffset + _dataContext.xResolution * arrayIndex;
                                    profileBuffer[arrayIndex].z = -32768;
                                }
                            }


                            sb.Clear();

                            if (!File.Exists(filePath))
                            {

                                sb.Append("Index,");

                                for (int k = 0; k < profileBuffer.Length; k++)
                                {
                                    sb.Append(profileBuffer[k].x.ToString() + ",");
                                }
                                sb.Append(Environment.NewLine);
                                File.AppendAllText(filePath, sb.ToString());
                                //updateMsg("Create file sucessfully");
                                sb.Clear();

                            }

                            sb.Append(dataQuene.ToString() + ",");
                            //Get Value   save to file
                            for (int k = 0; k < profileBuffer.Length; k++)
                            {

                                sb.Append(profileBuffer[k].z.ToString() + ",");
                            }
                            sb.Append(Environment.NewLine);
                            // write to file                        
                            File.AppendAllText(filePath, sb.ToString());
                            sb.Clear();
                        }
                        break;
                    default: break;
                }

            }
        }



        void updateMsg(string msg)
        {
            MsgBlock.AppendText(msg + Environment.NewLine);

        }


        bool initialApi()
        {
            updateMsg("default ip address:" + ipAddr);
            updateMsg("Initial API construct");
            KApiLib.Construct();
            GoSdkLib.Construct();
            _system = new GoSystem();
            KIpAddress addr = KIpAddress.Parse(ipAddr);
            _sensor = _system.FindSensorByIpAddress(addr);
            _sensorInfo = new GoSensorInfo();
            if (_sensor != null)
            {
                updateMsg("Find Device");
            }
            else
            {
                updateMsg("Device not found");
                return false;
            }
            _sensor.Connect();
            if (_sensor.IsConnected())
            {
                updateMsg("Sensor Connected");
                string IDStr = _sensor.Id.ToString();

                updateMsg("Sensor ID:" + IDStr);
                _system.EnableData(true);
                updateMsg("Enable Data");
                _sensor.SetDataHandler(OnDataReceived);
                updateMsg("regist data handller");
                updateMsg("Waiting data....");
                string sts = _sensor.State.ToString();
                updateMsg("sensor current state:" + sts);
                return true;
            }

            return false;
        }

        string timeStamp()
        {
            string timeStr = DateTime.Now.ToLongTimeString();

            return timeStr;
        }
        int count = 1;
        int index = 0;

        private void OnDataReceived(KObject data)
        {
            _dataList.Add(data);
            if (_dataList.Count == 52)
            {
                _syncContext.Post(new SendOrPostCallback(delegate { updateMsg("Data Stream End"); updateMsg(_dataList.Count.ToString()); }), null);

                //beging data processing thread
                _profileProcessWorker.RunWorkerAsync();
            }
        }


        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _system.Stop();
            string sts = _sensor.State.ToString();
            updateMsg("sensor current state:" + sts);
            updateMsg("System stopped");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {

            if (_sensor.IsConnected())
            {
                _sensor.Disconnect();
                updateMsg("Disconn");

            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

            _sensor.Start();
            string sts = _sensor.State.ToString();
            updateMsg("sensor current state:" + sts);
            updateMsg("Started");
        }




        BitmapImage ImageGenerator(List<ProfilePoint> pointList)
        {

            BitmapImage bitmap = new BitmapImage();
            
            return null;
        }

    }
}
