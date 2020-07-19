using System;
using System.Windows.Forms;

namespace TimeTrack.Status.UI
{
    internal static class DatabindingExtensions
    {
        public static void AddWithFormatting(this ControlBindingsCollection controlBindingsCollection, string propertyName, object dataSource, string dataMember, IFormatProvider formatProvider, string formatString)
        {
            controlBindingsCollection.AddWithFormatting(propertyName, dataSource, dataMember, e => ConvertWithFormatting(e, formatProvider, formatString));
        }

        public static void AddWithFormatting(this ControlBindingsCollection controlBindingsCollection, string propertyName, object dataSource, string dataMember, Action<ConvertEventArgs> formatAction)
        {
            var binding = new Binding(propertyName, dataSource, dataMember);
            binding.Format += (sender, arg) => { formatAction?.Invoke(arg); };
            controlBindingsCollection.Add(binding);
        }

        private static void ConvertWithFormatting(ConvertEventArgs e, IFormatProvider formatProvider, string formatString)
        {
            e.Value = string.Format(formatProvider, formatString, e.Value);
        }
    }
}