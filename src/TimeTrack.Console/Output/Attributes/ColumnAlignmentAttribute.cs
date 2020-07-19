using System;

namespace TimeTrack.Console.Output.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnAlignmentAttribute : Attribute
    {
        public ColumnAlignmentAttribute(bool rightAlign)
        {
            RightAlign = rightAlign;
        }

        public bool RightAlign { get; }
    }
}