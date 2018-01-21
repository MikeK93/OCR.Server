using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;
using SpendyBoxDetector;
using ThreadState = System.Threading.ThreadState;

namespace TextRecognition
{
    public class BoxViewModel : INotifyPropertyChanged
    {
        private BitmapSource _image;

        public BitmapSource Image
        {
            get { return _image; }
            set
            {
                if (_image != null && _image.Equals(value))
                    return;
                _image = value;
                OnPropertyChanged();
            }
        }

        private Box _box;

        public Box Box
        {
            get { return _box; }
            set
            {
                if (_box != null && _box.Equals(value))
                    return;
                _box = value;
                OnPropertyChanged();
            }
        }

        private ICommand _saveBox;
        private BoxSaver saver;

        public ICommand SaveBox
            => _saveBox ?? (_saveBox = new DelegateCommand(SaveBoxAction));


        private void SaveBoxAction()
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Image Files(*.JPG; *.jpeg)| *.JPG;*.JPEG";
            var result = dialog.ShowDialog();
          
            if (result == true )
            {
                saver.Save(dialog.FileName,Image);
              
            }
        }

        public BoxViewModel(Box box,BitmapSource image)
        {
            Box = box;
            Image = image;
            saver=new BoxSaver();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
        }
    }
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly Dispatcher _dispatcher;
        private ICommand _startCommand;

        public ICommand StartCommand
            => _startCommand ?? (_startCommand = new DelegateCommand(StartCommandAction));

        private void StartCommandAction()
        {
            switch (detectThread.ThreadState)
            {
                case ThreadState.Running:
                    detectThread.Suspend();
                    IsRunning = false;
                    break;
                case ThreadState.Unstarted:
                    detectThread.Start();
                    IsRunning = true;
                    break;
                case ThreadState.Suspended:
                    detectThread.Resume();
                    IsRunning = true;
                    break;
                case ThreadState.Aborted:
                case ThreadState.Stopped:
                    break;
            }

        }

        private ObservableCollection<BoxViewModel> _boxes;

        public ObservableCollection<BoxViewModel> Boxes
        {
            get { return _boxes; }
            set
            {
                if (_boxes != null && _boxes.Equals(value))
                    return;
                _boxes = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _ellapsed;

        public TimeSpan Ellapsed
        {
            get { return _ellapsed; }
            set
            {
                if ( _ellapsed.Equals(value))
                    return;
                _ellapsed = value;
                OnPropertyChanged();
            }
        }



        private ICommand _stopCommand;

        public ICommand StopCommand
            => _stopCommand ?? (_stopCommand = new DelegateCommand(StopCommandAction));

        private void StopCommandAction()
        {
            if(detectThread.ThreadState==ThreadState.Suspended)
                detectThread.Resume();
            detectThread.Abort();
            detectThread = new Thread(new ThreadStart(Target));
            Boxes.Clear();
            Ellapsed=TimeSpan.Zero;
            IsRunning = false;
        }

        private ICommand _selectFileCommand;

        public ICommand SelectFileCommand
            => _selectFileCommand ?? (_selectFileCommand = new DelegateCommand(SelectFileCommandAction));

        private void SelectFileCommandAction()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image Files(*.JPG; *.jpeg)| *.JPG;*.JPEG";
            var result = dialog.ShowDialog();
            if (result == true)
            {
                FileName = dialog.FileName;
                ImageSource=new BitmapImage(new Uri(FileName));
            }
        }


        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set
            {
                if (_fileName != null && _fileName.Equals(value))
                    return;
                _fileName = value;
                OnPropertyChanged();
            }
        }

        private byte _redLevel;

        public byte RedLevel
        {
            get { return _redLevel; }
            set
            {
                if (_redLevel.Equals(value))
                    return;
                _redLevel = value;
                OnPropertyChanged();
            }
        }

        private byte _greenLevel;

        public byte GreenLevel
        {
            get { return _greenLevel; }
            set
            {
                if (_greenLevel.Equals(value))
                    return;
                _greenLevel = value;
                OnPropertyChanged();
            }

        }

        private byte _blueLevel;

        public byte BlueLevel
        {
            get { return _blueLevel; }
            set
            {
                if (_blueLevel.Equals(value))
                    return;
                _blueLevel = value;
                OnPropertyChanged();
            }
        }

        public Thread detectThread;

        private ICommand _previewBlackAndWhitefilterCommand;

        public ICommand PreviewBlackAndWhitefilterCommand
            => _previewBlackAndWhitefilterCommand ?? (_previewBlackAndWhitefilterCommand = new DelegateCommand(PreviewBwFilter));


        private void PreviewBwFilter()
        {
            if (string.IsNullOrWhiteSpace(FileName) || !File.Exists(FileName))
                return;
            var source = new BitmapImage(new Uri(FileName)) as BitmapImage;
            var pixels = pixConvertor.ToPixels(source);
            //  pixels = BWFilter.ApplyFilter(pixels);
            
            pixels = BWFilter.ApplyFilter(pixels, new Pixel {Blue = BlueLevel, Green = GreenLevel, Red = RedLevel});
            if(UseNoizeFilter)
                pixels=noizeFilter.ApplyFilter(pixels,NoisePercent);
            ImageSource = pixConvertor.ToBitMap(pixels);

        }


        public ViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            detectThread = new Thread(new ThreadStart(Target));
            Boxes=new ObservableCollection<BoxViewModel>();
            RedLevel = 110;
            BlueLevel = 110;
            GreenLevel = 110;
            MinHeight = 10;
            MinWidth = 10;
            MaxHeight = 500;
            MaxWidth = 500;
            pixConvertor = new PixelConvertor();
            BWFilter = new BlackAndWhiteFilter();
            noizeFilter = new NoizeFilter();
            NoisePercent = 20.0;
        }

        private bool _useNoizeFilter;

        public bool UseNoizeFilter
        {
            get { return _useNoizeFilter; }
            set
            {
                if ( _useNoizeFilter.Equals(value))
                    return;
                _useNoizeFilter = value;
                OnPropertyChanged();
            }
        }

        private void Target()
        {
            if(string.IsNullOrWhiteSpace(FileName) || !File.Exists(FileName))
                return;
            var image = new BitmapImage(new Uri(FileName));
            // img.Source = image;
            
            var pixels = pixConvertor.ToPixels(image);
            
            pixels = BWFilter.ApplyFilter(pixels, new Pixel {Blue = BlueLevel, Green = GreenLevel, Red = RedLevel});
            if (UseNoizeFilter)
                pixels = noizeFilter.ApplyFilter(pixels,NoisePercent);
            var res = new PictureAnalyzer().Analyze(pixels);
            pixels = new Cropper().Crop(pixels, res.MinX, res.MinY, res.MaxX - res.MinX, res.MaxY - res.MinY);
            var sw = new Stopwatch();
            sw.Start();
            var boxCounter = 0;

            Action<Pixel[,], Box> action = (p, b) =>
            {
                Task.Factory.StartNew(() =>
                {
                    var width = b.X2 - b.X1 + 1;
                    var height = b.Y2 - b.Y1 + 1;
                    var newPixels = new Pixel[width, height];


                    for (int i = 0; i < width; i++)
                    {


                        for (int j = 0; j < height; j++)
                        {
                            newPixels[i, j] = p[b.X1 + i, b.Y1 + j];
                            if (newPixels[i, j].IsRed())
                                newPixels[i, j].PaintWhite();
                        }

                    }
                    var bitmap = pixConvertor.ToBitMap(newPixels);
                    bitmap.Freeze();

                    InUI(() =>
                    {
                       // var img = new Image {Height = height, Width = width, Source = bitmap};
                       // img.Margin = new Thickness(3);
                        Boxes.Add(new BoxViewModel(b,bitmap));
                        String photolocation = "Box" + (++boxCounter) + ".jpg"; //file name 

 
                    });
                });
            };
            var args=new BoxDetectorArgs();
            args.BoxDetectCallback = action;
            args.MaxHeight = MaxHeight;
            args.MinHeight = MinHeight;
            args.MaxWidth = MaxWidth;
            args.MinWidth = MinWidth;
            args.AndFilterStrategy = AndFilterStrategy;
            new BoxDetector().GetBoxes(pixels, args);
            sw.Stop();
            Console.WriteLine("Detection Elapsed=" + sw.ElapsedMilliseconds);
            Ellapsed = TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds);
            var newImage = pixConvertor.ToBitMap(pixels);
            newImage.Freeze();
            ImageSource = newImage;
            IsRunning = false;
            //InUI(() =>
            //{
            //    img.Source = newImage;

            //});


        }

        private double _noisePercent;

        public double NoisePercent
        {
            get { return _noisePercent; }
            set
            {
                if (_noisePercent.Equals(value))
                    return;
                _noisePercent = value;
                OnPropertyChanged();
            }
        }

        private bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                if ( _isRunning.Equals(value))
                    return;
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        private bool _andFilterStrategy;

        public bool AndFilterStrategy
        {
            get { return _andFilterStrategy; }
            set
            {
                if (_andFilterStrategy.Equals(value))
                    return;
                _andFilterStrategy = value;
                OnPropertyChanged();
            }
        }

        private int _maxHeight;

        public int MaxHeight
        {
            get { return _maxHeight; }
            set
            {
                if ( _maxHeight.Equals(value))
                    return;
                _maxHeight = value;
                OnPropertyChanged();
            }
        }

        private int _maxWidth;

        public int MaxWidth
        {
            get { return _maxWidth; }
            set
            {
                if (_maxWidth.Equals(value))
                    return;
                _maxWidth = value;
                OnPropertyChanged();
            }
        }



        private int _minWidth;

        public int MinWidth
        {
            get { return _minWidth; }
            set
            {
                if ( _minWidth.Equals(value))
                    return;
                _minWidth = value;
                OnPropertyChanged();
            }
        }

        private int _minHeight;

        public int MinHeight
        {
            get { return _minHeight; }
            set
            {
                if ( _minHeight.Equals(value))
                    return;
                _minHeight = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _imageSource;
        private PixelConvertor pixConvertor;
        private BlackAndWhiteFilter BWFilter;
        private NoizeFilter noizeFilter;

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                if (_imageSource != null && _imageSource.Equals(value))
                    return;
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void InUI(Action action)
        {
            _dispatcher.Invoke(action);
        }
    }




}
