using System;
using System.Management.Automation;
using TimeTrack.Interfaces;

namespace TimeTrack.Powershell.Cmdlet
{
    [Cmdlet(VerbsCommon.Close, "TrackedActivitiesDataStorage")]
    [OutputType(typeof(IDataStorage))]
    public class CloseTrackedActivitiesDataStorageCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public IDataStorage Storage { get; set; }

        protected override void ProcessRecord()
        {
            if (Storage is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}