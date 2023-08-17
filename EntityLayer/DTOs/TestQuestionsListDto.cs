using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class TestQuestionsListDto
    {
        public Test Test { get; set; }
        public List<Question> Questions { get; set; }
        public Dictionary<int, List<Image>> QuestionImages { get; set; }
        public Dictionary<int, List<QuestionOption>> QuestionOptions { get; set; }



   

    }
}
