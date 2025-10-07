using System.Text;
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
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing; // For Bitmap
using System.Windows.Media.Imaging;



namespace Info_Projektkurs1
{
    public partial class MainWindow : Window
    {
        private VideoCapture _capture; //VideoCapture-Objekt
        private Mat _frame;            //Image frame
        private bool _isRunning;       //when camera is running
        public MainWindow()
        {
            InitializeComponent();
            _frame = new Mat();
            _isRunning = false;
            InitializeCamera();

        }

        private void InitializeCamera()
        {
            try
            {
                //inizialize camera with id 0
                _capture = new VideoCapture(0);
                _capture.ImageGrabbed += Capture_ImageGrabbed;  //eventhandler
                _isRunning = true;
                _capture.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler bei der Kamerainitialisierung: {ex.Message}");
            }
        }
        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            _capture.Retrieve(_frame);

            var bitmapImage = ConvertMatToBitmapImage(_frame);

            Dispatcher.Invoke(() =>
            {
                cameraImage.Source = bitmapImage;
            });
        }
        private BitmapImage ConvertMatToBitmapImage(Mat mat)
        {
            using (Bitmap bitmap = mat.ToBitmap())
            using (var memoryStream = new System.IO.MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // important for Thread-Securaty!

                return bitmapImage;
            }
        }


        // Workaround
        private Bitmap MatToBitmap(Mat mat)
        {
            return mat.ToBitmap();
        }


        // Window closed = camera stopped
        private void Window_Closed(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                _capture.Stop();
                _capture.Dispose();
            }
        }

        
    }
}