using common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Model
{
    public static class UserSession
    {
        public static UserDto CurrentUser { get; set; }
    }
}
