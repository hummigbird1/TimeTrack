using System;

namespace TimeTrack.Console.OutputFormatting
{
    public struct OutputProperty<T>
    {
        public string HeaderText { get; set; }
        public string PropertyName { get; set; }
        public bool RightAligned { get; set; }
        public Func<T, string> ValueConversionFunction { get; set; }
    }
}