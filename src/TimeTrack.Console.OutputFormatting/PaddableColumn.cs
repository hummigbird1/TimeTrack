namespace TimeTrack.Console.OutputFormatting
{
    public class PaddableColumn
    {
        public int? PaddingTotalWith { get; set; }
        public bool PadLeft { get; set; } = false;
        public int TotalLength => PaddingTotalWith.GetValueOrDefault((Value?.Length).GetValueOrDefault(0));
        public string Value { get; set; }

        public override string ToString()
        {
            if (PaddingTotalWith.HasValue)
            {
                if (PadLeft)
                {
                    return SafePadLeft(Value, PaddingTotalWith.Value);
                }
                else
                {
                    return SafePadRight(Value, PaddingTotalWith.Value);
                }
            }

            return Value;
        }

        private string SafePadLeft(string s, int totalWidth)
        {
            return (s ?? string.Empty).PadLeft(totalWidth);
        }

        private string SafePadRight(string s, int totalWidth)
        {
            return (s ?? string.Empty).PadRight(totalWidth);
        }
    }
}