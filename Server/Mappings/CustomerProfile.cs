using AutoMapper;
using SetValues.Contracts.Models.Requests;
using SetValues.Contracts.Models.Responses;
using SetValues.Server.Entities;

namespace SetValues.Server.Mappings;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<AddEditCustomerCommand, Customer>()
            .ForMember(
                m => m.Address,
                options => options.MapFrom(p => new Address
                {
                    Number = p.Number,
                    Street = p.Street
                }))
            .ForMember(
                m => m.Contact,
                options => options.MapFrom(p => new Contact
                {
                    Email = p.Email
                }))
            .ForMember(m => m.CreatedBy, options => options.Ignore())
            .ForMember(m => m.CreatedOn, options => options.Ignore())
            .ForMember(m => m.LastModifiedBy, options => options.Ignore())
            .ForMember(m => m.LastModifiedOn, options => options.Ignore())
            .ReverseMap();
        
        CreateMap<Customer, GetAllPagedCustomersResponse>()
            .ForMember(m => m.Number, options => options.MapFrom(p => p.Address.Number))
            .ForMember(m => m.Street, options => options.MapFrom(p => p.Address.Street))
            .ForMember(m => m.Email, options => options.MapFrom(p => p.Contact.Email))
            .ReverseMap();
    }
}