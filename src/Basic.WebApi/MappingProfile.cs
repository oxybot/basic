// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

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
            // General conversions
            this.CreateMap<byte[], string>()
                .ConvertUsing((bytes) => Convert.ToBase64String(bytes));
            this.CreateMap<string, byte[]>()
                .ConvertUsing((text) => Convert.FromBase64String(text));
            this.CreateMap<TypedFile, Base64File>().ReverseMap();

            // Entity reference conversions
            this.CreateMap<Client, EntityReference>();
            this.CreateMap<Product, EntityReference>();
            this.CreateMap<User, EntityReference>();
            this.CreateMap<EventCategory, EntityReference>();
            this.CreateMap<EventStatus, EntityReference>();

            // Special entity reference conversions
            this.CreateMap<User, UserReference>();
            this.CreateMap<Status, StatusReference>();

            // All entities conversions
            this.CreateMap<Client, ClientForList>();
            this.CreateMap<Client, ClientForView>();
            this.CreateMap<Client, ClientForEdit>().ReverseMap();

            this.CreateMap<Agreement, AgreementForList>();
            this.CreateMap<Agreement, AgreementForView>();
            this.CreateMap<Agreement, AgreementForEdit>()
                .ReverseMap()
                    .ForMember(a => a.Client, options => options.Ignore())
                    .ForMember(a => a.Items, options => options.Ignore());

            this.CreateMap<AgreementItem, AgreementItemForList>();
            this.CreateMap<AgreementItem, AgreementItemForEdit>()
                .ReverseMap()
                    .ForMember(i => i.Agreement, options => options.Ignore())
                    .ForMember(i => i.Product, options => options.Ignore());
            this.CreateMap<AgreementItem, AgreementItemForEditWithIdentifier>()
                .ReverseMap()
                    .ForMember(i => i.Agreement, options => options.Ignore())
                    .ForMember(i => i.Product, options => options.Ignore());

            this.CreateMap<Balance, BalanceForList>();
            this.CreateMap<Balance, BalanceForEdit>()
                .ReverseMap()
                    .ForMember(e => e.User, options => options.Ignore())
                    .ForMember(e => e.Category, options => options.Ignore());

            this.CreateMap<BaseModelStatus, ModelStatusForList>();

            this.CreateMap<Event, EventForList>();
            this.CreateMap<Event, EventForView>();
            this.CreateMap<Event, EventForEdit>()
                .ReverseMap()
                    .ForMember(e => e.User, options => options.Ignore())
                    .ForMember(e => e.Category, options => options.Ignore());

            this.CreateMap<EventCategory, EventCategoryForList>();
            this.CreateMap<EventCategory, EventCategoryForEdit>().ReverseMap();

            this.CreateMap<GlobalDayOff, GlobalDayOffForList>();
            this.CreateMap<GlobalDayOff, GlobalDayOffForEdit>().ReverseMap();

            this.CreateMap<Product, ProductForList>();
            this.CreateMap<Product, ProductForView>();
            this.CreateMap<Product, ProductForEdit>().ReverseMap();

            this.CreateMap<Role, RoleForList>();

            this.CreateMap<Schedule, ScheduleForList>();
            this.CreateMap<Schedule, ScheduleForView>();
            this.CreateMap<Schedule, ScheduleForEdit>()
                .ReverseMap()
                    .ForMember(s => s.User, options => options.Ignore());

            this.CreateMap<User, UserForList>();
            this.CreateMap<User, UserForView>();
            this.CreateMap<User, UserForEdit>().ReverseMap();

            this.CreateMap<User, MyUserForView>();
            this.CreateMap<User, MyUserForEdit>().ReverseMap();

            this.CreateMap<BaseAttachment, AttachmentForList>();
            this.CreateMap<BaseAttachment, AttachmentForView>();
            this.CreateMap<BaseAttachment, AttachmentForEdit>().ReverseMap();

            this.CreateMap<Role, EntityReference>()
                .ForMember(e => e.DisplayName, options => options.MapFrom(r => r.Code));
        }
    }
}
