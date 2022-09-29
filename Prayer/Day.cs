namespace Prayer
{
    internal record Day()
    {
        public string? DayName { get; set; }
        public string? HijriDay { get; set; }
        public string? MiladiDay { get; set; }
        public List<Prayer> Prayers { get; set; } = new List<Prayer>();
    }
    internal record Prayer(string Description, string Time);
}
