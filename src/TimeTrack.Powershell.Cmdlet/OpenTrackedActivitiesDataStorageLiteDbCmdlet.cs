using System;
using System.Management.Automation;
using TimeTrack.Interfaces;
using TimeTrack.LiteDb;

namespace TimeTrack.Powershell.Cmdlet
{
    [Cmdlet(VerbsCommon.Open, "TrackedActivitiesDataStorageLiteDb")]
    [OutputType(typeof(IDataStorage))]
    public class OpenTrackedActivitiesDataStorageLiteDbCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "ConnectionString")]
        [Alias("Connection")]
        public string ConnectionString { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "Parameters")]
        [Alias("Exclusive", "Locked")]
        public bool ExclusiveAccess { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Parameters")]
        [Alias("StorageLocation", "FilePath", "Path")]
        public string FileName { get; set; }

        protected override void ProcessRecord()
        {
            var connectionString = ConnectionString;
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                var connectionStringBuilder = new LiteDbConnectionStringBuilder
                {
                    FileName = Environment.ExpandEnvironmentVariables(FileName),
                    ExclusiveMode = ExclusiveAccess
                };
                connectionString = connectionStringBuilder.ConnectionString;
            }

            WriteObject(new LiteDbDataStorage(connectionString));
        }
    }
}