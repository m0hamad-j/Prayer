using System.Drawing;
using System.Runtime.InteropServices;

using Microsoft.Win32;
namespace Prayer
{
    internal class WindowsManager
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

        private static readonly uint SPI_SETDESKWALLPAPER = 0x14;
        private static readonly uint SPIF_UPDATEINIFILE = 0x01;
        private static readonly uint SPIF_SENDWININICHANGE = 0x02;

        static public void SetWallpaper(string path)
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key?.SetValue(@"WallpaperStyle", 0.ToString()); // 2 is stretched
            key?.SetValue(@"TileWallpaper", 0.ToString());

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
        public WindowsManager(string imagePath, string imageText,List<SectionDetails> sectionDetails)
        {
            TextImageGenerator.DrawText(imageText, sectionDetails, new("Arial", 40), Color.Black, 1920, 1080, imagePath);
            SetWallpaper(imagePath);
        }
    }
}

