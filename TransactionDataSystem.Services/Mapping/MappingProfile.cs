using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionDataSystem.Domain.Entities;
using TransactionDataSystem.Services.DTOs;

namespace TransactionDataSystem.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            // Transaction mappings
            CreateMap<Domain.Entities.Transaction, TransactionDto>();
            CreateMap<CreateTransactionDto, Domain.Entities.Transaction>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // UserAction mappings
            CreateMap<UserTransaction, UserTransactionDto>();
        }
    }
}
