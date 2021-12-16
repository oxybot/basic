using AutoMapper;
using Basic.Model;
using Basic.WebApi.Models;

namespace Basic.WebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientDTO>().ReverseMap();
            CreateMap<Client, SimpleClientDTO>().ReverseMap();
        }
    }
}
