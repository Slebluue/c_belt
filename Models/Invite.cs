using System.ComponentModel.DataAnnotations; // This is for validations
using System;
using System.Collections.Generic;

namespace test.Models
{
    public class Invite: BaseEntity
    {
        public int inviteid {get; set;}
        public int userid {get; set;}
        public int senderid {get; set;}
        public User sender {get; set;}

    }
}