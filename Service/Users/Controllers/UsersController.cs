using Application.Users.Commands;
using Application.Users.Models;
using Application.Users.Queries.Interfaces;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Service.Common.Extensions;
using Application.Common.Helper;
using System.Collections.Generic;
using Application.Common.Models;
using System.Dynamic;
using Polly;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Service.Users.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserQuery _userQuery;
        private readonly IUserCommand _userCommand;
        private readonly IUrlHelper _urlHelper;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IUserQuery userQuery, IUserCommand userCommand, IUrlHelper urlHelper, ILogger<UsersController> logger)
        {
            _userQuery = userQuery;
            _userCommand = userCommand;
            _urlHelper = urlHelper;
            _logger = logger;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetUsers(UserQueryParameters parameters, [FromHeader(Name = "Accept")] string mediaType )
        {
           
            _logger.LogInformation("Querying for users");
            var userDtos = await _userQuery.ExecuteGetResourcesAsync(parameters);

            _logger.LogInformation($"Shaping user resource based of off field(s): {parameters.Fields}");
            var expandoObjects = userDtos.ShapeData(parameters.Fields);

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var outerFacingModels = CreateUsersWithLinks(userDtos, expandoObjects, parameters);

                _logger.LogInformation($"returning users resource");

                return Ok(outerFacingModels);
            }

            _logger.LogInformation($"returning users resource");

            return Ok(expandoObjects);
         }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id, UserQueryParameters parameters)
        {
         
            var userDto = await _userQuery.ExecuteGetResourceByIdAsync(id);

            if (userDto is null)
            {
                return NotFound();
            }

            var links = CreateLinks(id, parameters.Fields);

             var outerFacingModel =  userDto.ShapeData(parameters.Fields);

            outerFacingModel.TryAdd("Links",links);
            

            return Ok(outerFacingModel);
        }

        [HttpPost(Name ="CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserToCreateDto dto)
        {

            if (dto is null)
            {
                return BadRequest();
            }

            var outerFacingModelUser = _userCommand.ExecuteAddResource(dto);

            if (!(await _userCommand.ExecuteSaveAsync()))
            {
                throw new Exception("There was a problem with saving the user resource");
            }

            outerFacingModelUser.ShapeData(null)
               .TryAdd("links", CreateLinks(outerFacingModelUser.Id, null));

            return CreatedAtRoute("GetUser",
                new { id = outerFacingModelUser.Id },outerFacingModelUser );
        }

        [HttpPut("{id}", Name ="UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UserToUpdateDto dto)
        {
            if (dto is null)
            {
                return BadRequest();
            }
            UserDto outerFacingModelUser;

            User resourceEntity = await _userQuery.ExecuteGetResourceByIdWithTracking(id);

            if (resourceEntity is null)
            {
                outerFacingModelUser = _userCommand.ExecuteAddResource(dto, id);

                if (!(await _userCommand.ExecuteSaveAsync()))
                {
                    throw new Exception("There was a problem with saving the user resource");
                }

                outerFacingModelUser
                   .ShapeData(null)
                   .TryAdd("links", CreateLinks(id, null));

                return CreatedAtRoute("GetUser", new { id = id },outerFacingModelUser);
            }


            //if it does update the user with tracking
            _userCommand.ExecuteUpdateResource(resourceEntity, dto);

            //then save changes
            if (!(await _userCommand.ExecuteSaveAsync()))
            {
                throw new Exception("There was a problem with saving the user resource");
            }

            Response.Headers.Add("Location", _urlHelper.Link("GetUser", new { id = resourceEntity.Id }));
            return NoContent();
        }

        [HttpPatch("{id}",Name ="PartialUpdateUser")]
        public async Task<IActionResult> PartiallyUpdateUser(Guid id, [FromBody] JsonPatchDocument<UserManipulationDto> patch)
        {

            if (patch is null)
            {
                return BadRequest();
            }

            UserDto outerFacingModelUser;
            UserToUpdateDto dto = new UserToUpdateDto();
            patch.ApplyTo(dto);


            User resourceEntity = await _userQuery.ExecuteGetResourceByIdWithTracking(id);

            if (resourceEntity is null)
            {
                outerFacingModelUser = _userCommand.ExecuteAddResource(dto, id);

                if (!(await _userCommand.ExecuteSaveAsync()))
                {
                    throw new Exception("There was a problem with saving the user resource");
                }

                outerFacingModelUser
                 .ShapeData(null)
                 .TryAdd("links", CreateLinks(id, null));

                return CreatedAtRoute("GetUser", new { id = id }, outerFacingModelUser);
            }

            _userCommand.ExecuteUpdateResource(resourceEntity, dto);

            if (!(await _userCommand.ExecuteSaveAsync()))
            {
                throw new Exception("There was a problem with saving the user resource");
            }

            Response.Headers.Add("Location", _urlHelper.Link("GetUser", new { id = resourceEntity.Id }));
            return NoContent();

        }

        [HttpDelete("{id}", Name ="DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromRoute]Guid id)
        {
          if( (await _userCommand.ExecuteDeleteResource(id)) <= 0)
            {
                return NotFound();
            } 

            return NoContent();
        }
        private IEnumerable<LinkDto> CreateLinks(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if(string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkDto()
                {
                    Href = _urlHelper.Link("GetUser", new { id = id}),
                    Method = "GET",
                    Rel = "thisUser"
                });
            } else
            {
                links.Add(new LinkDto()
                {
                    Href = _urlHelper.Link("GetUser", new { id = id, fields = fields}),
                    Method = "GET",
                    Rel = "thisUser"
                });
            }
            links.Add(new LinkDto()
            {
                Href = _urlHelper.Link("DeleteUser", new { id = id}),
                Method = "DELETE",
                Rel = "deleteUser"
            });

            links.Add(new LinkDto()
            {
                Href = _urlHelper.Link("CreateUser", new { }),
                Method = "POST",
                Rel = "createUser"
            });

            links.Add(new LinkDto()
            {
                Href = _urlHelper.Link("UpdateUser", new {id = id }),
                Method = "PUT",
                Rel = "updateUser"
            });

            links.Add(new LinkDto()
            {
                Href = _urlHelper.Link("PartialUpdateUser", new {id = id }),
                Method = "PATCH",
                Rel = "partiallyUpdateUser"
            });

            links.Add(new LinkDto()
            {
                Href = _urlHelper.Link("GetUserLists", new { userId = id}),
                Method = "GET",
                Rel = "getUserLists"
            });

            links.Add(new LinkDto()
            {
                Href = _urlHelper.Link("CreateUserList", new { userId = id}),
                Method = "POST",
                Rel = "createUserList"
            });


            return links;
        }

        private IEnumerable<ExpandoObject> CreateUsersWithLinks(IEnumerable<UserDto> userDtos, IEnumerable<ExpandoObject> expandoObjects, UserQueryParameters parameters )
        {
           var outerFacingModels =  new List<ExpandoObject>();

            foreach (ExpandoObject user in expandoObjects)
            {
                foreach (UserDto dto in userDtos)
                {
                    user.TryAdd("Links", CreateLinks(dto.Id, parameters.Fields));
                    outerFacingModels.Add(user);
                }
            }

            return outerFacingModels;
        }
    }
}
