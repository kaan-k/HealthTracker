using Business.Abstract;
using Entities.Dtos;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HealthTracker
{
    public class WaterLogForm : MaterialForm
    {
        private readonly UserDto _user;
        private readonly IWaterLogService _waterLogService;

        private FlowLayoutPanel panelButtons;
        private MaterialButton btnAdd;
        private MaterialButton btnEdit;
        private MaterialButton btnDelete;
        private MaterialButton btnRefresh;
        private MaterialListView listWaterLogs;

        public WaterLogForm(UserDto user, IWaterLogService waterLogService)
        {
            _user = user;
            _waterLogService = waterLogService;

            ApplyMaterialSkinTheme();
            InitializeComponents();
            LoadWaterLogs();
        }

        private void ApplyMaterialSkinTheme()
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(Primary.LightBlue500, Primary.LightBlue700, Primary.LightBlue200, Accent.Blue200, TextShade.WHITE);

            this.Text = $"{_user.FullName} - Su Tüketimi";
            this.Size = new Size(900, 600);
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

            listWaterLogs = new MaterialListView
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                BorderStyle = BorderStyle.None,
                OwnerDraw = true,
                View = View.Details,
                MouseState = MaterialSkin.MouseState.OUT
            };

            listWaterLogs.Columns.Add("ID", 70);
            listWaterLogs.Columns.Add("Tarih", 200);
            listWaterLogs.Columns.Add("Miktar (ml)", 200);

            this.Controls.Add(listWaterLogs);
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

        private void LoadWaterLogs()
        {
            var logs = _waterLogService.GetLogsByUserId(_user.Id.Value);
            listWaterLogs.Items.Clear();

            foreach (var log in logs)
            {
                var item = new ListViewItem(log.Id.ToString());
                item.SubItems.Add(log.Date.ToString("yyyy-MM-dd"));
                item.SubItems.Add($"{log.AmountMl} ml");
                listWaterLogs.Items.Add(item);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new AddEditWaterLogForm(_user.Id.Value, _waterLogService);
            if (form.ShowDialog() == DialogResult.OK)
                LoadWaterLogs();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listWaterLogs.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlenecek kaydı seçin.");
                return;
            }

            int id = int.Parse(listWaterLogs.SelectedItems[0].SubItems[0].Text);
            var log = _waterLogService.GetById(id);

            var form = new AddEditWaterLogForm(log, _waterLogService);
            if (form.ShowDialog() == DialogResult.OK)
                LoadWaterLogs();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listWaterLogs.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek kaydı seçin.");
                return;
            }

            int id = int.Parse(listWaterLogs.SelectedItems[0].SubItems[0].Text);
            var confirm = MessageBox.Show("Kayıt silinsin mi?", "Onay", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                _waterLogService.Delete(id);
                LoadWaterLogs();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadWaterLogs();
        }
    }

    //static class Extensions
    //{
    //    public static T Also<T>(this T self, Action<T> act)
    //    {
    //        act(self);
    //        return self;
    //    }
    //}
}
