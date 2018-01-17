using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace TextRecognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public IEnumerable<Box> Boxes { get; private set; }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.SetMinThreads(32, 32);
            await Task.Run(() =>
            {
                var image = new BitmapImage(new Uri(@"Checks/1.jpg", UriKind.Relative));
                // img.Source = image;
                var pixConvertor = new PixelConvertor();
                var pixels = pixConvertor.ToPixels(image);

                pixels = new BlackAndWhiteFilter().ApplyFilter(pixels, new Pixel { Blue = 110, Green = 110, Red = 110 });
                var res = new PictureAnalyzer().Analyze(pixels);
                pixels = new Cropper().Crop(pixels, res.MinX, res.MinY, res.MaxX - res.MinX, res.MaxY - res.MinY);
                var sw = new Stopwatch();
                sw.Start();
                Boxes = new BoxDetector().GetBoxes(pixels);
                sw.Stop();
                Console.WriteLine("Detection Elapsed="+sw.ElapsedMilliseconds);
                var newImage = pixConvertor.ToBitMap(pixels);
                newImage.Freeze();
                InUI(() =>
                {
                    img.Source = newImage;
     
                });

            });


        }

        public void InUI(Action action)
        {
            this.Dispatcher.BeginInvoke(action);
        }
    }
}
