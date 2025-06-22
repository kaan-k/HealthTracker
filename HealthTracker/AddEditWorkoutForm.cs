using Business.Abstract;
using Entities.Dtos;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HealthTracker
{
    public class AddEditWorkoutForm : MaterialForm
    {
        private readonly IWorkoutService _workoutService;
        private readonly UserDto _user;
        private readonly WorkoutDto _editingWorkout;

        private DateTimePicker datePicker;
        private MaterialMultiLineTextBox2 txtPlan;
        private MaterialButton btnSave, btnCancel;
        private TableLayoutPanel layout;

        public AddEditWorkoutForm(UserDto user, IWorkoutService workoutService)
        {
            _user = user;
            _workoutService = workoutService;
            this.Text = "Yeni Antrenman";
            InitializeForm();
            InitializeComponents();
        }

        public AddEditWorkoutForm(UserDto user, IWorkoutService workoutService, WorkoutDto workoutToEdit)
            : this(user, workoutService)
        {
            _editingWorkout = workoutToEdit;
            this.Text = "Antrenmanı Düzenle";

            datePicker.Value = workoutToEdit.Date;
            txtPlan.Text = workoutToEdit.Plan;
        }

        private void InitializeForm()
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(Primary.Green500, Primary.Green700, Primary.Green200, Accent.LightGreen200, TextShade.WHITE);

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
                RowCount = 3
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));

            var lblDate = new Label { Text = "Tarih:", Anchor = AnchorStyles.Right, AutoSize = true };
            datePicker = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 200 };

            var lblPlan = new Label { Text = "Plan:", Anchor = AnchorStyles.Right, AutoSize = true };
            txtPlan = new MaterialMultiLineTextBox2
            {
                Hint = "Antrenman planını buraya yaz...",
                Size = new Size(300, 150)
            };

            btnSave = new MaterialButton { Text = "Kaydet", Type = MaterialButton.MaterialButtonType.Contained };
            btnSave.Click += BtnSave_Click;

            btnCancel = new MaterialButton { Text = "İptal", Type = MaterialButton.MaterialButtonType.Contained };
            btnCancel.Click += (s, e) => this.Close();

            layout.Controls.Add(lblDate, 0, 0);
            layout.Controls.Add(datePicker, 1, 0);

            layout.Controls.Add(lblPlan, 0, 1);
            layout.Controls.Add(txtPlan, 1, 1);

            layout.Controls.Add(btnSave, 0, 2);
            layout.Controls.Add(btnCancel, 1, 2);

            this.Controls.Add(layout);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlan.Text))
            {
                MessageBox.Show("Lütfen plan girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_editingWorkout == null)
            {
                var newWorkout = new WorkoutDto
                {
                    UserId = _user.Id,
                    Date = DateTime.SpecifyKind(datePicker.Value.Date, DateTimeKind.Utc),
                    Plan = txtPlan.Text
                };
                _workoutService.Add(newWorkout);
            }
            else
            {
                _editingWorkout.Date = DateTime.SpecifyKind(datePicker.Value.Date, DateTimeKind.Utc);
                _editingWorkout.Plan = txtPlan.Text;
                _workoutService.Update(_editingWorkout);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
