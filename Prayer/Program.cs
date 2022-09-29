using System.Drawing;

using HtmlAgilityPack;

using Prayer;

Console.WriteLine("Al Salat scraper started...");
Day prayer = new Day();
HtmlWeb web = new();
Console.WriteLine("Retrieving data from www.almaaref.org");
Console.WriteLine("Loading...");
var maarefDoc = await web.LoadFromWebAsync("https://www.almaaref.org/");
var salaHeader = maarefDoc.DocumentNode.SelectNodes("//div[@class='sala-header']");
Console.WriteLine("Setting Hijri day..");
var day = salaHeader.First().InnerHtml.Trim();
var koffarDay = salaHeader.Last().FirstChild.InnerHtml.Trim();
var hijriDay = salaHeader.Last().LastChild.InnerHtml.Trim();
prayer.DayName = day;
prayer.HijriDay = hijriDay;
prayer.MiladiDay = koffarDay;
Console.WriteLine("Done.");
Console.WriteLine("Retrieving data from alnour.com.lb");
Console.WriteLine("Loading...");
var alNourDoc = await web.LoadFromWebAsync("http://alnour.com.lb/");
var htmlPrayersTimes = alNourDoc.DocumentNode.SelectSingleNode("//ul[@class='day-times']");
Console.WriteLine("Setting prayer times..");
var salat = htmlPrayersTimes.ChildNodes.Where(s => s.Name == "li");

foreach (var htmlPrayersTime in salat)
{
    prayer.Prayers.Add(new(htmlPrayersTime.FirstChild.InnerHtml.Trim(), htmlPrayersTime.LastChild.InnerHtml.Trim()));
}
string textToImage = prayer.DayName + "\n";
textToImage += prayer.HijriDay + "\n";
textToImage += prayer.MiladiDay + "\n";

foreach (var sala in prayer.Prayers)
{
    textToImage += sala.Description + ":" + sala.Time + "\n";
}
Console.WriteLine("Done scraping..");
Console.WriteLine("Setup wallpaper");

var windowsManager = new WindowsManager("C:\\Users\\m.jawad\\imageJPEG.jpeg", textToImage);

