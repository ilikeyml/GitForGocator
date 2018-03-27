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
using Lmi3d.GoSdk.Messages;
using System.Runtime.InteropServices;
using System.IO;
using System.Timers;
using System.ComponentModel;
using System.Threading;
using System.Drawing;


namespace ProfileAnalysisTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {


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

        XMLResolve _xmlRev = XMLResolve.GetInstance();
        string IPAddr = XMLResolve.getIPAddr();
        long profileCount = 600;

        BackgroundWorker backgroundWorker;
        Rect activeArea;
        GoSystem _system;
        GoSensor _sensor;
        SynchronizationContext _syncContext;
        long _profileCount = 0;
        string CSVFilePath = @"C:\rawData.csv";

        void initialApi()
        {
            KApiLib.Construct();
            GoSdkLib.Construct();
            _syncContext = SynchronizationContext.Current;
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _syncContext.Post(delegate { UpdateMsg("Process Completed"); }, null);
            _system.Stop();
            if (_sensor.State == GoState.Ready)
            {
                _syncContext.Post(delegate { UpdateMsg("Please Stop System first"); }, null);
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _syncContext.Post(delegate { UpdateMsg("BackGroundDataStream End"); }, null);
            _syncContext.Post(delegate
            {
                __rawData = GetProfileData(_objList);

                Bitmap bmp;
                Save2CSV(__rawData, CSVFilePath);


                UpdateMsg("CSV File Saved");

                if (Gray.IsChecked == true)
                {
                    bmp = BMPGenerator8Gray(__rawData);
                    SaveXXX(bmp);
                }
                else if (Gray.IsChecked == false)
                {
                    bmp = BMPGenerator16Color(__rawData);
                    SaveXXX(bmp);
                }
                else
                {
                    bmp = BMPGenerator8Gray(__rawData);
                    SaveXXX(bmp);
                    UpdateMsg("SaveColorImageFailed");
                }

                UpdateMsg("BMP File Saved");

                IntPtr ip = bmp.GetHbitmap();//从GDI+ Bitmap创建GDI位图对象

                BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                imgBox.Source = bitmapSource;

            }, null);
        }

        public void GoSystemConstruct(string IPAddress, long profileCount)
        {
            initialApi();
            _system = new GoSystem();
            _profileCount = profileCount;
            KIpAddress addr = KIpAddress.Parse(IPAddress);
            _sensor = _system.FindSensorByIpAddress(addr);
            _syncContext.Post(delegate { yRes.Content = profileCount.ToString(); }, null);
            _system.Connect();
            if (_sensor.IsConnected())
            {
                _system.EnableData(true);
                _system.SetDataHandler(OnDataReceived);
                _syncContext.Post(delegate { UpdateMsg("Systeme Construct successfully"); }, null);
            }
            else
                _syncContext.Post(delegate { UpdateMsg("System Construct Failed"); }, null);

        }

        long keyCount = 0;
        List<KObject> _objList = new List<KObject>();
        List<ProfilePoint[]> __rawData = new List<ProfilePoint[]>();
        private void OnDataReceived(KObject data)
        {
            _syncContext.Post(delegate { UpdateMsg("Profile count: ____" + keyCount.ToString());

                keyCount++;
            }, null);
            _objList.Add(data);
            if (_objList.Count == _profileCount)
            {
                backgroundWorker.RunWorkerAsync();
                _syncContext.Post(delegate { UpdateMsg("StreamEND, Data Count" + _objList.Count.ToString()); }, null);
            }
            
        }

        public List<ProfilePoint[]> GetProfileData(List<KObject> dataObjList)
        {
            List<ProfilePoint[]> data = new List<ProfilePoint[]>();
            foreach (var item in dataObjList)
            {
                data.Add(TransProfileData(item));
            }
            return data;
        }

        DataContext _dataContext;
        ProfilePoint[] TransProfileData(KObject rawData)
        {
            ProfilePoint[] profileBuffer = null;
            GoDataSet _dataSource = new GoDataSet();
            _dataSource = (GoDataSet)rawData;
            _dataContext = new DataContext();
            ProfileShape _profileShape = new ProfileShape();
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
                            _syncContext.Post(delegate
                            {
                                xRes.Content = _dataContext.xResolution.ToString();
                                zRes.Content = _dataContext.zResolution.ToString();
                            }, null);
                            //generate csv file for point data save
                            short[] points = new short[_profileShape.width];
                            profileBuffer = new ProfilePoint[_profileShape.width];
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
                                    profileBuffer[arrayIndex].z = activeArea.Top;
                                }

                            }
                        }
                        break;
                    default: break;
                }

            }

            return profileBuffer;
        }


        public void Save2CSV(List<ProfilePoint[]> data, string filePath)
        {

            StringBuilder sbX = new StringBuilder();
            if (!File.Exists(CSVFilePath))
            {
                foreach (var item in data)
                {
                    sbX.Append(data.IndexOf(item) + ",");
                    foreach (var value in item)
                    {
                        sbX.Append(value.z.ToString() + ",");
                    }
                    sbX.Append(Environment.NewLine);
                    File.AppendAllText(CSVFilePath, sbX.ToString());

                    sbX.Clear();
                }
            }


        }



        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            if (_sensor.State == GoState.Ready)
            {
                keyCount = 0;
                _objList.Clear();
                _system.Start();

                _syncContext.Post(delegate { UpdateMsg("System Started"); }, null);
            }


        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (_sensor.State == GoState.Running)
            {
                _system.Stop();

               
                _syncContext.Post(delegate { UpdateMsg("System Stopped"); }, null);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void KeepRatio_Checked(object sender, RoutedEventArgs e)
        {

        }



        private void Window_Initialized(object sender, EventArgs e)
        {
            GoSystemConstruct(IPAddr, profileCount);
            activeArea = GetXZROI();
            if (File.Exists(CSVFilePath))
            {
                File.Delete(CSVFilePath);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_sensor.State == GoState.Running)
            {
                _system.Stop();
                //_syncContext.Post(delegate { UpdateMsg("System Stopped"); }, null);
                _system.Disconnect();
            }
        }

        void UpdateMsg(string msg)
        {
            MsgBlock.AppendText(msg + Environment.NewLine);
        }



        Bitmap BMPGenerator8Gray(List<ProfilePoint[]> rawData)
        {

            int width = rawData[0].Length;
            int height = rawData.Count;
            //int stride = (width*)
            System.Drawing.Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    double zValue = rawData[i][j].z;
                    int[] colorRGB = IntensityTransfer8bit(zValue);
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(colorRGB[0], colorRGB[0], colorRGB[0]);
                    bmp.SetPixel(j, i, color);
                }
            }
            return bmp;
        }


        Bitmap BMPGenerator16Color(List<ProfilePoint[]> rawData)
        {
            int width = rawData[0].Length;
            int height = rawData.Count;
            //int stride = (width*)
            System.Drawing.Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    double zValue = rawData[i][j].z;
                    int[] colorRGB = IntensityTrans2RGB(IntensityTransfer16bit(zValue));
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(colorRGB[0], colorRGB[1], colorRGB[2]);
                    bmp.SetPixel(j, i, color);
                }
            }

            return bmp;
        }



        void SaveXXX(Bitmap wtbBmp)
        {
            if (wtbBmp == null)
            {
                return;
            }
            try
            {
                string strDir = @"C:\XXX\";
                string strpath = strDir + DateTime.Now.ToString("yyyyMMddfff") + ".bmp";
                if (!Directory.Exists(strDir))
                {
                    Directory.CreateDirectory(strDir);
                }
                if (!File.Exists(strpath))
                {
                    wtbBmp.Save(File.OpenWrite(strpath), System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }


        int IntensityTransfer16bit(double zValue)
        {
            //获取Z向最大最小值
            double zMin = activeArea.Top;
            double zMax = activeArea.Bottom;
            return (int)((zValue - zMin) * (Math.Pow(2, 24) - 1) / (zMax - zMin));
        }


        int[] IntensityTransfer8bit(double zValue)
        {
            //获取Z向最大最小值
            double zMin = activeArea.Top;
            double zMax = activeArea.Bottom;
            int[] color = { (int)((zValue - zMin) * (Math.Pow(2, 8) - 1) / (zMax - zMin)) };
            return color;
        }

        int[] IntensityTrans2RGB(int intensity)
        {
            int rByte = intensity >> 16;
            int gByte = intensity >> 8 & 0x0000ff;
            int bByte = intensity & 0x0000ff;
            int[] color = new int[3];
            color[0] = rByte;
            color[1] = gByte;
            color[2] = bByte;

            return color;
        }


        Rect GetXZROI()
        {
            GoSetup _setup = _sensor.Setup;
            GoRole role = _sensor.Role;
            string jobName = _sensor.DefaultJob;
            _syncContext.Post(delegate { jobFile.Content = jobName; }, null);
            System.Windows.Point topLeft = new System.Windows.Point(_setup.GetActiveAreaXLimitMin(role), _setup.GetActiveAreaZLimitMax(role));
            System.Windows.Point bottomRight = new System.Windows.Point(_setup.GetActiveAreaXLimitMax(role), _setup.GetActiveAreaZLimitMin(role));
            Rect rect = new Rect(topLeft, bottomRight);

            return rect;
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            //List<ProfilePoint[]> newData = OutputKeepRatioGradient(__rawData, 0.05 / 0.002);
            List<ProfilePoint[]> newData = OutputKeepRatioAvarage(__rawData, 0.05 / 0.002); 
            Bitmap bmp = BMPGenerator8Gray(newData);
            SaveXXX(bmp);
            MessageBox.Show("Trans OK");
        }

        private void Gray_Checked(object sender, RoutedEventArgs e)
        {
            Gray.IsChecked = true;
        }


        List<ProfilePoint[]> OutputKeepRatioGradient(List<ProfilePoint[]> rawData, double rawRatio)
        {
            List<ProfilePoint[]> newData = new List<ProfilePoint[]>();
            
            int ratio = (int)Math.Ceiling(rawRatio);
            //处理前Count-1数据
            for (int i = 0; i < rawData.Count - 1; i++)
            {
                double[] delta = new double[rawData[0].Length];
                for (int j = 0; j < rawData[0].Length; j++)
                {                   
                    delta[j] = rawData[i][j].z - rawData[i + 1][j].z;
                }
                for (int k = 0; k < ratio; k++)
                {
                    ProfilePoint[] temp = new ProfilePoint[rawData[0].Length];
                    for (int w = 0; w < temp.Length; w++)
                    {
                        temp[w].x = rawData[i][w].x;
                        temp[w].z = rawData[i][w].z - k * delta[w] / ratio;
                    }
                    newData.Add(temp);
                }
            }

            //处理last stride
            double[] alpha = new double[rawData[0].Length];
            for (int j = 0; j < rawData[0].Length; j++)
            {
                alpha[j] = rawData[rawData.Count-1][j].z - 0;
            }
            for (int k = 0; k < ratio; k++)
            {
                ProfilePoint[] temp = new ProfilePoint[rawData[0].Length];
                for (int w = 0; w < temp.Length; w++)
                {
                    temp[w].x = rawData[rawData.Count - 1][w].x;
                    temp[w].z = rawData[rawData.Count - 1][w].z - k * alpha[w] / ratio;
                }
                newData.Add(temp);
            }

            return newData;
        }



        List<ProfilePoint[]> OutputKeepRatioAvarage(List<ProfilePoint[]> rawData, double rawRatio)
        {
            List<ProfilePoint[]> newData = new List<ProfilePoint[]>();
            int ratio = (int)Math.Ceiling(rawRatio);
            for (int i = 0; i < rawData.Count; i++)
            {
                for (int j = 0; j < ratio; j++)
                {
                    newData.Add(rawData[i]);
                }
            }
            return newData;
        }

        private void ActiveArea_Click(object sender, RoutedEventArgs e)
        {

        }

        private void imgBox_MouseMove(object sender, MouseEventArgs e)
        {
            //e.GetPosition()
        }
    }
}
