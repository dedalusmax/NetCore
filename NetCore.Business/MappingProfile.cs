using AutoMapper;
using NetCore.Business.Authentication;
using NetCore.Business.Services;
using NetCore.Business.Validation;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Business
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ValidationResult, ValidationError>()
                .ForMember(_ => _.PropertyNames, _ => _.MapFrom(__ => __.MemberNames));

            AuthenticationService.RegisterMappings(this);
            CurrencyService.RegisterMappings(this);
            CountryService.RegisterMappings(this);
            RoleService.RegisterMappings(this);
            UserService.RegisterMappings(this);
        }
    }
}
