using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using common.Interface;
using common.DTOs;

namespace BRM.Views
{
    public partial class MainView : Form,IMainView
    {
        private Dictionary<string, Panel> _menuPanels = new Dictionary<string, Panel>();
        public MainView()
        {
            InitializeComponent();
            ViewEvent();
        }

        public string MenuTitle
        {
            get { return lblMenuTitle.Text; }
            set { lblMenuTitle.Text = value; }
        }

        public event EventHandler<string> OnClickMenu;
        public event EventHandler OnClickSearch;
        public event EventHandler DBConfigEvent;

        public void CreateMenu(List<MenuCategoryDto> menu)
        {
            //pnlSideMenu.Controls.Clear();
            _menuPanels.Clear();

            foreach (var category in menu)
            {
                //카테고리 버튼
                var categoryBtn = new Button
                {
                    Text = category.CategoryDisplay,
                    Dock = DockStyle.Top,
                    Height = 40,
                    Tag = category.CategoryKey,
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.FromArgb(39, 39, 58),
                    ForeColor = Color.White,
                    Font = new Font("맑은 고딕", 10F),
                    FlatStyle = FlatStyle.Flat,
                };
                categoryBtn.FlatAppearance.BorderSize = 0;

                categoryBtn.Click += (s, e) =>
                {
                    ShowOnlyPanel(category.CategoryKey);
                };

                //pnlSideMenu.Controls.Add(categoryBtn);

                var itemPanel = new Panel
                {
                    //Name = $"panel_{category.CategoryKey}",
                    Dock = DockStyle.Top,
                    Visible = false,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    BackColor = Color.FromArgb(50, 50, 70),
                    Padding = new Padding(20, 0, 0, 0)
                };

                foreach (var item in category.MenuItems)
                {
                    var btnMenu = new Button
                    {
                        Text = item.MenuDisplay,
                        Dock = DockStyle.Top,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        UseVisualStyleBackColor = false,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = new Size(150, 35),
                        Font = new Font("맑은 고딕", 10F),
                        Tag = item.Menukey
                    };
                    btnMenu.FlatAppearance.BorderSize = 0;

                    btnMenu.Click += (s, e) =>
                    {
                        MenuTitle = item.MenuDisplay;
                        OnClickMenu?.Invoke(this, item.Menukey);
                    };

                    itemPanel.Controls.Add(btnMenu);
                    itemPanel.Controls.SetChildIndex(btnMenu, 0);
                }

                _menuPanels[category.CategoryKey] = itemPanel;

                pnlSideMenu.Controls.Add(categoryBtn);
                pnlSideMenu.Controls.SetChildIndex(categoryBtn, 0);
                pnlSideMenu.Controls.Add(itemPanel);
                pnlSideMenu.Controls.SetChildIndex(itemPanel, 0);

            }
        }

        public void LoadControlToPanel(Control control)
        {
            pnllViewer.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnllViewer.Controls.Add(control);
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void ShowOnlyPanel(string key)
        {
            foreach (var pair in _menuPanels)
            {
                pair.Value.Visible = pair.Key == key;
            }
        }
        private void ViewEvent()
        {
            btnDatabaseConfig.Click += (s, e) => DBConfigEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
