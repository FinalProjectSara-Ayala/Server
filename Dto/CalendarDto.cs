using AutoMapper;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class CalendarDto
    {
        public CalendarDto()
        {

        }
        public int service_id { get; set; }
        public Nullable<bool> sunday { get; set; }
        public Nullable<bool> monday { get; set; }
        public Nullable<bool> tuesday { get; set; }
        public Nullable<bool> wednesday { get; set; }
        public Nullable<bool> thursday { get; set; }
        public Nullable<bool> friday { get; set; }
        public Nullable<bool> saturday { get; set; }
        public Nullable<double> start_date { get; set; }
        public Nullable<double> end_date { get; set; }

        public static CalendarDto DalToDto(calendarTbl calendar)
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<calendarTbl, CalendarDto>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<CalendarDto>(calendar);
        }

        public calendarTbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<CalendarDto, calendarTbl>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<calendarTbl>(this);
        }
    }
}
