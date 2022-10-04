using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Prayer
{
    internal class TextImageGenerator
    {
        public static void DrawText(string text, List<SectionDetails> sectionDetails, Font font, Color textColor, int width, int height, string path)
        {
            //first, create a dummy bitmap just to get a graphics object
            //measure the string to see how big the image needs to be

            //set the stringformat flags to rtl
            StringFormat sf = new StringFormat();
            //uncomment the next line for right to left languages
            //sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            sf.Trimming = StringTrimming.Word;
            sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sf.SetDigitSubstitution(0x0C01, StringDigitSubstitute.National);


            //create a new image of the right size
            Image img = new Bitmap(width, height);

            var drawing = Graphics.FromImage(img);
            //Adjust for high quality
            drawing.CompositingQuality = CompositingQuality.HighQuality;
            drawing.InterpolationMode = InterpolationMode.HighQualityBilinear;
            drawing.PixelOffsetMode = PixelOffsetMode.HighQuality;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            //paint the background
            drawing.Clear(Color.Transparent);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);
            drawing.FillRectangle(new SolidBrush(Color.FromArgb(238, 238, 242)), new RectangleF(0, 0, width, height));
            drawing.DrawString(text, font, textBrush, new RectangleF(0, 0, width, height), sf);


            sf.Alignment = StringAlignment.Far;
            sf.LineAlignment = StringAlignment.Near;
            text = sectionDetails[0].Section.ToString() + "\n";
            text += sectionDetails[0].A3mal.Aggregate((a, b) => a + "\n" + b);
            drawing.DrawString(text, new("Arial", 25), textBrush, new RectangleF(20, 20, width, height), sf);


            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            text = sectionDetails[1].Section.ToString() + "\n";
            text += sectionDetails[1].A3mal.Aggregate((a, b) => a + "\n" + b);
            drawing.DrawString(text, new("Arial", 25), textBrush, new RectangleF(-20, 20, width, height), sf);
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Far;
            text = sectionDetails[2].Section.ToString() + "\n";
            text += sectionDetails[2].A3mal.Aggregate((a, b) => a + "\n" + b);
            drawing.DrawString(text, new("Arial", 25), textBrush, new RectangleF(-20, -45, width, height), sf);
            sf.Alignment = StringAlignment.Far;
            sf.LineAlignment = StringAlignment.Far;
            text = sectionDetails[3].Section.ToString() + "\n";
            text += sectionDetails[3].A3mal.Aggregate((a, b) => a + "\n" + b);
            drawing.DrawString(text, new("Arial", 25), textBrush, new RectangleF(20, -45, width, height), sf);
            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();
            img.Save(path, ImageFormat.Png);
            img.Dispose();

        }
    }

}
