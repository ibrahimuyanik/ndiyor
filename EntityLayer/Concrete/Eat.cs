using CoreLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Eat : IEntity
    {
        public int EatID { get; set; }
        public string EatTitle { get; set; }
        public string Content { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<Comment> Comments { get; set; }
        public bool? EatStatus { get; set; }

        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string? SavedUrl { get; set; }

        [NotMapped]
        public string? SignedUrl { get; set; }
        public string? SavedFileName { get; set; }

    }
}
