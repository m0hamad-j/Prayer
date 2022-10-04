public record Section(string Description, string Value)
{
    public override string ToString()
    {
        return $"{Description} {Value}";
    }
}
