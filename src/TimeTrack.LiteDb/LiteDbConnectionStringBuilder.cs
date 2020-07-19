using System.Text;

namespace TimeTrack.LiteDb
{
    public class LiteDbConnectionStringBuilder
    {
        public string ConnectionString
        {
            get
            {
                return MakeConnectionString();
            }
        }

        public bool ExclusiveMode { get; set; }
        public string FileName { get; set; }

        private string MakeConnectionString()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                sb.AppendFormat("FileName={0}", FileName);
                sb.Append(";");
            }

            sb.AppendFormat("Connection={0}", ExclusiveMode ? "Direct" : "Shared");

            return sb.ToString();
        }
    }
}