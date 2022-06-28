using AutoMapper;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class TripsDto
    {
        public TripsDto()
        {

        }

        public int route_id { get; set; }
        public int service_id { get; set; }
        public string trip_id { get; set; }
        public string trip_headsign { get; set; }
        public int direction_id { get; set; }
        public Nullable<double> shape_id { get; set; }
        public Nullable<System.DateTime> last_date { get; set; }
        public Nullable<System.TimeSpan> last_time { get; set; }
        public int id { get; set; }

        public static TripsDto DalToDto(tripsTbl trip)
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<tripsTbl, TripsDto>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<TripsDto>(trip);
        }

        public tripsTbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<TripsDto, tripsTbl>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<tripsTbl>(this);
        }
    }
}
