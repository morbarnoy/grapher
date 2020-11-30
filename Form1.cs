using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        int GraphWidth = 30, GraphHeight = 25;
        public Graphics line3d;
        public Form1()
        {
            InitializeComponent();
            line3d = panel1.CreateGraphics();
            GraphWidth = panel1.Width / 50;
            GraphHeight = panel1.Height / 50;
           
        }
        private void Draw3dFunc(BinNode<string> tr)
        {
            double alpha = 0.7;
            int res = 200;       
            float[,] var = new float[res, res];
            int[,] XT = new int[res, res];
            int[,] YT = new int[res, res];
            float zMax = -10000;
            float zMin = 10000;
            double dx = (double)(decimal.Divide(GraphWidth, res));

            for (int i = 0; i < var.GetLength(0); i++)
            {
                for (int j = 0; j < var.GetLength(1); j++)
                {
                    var[i, j] = (float)(Calculate(tr, -GraphWidth / 2 + i * dx, -GraphHeight / 2 + j * dx));
                    if (var[i, j] > zMax)
                        zMax = var[i, j];
                    if (var[i, j] < zMin)
                        zMin = var[i, j];
                    float GraphY = (float)(var[i, j] + (-GraphHeight / 2 + j * dx) * Math.Sin(alpha));
                    float GraphX = (float)(-GraphWidth / 2 + i * dx + (-GraphHeight / 2 + j * dx) * Math.Cos(alpha));
                    XT[i, j] = (int)(panel1.Width / GraphWidth * GraphX + panel1.Width / 2);
                    YT[i, j] = (int)(-panel1.Width / GraphWidth * GraphY + panel1.Width / 2);
                }
            }
            for (int i = 0; i < var.GetLength(0) - 2; i++)
            {
                for (int j = 0; j < var.GetLength(1) - 2; j++)
                {

                    if (XT[i, j] < 1E4 && YT[i, j] < 1E4 && XT[i, j] > -1E4 && YT[i, j] > -1E4)
                        if (XT[i + 1, j] < 1E4 && YT[i + 1, j] < 1E4 && XT[i + 1, j] > -1E4 && YT[i + 1, j] > -1E4)
                            if (XT[i, j + 1] < 1E4 && YT[i, j + 1] < 1E4 && XT[i, j + 1] > -1E4 && YT[i, j + 1] > -1E4)
                            {
                                Point[] P = new Point[4];
                                P[0] = new Point(XT[i, j], YT[i, j]);
                                P[1] = new Point(XT[i + 2, j], YT[i + 2, j]);
                                P[2] = new Point(XT[i, j + 2], YT[i, j + 2]);
                                P[3] = new Point(XT[i + 2, j + 2], YT[i + 2, j + 2]);

                                double PrecentOfHeight = ((var[i, j] - zMin) / (zMax - zMin)) * 100;
                                PrecentOfHeight = Math.Floor(PrecentOfHeight);

                                Color c = PickColor(int.Parse(PrecentOfHeight.ToString()));

                                line3d.FillPolygon(new SolidBrush(c), P);
                            }
                }
            }
        }
        private Color PickColor1(int Precent)
        {
            MessageBox.Show("Test");
            Color Start = Color.Red;
            Color Middle = Color.Yellow;
            Color End = Color.FromArgb(0, 0, 204);

            if (Precent < 50)
            {
                double Factor = Precent * 2 / 100.0;
                double red = Start.R - (Start.R - Middle.R) * Factor;
                double green = Start.G - (Start.G - Middle.G) * Factor;
                double blue = Start.B - (Start.B - Middle.B) * Factor;

                return Color.FromArgb(int.Parse(Math.Round(red).ToString()), int.Parse(Math.Round(green).ToString()), int.Parse(Math.Round(blue).ToString()));
            }
            else if (Precent > 50)
            {
                double Factor = (Precent - 50) * 2 / 100.0;
                double red = Middle.R - (Middle.R - End.R) * Factor;
                double green = Middle.G - (Middle.G - End.G) * Factor;
                double blue = Middle.B - (Middle.B - End.B) * Factor;

                return Color.FromArgb(int.Parse(Math.Round(red).ToString()), int.Parse(Math.Round(green).ToString()), int.Parse(Math.Round(blue).ToString()));
            }

            return Middle;
        }
        private Color PickColor(int Precent)
        {
            Color Start = Color.Orange;
            Color Middle = Color.Yellow;
            Color End = Color.FromArgb(0, 0, 0);

            if (Precent < 50)
            {
                double Factor = Precent * 2 / 100.0;
                double red = Start.R - (Start.R - Middle.R) * Factor;
                double green = Start.G - (Start.G - Middle.G) * Factor;

                double blue = Start.B - (Start.B - Middle.B) * Factor;

                return Color.FromArgb(100, 100, int.Parse(Math.Round(blue).ToString()));
            }
            else if (Precent > 50)
            {
                double Factor = (Precent - 50) * 2 / 100.0;
                double red = Middle.R - (Middle.R - End.R) * Factor;
                double green = Middle.G - (Middle.G - End.G) * Factor;

                double blue = Middle.B - (Middle.B - End.B) * Factor;

                return Color.FromArgb(100,100, int.Parse(Math.Round(blue).ToString()));
            }

            return Middle;
        }

        #region calculator       
        static BinNode<string> BuildFormulaTree(string Formula)// final 
        {
            int check = 0;
            // backwards pemdas 

            check = 0;
            {
                for (int i = Formula.Length - 1; i > 0; i--)
                {
                    if (Formula[i] == ')')
                        check++;
                    if (Formula[i] == '(')
                        check--;
                    if ((Formula[i] == '-' || Formula[i] == '+') && check == 0)
                    {
                        string left = Formula.Substring(0, i);
                        string right = Formula.Substring(i + 1);
                        Formula = Formula.Substring(i, 1);
                        return new BinNode<string>(BuildFormulaTree(left), Formula, BuildFormulaTree(right));//left value right
                    }
                }
                for (int i = Formula.Length - 1; i >= 0; i--)
                {
                    if (Formula[i] == ')')
                        check++;
                    if (Formula[i] == '(')
                        check--;
                    if ((Formula[i] == '*' || Formula[i] == '/') && check == 0)
                    {
                        string left = Formula.Substring(0, i);
                        string right = Formula.Substring(i + 1);
                        Formula = Formula.Substring(i, 1);
                        return new BinNode<string>(BuildFormulaTree(left), Formula, BuildFormulaTree(right));
                    }
                }
            }// 4 main 
            check = 0;
            for (int i = Formula.Length - 1; i >= 0; i--)
            {
                if (Formula[i] == ')')
                    check++;
                if (Formula[i] == '(')
                    check--;
                if ((Formula[i] == '^') && check == 0)
                    return new BinNode<string>(BuildFormulaTree(Formula.Substring(0, i)), Formula.Substring(i, 1), BuildFormulaTree(Formula.Substring(i + 1)));
                if (i < Formula.Length - 4 && (Formula.Substring(i, 4) == "root") && check == 0)//root is a 4 letter word - before is base after is what you are doing the root to 
                    return new BinNode<string>(BuildFormulaTree(Formula.Substring(0, i)), Formula.Substring(i, 4), BuildFormulaTree(Formula.Substring(i + 4)));
                if (i < Formula.Length - 3 && Formula.Substring(i, 3) == "log" && check == 0)// before is the base and after is the number to do on 
                    return new BinNode<string>(BuildFormulaTree(Formula.Substring(0, i)), Formula.Substring(i, 3), BuildFormulaTree(Formula.Substring(i + 3)));//
            }
            if (Formula.Length > 5 && (Formula.Substring(0, 3) == "cos" || Formula.Substring(0, 3) == "sin" || Formula.Substring(0, 3) == "tan" || Formula.Substring(0, 3) == "abs"))//cos(x)>5
                return new BinNode<string>(null, Formula.Substring(0, 3), BuildFormulaTree(Formula.Substring(4, Formula.Length - 5)));// nothing to left - value is cos/tan/sin right is the x 

            if (Formula[0] == '(')
            {
                Formula = Formula.Substring(1, Formula.Length - 2);
                return BuildFormulaTree(Formula);
            }
            return new BinNode<string>(Formula);
        }
        static double Calculate(BinNode<string> Tree, double x, double y)
        {
            if (Tree.GetValue() == "+")
                return Calculate(Tree.GetLeft(), x, y) + Calculate(Tree.GetRight(), x, y);
            if (Tree.GetValue() == "-")
                return Calculate(Tree.GetLeft(), x, y) - Calculate(Tree.GetRight(), x, y);

            if (Tree.GetValue() == "*")
                return Calculate(Tree.GetLeft(), x, y) * Calculate(Tree.GetRight(), x, y);
            if (Tree.GetValue() == "/")
                return Calculate(Tree.GetLeft(), x, y) / Calculate(Tree.GetRight(), x, y);

            if (Tree.GetValue() == "^")
                return Math.Pow(Calculate(Tree.GetLeft(), x, y), Calculate(Tree.GetRight(), x, y));
            if (Tree.GetValue() == "cos")
                return Math.Cos(Calculate(Tree.GetRight(), x, y));

            if (Tree.GetValue() == "sin")
                return Math.Sin(Calculate(Tree.GetRight(), x, y));
            if (Tree.GetValue() == "tan")
                return Math.Tan(Calculate(Tree.GetRight(), x, y));
            if (Tree.GetValue() == "root")
                return Math.Pow(Calculate(Tree.GetRight(), x, y), 1 / Calculate(Tree.GetLeft(), x, y));
            if (Tree.GetValue() == "log")
                return Math.Log(Calculate(Tree.GetRight(), x, y), Calculate(Tree.GetLeft(), x, y));
            if (Tree.GetValue() == "abs")
                return Math.Abs(Calculate(Tree.GetRight(), x, y));
            if (Tree.GetValue() == "pi" || Tree.GetValue() == "PI")
                return Math.PI;
            if (Tree.GetValue() == "e" || Tree.GetValue() == "E")
                return Math.E;
            if (Tree.GetValue() == "x" || Tree.GetValue() == "X")
                return x;
            if (Tree.GetValue() == "y" || Tree.GetValue() == "Y")
                return y;
            return double.Parse(Tree.GetValue().ToString());
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            
               
                panel1.BorderStyle = BorderStyle.FixedSingle;
                Graphics y = panel1.CreateGraphics();
                y.DrawLine(Pens.DarkCyan, panel1.Width / 2, 0, panel1.Width / 2, panel1.Height);
                Graphics x = panel1.CreateGraphics();
                x.DrawLine(Pens.DarkCyan, 0, panel1.Height / 2, panel1.Width, panel1.Height / 2);
                int m = 50;

                {
                    for (int i = panel1.Height / 2; i < panel1.Height; i = i + m)
                    {
                        Graphics d2 = panel1.CreateGraphics();
                        d2.DrawLine(Pens.Red, panel1.Width / 2 - 2, i, panel1.Width / 2 + 2, i);

                    }
                    for (int i = panel1.Height / 2; i > 0; i = i - m)
                    {
                        Graphics d2 = panel1.CreateGraphics();
                        d2.DrawLine(Pens.Red, panel1.Width / 2 - 2, i, panel1.Width / 2 + 2, i);

                    }
                    for (int i = panel1.Width / 2; i < panel1.Width; i = i + m)
                    {
                        Graphics d2 = panel1.CreateGraphics();
                        d2.DrawLine(Pens.Red, i, panel1.Height / 2 - 2, i, panel1.Height / 2 + 2);

                    }
                    for (int i = panel1.Width / 2; i > 0; i = i - m)
                    {
                        Graphics d2 = panel1.CreateGraphics();
                        d2.DrawLine(Pens.Red, i, panel1.Height / 2 - 2, i, panel1.Height / 2 + 2);

                    }
                }           
            Draw3dFunc(BuildFormulaTree(textBox1.Text));
            {
              
                //double[,] var = new double[100, 100];
                //double[,] XT = new double[100, 100];//x'
                //double[,] YT = new double[100, 100];//y'
                //int[,] XT= new int [100, 100];
                //int[,] YT= new int[100, 100];
                //double acc = 0.4;
                //double alpha = 0.3;
                //int zoomx = 6;
                //int zoomy = 6;

                //for (int x = 0; x < var.GetLength(0); x++)// fill up the 
                //{
                //    for (int y = 0; y < var.GetLength(1); y++)
                //    {                                                                                                                             
                //        var[x, y] = Calculate(BuildFormulaTree(textBox1.Text), (x - var.GetLength(0) / 2) * acc, (y - var.GetLength(1) / 2) * acc);                                                                                                // var[x, y] =5*(x * acc) +2;// our formula 
                //                                                                                                            /// we changed y with the z we are looking at the (x,z) axis 
                //        XT[x, y] = zoomx * (x - var.GetLength(0) / 2 + (y - var.GetLength(1) / 2) * Math.Cos(alpha));
                //        YT[x, y] = zoomy * (var[x, y] + (y - var.GetLength(1) / 2) * Math.Sin(alpha));
                //        XT[x, y] = (int)(zoomx * XT[x, y]) + panel1.Width / 2;
                //        YT[x, y] = -(int)(zoomy * YT[x, y]) + panel1.Width / 2;



                //    }
                //}

                //for (int y = YT.GetLength(0) - 2; y > -1; y--)//from the far away hills closer 
                //{
                //    for (int x = 0; x < XT.GetLength(1) - 1; x++)
                //    {
                //        line3d.DrawLine(Pens.Purple, (float)XT[x, y] + panel1.Width / 2, -(float)YT[x, y] + panel1.Height / 2, (float)XT[x + 1, y] + panel1.Width / 2, -(float)YT[x + 1, y] + panel1.Height / 2);
                //        line3d.DrawLine(Pens.Purple, (float)XT[x, y] + panel1.Width / 2, -(float)YT[x, y] + panel1.Height / 2, (float)XT[x, y + 1] + panel1.Width / 2, -(float)YT[x, y + 1] + panel1.Height / 2);
                //    }
                //}
                //double d = panel1.Width;
                //double c1 = 255 / d;
                //float zMax = -1000;
                //float zMin = 1000;



                //for (int i = 0; i < var.GetLength(0) - 2; i++)
                //{
                //    for (int j = 0; j < var.GetLength(1) - 2; j++)
                //    {
                //        if (XT[i, j] < 1E4 && YT[i, j] < 1E4 && XT[i, j] > -1E4 && YT[i, j] > -1E4)
                //            if (XT[i + 1, j] < 1E4 && YT[i + 1, j] < 1E4 && XT[i + 1, j] > -1E4 && YT[i + 1, j] > -1E4)
                //                if (XT[i, j + 1] < 1E4 && YT[i, j + 1] < 1E4 && XT[i, j + 1] > -1E4 && YT[i, j + 1] > -1E4)
                //                {
                //                    Point[] P = new Point[4];
                //                    P[0] = new Point(XT[i, j], YT[i, j]);
                //                    P[1] = new Point(XT[i + 2, j], YT[i + 2, j]);
                //                    P[2] = new Point(XT[i, j + 2], YT[i, j + 2]);
                //                    P[3] = new Point(XT[i + 2, j + 2], YT[i + 2, j + 2]);

                //                    double PrecentOfHeight = ((var[i, j] - zMin) / (zMax - zMin)) * 100;
                //                    PrecentOfHeight = Math.Floor(PrecentOfHeight);

                //                    Color c = PickColor(int.Parse(PrecentOfHeight.ToString()));

                //                    line3d.FillPolygon(new SolidBrush(c), P);
                //                }
                //    }
                //}
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
