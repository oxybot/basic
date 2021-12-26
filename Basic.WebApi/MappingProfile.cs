﻿using AutoMapper;
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
            CreateMap<Client, EntityReference>();
            CreateMap<Product, EntityReference>();

            CreateMap<Client, ClientForList>();
            CreateMap<Client, ClientForView>();
            CreateMap<Client, ClientForEdit>().ReverseMap();

            CreateMap<Agreement, AgreementForList>();
            CreateMap<Agreement, AgreementForView>();
            CreateMap<Agreement, AgreementForEdit>()
                .ReverseMap()
                    .ForMember(a => a.Client, options => options.Ignore());

            CreateMap<AgreementItem, AgreementItemForList>();
            CreateMap<AgreementItem, AgreementItemForEdit>()
                .ReverseMap()
                    .ForMember(i => i.Agreement, options => options.Ignore())
                    .ForMember(i => i.Product, options => options.Ignore());

            CreateMap<Product, ProductForList>();
            CreateMap<Product, ProductForView>();
            CreateMap<Product, ProductForEdit>().ReverseMap();
        }
    }
}
