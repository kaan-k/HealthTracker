using Business;
using Business.Abstract;
using Entities.Dtos;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HealthTracker
{
    public class WeightLogForm : MaterialForm
    {
        private readonly UserDto _user;
        private readonly IWeightLogService _weightLogService;
        private readonly IUserService _userService;

        private FlowLayoutPanel panelButtons;
        private MaterialButton btnAdd;
        private MaterialButton btnEdit;
        private MaterialButton btnDelete;
        private MaterialButton btnRefresh;
        private MaterialListView listLogs;

        public WeightLogForm(UserDto userDto, IWeightLogService weightLogService, IUserService userService)
        {
            _user = userDto;
            _weightLogService = weightLogService;
            _userService = userService;

            ApplyMaterialSkinTheme();
            InitializeComponents();
            LoadLogs();
        }

        private void ApplyMaterialSkinTheme()
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo200, Accent.LightBlue200, TextShade.WHITE);

            this.Text = $"{_user.FullName} - Kilo Geçmişi";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void InitializeComponents()
        {
            panelButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(20),
                BackColor = Color.White
            };

            btnAdd = CreateButton("Ekle", btnAdd_Click);
            btnEdit = CreateButton("Düzenle", btnEdit_Click);
            btnDelete = CreateButton("Sil", btnDelete_Click);
            btnRefresh = CreateButton("Yenile", btnRefresh_Click);

            panelButtons.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, btnRefresh });

            listLogs = new MaterialListView
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                BorderStyle = BorderStyle.None,
                OwnerDraw = true,
                View = View.Details,
                MouseState = MaterialSkin.MouseState.OUT
            };

            listLogs.Columns.Add("ID", 80);
            listLogs.Columns.Add("Tarih", 200);
            listLogs.Columns.Add("Kilo (kg)", 150);

            this.Controls.Add(listLogs);
            this.Controls.Add(panelButtons);
        }

        private MaterialButton CreateButton(string text, EventHandler handler)
        {
            return new MaterialButton
            {
                Text = text,
                Type = MaterialButton.MaterialButtonType.Contained,
                AutoSize = true,
                UseAccentColor = false
            }.Also(b => b.Click += handler);
        }

        private void LoadLogs()
        {
            var logs = _weightLogService.GetLogsByUserId(_user.Id.Value);
            listLogs.Items.Clear();

            foreach (var log in logs)
            {
                var item = new ListViewItem(log.Id.ToString());
                item.SubItems.Add(log.Date.ToString("yyyy-MM-dd"));
                item.SubItems.Add(log.WeightKg.ToString("F1"));
                listLogs.Items.Add(item);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new AddEditWeightLogForm(_user.Id.Value, _weightLogService, _userService);
            if (form.ShowDialog() == DialogResult.OK)
                LoadLogs();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listLogs.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlenecek logu seçin.");
                return;
            }

            int logId = int.Parse(listLogs.SelectedItems[0].SubItems[0].Text);
            var log = _weightLogService.GetById(logId);

            var form = new AddEditWeightLogForm(log, _weightLogService, _userService);
            if (form.ShowDialog() == DialogResult.OK)
                LoadLogs();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listLogs.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek logu seçin.");
                return;
            }

            int logId = int.Parse(listLogs.SelectedItems[0].SubItems[0].Text);
            var confirm = MessageBox.Show("Log silinsin mi?", "Onay", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                //_weightLogService.Delete(logId);
                LoadLogs();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLogs();
        }
    }

    static class Extensions
    {
        public static T Also<T>(this T self, Action<T> act)
        {
            act(self);
            return self;
        }
    }
}
