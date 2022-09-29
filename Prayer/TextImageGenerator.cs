using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;

namespace Prayer
{
    internal class TextImageGenerator
    {
        private Color TextColor { get; set; }
        private Color BackgroundColor { get; set; }
        private Font Font { get; set; }

        public TextImageGenerator(Color textColor, Color backgroundColor, string font, int fontSize)
        {
            TextColor = textColor;
            BackgroundColor = backgroundColor;
            Font = new Font(font, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
        }
        public TextImageGenerator() : this(Color.Orange, Color.Black, "Arial", 20) { }
        public Bitmap CreateBitmap(string text)
        {
            Bitmap retBitmap = new Bitmap(1920, 1080);
            var retBitmapGraphics = Graphics.FromImage(retBitmap);
            retBitmapGraphics.Clear(BackgroundColor);
            retBitmapGraphics.SmoothingMode = SmoothingMode.HighQuality;
            retBitmapGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            StringFormat stringFormat = new StringFormat() { FormatFlags = StringFormatFlags.DirectionRightToLeft };
            text = string.Format(text, new CultureInfo("ar-LB"));
            retBitmapGraphics.DrawString(text, Font, new SolidBrush(TextColor), 1020, 270, stringFormat);
            retBitmapGraphics.Flush(FlushIntention.Sync);
            return retBitmap;
        }
        public string CreateBase64Image(string text, ImageFormat imageFormat)
        {
            var bitmap = CreateBitmap(text);
            var stream = new MemoryStream();
            bitmap.Save(stream, imageFormat);
            var imageBytes = stream.ToArray();
            return Convert.ToBase64String(imageBytes);
        }
        public void SaveAsJpg(string filename, string text)
        {
            var bitmap = CreateBitmap(text);
            bitmap.Save(filename, ImageFormat.Jpeg);
        }
        public void SaveAsPng(string filename, string text)
        {
            var bitmap = CreateBitmap(text);
            bitmap.Save(filename, ImageFormat.Png);
        }
        public void SaveAsGif(string filename, string text)
        {
            var bitmap = CreateBitmap(text);
            bitmap.Save(filename, ImageFormat.Gif);
        }
        public void SaveAsBmp(string filename, string text)
        {
            var bitmap = CreateBitmap(text);
            bitmap.Save(filename, ImageFormat.Bmp);
        }
    }

}
