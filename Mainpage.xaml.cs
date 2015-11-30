using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using Microsoft.Phone;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



namespace PhoneApp10
{

    public partial class MainPage : PhoneApplicationPage
    {

        CameraCaptureTask cameraCaptureTask;
        public MainPage()
        {
            InitializeComponent();
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new
            EventHandler<PhotoResult>(cameraCaptureTask_Completed);


        }




        private void image1_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }


        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK && e.ChosenPhoto != null)
            {
                WriteableBitmap bitmapImage = PictureDecoder.DecodeJpeg(e.ChosenPhoto);
                image1.Source = bitmapImage;
            }
        }



      






        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            cameraCaptureTask.Show();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var wb = image1.Source as WriteableBitmap;

            if (wb == null) return;

            byte a, r, b, g, max, min;
            int pixel;
            for (int i = 0; i < wb.Pixels.Length; i++)
            {
                pixel = wb.Pixels[i];
                a = (byte)(pixel >> 24);
                r = (byte)(pixel >> 16);
                g = (byte)(pixel >> 8);
                b = (byte)pixel;

                max = Math.Max(r, g);
                max = Math.Max(max, b);

                min = Math.Max(r, g);
                min = Math.Max(min, b);

                pixel = (max + min) / 2;
                wb.Pixels[i] = (a << 24) | (pixel << 16) | (pixel << 8) | pixel;
            }

            image1.Source = wb;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var wb = image1.Source as WriteableBitmap;

            var memoryStream = new MemoryStream();
            wb.SaveJpeg(memoryStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
            memoryStream.Position = 0;

            var library = new MediaLibrary();
            library.SavePicture(string.Format("{0}.jpg", Guid.NewGuid()), memoryStream);
        }







        











    }
}
