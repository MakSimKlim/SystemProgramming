using System;

namespace GeometryLib
{
    public static class Geometry
    {
        //площадь квадрата
        public static double SquareArea(double side)
        {
            return side * side;
        }

        //площадь прямоугольника
        public static double RectangleArea(double length, double width)
        {
            return length * width;
        }
        //площадь треугольника
        public static double TriangleArea(double basis, double height)
        {
            return 0.5 * basis * height;
        }
    }
}