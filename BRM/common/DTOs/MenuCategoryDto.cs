using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.DTOs
{
    public class MenuCategoryDto
    {
        public string CategoryKey { get; set; }
        public string CategoryDisplay { get; set; }
        public List<MenuItemDto> MenuItems { get; set; } = new List<MenuItemDto>();
    }
}
