using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class TestCategory : IEntity
    {
        public int TestCategoryID { get; set; }
        public string TestCategoryName { get; set; }
        public List<Test> Tests { get; set; }
        public List<Question> Questions { get; set; }
        public List<TestAnswer> TestAnswers { get; set; }
    }
}
