using System.ComponentModel.DataAnnotations; // This is for validations
using System;
using System.Collections.Generic;

namespace test.Models
{
    public class UserWrapper: BaseEntity
    {
        public List<User> users {get; set;}
        public List<Friendship> friends {get; set;}

        public UserWrapper(List<User> theuser, List<Friendship> thefriends )
        {
            users = theuser;
            friends = thefriends;
        }
    }
}