using Business.Abstract;
using Entities.Dtos;
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

        private TableLayoutPanel tableLayoutPanel;
        private MaterialTextBox2 txtFullName;
        private MaterialTextBox2 txtAge;
        private ComboBox cmbGender;
        private MaterialTextBox2 txtHeight;
        private MaterialTextBox2 txtCurrentWeight;
        private MaterialTextBox2 txtTargetWeight;
        private MaterialButton btnSave;
        private MaterialButton btnCancel;

        public AddEditUserForm(IUserService userService)
        {
            _userService = userService;

            this.Text = "Yeni Kullanıcı Ekle";
            ApplyMaterialSkinTheme();
            InitializeComponents();
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
        }

        private void ApplyMaterialSkinTheme()
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Indigo500, Primary.Indigo700, Primary.Indigo200, Accent.LightBlue200, TextShade.WHITE);

            this.Size = new Size(1920, 1080);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponents()
        {
            tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(100),
                ColumnCount = 2,
                RowCount = 8
            };

            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 8; i++)
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));

            txtFullName = new MaterialTextBox2() { Hint = "İsim" };
            txtAge = new MaterialTextBox2() { Hint = "Yaş" };
            cmbGender = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Height = 40 };
            cmbGender.Items.AddRange(new object[] { "Erkek", "Kadın" });
            txtHeight = new MaterialTextBox2() { Hint = "Boy (cm)" };
            txtCurrentWeight = new MaterialTextBox2() { Hint = "Mevcut Kilo (kg)" };
            txtTargetWeight = new MaterialTextBox2() { Hint = "Hedef Kilo (kg)" };

            btnSave = new MaterialButton() { Text = "Kaydet", Type = MaterialButton.MaterialButtonType.Contained };
            btnSave.Click += btnSave_Click;

            btnCancel = new MaterialButton() { Text = "İptal", Type = MaterialButton.MaterialButtonType.Contained };
            btnCancel.Click += (s, e) => this.Close();

            tableLayoutPanel.Controls.Add(new Label() { Text = "İsim:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 0);
            tableLayoutPanel.Controls.Add(txtFullName, 1, 0);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Yaş:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 1);
            tableLayoutPanel.Controls.Add(txtAge, 1, 1);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Cinsiyet:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 2);
            tableLayoutPanel.Controls.Add(cmbGender, 1, 2);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Boy (cm):", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 3);
            tableLayoutPanel.Controls.Add(txtHeight, 1, 3);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Mevcut Kilo:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 4);
            tableLayoutPanel.Controls.Add(txtCurrentWeight, 1, 4);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Hedef Kilo:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 5);
            tableLayoutPanel.Controls.Add(txtTargetWeight, 1, 5);
            tableLayoutPanel.Controls.Add(btnSave, 0, 6);
            tableLayoutPanel.Controls.Add(btnCancel, 1, 6);

            this.Controls.Add(tableLayoutPanel);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!TryBuildDtoFromForm(out UserDto dto))
            {
                MessageBox.Show("Lütfen tüm alanları doğru doldurun.", "Validation Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_editingUser == null)
                {
                    _userService.AddUser(dto);
                }
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
            {
                return false;
            }

            dto = new UserDto
            {
                FullName = txtFullName.Text.Trim(),
                Age = age,
                Gender = cmbGender.SelectedItem.ToString(),
                HeightCm = height,
                CurrentWeightKg = currentWeight,
                TargetWeightKg = targetWeight
            };

            return true;
        }
    }
}
