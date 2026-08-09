using AutoMapper;
using Store.KH.Core.Dtos.Auth;
using Store.KH.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Core.Mapping.Auth
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<AddressDto, Address>()
                .ForMember(dest => dest.FName,
                    opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LName,
                    opt => opt.MapFrom(src => src.LastName));

            CreateMap<Address, AddressDto>()
                .ForMember(dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.FName))
                .ForMember(dest => dest.LastName,
                    opt => opt.MapFrom(src => src.LName));
        }

    }
}
