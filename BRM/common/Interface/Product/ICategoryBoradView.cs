using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace common.Interface
{
    public interface ICategoryBoradView
    {
        bool CategoryStatus { get; }

        event EventHandler SaveEvent;
        event EventHandler CloseFormEvent;
        event EventHandler ShowFormEvent;

        event EventHandler CategoryTopEditEvent;
        event EventHandler CategoryTopAddEvent;
        event EventHandler CategoryMidEditEvetn;
        event EventHandler CategoryMidAddEvent;
        event EventHandler CategoryBotEditEvent;
        event EventHandler CategoryBotAddEvent;
        event EventHandler SelectCategoryTopEvent;
        event EventHandler SelectCategoryMidEvent;

        void CloseForm();
        void ShowForm();
        void ShowMessage(string message);
        void CategoryTopBindign(DataTable result);
        void CategoryMidBindign(DataTable result);
        void CategoryBotBindign(DataTable result);
        int GetCurrentTop();
        int GetCurrentMid();
        int GetCurrentBot();
        void SetIsEidtMod(int mode);


    }
}
