using System;
using System.Collections.Generic;
using Application.Users.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Service.Common.Helpers;
using Application.Users.Queries.Interfaces;
using Application.Users.Commands;
using System.Linq;

namespace Service.Users.Controllers
{
    [Route("api/userscollection")]
   public class UsersCollectionController : Controller
    {
        private readonly IUserQuery _userQuery;
        private readonly IUserCommand _userCommand;
        public UsersCollectionController(IUserQuery userQuery, IUserCommand userCommand)
        {
            _userQuery = userQuery;
            _userCommand = userCommand;
        }

        [HttpGet("{ids}", Name ="GetUsersById")]
        public async Task<IActionResult> GetUsers([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            var outerFacingModelUsers = await _userQuery.ExecuteGetResourcesById(ids);

            return Ok(outerFacingModelUsers);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateUsers([FromBody] IEnumerable<UserToCreateDto> userToCreateDtos)
        {
            if(userToCreateDtos is null)
            {
                return BadRequest("Need user(s) resource in request body.");
            }

            List<UserDto> outerFacingModelUsers = new List<UserDto>();

            foreach(UserToCreateDto user in userToCreateDtos)
            {
                outerFacingModelUsers.Add(_userCommand.ExecuteAddResource(user));
            }

            if(!(await _userCommand.ExecuteSaveAsync())) 
            {
                throw new Exception("Server had problems saving user resources");
            }

           var idsAsString = string.Join(",", outerFacingModelUsers.Select(u => u.Id));


            return CreatedAtRoute("GetUsersById",new {ids = idsAsString },outerFacingModelUsers);
        }
    }
}
