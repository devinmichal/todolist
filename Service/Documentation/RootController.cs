using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Service.Documentation
{
    [Route("api")]
   public class RootController : Controller
    {
        private readonly IUrlHelper _urlHelper;
        public RootController(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if(mediaType == "application/vnd.marvin.hateoas+json")
            {
                var links = new List<LinkDto>();


                links.Add(new LinkDto()
                {
                    Href = _urlHelper.Link("GetRoot", new { }),
                    Rel = "self",
                    Method = "GET"
                });

                links.Add(new LinkDto()
                {
                    Href = _urlHelper.Link("GetUsers", new { }),
                    Rel = "getUsers",
                    Method = "GET"
                });

                links.Add(new LinkDto()
                {
                    Href = _urlHelper.Link("CreateUsers", new { }),
                    Rel = "createUsers",
                    Method = "POST"
                });



                return Ok(links);
            }



            return NoContent();
        }
    }
}
