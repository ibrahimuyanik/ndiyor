using CoreLayer.Entities;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class TestCategoryDetailDto : IDto
    {
        public int TestCategoryID { get; set; }
        public string TestCategoryName { get; set; }
        public int TestID { get; set; }
        public string Title { get; set; }
        public int? ImageID { get; set; }
        public string ImageUrl { get; set; }
        public int QuestionID { get; set; }
        public List<Question> Questions { get; set; }

    }
}
