using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphicsProgram
{
    public partial class SimpleChart : Form
    {
        public double VMax { get; set; }
        public double KM { get; set; }

        public double[] V { get; set; }
        public double[] S { get; set; }

        public SimpleChart(double vMax, double kM)
        {
            InitializeComponent();

            VMax = vMax;
            KM = kM;

            Width = 500;
            Height = 500;

            GenerateVANDS();

            MainChart.Series.Clear();

            var series = MainChart.Series.Add("S -- V");
            series.Points.DataBindXY(S, "S", V, "V");

            series.ChartType = SeriesChartType.Spline;
            series.Color = Color.Black;
            series.BorderWidth = 3;
        }

        public void GenerateVANDS()
        {
            List<double> v = new List<double>();
            List<double> s = new List<double>();

            for (int i = 0; i < 10; i++)
            {
                double tmpS = 1000 * (i + 1);
                double tmpV = (VMax * tmpS) / (KM + tmpS);

                s.Add(tmpS);
                v.Add(tmpV);
            }

            S = s.ToArray();
            V = v.ToArray();
        }
    }
}
