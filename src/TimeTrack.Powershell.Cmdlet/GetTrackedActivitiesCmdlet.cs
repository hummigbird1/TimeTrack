using System;
using System.Linq;
using System.Management.Automation;
using TimeTrack.Core;
using TimeTrack.Interfaces;

namespace TimeTrack.Powershell.Cmdlet
{
    [Cmdlet(VerbsCommon.Get, "TrackedActivities")]
    [OutputType(typeof(TrackedActivity))]
    public class GetTrackedActivitiesCmdlet : PSCmdlet
    {
        [Parameter]
        public DateTime? From { get; set; }

        [Parameter(Mandatory = true)]
        public IDataStorage Storage { get; set; }

        [Parameter]
        public DateTime? To { get; set; }

        protected override void ProcessRecord()
        {
            var queryProvider = new DataStorageQueryProvider(Storage);
            WriteObject(queryProvider.GetTrackedActivitiesInTimeRange(From.GetValueOrDefault(DateTime.MinValue), To.GetValueOrDefault(DateTime.MaxValue)).Select(TrackedActivity.Create), true);
        }
    }
}