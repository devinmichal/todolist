using System;
using System.Collections.Generic;
using System.Text;
using Application.Items.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Items.Queries.Interfaces;
using Service.Common.Extensions;
using Application.Items.Commands;
using Microsoft.AspNetCore.JsonPatch;

namespace Service.Items.Controllers
{

    [Route("api/")]
    public class ItemsController : Controller
    {
        private readonly IItemQuery _itemQuery;
        private readonly IItemsCommand _itemsCommand;
        public ItemsController(IItemQuery itemQuery, IItemsCommand itemsCommand)
        {
            _itemQuery = itemQuery;
            _itemsCommand = itemsCommand;
        }

        [HttpGet("lists/{listId}/items", Name = "GetListItems")]
        public async Task<IActionResult> GetListItems([FromRoute] Guid listId)
        {
            var outerFacingModelItems = await _itemQuery.ExecuteGetResourcesAsync(listId);

            return Ok(outerFacingModelItems.ShapeData(null));
        }

        [HttpGet("lists/{listId}/items/{id}", Name = "GetListItem")]
        public async Task<IActionResult> GetListItem([FromRoute] Guid listId, [FromRoute] Guid id)
        {
            var outerFacingModeItem = await _itemQuery.ExecuteGetResourceByIdAsync(listId, id);

            return Ok(outerFacingModeItem.ShapeData(null));
        }

        [HttpPost("lists/{listId}/items",Name = "CreateListItem")]
        public async Task<IActionResult> CreateListItem([FromRoute] Guid listId, [FromBody]ItemToCreateDto dto)
        {
            var outerFacingModelItem = _itemsCommand.ExecuteAddResource(dto, listId);

            if(!(await _itemsCommand.ExecuteSaveAsync()))
            {
                throw new Exception("Problem saving the created item resource");
            }

            return CreatedAtRoute("GetListItem", new { listId = listId, id = outerFacingModelItem.Id } , outerFacingModelItem);
        }

        [HttpDelete("lists/items{id}", Name ="DeleteItem")]
        public async Task<IActionResult> DeleteItem([FromRoute] Guid id)
        {
            if((await _itemsCommand.ExecuteDeleteResource(id)) <= 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("lists/items/{id}", Name = "PartiallyUpdateItem")]
        public async Task<IActionResult> PartiallyUpdateItem([FromRoute] Guid id, [FromBody] JsonPatchDocument<ItemToUpdateDto> patch)
        {
            var dto = new ItemToUpdateDto();

            if(patch is null)
            {
                return BadRequest();
            }
            patch.ApplyTo(dto);

            var resource = await _itemQuery.ExecuteGetResourceByIdWithTracking(id);

            if (resource is null)
            {
                return NotFound();
            }

            _itemsCommand.ExecuteUpdateResource(resource, dto);

            if(!(await _itemsCommand.ExecuteSaveAsync()))
            {
                throw new Exception("There was a problem with updating item resource");
            }

            return NoContent();
        }
    }
}
