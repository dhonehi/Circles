using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Circles {
    public class CirclesController {
        private List<Circle> _circles = new List<Circle>();
        private readonly Form _context;
        private Random _random = new Random();

        private void DrawCircle(Circle circle, Graphics graphics) {
            graphics.FillEllipse(
                circle.Color,
                circle.X - circle.Radius,
                circle.Y - circle.Radius,
                circle.Radius * 2,
                circle.Radius * 2);
        }

        private void Add(MouseEventArgs e) {
            float circleRadius = CircleConfig.Radius;

            if (e.X <= circleRadius || e.X >= _context.ClientSize.Width - circleRadius ||
                e.Y <= circleRadius || e.Y >= _context.ClientSize.Height - circleRadius) return;

            _circles.Add(new Circle{
                    X = e.X, Y = e.Y, Radius = CircleConfig.Radius,
                    DeltaX = 2 * _random.Next(0, 2) * 2 - 1,
                    DeltaY = 2 * _random.Next(0, 2) * 2 - 1,
                    Color = GenerateColor()
                });
        }

        private SolidBrush GenerateColor() {
            return new SolidBrush(
                Color.FromArgb(
                    _random.Next(256),
                    _random.Next(256),
                    _random.Next(256)
                )
            );
        }

        private void ProcessIntersectionWithBorder() {
            foreach (var circle in _circles) {
                float circleBoxTop = circle.Y - circle.Radius,
                    circleBoxBottom = circle.Y + circle.Radius,
                    circleBoxRight = circle.X + circle.Radius,
                    circleBoxLeft = circle.X - circle.Radius;

                if (circleBoxTop <= 0 || circleBoxBottom >= _context.ClientSize.Height) {
                    circle.DeltaY *= -1;
                }

                if (circleBoxLeft <= 0 || circleBoxRight >= _context.ClientSize.Width) {
                    circle.DeltaX *= -1;
                }
            }
        }

        private bool IsIntersectCircles(Circle circle1, Circle circle2) {
            return Math.Sqrt(Math.Pow(circle1.X - circle2.X, 2) + Math.Pow(circle1.Y - circle2.Y, 2)) <=
                   circle1.Radius + circle2.Radius;
        }

        private void ProcessIntersectionWithCircles() {
            for (int i = 0; i < _circles.Count - 1; ++i) {
                for (int j = i + 1; j < _circles.Count; ++j) {
                    Circle circle1 = _circles[i], circle2 = _circles[j];

                    if (IsIntersectCircles(circle1, circle2)) {
                        double deltaX = circle1.X - circle2.X, deltaY = circle1.Y - circle2.Y;
                        double alfa = Math.Atan2(deltaY, deltaX);

                        double angle = Math.Atan2(circle2.DeltaY, circle2.DeltaX) - alfa,
                            angleCircle = Math.Atan2(circle1.DeltaY, circle1.DeltaX) - alfa,
                            mod = Math.Sqrt(circle2.DeltaX * circle2.DeltaX + circle2.DeltaY * circle2.DeltaY),
                            modCircle = Math.Sqrt(circle1.DeltaX * circle1.DeltaX + circle1.DeltaY * circle1.DeltaY);

                        double newDeltaX1 = mod * Math.Cos(angle),
                            newDeltaY = mod * Math.Sin(angle),
                            newDeltaX = modCircle * Math.Cos(angleCircle),
                            newDeltaY1 = modCircle * Math.Sin(angleCircle);

                        angle = Math.Atan2(newDeltaY, newDeltaX) + alfa;
                        angleCircle = Math.Atan2(newDeltaY1, newDeltaX1) + alfa;
                        mod = Math.Sqrt(newDeltaX * newDeltaX + newDeltaY * newDeltaY);
                        modCircle = Math.Sqrt(newDeltaX1 * newDeltaX1 + newDeltaY1 * newDeltaY1);

                        circle2.DeltaX = (float) (mod * Math.Cos(angle));
                        circle2.DeltaY = (float) (mod * Math.Sin(angle));
                        circle1.DeltaX = (float) (modCircle * Math.Cos(angleCircle));
                        circle1.DeltaY = (float) (modCircle * Math.Sin(angleCircle));

                        circle1.Color = GenerateColor();
                        circle2.Color = GenerateColor();
                    }
                }
            }
        }

        public CirclesController(Form context) {
            _context = context;
        }

        public void AddOrRemoveCircle(MouseEventArgs e) {
            Random random = new Random();

            if (_circles.Count == 0) {
                Add(e);
                return;
            }

            bool isPointInCircle = false;

            foreach (var circle in _circles) {
                double lineLength = Math.Sqrt(Math.Pow(circle.X - e.X, 2) + Math.Pow(circle.Y - e.Y, 2));
                float circleRadius = CircleConfig.Radius;

                if (lineLength <= circleRadius * 2 && lineLength > circleRadius) return;

                if (lineLength <= circle.Radius) {
                    _circles = _circles.Where(circleItem => circleItem != circle).ToList();
                    isPointInCircle = true;
                    break;
                }
            }

            if (!isPointInCircle) {
                Add(e);
            }
        }

        public void Move() {
            if (_circles.Count == 0) return;

            ProcessIntersectionWithCircles();
            ProcessIntersectionWithBorder();

            foreach (var circle in _circles) {
                circle.X += circle.DeltaX;
                circle.Y += circle.DeltaY;
            }
        }

        public void RepaintAll(Graphics graphics) {
            graphics.Clear(Color.WhiteSmoke);

            foreach (var circle in _circles) {
                DrawCircle(circle, graphics);
            }
        }
    }
}