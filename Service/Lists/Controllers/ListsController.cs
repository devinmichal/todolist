using System;
using Application.Lists.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Lists.Models;
using Application.Lists.Commands;
using Microsoft.AspNetCore.JsonPatch;
using Application.Common.Models;
using System.Collections.Generic;

namespace Service.Lists.Controllers
{
    [Route("api/")]
   public class ListsController : Controller
    {
        private readonly IListQuery _listQuery;
        private readonly IListCommand _listCommand;
        private readonly IUrlHelper _urlHelper;
        public ListsController(IListQuery listQuery, IListCommand listCommand, IUrlHelper urlHelper)
        {
            _listQuery = listQuery;
            _listCommand = listCommand;
            _urlHelper = urlHelper;
        }

        [HttpGet("users/{userId}/lists",Name ="GetUserLists")]
        public async Task<IActionResult> GetUserLists([FromRoute] Guid userId)
        {
            var outerFacingModelLists =  await _listQuery.ExecuteGetResourcesAsync(userId);

            AddLinksToLists(outerFacingModelLists);

            return Ok(outerFacingModelLists);
        }

        [HttpGet("users/{userId}/lists/{id}", Name = "GetUserList")]
        public async Task<IActionResult> GetUserList([FromRoute] Guid userId,Guid id)
        {
            var outerFacingModelList = await _listQuery.ExecuteGetResourceByIdAsync(userId,id);

            if(outerFacingModelList is null)
            {
                return NotFound();
            }

            return Ok(outerFacingModelList);
        }

        [HttpPost("users/{userId}/lists", Name ="CreateUserList")]
        public async Task<IActionResult> CreateUserList([FromRoute] Guid userId, [FromBody] ListToCreateDto dto)
        {
            var outerFacingModelList = _listCommand.ExecuteAddResource(dto, userId);

            if(!(await _listCommand.ExecuteSaveAsync()))
            {
                throw new Exception("Problem saving list resource");
            }

            outerFacingModelList.Links = CreateLinks(outerFacingModelList.Id, userId);

           return CreatedAtRoute("GetUserList", new {userId = userId, id = outerFacingModelList.Id }, outerFacingModelList);
        }

        [HttpDelete("users/lists/{id}", Name ="DeleteUserList")]
        public async Task<IActionResult> DeleteUserList([FromRoute] Guid id)
        {
            if((await _listCommand.ExecuteDeleteResource(id) <= 0))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("users/lists/{id}", Name ="PartiallyUpdateList")]
        public async Task<IActionResult> PartialUpdateList([FromRoute] Guid id, JsonPatchDocument<ListToUpdateDto> patch)
        {
            if(patch is null)
            {
                return BadRequest();
            }
            var dto = new ListToUpdateDto() { };
            patch.ApplyTo(dto);

            var list = await _listQuery.ExecuteGetResourceByIdAsync(id);

            if(list is null)
            {
                return NotFound();
            }

            _listCommand.ExecuteUpdateResource(list, dto);

            if(!(await _listCommand.ExecuteSaveAsync()))
            {
                throw new Exception("Problem saving changes to list resource");
            }


            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinks(Guid id, Guid userId) 
        {
            var links = new List<LinkDto>();

            if(userId != null)
            {
                links.Add(new LinkDto()
                {
                    Href = _urlHelper.Link("GetUserList", new { userId = userId, id = id}),
                    Method = "GET",
                    Rel = "getUserList"
                });
            }

            links.Add(new LinkDto()
            {
                Href = _urlHelper.Link("DeleteUserList", new { id = id}),
                Method = "DELETE",
                Rel = "deleteUserList"
            });

            links.Add(new LinkDto()
            {
                Href = _urlHelper.Link("PartiallyUpdateList", new { id = id }),
                Method = "PATCH",
                Rel = "partiallyUpdateList"
            });

            links.Add(new LinkDto()
            {
                Href = _urlHelper.Link("GetListItems",new {listId = id }),
                Method = "GET",
                Rel = "getListItems"

            });

            links.Add(new LinkDto()
            {
                Href = _urlHelper.Link("CreateListItem", new {listId = id }),
                Method = "POST",
                Rel = "createListItem"

            });

            return links;
        }

        private void AddLinksToLists(IEnumerable<ListDto> dtos)
        {
            foreach( ListDto dto in dtos)
            {
                dto.Links = CreateLinks(dto.Id, dto.AuthorId);
            }

        }
    }
}
