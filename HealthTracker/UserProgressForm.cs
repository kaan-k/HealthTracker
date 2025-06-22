using Business.Abstract;
using Entities.Dtos;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CartesianChart = LiveCharts.WinForms.CartesianChart;

namespace HealthTracker
{
    public class UserProgressForm : Form
    {
        private readonly UserDto _user;
        private readonly IWeightLogService _weightLogService;

        private CartesianChart weightChart;

        public UserProgressForm(UserDto user, IWeightLogService weightLogService)
        {
            _user = user;
            _weightLogService = weightLogService;

            this.Text = $"{_user.FullName} - Kilo Değişimi";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            InitializeChart();
            LoadWeightData();
        }

        private void InitializeChart()
        {
            weightChart = new CartesianChart
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.White
            };

            this.Controls.Add(weightChart);
        }

        private void LoadWeightData()
        {
            var logs = _weightLogService.GetLogsByUserId(_user.Id.GetValueOrDefault())
                                         .OrderBy(log => log.Date)
                                         .ToList();

            if (logs.Count == 0)
            {
                MessageBox.Show("Kullanıcıya ait kilo kaydı bulunamadı.");
                return;
            }

            var values = new ChartValues<double>();
            var labels = new List<string>();

            foreach (var log in logs)
            {
                values.Add(log.WeightKg);
                labels.Add(log.Date.ToString("dd MMM"));
            }

            var series = new LineSeries
            {
                Title = "Kilo (kg)",
                Values = values,
                StrokeThickness = 3,
                LineSmoothness = 0.3,
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 10,
                Fill = System.Windows.Media.Brushes.LightBlue,
                Stroke = System.Windows.Media.Brushes.DarkBlue,
                DataLabels = true
            };

            weightChart.Series = new SeriesCollection { series };

            weightChart.AxisX.Clear();
            weightChart.AxisX.Add(new Axis
            {
                Title = "Tarih",
                Labels = labels,
                LabelsRotation = 15,
                Separator = new Separator { Step = 1 },
                FontSize = 14,
            });

            weightChart.AxisY.Clear();
            weightChart.AxisY.Add(new Axis
            {
                Title = "Kilo",
                LabelFormatter = val => val + " kg",
                FontSize = 14,
            });
        }
    }
}
