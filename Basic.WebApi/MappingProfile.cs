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
            CreateMap<byte[], string>()
                .ConvertUsing((bytes) => Convert.ToBase64String(bytes));
            CreateMap<string, byte[]>()
                .ConvertUsing((text) => Convert.FromBase64String(text));
            CreateMap<TypedFile, Base64File>().ReverseMap();

            CreateMap<Client, EntityReference>();
            CreateMap<Product, EntityReference>();
            CreateMap<User, EntityReference>();
            CreateMap<EventCategory, EntityReference>();

            CreateMap<User, UserReference>();

            CreateMap<Client, ClientForList>();
            CreateMap<Client, ClientForView>();
            CreateMap<Client, ClientForEdit>().ReverseMap();

            CreateMap<Agreement, AgreementForList>();
            CreateMap<Agreement, AgreementForView>();
            CreateMap<Agreement, AgreementForEdit>()
                .ReverseMap()
                    .ForMember(a => a.Client, options => options.Ignore())
                    .ForMember(a => a.Items, options => options.Ignore());

            CreateMap<AgreementItem, AgreementItemForList>();
            CreateMap<AgreementItem, AgreementItemForEdit>()
                .ReverseMap()
                    .ForMember(i => i.Agreement, options => options.Ignore())
                    .ForMember(i => i.Product, options => options.Ignore());
            CreateMap<AgreementItem, AgreementItemForEditWithIdentifier>()
                .ReverseMap()
                    .ForMember(i => i.Agreement, options => options.Ignore())
                    .ForMember(i => i.Product, options => options.Ignore());

            CreateMap<Balance, BalanceForList>();
            CreateMap<Balance, BalanceForEdit>()
                .ReverseMap()
                    .ForMember(e => e.User, options => options.Ignore())
                    .ForMember(e => e.Category, options => options.Ignore());

            CreateMap<Event, EventForList>();
            CreateMap<Event, EventForView>();
            CreateMap<Event, EventForEdit>()
                .ReverseMap()
                    .ForMember(e => e.User, options => options.Ignore())
                    .ForMember(e => e.Category, options => options.Ignore());

            CreateMap<EventCategory, EventCategoryForList>();
            CreateMap<EventCategory, EventCategoryForEdit>().ReverseMap();

            CreateMap<Product, ProductForList>();
            CreateMap<Product, ProductForView>();
            CreateMap<Product, ProductForEdit>().ReverseMap();

            CreateMap<Role, RoleForList>();


            CreateMap<Schedule, ScheduleForList>();
            CreateMap<Schedule, ScheduleForView>();
            CreateMap<Schedule, ScheduleForEdit>()
                .ReverseMap()
                    .ForMember(s => s.User, options => options.Ignore());

            CreateMap<User, UserForList>();
            CreateMap<User, UserForView>();
            CreateMap<User, UserForEdit>().ReverseMap();
        }
    }
}
