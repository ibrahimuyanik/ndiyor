using CoreLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class AppUser: IdentityUser<int>, IEntity
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public int? ConfirmCode { get; set; }
        public List<TestAnswer> TestAnswers { get; set; }
        public List<Comment> Comments { get; set; }


    }
}
