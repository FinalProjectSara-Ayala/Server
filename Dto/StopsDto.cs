using AutoMapper;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class StopsDto
    {
        public StopsDto()
        {

        }
        public int stop_id { get; set; }
        public string stop_code { get; set; }
        public string stop_name { get; set; }
        public string stop_desc { get; set; }
        public Nullable<double> stop_lat { get; set; }
        public Nullable<double> stop_lon { get; set; }
        public Nullable<double> location_type { get; set; }
        public string parent_station { get; set; }
        public Nullable<int> zone_id { get; set; }

        public static StopsDto DalToDto(stopsTbl stop)
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<stopsTbl, StopsDto>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<StopsDto>(stop);
        }

        public stopsTbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<StopsDto, stopsTbl>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<stopsTbl>(this);
        }
    }
}
