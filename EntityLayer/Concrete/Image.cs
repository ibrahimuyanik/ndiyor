﻿using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Image : IEntity
    {
        public int ImageID { get; set; }
        public string ImageUrl { get; set; }
        public int? NewsID { get; set; }

        public int? TestID { get; set; }
        public int? QuestionID { get; set; }

        public News News { get; set; }
        public Test Test { get; set; }
        public Question Question { get; set; }
    }
}
