using Business.Abstract;
using Core.Constants;
using Entities.Dtos;
using Entities.Enums;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HealthTracker
{
    public class AddEditMealForm : MaterialForm
    {
        private readonly IMealService _mealService;
        private readonly MealDto _editingMeal;
        private readonly int _userId;

        private TableLayoutPanel layout;
        private DateTimePicker datePicker;
        private ComboBox cmbType;
        private MaterialTextBox2 txtDescription;
        private NumericUpDown numCalories;
        private MaterialButton btnSave;
        private MaterialButton btnCancel;

        public AddEditMealForm(int userId, IMealService mealService)
        {
            _userId = userId;
            _mealService = mealService;

            this.Text = "Yeni Öğün";
            InitializeForm();
            InitializeComponents();
        }

        public AddEditMealForm(MealDto mealToEdit, IMealService mealService) : this(mealToEdit.UserId, mealService)
        {
            _editingMeal = mealToEdit;
            this.Text = "Öğünü Düzenle";

            datePicker.Value = _editingMeal.Date;
            cmbType.SelectedItem = _editingMeal.Type;
            txtDescription.Text = _editingMeal.Description;
            numCalories.Value = _editingMeal.Calories;
        }

        private void InitializeForm()
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Pink500, Primary.Pink700, Primary.Pink200, Accent.DeepOrange200, TextShade.WHITE);

            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void InitializeComponents()
        {
            layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40),
                ColumnCount = 2,
                RowCount = 5
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 5; i++)
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));

            var lblDate = new Label { Text = "Tarih:", Anchor = AnchorStyles.Right, AutoSize = true };
            datePicker = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 200 };

            var lblType = new Label { Text = "Tür:", Anchor = AnchorStyles.Right, AutoSize = true };
            cmbType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
            cmbType.DataSource = Enum.GetValues(typeof(MealType));

            if (cmbType.Items.Count > 0)
                cmbType.SelectedIndex = 0;

            var lblDesc = new Label { Text = "Açıklama:", Anchor = AnchorStyles.Right, AutoSize = true };
            txtDescription = new MaterialTextBox2 { Hint = "Ne yedin?", Width = 200 };

            var lblCalories = new Label { Text = "Kalori:", Anchor = AnchorStyles.Right, AutoSize = true };
            numCalories = new NumericUpDown
            {
                Width = 200,
                Minimum = ValidationConstants.MinCalories,
                Maximum = ValidationConstants.MaxCalories,
                Value = ValidationConstants.MinCalories
            };

            btnSave = new MaterialButton { Text = "Kaydet", Type = MaterialButton.MaterialButtonType.Contained };
            btnSave.Click += BtnSave_Click;

            btnCancel = new MaterialButton { Text = "İptal", Type = MaterialButton.MaterialButtonType.Contained };
            btnCancel.Click += (s, e) => this.Close();

            layout.Controls.Add(lblDate, 0, 0);
            layout.Controls.Add(datePicker, 1, 0);
            layout.Controls.Add(lblType, 0, 1);
            layout.Controls.Add(cmbType, 1, 1);
            layout.Controls.Add(lblDesc, 0, 2);
            layout.Controls.Add(txtDescription, 1, 2);
            layout.Controls.Add(lblCalories, 0, 3);
            layout.Controls.Add(numCalories, 1, 3);
            layout.Controls.Add(btnSave, 0, 4);
            layout.Controls.Add(btnCancel, 1, 4);

            this.Controls.Add(layout);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem == null || numCalories.Value <= 0)
            {
                MessageBox.Show("Lütfen geçerli bilgiler girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = _editingMeal ?? new MealDto { UserId = _userId };

            dto.Date = DateTime.SpecifyKind(datePicker.Value.Date, DateTimeKind.Utc);
            dto.Type = (MealType)cmbType.SelectedItem;
            dto.Description = txtDescription.Text;
            dto.Calories = (int)numCalories.Value;

            if (_editingMeal == null)
                _mealService.Add(dto);
            else
                _mealService.Update(dto);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
