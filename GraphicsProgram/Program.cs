using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphicsProgram
{
    class Program
    {
        public static List<Dot> Input { get; set; }
        public static int InputCount { get; set; } = 3;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            var result = ShowMenu();

            if(result == -1)
            {
                Console.Clear();
                Console.WriteLine("Stop command");
                Console.WriteLine("Bye");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            Console.WriteLine("Your input is:");
            foreach(var dot in Input)
            {
                string line = $"{dot.X} || {dot.Y}";
                Console.WriteLine(line);
            }

            Console.WriteLine();
            Console.WriteLine("Start processig? Type y/n (yes or no)");
            Console.WriteLine("");

            bool isOk = false;
            while(true)
            {
                Console.Write(":> ");
                string input = Console.ReadLine().Trim().ToLower();

                if (input == "yes" || input == "y")
                {
                    isOk = true;
                    break;
                }

                else if (input == "no" || input == "n")
                    break;
            }

            Console.Clear();

            if (isOk)
            {
                Console.WriteLine("Start processing");
                var response = Processing(Dot.ToArray(Input));

                string r = $"VMax = {response.X} and Km = {response.Y}";
                Console.WriteLine(r);
                Task.Factory.StartNew(() =>  Application.Run(new SimpleChart(response.X, response.Y)));
                Task.Factory.StartNew(() =>  Application.Run(new SecondChart(response.X, response.Y)));
                Task.Factory.StartNew(() =>  Application.Run(new ThirdChart(response.X, response.Y)));
            }

            Console.WriteLine("Finished!");
            Console.WriteLine("Push any button to exit!");
            Console.ReadLine();
        }

        public static int ShowMenu()
        {
            string input = string.Empty;

            Input = new List<Dot>();
            // Pairs

            while(Input.Count < InputCount)
            {
                Console.WriteLine($"Input pair #{Input.Count + 1} / {InputCount}. (Put space between numbers. Type stop to exit)");

                Console.Write(":> ");
                input = Console.ReadLine().Trim();

                if (input.ToLower() == "stop")
                    return -1;

                var inputArray = input.Split(' ');

                if(inputArray.Length != 2)
                {
                    Console.Clear();
                    Console.WriteLine("Not valid input. Please give me just 2 numbers");
                    continue;
                }

                List<double> numbers = new List<double>();

                foreach(var s in inputArray)
                {
                    double number = double.NaN;
                    if(!double.TryParse(s, out number))
                    {
                        Console.WriteLine($"{s} is not valid number!");
                        break;
                    }

                    numbers.Add(number); ;
                }

                if (numbers.Count != 2)
                    continue;

                Input.Add(new Dot(numbers[0], numbers[1]));
            }

            return 1;
        }


        // Get V = y and S = x as input vector
        public static Dot Processing(double[,] input)
        {
            int maxI = 3;
            double n = 3;
            double sumXPow = 0d;
            double sumXY = 0d;
            double sumX = 0d;
            double sumY = 0d;
            // create 1/input 

            for(int i = 0; i < maxI; i++)
            {
                input[i, 0] = 1 / input[i, 0];
                input[i, 1] = 1 / input[i, 1];
            }

            // Sum x^2

            for(int i = 0; i < maxI; i++)
                sumXPow += Math.Pow(input[i, 0], 2);

            // Sum X

            for (int i = 0; i < maxI; i++)
                sumX += input[i, 0];

            // Sum X Y

            for (int i = 0; i < maxI; i++)
                sumXY += (input[i, 0] * input[i, 1]);

            // Sum Y

            for (int i = 0; i < maxI; i++)
                sumY += input[i, 1];


            double delta = sumXPow * n - sumX * sumX;
            double deltaA = sumXY * n - sumY * sumX;
            double deltaB = sumXPow * sumY - sumXY * sumX;

            double a = deltaA / delta;
            double b = deltaB / delta;

            double vMax = 1 / b;
            double kM = a / b;

            return new Dot(vMax, kM);
        }


       
    }

    public class Dot
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Dot()
        {

        }

        public Dot(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static double[,] ToArray(IEnumerable<Dot> list)
        {
            var result = new double[list.Count(), 2];

            for (int i = 0; i < list.Count(); i++)
            {
                result[i, 0] = list.ElementAt(i).X;
                result[i, 1] = list.ElementAt(i).Y;
            }

            return result;
        }
    }


    public class BasicChart : Form
    {
        public double VMax { get; set; }
        public double KM { get; set; }

        public double[] V { get; set; }
        public double[] S { get; set; }

        public BasicChart(double vMax, double kM)
        {
            VMax = vMax;
            KM = KM;

            Width = 500;
            Height = 500;

            GenerateVANDS();

            var chart = new Chart();
            chart.Width = 500;
            chart.Height = 500;
            chart.Size = new Size(500, 500);

            var series = chart.Series.Add("Basic");// new Series("Basic");

            for (int i = 0; i < V.Count(); i++)
                series.Points.AddXY(S[i], V[i]);

            series.ChartType = SeriesChartType.Spline;
            series.Color = Color.Black;
            series.BorderWidth = 3;

            chart.Location = new Point(0, 0);
            chart.ApplyPaletteColors();

            Controls.Add(chart);
        }

        public void GenerateVANDS()
        {
            List<double> v = new List<double>();
            List<double> s = new List<double>();

            for(int i = 0; i < 10; i++)
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
