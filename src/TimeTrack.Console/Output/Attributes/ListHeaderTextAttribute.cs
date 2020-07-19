using System;

namespace TimeTrack.Console.Output.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ListHeaderTextAttribute : Attribute
    {
        public ListHeaderTextAttribute(string headerText)
        {
            HeaderText = headerText;
        }

        public string HeaderText { get; }
    }
}