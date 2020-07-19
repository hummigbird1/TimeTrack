using System;
using System.Linq;
using TimeTrack.Console.Interfaces;
using TimeTrack.Core;
using TimeTrack.Interfaces;

namespace TimeTrack.Console.Plugin.ResumeCommand
{
    public class ResumeCommand : ICustomPlugin
    {
        private readonly IDataStorage _dataStorage;
        private readonly TimeTrackManager _timeTrackManager;

        public ResumeCommand(IDataStorage dataStorage, TimeTrackManager timeTrackManager)
        {
            _dataStorage = dataStorage;
            _timeTrackManager = timeTrackManager;
        }

        public string Name => "Resume-Activity";

        public void Execute(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (int.TryParse(command, out var value) && value > -1)
            {
                var activity = GetTrackedActivityAtPosition(value);
                if (activity != null)
                {
                    _timeTrackManager.StartTrackingNow(activity.Identifier);
                }
                return;
            }

            switch (command.ToLowerInvariant())
            {
                case "last":
                    {
                        var isTracking = _dataStorage.GetCurrentTrackingActivity() != null;
                        var activity = GetTrackedActivityAtPosition(isTracking ? 0 : 1);
                        if (activity != null)
                        {
                            _timeTrackManager.StartTrackingNow(activity.Identifier);
                        }
                        return;
                    }
            }

            // TODO Command include where conditions? e.g. Where activity duration > x, or Identifier like *xyz* etc.

            throw new ArgumentException($"Could not process command '{command}'");
        }

        private ITrackedActivity GetTrackedActivityAtPosition(int skip)
        {
            return _dataStorage.GetTrackedActivities()
                .OrderByDescending(x => x.Modified)
                .Skip(skip)
                .FirstOrDefault();
        }
    }
}
