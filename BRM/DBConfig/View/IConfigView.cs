using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConfig.View
{
    public interface IConfigView
    {
        string Address { get; set; }
        string Port { get; set; }
        string User { get; set; }
        string Password { get; set; }
        string Database { get; set; }

        event EventHandler ConnectionTestEvent;
        event EventHandler CloseFormEvent;
        event EventHandler SaveEvent;
        event EventHandler PortChangedEvent;

        void CloseForm();
        void ShowFrom();
        void ShowMessage(string message);
        bool ShowConfirmMessage(string message);

    }
}
