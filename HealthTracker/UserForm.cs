using Business;
using Business.Abstract;
using Business.Concrete;
using Entities;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HealthTracker
{
    public class UserForm : MaterialForm
    {
        private readonly IUserService _userService;
        private readonly IMealService _mealService;
        private readonly IWeightLogService _weightLogService;
        private readonly IWorkoutService _workoutService;

        private FlowLayoutPanel panelButtons;
        private MaterialButton btnAddUser;
        private MaterialButton btnProgressChart;
        private MaterialButton btnEditUser;
        private MaterialButton btnDeleteUser;
        private MaterialButton btnRefresh;
        private MaterialButton btnWeightLogs;
        private MaterialListView dgvUsers;
        private MaterialButton btnMeals;

        public UserForm(IUserService userService,IMealService mealService,IWeightLogService weightLogService,IWorkoutService workoutService)
        {
            _userService = userService;
            _mealService = mealService;
            _weightLogService = weightLogService;
            _workoutService = workoutService;

            ApplyMaterialSkinTheme();
            InitializeComponents();
            LoadUsers();
        }

        private void ApplyMaterialSkinTheme()
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo200, Accent.LightBlue200, TextShade.WHITE);

            this.Text = "Kullanıcı Yönetimi";
            this.Size = new Size(1920, 1080);
            this.StartPosition = FormStartPosition.CenterScreen;
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

            btnAddUser = CreateButton("Ekle", btnAddUser_Click);
            btnEditUser = CreateButton("Düzenle", btnEditUser_Click);
            btnDeleteUser = CreateButton("Sil", btnDeleteUser_Click);
            btnRefresh = CreateButton("Yenile", btnRefresh_Click);
            btnWeightLogs = CreateButton("Kilo Geçmişi", btnWeightLogs_Click);

            btnProgressChart = CreateButton("Kilo Değişimi", btnProgressChart_Click);
            panelButtons.Controls.Add(btnProgressChart);

            btnMeals = CreateButton("Öğünler", btnMeals_Click);

            MaterialButton btnWorkouts = CreateButton("Antrenmanlar", btnWorkouts_Click);
            panelButtons.Controls.Add(btnWorkouts);


            panelButtons.Controls.Add(btnMeals);

            panelButtons.Controls.Add(btnAddUser);
            panelButtons.Controls.Add(btnEditUser);
            panelButtons.Controls.Add(btnDeleteUser);
            panelButtons.Controls.Add(btnRefresh);
            panelButtons.Controls.Add(btnWeightLogs);

            dgvUsers = new MaterialListView
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                BorderStyle = BorderStyle.None,
                OwnerDraw = true,
                View = View.Details,
                MouseState = MaterialSkin.MouseState.OUT
            };

            dgvUsers.Columns.Add("ID", 80);
            dgvUsers.Columns.Add("Ad Soyad", 300);
            dgvUsers.Columns.Add("Yaş", 80);
            dgvUsers.Columns.Add("Cinsiyet", 100);
            dgvUsers.Columns.Add("Şu anki Kilo", 150);
            dgvUsers.Columns.Add("Hedef Kilo", 150);

            this.Controls.Add(dgvUsers);
            this.Controls.Add(panelButtons);

            this.Resize += (s, e) => AutoResizeListViewColumns();
        }

        private MaterialButton CreateButton(string text, EventHandler clickHandler)
        {
            var button = new MaterialButton
            {
                Text = text,
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                AutoSize = true
            };
            button.Click += clickHandler;
            return button;
        }

        private void LoadUsers()
        {
            var users = _userService.GetAllUsers();
            dgvUsers.Items.Clear();

            foreach (var user in users)
            {
                var item = new ListViewItem(user.Id.ToString());
                item.SubItems.Add(user.FullName);
                item.SubItems.Add(user.Age.ToString());
                item.SubItems.Add(user.Gender);
                item.SubItems.Add(user.CurrentWeightKg.ToString());
                item.SubItems.Add(user.TargetWeightKg.ToString());
                dgvUsers.Items.Add(item);
            }

            AutoResizeListViewColumns();
        }

        private void AutoResizeListViewColumns()
        {
            if (dgvUsers.Columns.Count == 0) return;

            int totalWidth = dgvUsers.ClientSize.Width;

            dgvUsers.Columns[0].Width = (int)(totalWidth * 0.05);
            dgvUsers.Columns[1].Width = (int)(totalWidth * 0.30);
            dgvUsers.Columns[2].Width = (int)(totalWidth * 0.10);
            dgvUsers.Columns[3].Width = (int)(totalWidth * 0.10);
            dgvUsers.Columns[4].Width = (int)(totalWidth * 0.20);
            dgvUsers.Columns[5].Width = (int)(totalWidth * 0.20);
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            var form = new AddEditUserForm(_userService); 
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadUsers();
            }
        }
        private void btnProgressChart_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen kullanıcı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = int.Parse(dgvUsers.SelectedItems[0].SubItems[0].Text);
            var user = _userService.GetUserById(userId);
            var form = new UserProgressForm(user, _weightLogService);
            form.ShowDialog();
        }
        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlenecek kullanıcıyı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = dgvUsers.SelectedItems[0];
            int userId = int.Parse(selectedItem.SubItems[0].Text);
            var selectedUser = _userService.GetUserById(userId);

            var form = new AddEditUserForm(_userService, selectedUser);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadUsers();
            }
        }
        private void btnWorkouts_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen kullanıcı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = dgvUsers.SelectedItems[0];
            int userId = int.Parse(selectedItem.SubItems[0].Text);
            var selectedUser = _userService.GetUserById(userId);

            var form = new WorkoutForm(selectedUser, _workoutService);
            form.ShowDialog();
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek kullanıcıyı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = dgvUsers.SelectedItems[0];
            int userId = int.Parse(selectedItem.SubItems[0].Text);

            var confirm = MessageBox.Show("Kullanıcıyı silmek istediğine emin misin?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                _userService.DeleteUser(userId);
                LoadUsers();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }
        private void btnMeals_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen kullanıcı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = dgvUsers.SelectedItems[0];
            int userId = int.Parse(selectedItem.SubItems[0].Text);
            var selectedUser = _userService.GetUserById(userId);

            var form = new MealForm(selectedUser, _mealService);
            form.ShowDialog();
        }

        private void btnWeightLogs_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen kullanıcı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = dgvUsers.SelectedItems[0];
            int userId = int.Parse(selectedItem.SubItems[0].Text);
            var selectedUser = _userService.GetUserById(userId);

            var form = new WeightLogForm(selectedUser, _weightLogService, _userService);
            form.ShowDialog();
        }
    }

}
