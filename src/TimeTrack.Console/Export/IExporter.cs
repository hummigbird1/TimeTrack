using System;
using System.IO;

namespace TimeTrack.Console.Export
{
    internal interface IExporter
    {
        void Export(bool header, string separator, DateTime from, DateTime to, Stream stream);
    }
}