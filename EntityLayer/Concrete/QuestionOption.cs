using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class QuestionOption : IEntity
    {
        public int QuestionOptionID { get; set; }
        public int QuestionID { get; set; }
        public string Option { get; set; }

        public Question Question { get; set; }
    }
}
