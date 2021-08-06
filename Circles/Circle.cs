using System.Drawing;

namespace Circles {
    public class Circle {
        public float X{ get; set; }
        public float Y{ get; set; }
        public float Radius{ get; set; }
        public float DeltaX{ get; set; }
        public float DeltaY{ get; set; }
        
        public SolidBrush Color{ get; set; }

        public Circle() {
        }

        public Circle(int x, int y, float radius) {
            X = x;
            Y = y;
            Radius = radius;
        }

        public static bool operator ==(Circle circle1, Circle circle2) {
            return !(circle2 is null) && !(circle1 is null) &&
                   circle1.X == circle2.X && circle1.Y == circle2.Y;
        }

        public static bool operator !=(Circle circle1, Circle circle2) {
            return !(circle1 == circle2);
        }
    }
}