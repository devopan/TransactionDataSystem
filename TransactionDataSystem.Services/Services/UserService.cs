using TransactionDataSystem.Domain.Entities;
using TransactionDataSystem.Infrastructure.UnitOfWork;
using TransactionDataSystem.Services.DTOs;
using TransactionDataSystem.Services.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionDataSystem.Domain.Entities;
using TransactionDataSystem.Infrastructure.UnitOfWork;
using TransactionDataSystem.Services.Interfaces;

namespace TransactionDataSystem.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                return null;

            _mapper.Map(updateUserDto, user);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                return false;

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
