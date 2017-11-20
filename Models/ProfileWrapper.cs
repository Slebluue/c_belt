using System.ComponentModel.DataAnnotations; // This is for validations
using System;
using System.Collections.Generic;

namespace test.Models
{
    public class ProfileWrapper: BaseEntity
    {
        public User user {get; set;}
        public List<Invite> invites {get; set;}
        public List<Friendship> friends {get; set;}
        public ProfileWrapper(User theuser, List<Invite> theinvites, List<Friendship> thefriends )
        {
            user = theuser;
            invites = theinvites;
            friends = thefriends;
        }

    }
}