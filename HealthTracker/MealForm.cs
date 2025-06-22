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
    public class MealForm : MaterialForm
    {
        private readonly UserDto _user;
        private readonly IMealService _mealService;

        private FlowLayoutPanel panelButtons;
        private MaterialButton btnAdd;
        private MaterialButton btnEdit;
        private MaterialButton btnDelete;
        private MaterialButton btnRefresh;
        private MaterialListView listMeals;

        public MealForm(UserDto userDto, IMealService mealService)
        {
            _user = userDto;
            _mealService = mealService;

            ApplyMaterialSkinTheme();
            InitializeComponents();
            LoadMeals();
        }

        private void ApplyMaterialSkinTheme()
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(Primary.DeepPurple500, Primary.DeepPurple700, Primary.DeepPurple200, Accent.Purple200, TextShade.WHITE);

            this.Text = $"{_user.FullName} - Öğün Geçmişi";
            this.Size = new Size(1100, 700);
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

            listMeals = new MaterialListView
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                BorderStyle = BorderStyle.None,
                OwnerDraw = true,
                View = View.Details,
                MouseState = MaterialSkin.MouseState.OUT
            };

            listMeals.Columns.Add("ID", 70);
            listMeals.Columns.Add("Tarih", 150);
            listMeals.Columns.Add("Tür", 150);
            listMeals.Columns.Add("Açıklama", 400);
            listMeals.Columns.Add("Kalori", 100);

            this.Controls.Add(listMeals);
            this.Controls.Add(panelButtons);
        }

        private MaterialButton CreateButton(string text, EventHandler handler)
        {
            var button = new MaterialButton
            {
                Text = text,
                Type = MaterialButton.MaterialButtonType.Contained,
                AutoSize = true,
                UseAccentColor = false
            };
            button.Click += handler;
            return button;
        }

        private void LoadMeals()
        {
            var meals = _mealService.GetMealsByUserId(_user.Id.Value);
            listMeals.Items.Clear();

            foreach (var meal in meals)
            {
                var item = new ListViewItem(meal.Id.ToString());
                item.SubItems.Add(meal.Date.ToString("yyyy-MM-dd"));
                item.SubItems.Add(meal.Type.ToString());
                item.SubItems.Add(meal.Description);
                item.SubItems.Add(meal.Calories.ToString());
                listMeals.Items.Add(item);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new AddEditMealForm(_user.Id.Value, _mealService);
            if (form.ShowDialog() == DialogResult.OK)
                LoadMeals();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listMeals.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlenecek öğünü seçin.");
                return;
            }

            int mealId = int.Parse(listMeals.SelectedItems[0].SubItems[0].Text);
            var meal = _mealService.GetById(mealId);

            var form = new AddEditMealForm(meal.Id, _mealService);
            if (form.ShowDialog() == DialogResult.OK)
                LoadMeals();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listMeals.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek öğünü seçin.");
                return;
            }

            int mealId = int.Parse(listMeals.SelectedItems[0].SubItems[0].Text);
            var confirm = MessageBox.Show("Öğün silinsin mi?", "Onay", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                _mealService.Delete(mealId);
                LoadMeals();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMeals();
        }
    }
}
