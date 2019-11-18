using Domain.Entities.Lists;
using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.Common.Extensions
{
  public static class ToDoListContextExtension
    {
        public static void SeedDatabase(this ToDoListContext _context)
        {
            //Removes all records from DB
            _context.RemoveRange(_context.Users);
            _context.RemoveRange(_context.Lists);
            _context.RemoveRange(_context.Items);
            _context.SaveChanges();

            //Create data to populate DB
            List<User> users = new List<User>();
            var user = new User()
            {
                Id = new Guid("27745005-8975-49f6-8321-86d88e3d38b3"),
                FirstName = "Jules",
                LastName = "Winnefield"
            };

            var user2 = new User()
            {
                Id = new Guid("02a0b085-3e2b-4df0-8ac8-b9de80c9ff99"),
                FirstName = "Vincent",
                LastName = "Vega"
            };

            for(int i = 0; i < 500; i++)
            {
                users.Add(new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "TestUser" + i,
                    LastName = "LastName" + i
                });
            }

            users.Add(user);
            users.Add(user2);

            _context.AddRange(users);

            List<List> lists = new List<List>();

            var julesList = new List()
            {
                Id = new Guid("27745005-8975-49f6-8321-86d88e3d38b3"),
                Created = DateTimeOffset.Now,
                Name = "today's Agenda",
                UserId = new Guid("27745005-8975-49f6-8321-86d88e3d38b3")

            };

            var vincentList = new List()
            {
                Id = new Guid("02a0b085-3e2b-4df0-8ac8-b9de80c9ff99"),
                Created = DateTimeOffset.Now,
                Name = "today's Agenda",
                UserId = new Guid("02a0b085-3e2b-4df0-8ac8-b9de80c9ff99")

            };

            lists.Add(julesList);
            lists.Add(vincentList);

            _context.AddRange(lists);
            _context.SaveChanges();
        }
    }
}
