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
    public class Question : IEntity
    {
        public int QuestionID { get; set; }
        public int TestCategoryID { get; set; }
        public string Content { get; set; }
        public int? TrueAnswer { get; set; }
        public TestCategory TestCategory { get; set; }

        public int? TestID { get; set; }
        public Test Test { get; set; }
        public TestAnswer TestAnswer { get; set; }
        public List<QuestionOption> QuestionOptions { get; set; }
        public List<Image> Images { get; set; }


        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string? SavedUrl { get; set; }

        [NotMapped]
        public string? SignedUrl { get; set; }
        public string? SavedFileName { get; set; }

    }
}
