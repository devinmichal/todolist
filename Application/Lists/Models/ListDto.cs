using Application.Common.Models;
using Application.Items.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Lists.Models
{
  public  class ListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid AuthorId { get; set; }
        public string Author { get; set; }
        public IEnumerable<ItemDto> Items { get; set; }
        public int ItemsLeftToComplete { get; set; }
        public IEnumerable<LinkDto> Links { get; set; } 
    }
}
