using System;
using common.DTOs;

namespace common.Interface
{
    public interface IViewFactory
    {
        IView CreateView(string menuKey);
        IView OpenPopup(string menuKey, int code);
        void OpenCallBackPopup(string menuKey,int code, Delegate callback);
        MenuItemDto GetDefinition(string menuKey);
    }
}
