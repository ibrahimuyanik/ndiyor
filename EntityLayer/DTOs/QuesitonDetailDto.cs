using CoreLayer.Entities;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class QuesitonDetailDto:IDto
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
    }
}
