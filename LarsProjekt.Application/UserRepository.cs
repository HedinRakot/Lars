﻿using LarsProjekt.Domain;

namespace LarsProjekt.Application;

public class UserRepository
{
    public UserRepository()
    {
        Users = new List<User>
        {
            new User()
            {
                Id = 1,
                Name = "Admin",  
                Description = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
            },
            //new User()
            //{
            //    Id = 2,
            //    Name = "Yury"
            //},
            //new User()
            //{
            //    Id = 3,
            //    Name = "Lars"
            //}
        };
    }

    public List<User> Users { get; set; }
}
