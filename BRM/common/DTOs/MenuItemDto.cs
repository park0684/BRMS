using System;

namespace common.DTOs
{
    public class MenuItemDto
    {
        public string Menukey { get; set; }
        public string MenuDisplay { get; set; }
        public ViewOpenType OpenType { get; set; }
        public int CallbackParamCount { get; set; } = 1;
    }

    public enum ViewOpenType
    {
        Panel,
        Popup
    }


}
