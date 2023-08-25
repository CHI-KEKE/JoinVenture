using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserDto
    {
        public string ShowName { get; set; }
        public string Token { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}