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
    public class AddEditWaterLogForm : MaterialForm
    {
        private readonly IWaterLogService _waterLogService;
        private readonly int _userId;
        private readonly WaterLogDto _editingLog;

        private TableLayoutPanel layout;
        private DateTimePicker datePicker;
        private NumericUpDown numAmount;
        private MaterialButton btnSave;
        private MaterialButton btnCancel;

        public AddEditWaterLogForm(int userId, IWaterLogService waterLogService)
        {
            _userId = userId;
            _waterLogService = waterLogService;

            this.Text = "Yeni Su Girişi";
            InitializeForm();
            InitializeComponents();
        }

        public AddEditWaterLogForm(WaterLogDto logDto, IWaterLogService waterLogService)
            : this(logDto.UserId, waterLogService)
        {
            _editingLog = logDto;
            this.Text = "Su Girişini Düzenle";

            datePicker.Value = logDto.Date;
            numAmount.Value = logDto.AmountMl;
        }

        private void InitializeForm()
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Blue500, Primary.Blue700, Primary.Blue200, Accent.LightBlue200, TextShade.WHITE);

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

            var lblAmount = new Label { Text = "Miktar (ml):", Anchor = AnchorStyles.Right, AutoSize = true };
            numAmount = new NumericUpDown
            {
                Width = 200,
                Minimum = 100,
                Maximum = 10000,
                Increment = 100,
                Value = 250
            };

            btnSave = new MaterialButton { Text = "Kaydet", Type = MaterialButton.MaterialButtonType.Contained };
            btnSave.Click += BtnSave_Click;

            btnCancel = new MaterialButton { Text = "İptal", Type = MaterialButton.MaterialButtonType.Contained };
            btnCancel.Click += (s, e) => this.Close();

            layout.Controls.Add(lblDate, 0, 0);
            layout.Controls.Add(datePicker, 1, 0);
            layout.Controls.Add(lblAmount, 0, 1);
            layout.Controls.Add(numAmount, 1, 1);
            layout.Controls.Add(btnSave, 0, 2);
            layout.Controls.Add(btnCancel, 1, 2);

            this.Controls.Add(layout);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (numAmount.Value <= 0)
            {
                MessageBox.Show("Geçerli bir miktar girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = _editingLog ?? new WaterLogDto { UserId = _userId };

            dto.Date = DateTime.SpecifyKind(datePicker.Value.Date, DateTimeKind.Utc);
            dto.AmountMl = (int)numAmount.Value;

            if (_editingLog == null)
                _waterLogService.Add(dto);
            else
                _waterLogService.Update(dto);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
