using System.Security.Cryptography.X509Certificates;

using HtmlAgilityPack;

using Prayer;
using Prayer.Models;

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
Console.WriteLine("Retrieving data from www.assirat.net");
Console.WriteLine("Loading...");
var assiratDoc = await web.LoadFromWebAsync("https://www.assirat.net/calendar/");
var sectionNodes = assiratDoc.DocumentNode.SelectNodes("//div[@class='main-content-carousel-holder box-shadow']/div/span");
Console.WriteLine("Done.");
Console.WriteLine("Retrieving data from www.assirat.net");
List<Section> sections = new List<Section>();
for (int i = 0; i < sectionNodes.Count - 4; i += 2)
{
    sections.Add(new(sectionNodes[i].InnerHtml, sectionNodes[i + 1].InnerHtml));
}
var innerSection = assiratDoc.DocumentNode.SelectNodes("//div[@class='main-content-carousel-holder box-shadow']/div");
List<HtmlNode> listOfSectionNodes = new();
foreach (var a3malSection in innerSection)
{
    var sectionDetail = a3malSection.ChildNodes.FirstOrDefault(n => n.Name == "ul");
    if (sectionDetail is not null)
        listOfSectionNodes.Add(sectionDetail);
}
List<SectionDetails> sectionDetails = new();
for (int i = 0; i < listOfSectionNodes.Count; i++)
{
    var sectionDetailNodes = listOfSectionNodes[i].ChildNodes.Where(n => n.Name == "li");
    List<string> listOfa3mal = sectionDetailNodes.Select(n => n.LastChild.InnerHtml).ToList();
    sectionDetails.Add(new(sections[i], listOfa3mal));
}
Console.WriteLine("Done scraping..");
Console.WriteLine("Setup wallpaper");
var windowsManager = new WindowsManager($"C:\\Users\\m.jawad\\Documents\\Prayer\\OutputImages\\{DateTime.Now:ddd-MMM-yyyy}.jpeg", textToImage, sectionDetails);
Console.WriteLine("Changed Wallpaper");
