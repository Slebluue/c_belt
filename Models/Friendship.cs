using System.ComponentModel.DataAnnotations; // This is for validations
using System;
using System.Collections.Generic;

namespace test.Models
{
    public class Friendship: BaseEntity
    {
        public int friendshipid {get; set;}
        public int userid {get; set;}
        public User friend {get; set;}
        public int friendid {get; set;}

    }
}