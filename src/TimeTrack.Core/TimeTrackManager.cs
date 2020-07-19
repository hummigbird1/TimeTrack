using System;
using TimeTrack.Interfaces;

namespace TimeTrack.Core
{
    public class TimeTrackManager
    {
        private readonly TimeTrackManagerConfiguration _configuration;
        private readonly IDataStorage _dataStorage;
        private readonly ITimeServiceProvider _timeServiceProvider;

        public TimeTrackManager(IDataStorage dataStorage, ITimeServiceProvider timeServiceProvider, TimeTrackManagerConfiguration configuration)
        {
            _dataStorage = dataStorage ?? throw new ArgumentNullException(nameof(dataStorage));
            _timeServiceProvider = timeServiceProvider ?? throw new ArgumentNullException(nameof(timeServiceProvider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            CurrentTrackingActivity = _dataStorage.GetCurrentTrackingActivity();
        }

        public ITrackingActivity CurrentTrackingActivity { get; private set; }

        public void Discard()
        {
            CurrentTrackingActivity = null;
            _dataStorage.UpdateCurrentTrackingActivity(null);
        }

        public void StartTracking(string identifier, DateTime startTime)
        {
            var sanitizedIdentifier = SanitizeIdentifier(identifier);
            if (NeedStopTracking(sanitizedIdentifier))
            {
                StopTrackingNow();
            }
            CurrentTrackingActivity = CreateNewTrackingActivity(startTime, sanitizedIdentifier);
            _dataStorage.UpdateCurrentTrackingActivity(CurrentTrackingActivity);
        }

        public void StartTrackingNow(string identifier)
        {
            StartTracking(identifier, _timeServiceProvider.Now());
        }

        public void StopTracking(DateTime stopTime)
        {
            if (CurrentTrackingActivity != null)
            {
                var activity = ConvertToTrackedActivity(CurrentTrackingActivity, stopTime);
                var save = IsSaveAllowed(activity);
                if (save)
                {
                    _dataStorage.SaveTrackedActivity(activity);
                }
            }

            CurrentTrackingActivity = null;
            _dataStorage.UpdateCurrentTrackingActivity(null);
        }

        public void StopTrackingNow()
        {
            StopTracking(_timeServiceProvider.Now());
        }

        private ITrackedActivity ConvertToTrackedActivity(ITrackingActivity trackingActivity, DateTime stopTime)
        {
            return new TrackedActivity
            {
                Identifier = trackingActivity.Identifier,
                Started = trackingActivity.Started,
                Stopped = stopTime,
                Created = _timeServiceProvider.Now(),
                Modified = _timeServiceProvider.Now()
            };
        }

        private TrackingActivity CreateNewTrackingActivity(DateTime startTime, string sanitizedIdentifier)
        {
            return new TrackingActivity
            {
                Identifier = sanitizedIdentifier,
                Started = startTime,
                Created = _timeServiceProvider.Now(),
                Modified = _timeServiceProvider.Now()
            };
        }

        private bool IsCurrentActivitySame(string identifier)
        {
            return string.Equals(CurrentTrackingActivity.Identifier, identifier, _configuration.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
        }

        private bool IsSaveAllowed(ITrackedActivity trackedActivity)
        {
            if (_configuration.AutoDiscardThreshold.GetValueOrDefault(TimeSpan.Zero) > TimeSpan.Zero)
            {
                var durationTracked = trackedActivity.Stopped - trackedActivity.Started;
                if (durationTracked <= _configuration.AutoDiscardThreshold.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private bool NeedStopTracking(string identifer)
        {
            if (CurrentTrackingActivity == null)
            {
                return false;
            }

            if (!IsCurrentActivitySame(identifer))
            {
                return true;
            }

            return !_configuration.IgnoreRestartSameActivity;
        }

        private string SanitizeIdentifier(string identifier)
        {
            var sanitizedIdentifier = identifier == null ? string.Empty : identifier.Trim();
            if (!_configuration.CaseSensitive)
            {
                sanitizedIdentifier = sanitizedIdentifier.ToUpperInvariant();
            }

            return sanitizedIdentifier;
        }
    }
}