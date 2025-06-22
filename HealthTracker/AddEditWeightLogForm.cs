using Business.Abstract;
using Core.Constants;
using Entities.Dtos;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HealthTracker
{
    public class AddEditWeightLogForm : MaterialForm
    {
        private readonly IWeightLogService _weightLogService;
        private readonly IUserService _userService;
        private readonly WeightLogDto _editingLog;
        private readonly int _userId;

        private TableLayoutPanel layout;
        private DateTimePicker datePicker;
        private NumericUpDown numWeight;
        private MaterialButton btnSave;
        private MaterialButton btnCancel;

        public AddEditWeightLogForm(int userId, IWeightLogService weightLogService, IUserService userService)
        {
            _userId = userId;
            _weightLogService = weightLogService;
            _userService = userService;

            this.Text = "Yeni Kilo Girişi";
            InitializeForm();
            InitializeComponents();
        }

        public AddEditWeightLogForm(WeightLogDto logDto, IWeightLogService weightLogService, IUserService userService)
            : this(logDto.UserId, weightLogService, userService)
        {
            _editingLog = logDto;
            this.Text = "Kilo Girişini Düzenle";

            datePicker.Value = logDto.Date;
            numWeight.Value = (decimal)logDto.WeightKg;
        }

        private void InitializeForm()
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Teal500, Primary.Teal700, Primary.Teal200, Accent.Teal200, TextShade.WHITE);

            this.Size = new Size(500, 300);
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void InitializeComponents()
        {
            layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40),
                ColumnCount = 2,
                RowCount = 3
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 3; i++)
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));

            var lblDate = new Label { Text = "Tarih:", Anchor = AnchorStyles.Right, AutoSize = true };
            datePicker = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 200 };

            var lblWeight = new Label { Text = "Kilo (kg):", Anchor = AnchorStyles.Right, AutoSize = true };
            numWeight = new NumericUpDown
            {
                Width = 200,
                DecimalPlaces = 1,
                Minimum = (decimal)ValidationConstants.MinWeightKg,
                Maximum = (decimal)ValidationConstants.MaxWeightKg,
                Value = (decimal)ValidationConstants.MinWeightKg
            };

            btnSave = new MaterialButton { Text = "Kaydet", Type = MaterialButton.MaterialButtonType.Contained };
            btnSave.Click += BtnSave_Click;

            btnCancel = new MaterialButton { Text = "İptal", Type = MaterialButton.MaterialButtonType.Contained };
            btnCancel.Click += (s, e) => this.Close();

            layout.Controls.Add(lblDate, 0, 0);
            layout.Controls.Add(datePicker, 1, 0);
            layout.Controls.Add(lblWeight, 0, 1);
            layout.Controls.Add(numWeight, 1, 1);
            layout.Controls.Add(btnSave, 0, 2);
            layout.Controls.Add(btnCancel, 1, 2);

            this.Controls.Add(layout);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (numWeight.Value <= 0)
            {
                MessageBox.Show("Geçerli bir kilo girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = _editingLog ?? new WeightLogDto { UserId = _userId };

            dto.Date = DateTime.SpecifyKind(datePicker.Value.Date, DateTimeKind.Utc);
            dto.WeightKg = (double)numWeight.Value;

            if (_editingLog == null)
                _weightLogService.AddWeightLog(dto);
            else
                _weightLogService.UpdateWeightLog(dto);


            //_userService.UpdateCurrentWeight(_userId, dto.WeightKg);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
