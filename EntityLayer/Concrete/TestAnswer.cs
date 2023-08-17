using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class TestAnswer : IEntity
    {
        public int TestAnswerID { get; set; }
        public int AppUserID { get; set; }
        public int TestID { get; set; }
        public int QuestionID { get; set; }
        public int TestCategoryID { get; set; }
        public int UserAnswer { get; set; }
        public TestCategory TestCategory { get; set; }
        public Question Question { get; set; }
        public AppUser AppUser { get; set; }
        //a
    }
}
