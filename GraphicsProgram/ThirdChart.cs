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
    public partial class ThirdChart : Form
    {
        public double VMax { get; set; }
        public double KM { get; set; }

        public double[] V { get; set; }
        public double[] S { get; set; }

        public ThirdChart(double vMax, double kM)
        {
            InitializeComponent();

            VMax = vMax;
            KM = kM;

            Width = 500;
            Height = 500;

            GenerateVANDS();

            MainChart.Series.Clear();

            var series = MainChart.Series.Add("V -- V/S");
            series.Points.DataBindXY(S, "V/S", V, "V");


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
                double tmpS = 150 * i;
                double tmpV = VMax - KM * tmpS;

                s.Add(tmpS);
                v.Add(tmpV);
            }

            S = s.ToArray();
            V = v.ToArray();
        }
    }
}
