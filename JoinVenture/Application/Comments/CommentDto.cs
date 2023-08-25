using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string Body { get; set; }
        public string UserName { get; set; }
        public string ShowName { get; set; }
        public string Image { get; set; }
    }
}