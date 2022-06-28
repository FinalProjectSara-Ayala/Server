using AutoMapper;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class CitiesDto
    {
        public string city_name { get; set; }
        public int city_id { get; set; }
        public CitiesDto()
        {

        }
        public static CitiesDto DalToDto(cityTbl city)
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<cityTbl, CitiesDto>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<CitiesDto>(city);
        }

        public cityTbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<CitiesDto, cityTbl>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<cityTbl>(this);
        }
    }
}
