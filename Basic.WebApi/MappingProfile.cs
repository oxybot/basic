using AutoMapper;
using Basic.Model;
using Basic.WebApi.Models;

namespace Basic.WebApi
{
    /// <summary>
    /// Defines the default auto-mapper profile.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Client, ClientDTO>().ReverseMap();
            CreateMap<Client, SimpleClientDTO>();

            CreateMap<Agreement, AgreementDTO>()
                .ReverseMap()
                .ForMember(c => c.Client, options => options.Ignore());
            CreateMap<Agreement, SimpleAgreementDTO>();
        }
    }
}
