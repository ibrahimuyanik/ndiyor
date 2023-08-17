using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Contact:IEntity
    {
        public int ContactID { get; set; }
        public string NameSurname { get; set; }
        public string SenderMail { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }

}
