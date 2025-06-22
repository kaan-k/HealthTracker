using Business.Abstract;
using Entities.Dtos;
using Entities.Enums;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HealthTracker
{
    public class AddEditUserForm : MaterialForm
    {
        private readonly IUserService _userService;
        private readonly UserDto _editingUser;

        private TableLayoutPanel layout;
        private MaterialTextBox2 txtFullName;
        private MaterialTextBox2 txtAge;
        private ComboBox cmbGender;
        private MaterialTextBox2 txtHeight;
        private MaterialTextBox2 txtCurrentWeight;
        private MaterialTextBox2 txtTargetWeight;
        private ComboBox cmbActivityLevel;
        private ComboBox cmbHealthCondition;
        private ComboBox cmbGoal;
        private MaterialButton btnSave;
        private MaterialButton btnCancel;

        public AddEditUserForm(IUserService userService)
        {
            _userService = userService;

            this.Text = "Yeni Kullanıcı Ekle";
            ApplyTheme();
            InitializeLayout();
        }

        public AddEditUserForm(IUserService userService, UserDto userToEdit) : this(userService)
        {
            _editingUser = userToEdit;
            this.Text = "Kullanıcı Düzenle";

            txtFullName.Text = userToEdit.FullName;
            txtAge.Text = userToEdit.Age.ToString();
            cmbGender.SelectedItem = userToEdit.Gender;
            txtHeight.Text = userToEdit.HeightCm.ToString();
            txtCurrentWeight.Text = userToEdit.CurrentWeightKg.ToString();
            txtTargetWeight.Text = userToEdit.TargetWeightKg.ToString();
            cmbActivityLevel.SelectedItem = userToEdit.ActivityLevel;
            cmbHealthCondition.SelectedItem = userToEdit.HealthCondition;
            cmbGoal.SelectedItem = userToEdit.Goal;
        }

        private void ApplyTheme()
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Indigo500, Primary.Indigo700, Primary.Indigo200, Accent.LightBlue200, TextShade.WHITE);

            this.Size = new Size(800, 950);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeLayout()
        {
            layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(60),
                ColumnCount = 2,
                RowCount = 10,
                AutoSize = true
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 10; i++)
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));

            txtFullName = new MaterialTextBox2 { Hint = "İsim" };
            txtAge = new MaterialTextBox2 { Hint = "Yaş" };
            cmbGender = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbGender.Items.AddRange(new[] { "Erkek", "Kadın" });

            txtHeight = new MaterialTextBox2 { Hint = "Boy (cm)" };
            txtCurrentWeight = new MaterialTextBox2 { Hint = "Mevcut Kilo (kg)" };
            txtTargetWeight = new MaterialTextBox2 { Hint = "Hedef Kilo (kg)" };

            cmbActivityLevel = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbActivityLevel.DataSource = Enum.GetValues(typeof(ActivityLevel));

            cmbHealthCondition = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbHealthCondition.DataSource = Enum.GetValues(typeof(HealthCondition));

            cmbGoal = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbGoal.DataSource = Enum.GetValues(typeof(UserGoal));

            btnSave = new MaterialButton { Text = "Kaydet", Type = MaterialButton.MaterialButtonType.Contained };
            btnSave.Click += BtnSave_Click;

            btnCancel = new MaterialButton { Text = "İptal", Type = MaterialButton.MaterialButtonType.Contained };
            btnCancel.Click += (s, e) => this.Close();

            AddLabeledControl("İsim:", txtFullName, 0);
            AddLabeledControl("Yaş:", txtAge, 1);
            AddLabeledControl("Cinsiyet:", cmbGender, 2);
            AddLabeledControl("Boy (cm):", txtHeight, 3);
            AddLabeledControl("Mevcut Kilo:", txtCurrentWeight, 4);
            AddLabeledControl("Hedef Kilo:", txtTargetWeight, 5);
            AddLabeledControl("Aktivite Düzeyi:", cmbActivityLevel, 6);
            AddLabeledControl("Sağlık Durumu:", cmbHealthCondition, 7);
            AddLabeledControl("Hedef:", cmbGoal, 8);

            layout.Controls.Add(btnSave, 0, 9);
            layout.Controls.Add(btnCancel, 1, 9);

            this.Controls.Add(layout);
        }

        private void AddLabeledControl(string labelText, Control inputControl, int row)
        {
            var label = new Label
            {
                Text = labelText,
                Anchor = AnchorStyles.Right,
                AutoSize = true
            };

            layout.Controls.Add(label, 0, row);
            layout.Controls.Add(inputControl, 1, row);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!TryBuildDtoFromForm(out UserDto dto))
            {
                MessageBox.Show("Lütfen tüm alanları doğru doldurun.", "Validation Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_editingUser == null)
                    _userService.AddUser(dto);
                else
                {
                    dto.Id = _editingUser.Id;
                    _userService.UpdateUser(dto);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TryBuildDtoFromForm(out UserDto dto)
        {
            dto = null;

            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                !int.TryParse(txtAge.Text, out int age) ||
                cmbGender.SelectedItem == null ||
                !double.TryParse(txtHeight.Text, out double height) ||
                !double.TryParse(txtCurrentWeight.Text, out double currentWeight) ||
                !double.TryParse(txtTargetWeight.Text, out double targetWeight))
                return false;

            dto = new UserDto
            {
                FullName = txtFullName.Text.Trim(),
                Age = age,
                Gender = cmbGender.SelectedItem.ToString(),
                HeightCm = height,
                CurrentWeightKg = currentWeight,
                TargetWeightKg = targetWeight,
                BMI = height > 0 ? currentWeight / ((height / 100) * (height / 100)) : 0,
                ActivityLevel = (ActivityLevel)cmbActivityLevel.SelectedItem,
                HealthCondition = (HealthCondition)cmbHealthCondition.SelectedItem,
                Goal = (UserGoal)cmbGoal.SelectedItem
            };

            return true;
        }
    }
}
