using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using SpendyBoxDetector;

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

        //public IEnumerable<Box> Boxes { get; private set; }

        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    ThreadPool.SetMinThreads(32, 32);
        //    await Task.Run(() =>
        //    {
        //        var image = new BitmapImage(new Uri(@"Checks/font.jpeg", UriKind.Relative));
        //        // img.Source = image;
        //        var pixConvertor = new PixelConvertor();
        //        var pixels = pixConvertor.ToPixels(image);

        //        pixels = new BlackAndWhiteFilter().ApplyFilter(pixels, new Pixel { Blue = 100, Green = 100, Red = 100 });
        //        var res = new PictureAnalyzer().Analyze(pixels);
        //        pixels = new Cropper().Crop(pixels, res.MinX, res.MinY, res.MaxX - res.MinX, res.MaxY - res.MinY);
        //        var sw = new Stopwatch();
        //        sw.Start();

        //        var boxCounter = 0;

        //        Action<Pixel[,],Box> action = (p,b) =>
        //          {
        //              Task.Factory.StartNew(() =>
        //              {
        //                  var width = b.X2 - b.X1 + 1;
        //                  var height = b.Y2 - b.Y1 + 1;
        //                  var newPixels = new Pixel[width, height];
                 
                         
        //                  for(int i=0; i<width;i++)
        //                  {
                              
      
        //                     for (int j = 0; j < height; j++)
        //                     {
        //                          newPixels[i, j] = p[b.X1+i, b.Y1+j];
        //                         if (newPixels[i, j].IsRed())
        //                             newPixels[i, j].PaintWhite();
        //                      }
         
        //                  }
        //                  var bitmap=pixConvertor.ToBitMap(newPixels);
        //                  bitmap.Freeze();
                        
        //                  InUI(() =>
        //                  {
        //                      var img = new Image {Height = height, Width = width, Source = bitmap};
        //                      img.Margin=new Thickness(3);
        //                      BoxHost.Children.Add(img);

        //                      JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                     
        //                      String photolocation ="Box"+(++boxCounter) + ".jpg";  //file name 

        //                      encoder.Frames.Add(BitmapFrame.Create(bitmap));

        //                      using (var filestream = new FileStream(photolocation, FileMode.Create))
        //                          encoder.Save(filestream);
        //                  });
        //              });
        //          };
        //        Boxes = new BoxDetector().GetBoxes(pixels,action);
        //        sw.Stop();
        //        Console.WriteLine("Detection Elapsed="+sw.ElapsedMilliseconds);
        //        var newImage = pixConvertor.ToBitMap(pixels);
        //        newImage.Freeze();
        //        InUI(() =>
        //        {
        //            img.Source = newImage;
     
        //        });


        //    });


        //}

        //public void InUI(Action action)
        //{
        //    this.Dispatcher.BeginInvoke(action);
        //}
        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            //if (isDragging)
            //{
                //var currentPosition = e.MouseDevice.GetPosition(ScrollViewer);
                //if (currentPosition.X > prevPosition.X)
                //{
                //    var dx =   currentPosition.X- prevPosition.X;
                //    if (dx + ScrollViewer.ContentHorizontalOffset > ScrollViewer.ExtentWidth)
                //        ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.ExtentWidth);
                //    else if (dx + ScrollViewer.ContentHorizontalOffset < 0)
                //        ScrollViewer.ScrollToHorizontalOffset(0);
                //    else
                //        ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.+dx);

                //    var dy = currentPosition.Y-prevPosition.Y;
                //    if (dy + ScrollViewer.ContentVerticalOffset > ScrollViewer.ExtentHeight)
                //        ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ExtentHeight);
                //    else if (dy - ScrollViewer.ContentVerticalOffset < 0)
                //        ScrollViewer.ScrollToVerticalOffset(0);
                //    else
                //        ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ContentVerticalOffset+dx);
                //    prevPosition = currentPosition;
                //}
            //}
        }

        //private bool isDragging;
        //private Point prevPosition;

        //private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    isDragging = true;
        //    Mouse.Capture(ScrollViewer,CaptureMode.SubTree);
        //    prevPosition= e.MouseDevice.GetPosition(ScrollViewer);
        //}

        //private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    Mouse.Capture(ScrollViewer, CaptureMode.None);
        //    isDragging = false;
        //}

        private void ScrollViewer_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                ScrollViewer scrollviewer = sender as ScrollViewer;
                if (e.Delta > 0)
                    scrollviewer.LineLeft();
                else
                    scrollviewer.LineRight();
                e.Handled = true;
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                ScrollViewer scrollviewer = sender as ScrollViewer;
            
                if (e.Delta > 0)
                {
                    var newVal = Slider.Value + 0.02;
                    if (newVal > Slider.Maximum)
                        newVal = Slider.Maximum;
                    Slider.Value = newVal;
                }
                else
                {
                    var newVal = Slider.Value - 0.02;
                    if (newVal < Slider.Minimum)
                        newVal = Slider.Minimum;
                    Slider.Value = newVal;
                }
                e.Handled = true;
            }
        }
    }
}
