using System.Collections.Generic;
using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console.Tests
{
    internal class MockOutputWriter : IOutputWriter
    {
        public IList<string> Lines { get; } = new List<string>();

        public void WriteLine(string s)
        {
            Lines.Add(s);
        }
    }
}