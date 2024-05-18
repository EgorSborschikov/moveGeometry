using System;
using System.Drawing;
using System.Windows.Forms;

namespace pr23_24
{
    public partial class Form1 : Form
    {
        private Circle redCircle;
        private Rectangle blueSquare;
        private Triangle yellowTriangle;
        private string shapeType;
        private string shapeColor;
        public Form1()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.MouseMove += new MouseEventHandler(Form1MouseMove);
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            this.MouseUp += new MouseEventHandler(Form1_MouseUp);
            InitializeShapes();
        }

        private void InitializeShapes()
        {
            redCircle = new Circle(Color.Red, new Point(50, 50), 50);
            blueSquare = new Rectangle(Color.Blue, new Point(150, 50), 100, 100);
            yellowTriangle = new Triangle(Color.Yellow, new Point(300, 50), 100, 100);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            redCircle.Draw(e.Graphics);
            blueSquare.Draw(e.Graphics);
            yellowTriangle.Draw(e.Graphics);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (redCircle.Contains(e.Location))
            {
                shapeType = "Circle";
                shapeColor = "red";
            }
            else if (blueSquare.Contains(e.Location))
            {
                shapeType = "Restangle";
                shapeColor = "Blue";
            }
            else if (yellowTriangle.Contains(e.Location))
            {
                shapeType = "Triangle";
                shapeColor = "yellow";
            }
            else
            {
                shapeType = "";
                shapeColor = "";
            }
            textBox1.Text = $"Type: {shapeType}, Color: {shapeColor}";
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // Проверка на нажатие правой кнопки мыши
            {
                if (redCircle.Contains(e.Location))
                {
                    redCircle.Drag = true;
                    redCircle.DragStartLocation = e.Location;
                }
                else if (blueSquare.Contains(e.Location))
                {
                    blueSquare.Drag = true;
                    blueSquare.DragStartLocation = e.Location;
                }
                else if (yellowTriangle.Contains(e.Location))
                {
                    yellowTriangle.Drag = true;
                    yellowTriangle.DragStartLocation = e.Location;
                }
            }
        }

        private void Form1_MouseUp (object sender, MouseEventArgs e)
        {
            redCircle.Drag = false;
            blueSquare.Drag = false;
            yellowTriangle.Drag = false;
        }
        private void Form1MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (redCircle.Drag)
                {
                    redCircle.Move(e.Location);
                }
                else if (blueSquare.Drag)
                {
                    blueSquare.Move(e.Location);
                }
                else if (yellowTriangle.Drag)
                {
                    yellowTriangle.Move(e.Location);
                }
                this.Invalidate();
            }

            if (redCircle.Contains(e.Location))
            {
                shapeType = "Circle";
                shapeColor = "Red";
            }
            else if (blueSquare.Contains(e.Location))
            {
                shapeType = "Rectangle";
                shapeColor = "Blue";
            }
            else if (yellowTriangle.Contains(e.Location))
            {
                shapeType = "Triangle";
                shapeColor = "Yellow";
            }
            else
            {
                shapeType = "";
                shapeColor = "";
            }
            textBox1.Text = $"Type: {shapeType}, Color: {shapeColor}";
        }
        
    }

    public abstract class Shape
    {
        public Color ShapeColor { get; set; }
        public Point Location { get; set; }
        public bool Drag { get; set; }
        public Point DragStartLocation { get; set; }
        public abstract bool Contains(Point point);
        public abstract void Draw(Graphics g);
        public abstract void Move(Point newLocation);
    }

    public class Circle : Shape
    {
        private int radius;
        public Circle(Color color, Point location, int radius)
        {
            ShapeColor = color;
            Location = location;
            this.radius = radius;
        }
        public override bool Contains(Point point)
        {
            double distance = Math.Sqrt(Math.Pow(point.X - Location.X, 2) + Math.Pow(point.Y - Location.Y, 2));
            return distance <= radius;
        }
        public override void Draw(Graphics graphics)
        {
            Brush brush = new SolidBrush(ShapeColor);
            graphics.FillEllipse(brush, Location.X - radius, Location.Y - radius, radius * 2, radius * 2);
        }
        public override void Move(Point newLocation)
        {
            int dx = newLocation.X - DragStartLocation.X;
            int dy = newLocation.Y - DragStartLocation.Y;
            Location = new Point(Location.X + dx, Location.Y + dy);
            DragStartLocation = newLocation;
        }
    }
    public class Rectangle : Shape
    {
        private int width;
        private int heigth;

        public Rectangle(Color color, Point location, int width, int heigth)
        {
            ShapeColor = color;
            Location = location;
            this.width = width;
            this.heigth = heigth;
        }
        public override bool Contains(Point point)
        {
            return point.X >= Location.X && point.X <= Location.X + width && point.Y >=Location.Y && point.Y <=Location.Y + heigth;
        }
        public override void Draw(Graphics graphics)
        {
            Brush brush = new SolidBrush(ShapeColor);
            graphics.FillRectangle(brush, Location.X, Location.Y, width, heigth);
        }
        public override void Move(Point newLocation)
        {
            int dx = newLocation.X - DragStartLocation.X;
            int dy = newLocation.Y - DragStartLocation.Y;
            Location = new Point(Location.X + dx, Location.Y + dy);
            DragStartLocation = newLocation;
        }
    }

    public class Triangle : Shape
    {
        private int width;
        private int heigth;

        public Triangle(Color color, Point location, int width, int heigth)
        {
            ShapeColor = color;
            Location = location;
            this.width = width;
            this.heigth = heigth;
        }
        public override bool Contains(Point point)
        {
            Point p1 = new Point(Location.X, Location.Y + heigth);
            Point p2 = new Point(Location.X + width, Location.Y + heigth);
            Point p3 = new Point(Location.X + width / 2, Location.Y);

            double area = Math.Abs((p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y));
            double area1 = Math.Abs((p1.X - point.X) * (p3.Y - point.Y) - (p3.X - point.X) * (p1.Y - point.Y));
            double area2 = Math.Abs((p2.X - point.X) * (p1.Y - point.Y) - (p1.X - point.X) * (p2.Y - point.Y));
            double area3 = Math.Abs((p3.X - point.X) * (p2.Y - point.Y) - (p2.X - point.X) * (p3.Y - point.Y));
            double sumOfAreas = area1 + area2 + area3;

            return Math.Abs(area - sumOfAreas) < 0.001;
        }
        public override void Draw(Graphics graphics)
        {
            Brush brush = new SolidBrush(ShapeColor);
            Point[] points = { new Point(Location.X, Location.Y + heigth), new Point(Location.X + width, Location.Y + heigth), new Point(Location.X + width / 2, Location.Y) };
            graphics.FillPolygon(brush, points);
        }
        public override void Move(Point newLocation)
        {
            int dx = newLocation.X - DragStartLocation.X;
            int dy = newLocation.Y - DragStartLocation.Y;
            Location = new Point(Location.X + dx, Location.Y + dy);
            DragStartLocation = newLocation;
        }
    }
}
