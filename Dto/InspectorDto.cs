using AutoMapper;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    
    public class InspectorDto
    {
        public InspectorDto()
        {

        }
        public int inspector_id { get; set; }
        public string inspector_name { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public Nullable<int> num_house { get; set; }
        public string phone { get; set; }
        public string area { get; set; }
        public Nullable<double> inspector_lon { get; set; }
        public Nullable<double> inspector_lat { get; set; }
        public string inspector_password { get; set; }

        public static InspectorDto DalToDto(inspectorTbl inspector)
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<inspectorTbl, InspectorDto>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<InspectorDto>(inspector);
        }

        public inspectorTbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<InspectorDto, inspectorTbl>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<inspectorTbl>(this);
        }
    }
}
