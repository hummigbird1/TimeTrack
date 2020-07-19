using AutoMapper;
using TimeTrack.Interfaces;

namespace TimeTrack.LiteDb
{
    internal class LiteDbStorageTypeMapper
    {
        private readonly Mapper _mapper;

        public LiteDbStorageTypeMapper()
        {
            var c = new MapperConfiguration(x =>
            {
                x.CreateMap<ITrackedActivity, TrackedActivity>();
                x.CreateMap<ITrackingActivity, TrackingActivity>();
            });
            _mapper = new Mapper(c);
        }

        public TStorage Map<TSource, TStorage>(TSource source)
        {
            return _mapper.Map<TStorage>(source);
        }
    }
}