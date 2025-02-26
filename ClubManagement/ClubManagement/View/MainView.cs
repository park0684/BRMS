using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Common.Menu;

namespace ClubManagement.View
{
    public partial class MainView : Form, IMainView
    {
        private readonly Dictionary<string, Panel> _categoryPanels = new Dictionary<string, Panel>();
        private string _activeCategory = null;
        public MainView()
        {
            InitializeComponent();
            
        }

        public void SetMenuItems(Dictionary<string, List<MenuItemInfo>> groupedItems)
        {
            foreach (var pair in groupedItems)
            {
                string category = pair.Key;
                List<MenuItemInfo> items = pair.Value;

                // 서브 패널 먼저 생성
                Panel subPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Visible = false,
                    AutoSize = true
                };

                foreach (var item in items)
                {
                    Button btn = new Button
                    {
                        Text = item.Title,
                        Dock = DockStyle.Top,
                        Height = 30,
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.FromArgb(45, 58, 88),
                        Font = new Font("맑은 고딕", 12F)
                    };
                    btn.FlatAppearance.BorderSize = 0;
                    
                    btn.Click += delegate { item.ClickAction(); };
                    

                    subPanel.Controls.Add(btn);
                }

                // 카테고리 버튼
                Button btnCategory = new Button
                {
                    Text = category,
                    Dock = DockStyle.Top,
                    Height = 40,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("맑은 고딕", 12F)
                };
                btnCategory.FlatAppearance.BorderSize = 0;
                btnCategory.Click += delegate { ToggleCategory(category); };

                pnlSideMenu.Controls.Add(subPanel);
                pnlSideMenu.Controls.Add(btnCategory);

                _categoryPanels[category] = subPanel;
            }
        }


        private void ToggleCategory(string category)
        {
            foreach (var key in _categoryPanels.Keys)
            {
                _categoryPanels[key].Visible = key == category && _activeCategory != category;
            }

            _activeCategory = _activeCategory == category ? null : category;
        }

        public void LoadContent(Form form)
        {
            pnlView.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            pnlView.Controls.Add(form);
            form.Show();
        }
    }
}
