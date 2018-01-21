using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TextRecognition
{
    public class BoxSaver
    {
        public void Save(string fileName,BitmapSource bitmap)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var filestream = new FileStream(fileName, FileMode.Create))
                encoder.Save(filestream);
        }
    }
}
