using AutoMapper;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class Stop_timesDto
    {

        public Stop_timesDto()
        {

        }

        public string trip_id { get; set; }
        public Nullable<System.TimeSpan> arrival_time { get; set; }
        public Nullable<System.TimeSpan> departure_time { get; set; }
        public int stop_id { get; set; }
        public int stop_sequence { get; set; }
        public int pickup_type { get; set; }
        public int drop_off_type { get; set; }
        public Nullable<double> shape_dist_traveled { get; set; }
        public int stop_times_id { get; set; }

        public static Stop_timesDto DalToDto(stop_timesTbl stopTimes)
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<stop_timesTbl, Stop_timesDto>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<Stop_timesDto>(stopTimes);
        }

        public stop_timesTbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<Stop_timesDto, stop_timesTbl>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<stop_timesTbl>(this);
        }
    }
}
