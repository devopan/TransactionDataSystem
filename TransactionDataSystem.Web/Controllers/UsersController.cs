using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionDataSystem.Services.DTOs;
using TransactionDataSystem.Services.Interfaces;
using TransactionDataSystem.Services.Services;

namespace TransactionDataSystem.Web.Controllers
{
    public class UsersController : BaseController<UserDto, CreateUserDto, Guid>
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto updateUserDto)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, updateUserDto);
            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        protected override async Task<UserDto> ReadSingleAsync(Guid id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        protected override async Task<UserDto> CreateAsync(CreateUserDto createDto)
        {
            return await _userService.CreateUserAsync(createDto);
        }

        protected override Guid GetEntityId(UserDto entity)
        {
            return entity.Id;
        }

        protected override async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _userService.GetAllUsersAsync();
        }
    }
}