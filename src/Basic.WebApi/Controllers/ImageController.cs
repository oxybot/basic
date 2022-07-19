/*using System.Drawing;
using Microsoft.AspNetCore.Mvc;

namespace CompressRezieImage.Controllers
{ 
    public class ImageController : Controller
    {
        public static string CropImage(string base64, int x, int y, int width, int height)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using (var ms = new MemoryStream(bytes))
            {
                Bitmap bmp = new Bitmap(ms);
                Rectangle rect = new Rectangle(x, y, width, height);

                Bitmap croppedBitmap = new Bitmap(rect.Width, rect.Height, bmp.PixelFormat);

                using (Graphics gfx = Graphics.FromImage(croppedBitmap))
                {
                    gfx.DrawImage(bmp, 0, 0, rect, GraphicsUnit.Pixel);
                }

                using (MemoryStream ms2 = new MemoryStream())
                {
                    croppedBitmap.Save(ms2, ImageFormat.Jpeg);
                    byte[] byteImage = ms2.ToArray();
                    var croppedBase64 = Convert.ToBase64String(byteImage);
                    return croppedBase64;
                }
            }
        }
    }
}*/