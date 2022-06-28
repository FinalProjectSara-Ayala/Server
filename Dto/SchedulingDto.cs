using AutoMapper;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class SchedulingDto
    {
        public SchedulingDto()
        {

        }
        

        public int scheduling_id { get; set; }
        public Nullable<int> inspector_id { get; set; }
        public Nullable<int> route_id { get; set; }
        public string trip_id { get; set; }
        public int stop_id { get; set; }
        public System.TimeSpan arrival_time { get; set; }
        public Nullable<int> workhour_id { get; set; }
        public Nullable<System.DateTime> dateToday { get; set; }

        public SchedulingDto(Nullable<int> inspector_id, Nullable<int> route_id,
              string trip_id, int stop_id, System.TimeSpan arrival_time, Nullable<int> workhour_id,DateTime date)
        {
            
            this.inspector_id = inspector_id;
            this.route_id = route_id;
            this.trip_id = trip_id;
            this.stop_id = stop_id;
            this.arrival_time = arrival_time;
            this.workhour_id = workhour_id;
            this.dateToday = date;
            
        }
    public static SchedulingDto DalToDto(schedulingTbl scheduling)
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<schedulingTbl, SchedulingDto>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<SchedulingDto>(scheduling);
        }

        public schedulingTbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<SchedulingDto, schedulingTbl>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<schedulingTbl>(this);
        }
    }
}
