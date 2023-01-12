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
var salaHeader = maarefDoc.DocumentNode.SelectNodes("//div[@class='card h20 mb-3']");
Console.WriteLine("Setting Hijri day..");
var data = salaHeader.First().InnerText.Trim();
var listOfData = data.Split(new[] { "\r\n" }, StringSplitOptions.None);
listOfData = listOfData.Where(d => !string.IsNullOrEmpty(d)).ToArray();
var cleanData = new List<string>();
foreach (var line in listOfData)
{
    var temp = RemoveWhitespace(line);
    var bol = line == "-";
    if (!string.IsNullOrEmpty(temp) && line.Trim() != "مواقيــت الصـــلاة ليوم" && line.Trim() != "-")
        cleanData.Add(line.Trim());
}
var day = cleanData.First();
var prayerTimes = cleanData.TakeLast(16).ToList();
prayer.DayName = day;
var listOfPrayer = new List<Prayer.Models.Prayer>();
for (int i = 0; i < prayerTimes.Count; i += 2)
{
    listOfPrayer.Add(new(prayerTimes[i + 1], prayerTimes[i]));
    prayer.Prayers.Add(new(prayerTimes[i + 1], prayerTimes[i]));
}

Console.WriteLine("Done.");

string textToImage = prayer.DayName + "\n";
//textToImage += prayer.HijriDay + "\n";
//textToImage += prayer.MiladiDay + "\n";

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
string RemoveWhitespace(string input)
    => new string(input.ToCharArray()
        .Where(c => !char.IsWhiteSpace(c))
        .ToArray());