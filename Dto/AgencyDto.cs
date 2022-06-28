using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
    public class AgencyDto
    {
        public AgencyDto()
        {

        }

        public int agency_id { get; set; }
        public string agency_name { get; set; }
        public string agency_url { get; set; }
        public string agency_timezone { get; set; }
        public string agency_lang { get; set; }
        public string agency_phone { get; set; }
        public string agency_fare_url { get; set; }


        public static AgencyDto DalToDto(agencyTbl agency)
        {
            var config =new MapperConfiguration(cfg =>
              cfg.CreateMap<agencyTbl,AgencyDto>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<AgencyDto>(agency); 
        }

        public  agencyTbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<AgencyDto, agencyTbl>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<agencyTbl>(this);
        }
    }
}
