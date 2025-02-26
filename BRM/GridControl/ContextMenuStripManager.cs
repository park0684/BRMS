using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GridControl
{
    class ContextMenuStripManager
    {
        public static ContextMenuStrip GetStandardContextMenu(BrsDataGridView grid, Action<string> onCustomAction)
        {
            var menu = new ContextMenuStrip();

            menu.Items.Add("펼쳐보기", null, (_, __) => grid.ShowExpendedColumns());
            menu.Items.Add("줄여보기", null, (_, __) => grid.ShowMinimalColumns());
            menu.Items.Add(new ToolStripSeparator());

            if (grid.EnableExport)
                menu.Items.Add("엑셀 저장", null, (_, __) => GridExport.ExportToExcel(grid));

            if (grid.EnablePrint)
                menu.Items.Add("인쇄", null, (_, __) => GridPrint.Print(grid));

            //if (grid.CustomForms.Count > 0)
            //{
            //    menu.Items.Add(new ToolStripSeparator());
            //    foreach (var form in grid.CustomForms)
            //    {
            //        menu.Items.Add(form.DisplayName + " 출력", null, (_, __) => onCustomAction?.Invoke("Form:" + form.Id));
            //    }
            //}

            return menu;
        }
    }
}
