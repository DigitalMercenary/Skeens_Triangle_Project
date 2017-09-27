using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Triangles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Point> _points = new List<Point>();
        private List<Polygon> _triangles = new List<Polygon>();
        private List<string> _rows = new List<string>();

        private List<string> Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }

        private List<Polygon> Triangles
        {
            get { return _triangles; }
            set { _triangles = value; }
        }

        private List<Point> Points
        {
            get { return _points; }
            set { _points = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            SetRows();
            DrawGrid();
            DrawUpTriangles();
            DrawDownTriangles();

            //GetTriangle(new Point(30, 30), new Point(30, 40), new Point(40, 40));
        }

        private void SetRows()
        {
            Rows.Add("A");
            Rows.Add("B");
            Rows.Add("C");
            Rows.Add("D");
            Rows.Add("E");
            Rows.Add("F");
        }

        private void DrawGrid()
        {
            double xCoord;
            double yCoord;

            for (int i = 0; i < 7; i++)
            {
                yCoord = i * 10;
                for (int j = 0; j < 7; j++)
                {
                    xCoord = j * 10;
                    Point p = new Point(xCoord, yCoord);
                    Points.Add(p);
                }
            }
        }

        private void DrawUpTriangles()
        {
            int r = 1;
            int rn = 1;

            for (int i = 0; i < 41; i++)
            {
                if ((r * 7) - i == 1)
                {
                    r++;
                    rn = 1;
                }
                else
                {
                    Polygon p = new Polygon();
                    p.Stroke = new SolidColorBrush(Colors.White);
                    p.StrokeThickness = 1;
                    p.Fill = new SolidColorBrush(Colors.Aquamarine);

                    p.Points.Add(Points[i]);
                    p.Points.Add(Points[i + 7]);
                    p.Points.Add(Points[i + 8]);

                    p.Name = Rows[r - 1] + rn.ToString();
                    p.ToolTip = p.Name;

                    p.MouseDown += polygon_Click;

                    Triangles.Add(p);
                    stage.Children.Add(p);
                    rn += 2;
                }
            }
        }

        private void DrawDownTriangles()
        {
            int r = 1;
            int rn = 2;

            for (int i = 0; i < 41; i++)
            {
                if ((r * 7) - i == 1)
                {
                    r++;
                    rn = 2;
                }
                else
                {
                    Polygon p = new Polygon();
                    p.Stroke = new SolidColorBrush(Colors.White);
                    p.StrokeThickness = 1;
                    p.Fill = new SolidColorBrush(Colors.Aquamarine);

                    p.Points.Add(Points[i]);
                    p.Points.Add(Points[i + 1]);
                    p.Points.Add(Points[i + 8]);

                    p.Name = Rows[r - 1] + rn.ToString();
                    p.ToolTip = p.Name;

                    p.MouseDown += polygon_Click;

                    Triangles.Add(p);
                    stage.Children.Add(p);
                    rn += 2;
                }
            }
        }

        private void GetTriangle(Point p1, Point p2, Point p3, bool showPoints = false)
        {
            List<Polygon> tempTriangles = Triangles;
            List<Point> ps = new List<Point>();
            ps.Add(p1);
            ps.Add(p2);
            ps.Add(p3);

            bool found = false;
            foreach (Polygon p in tempTriangles)
            {
                p.Fill = new SolidColorBrush(Colors.Aquamarine);
            }

            foreach (Polygon pg in tempTriangles)
            {
                int vertexMatch = 0;
                foreach (Point pt in pg.Points)
                {
                    if ((p1.X == pt.X && p1.Y == pt.Y) || (p2.X == pt.X && p2.Y == pt.Y) || (p3.X == pt.X && p3.Y == pt.Y))
                    {
                        vertexMatch++;
                        if (vertexMatch == 3)
                        {
                            found = true;
                            pg.Fill = new SolidColorBrush(Colors.IndianRed);
                            if (showPoints == false)
                            {
                                System.Windows.MessageBox.Show("Triangle coordinates are : " + pg.Name);
                            }
                            else
                            {
                                string c1 = "(X1:" + pg.Points[0].X.ToString() + ", " + "Y1:" + pg.Points[0].Y.ToString() + ")";
                                string c2 = "(X2:" + pg.Points[1].X.ToString() + ", " + "Y2:" + pg.Points[1].Y.ToString() + ")";
                                string c3 = "(X3:" + pg.Points[2].X.ToString() + ", " + "Y3:" + pg.Points[2].Y.ToString() + ")";

                                System.Windows.MessageBox.Show("Triangle coordinates are : " + pg.Name + ". Vertices are : " + c1 + "," + c2 + "," + c3);
                            }
                            break;
                        }
                    }
                }
            }
            if (found == false)
            {
                System.Windows.MessageBox.Show("Triangle not found. Please use valid coordinates that match a triangle in the grid.");
                ClearAll();
            }
        }

        private void find_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double x1v = double.Parse(x1Value.Text);
                double y1v = double.Parse(y1Value.Text);
                double x2v = double.Parse(x2Value.Text);
                double y2v = double.Parse(y2Value.Text);
                double x3v = double.Parse(x3Value.Text);
                double y3v = double.Parse(y3Value.Text);

                Point p1 = new Point(x1v, y1v);
                Point p2 = new Point(x2v, y2v);
                Point p3 = new Point(x3v, y3v);

                GetTriangle(p1, p2, p3);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Point conversion error. Please try numbers only, from 0 - 60.");
                ClearAll();
            }
        }

        private void input_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = sender as TextBox;
            try
            {
                int i = int.Parse(t.Text);
                if (i < 0)
                {
                    System.Windows.MessageBox.Show("There are no negative numbers in the grid.");
                    t.Clear();
                }
                if (i > 60)
                {
                    System.Windows.MessageBox.Show("The grid is 60x60. Numbers greater than 60 are invalid.");
                    t.Clear();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Invalid input. Only numbers from 0 - 60 are allowed.");
                t.Clear();
            }

        }

        private void polygon_Click(object sender, RoutedEventArgs e)
        {
            Polygon p = sender as Polygon;
            GetTriangle(p.Points[0], p.Points[1], p.Points[2], true);
        }

        private void ClearAll()
        {
            x1Value.Clear();
            y1Value.Clear();
            x2Value.Clear();
            y2Value.Clear();
            x3Value.Clear();
            y3Value.Clear();

            foreach (Polygon p in Triangles)
            {
                p.Fill = new SolidColorBrush(Colors.Aquamarine);
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }
    }
}
