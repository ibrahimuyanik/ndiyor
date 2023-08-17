using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Comment : IEntity
    {
        public int CommentID { get; set; }
        public int? TopCommentId { get; set; }
        public int? NewsID { get; set; }
        public int? EatID { get; set; }
        public string? Commentt { get; set; }
        public int? AppUserID { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public AppUser AppUser { get; set; }
        public News News { get; set; }
        public Eat Eat { get; set; }  


    }
}
