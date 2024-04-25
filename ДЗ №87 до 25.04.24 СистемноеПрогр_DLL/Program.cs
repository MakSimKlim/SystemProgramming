using System;
using GeometryLib;

class Program
{
    static void Main()
    {
        Console.WriteLine("Площадь квадрата со стороной 5: " + Geometry.SquareArea(5));
        Console.WriteLine("Площадь прямоугольника со сторонами 4 и 6: " + Geometry.RectangleArea(4, 6));
        Console.WriteLine("Площадь треугольника с основанием 3 и высотой 7: " + Geometry.TriangleArea(3, 7));
        Console.ReadKey();
    }
}