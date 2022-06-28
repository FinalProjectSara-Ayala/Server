using AutoMapper;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class workHourDto
    {

        public int id { get; set; }
        public int inspector_id { get; set; }
        public int dayWork { get; set; }
        public System.TimeSpan start_shift { get; set; }
        public System.TimeSpan stop_shift { get; set; }

        public static workHourDto DalToDto(workHour w)
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<workHour, workHourDto>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<workHourDto>(w);
        }

        public workHour DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<workHourDto, workHour>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<workHour>(this);
        }
    }
}
