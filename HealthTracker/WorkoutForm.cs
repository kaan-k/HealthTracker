using Business.Abstract;
using Entities.Dtos;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HealthTracker
{
    public class WorkoutForm : MaterialForm
    {
        private readonly UserDto _user;
        private readonly IWorkoutService _workoutService;

        private MaterialListView listWorkouts;
        private FlowLayoutPanel panelButtons;
        private MaterialButton btnAdd, btnEdit, btnDelete, btnRefresh;

        public WorkoutForm(UserDto user, IWorkoutService workoutService)
        {
            _user = user;
            _workoutService = workoutService;

            ApplyMaterialTheme();
            InitializeComponents();
            LoadWorkouts();
        }

        private void ApplyMaterialTheme()
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(Primary.Green500, Primary.Green700, Primary.Green200, Accent.LightGreen200, TextShade.WHITE);

            Text = $"{_user.FullName} - Antrenmanlar";
            Size = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterParent;
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

            btnAdd = CreateButton("Ekle", BtnAdd_Click);
            btnEdit = CreateButton("Düzenle", BtnEdit_Click);
            btnDelete = CreateButton("Sil", BtnDelete_Click);
            btnRefresh = CreateButton("Yenile", (s, e) => LoadWorkouts());

            panelButtons.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, btnRefresh });

            listWorkouts = new MaterialListView
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                BorderStyle = BorderStyle.None,
                OwnerDraw = true,
                View = View.Details,
                MouseState = MaterialSkin.MouseState.OUT
            };

            listWorkouts.Columns.Add("ID", 80);
            listWorkouts.Columns.Add("Tarih", 150);
            listWorkouts.Columns.Add("Plan", 700);

            Controls.Add(listWorkouts);
            Controls.Add(panelButtons);
        }

        private MaterialButton CreateButton(string text, EventHandler onClick)
        {
            return new MaterialButton
            {
                Text = text,
                Type = MaterialButton.MaterialButtonType.Contained,
                AutoSize = true
            }.Also(b => b.Click += onClick);
        }

        private void LoadWorkouts()
        {
            listWorkouts.Items.Clear();
            var workouts = _workoutService.GetWorkoutsByUserId(_user.Id.Value);

            foreach (var workout in workouts)
            {
                var item = new ListViewItem(workout.Id.ToString());
                item.SubItems.Add(workout.Date.ToString("yyyy-MM-dd"));
                item.SubItems.Add(workout.Plan);
                listWorkouts.Items.Add(item);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new AddEditWorkoutForm(_user, _workoutService);
            if (form.ShowDialog() == DialogResult.OK)
                LoadWorkouts();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (listWorkouts.SelectedItems.Count == 0) return;

            int id = int.Parse(listWorkouts.SelectedItems[0].Text);
            var workout = _workoutService.GetById(id);
            var form = new AddEditWorkoutForm(_user, _workoutService, workout);
            if (form.ShowDialog() == DialogResult.OK)
                LoadWorkouts();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (listWorkouts.SelectedItems.Count == 0) return;

            int id = int.Parse(listWorkouts.SelectedItems[0].Text);
            if (MessageBox.Show("Silinsin mi?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _workoutService.Delete(id);
                LoadWorkouts();
            }
        }
    }
}
