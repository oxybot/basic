using AutoMapper;
using Basic.Model;
using Basic.WebApi.DTOs;

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
            CreateMap<Client, EntityReference>()
                .ConvertUsing(c => new EntityReference() { Identifier = c.Identifier, DisplayName = c.DisplayName });

            CreateMap<Client, ClientForList>();
            CreateMap<Client, ClientForView>();
            CreateMap<Client, ClientForEdit>().ReverseMap();

            CreateMap<Agreement, AgreementForList>();
            CreateMap<Agreement, AgreementForView>();
            CreateMap<Agreement, AgreementForEdit>().ReverseMap();
        }
    }
}
