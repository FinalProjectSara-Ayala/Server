using AutoMapper;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class RoutsDto
    {
        public RoutsDto()
        {

        }

        public int route_id { get; set; }
        public int agency_id { get; set; }
        public string route_short_name { get; set; }
        public string route_long_name { get; set; }
        public string route_desc { get; set; }
        public int route_type { get; set; }
        public string route_color { get; set; }

        public static RoutsDto DalToDto(routsTbl rout)
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<routsTbl, RoutsDto>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<RoutsDto>(rout);
        }

        public routsTbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<RoutsDto, routsTbl>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<routsTbl>(this);
        }
    }
}
