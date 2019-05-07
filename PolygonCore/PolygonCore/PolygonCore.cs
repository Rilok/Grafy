using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCore
{
    struct PolygonData
    {
        public int x;
        public int y;
    }
    class PolygonCore
    {
        static void Main(string[] args)
        {
            int ymin, ymax;
            int corners = Convert.ToInt32(Console.ReadLine());
            PolygonData[] polygon = new PolygonData[corners];

            for (int i = 0; i < corners; i++)
            {
                Console.Write("X[" + i + "] = ");
                polygon[i].x = Convert.ToInt32(Console.ReadLine());
                Console.Write("Y[" + i + "] = ");
                polygon[i].y = Convert.ToInt32(Console.ReadLine());
            }

            ymin = InitMin(polygon);
            ymax = InitMax(polygon);

            if (corners == 1)
                CheckPolygon(polygon[0].y, polygon[0].y);
            else if (corners == 2)
                Line(polygon, ymin, ymax);
            else
                Polygon(polygon, corners, ymin, ymax);
            
            Console.ReadKey();
        }

        public static void Line(PolygonData[] polygon, int ymin, int ymax)
        {
            if (polygon[0].y > polygon[1].y)
            {
                ymin = polygon[0].y;
                ymax = polygon[1].y;
            }
            else
            {
                ymin = polygon[1].y;
                ymax = polygon[0].y;
            }
            CheckPolygon(ymin, ymax);
        }
        public static void Polygon(PolygonData[] polygon, int corners, int ymin, int ymax)
        {
            int det, addDet, before, next, additional;
            string info = "";
            string addInfo = "";

            Console.WriteLine("Podaj nazwę pliku:");
            string filename = Console.ReadLine();

            for (int j = 0; j < corners; j++)
            {
                before = j - 1;
                if (before < 0)
                    before = corners - 1;
                next = j + 1;
                if (next == corners)
                    next = 0;
                additional = next + 1;
                if (additional >= corners)
                    additional = 0;

                det = polygon[before].x * polygon[j].y + polygon[before].y * polygon[next].x +
                      polygon[j].x * polygon[next].y - polygon[before].x * polygon[next].y -
                      polygon[before].y * polygon[j].x - polygon[j].y * polygon[next].x;

                addDet = polygon[j].x * polygon[next].y + polygon[j].y * polygon[additional].x +
                         polygon[next].x * polygon[additional].y - polygon[j].x * polygon[additional].y -
                         polygon[j].y * polygon[next].x - polygon[next].y * polygon[additional].x;

                

                if (polygon[before].y != polygon[j].y && System.Math.Sign(det) < 0)
                {
                    if (polygon[j].y > polygon[before].y &&
                        polygon[j].y > polygon[next].y &&
                        polygon[j].y > ymax)
                    {
                        ymax = polygon[j].y;
                    }
                    else if (polygon[j].y < polygon[before].y &&
                             polygon[j].y < polygon[next].y &&
                             polygon[j].y < ymin)
                    {
                        ymin = polygon[j].y;
                    }
                    else if (polygon[j].y == polygon[next].y && System.Math.Sign(addDet) < 0)
                    {
                        if (polygon[next].y > polygon[additional].y && polygon[j].y > ymax)
                            ymax = polygon[j].y;
                        else if (polygon[next].y < polygon[additional].y && polygon[j].y < ymin)
                            ymin = polygon[j].y;
                    }
                }

                info = info + $"\nSignum[{j}] = {System.Math.Sign(det)}\nY min = {ymin} \n Y max = {ymax}";
            }

            info = info + CheckPolygon(ymin, ymax);
            using (StreamWriter writetext = new StreamWriter(filename))
            {
                writetext.Write($"{info}");
                
            }
            Console.WriteLine(CheckPolygon(ymin, ymax));
        }

        public static string CheckPolygon(int ymin, int ymax)
        {
            string info;
            if (ymin >= ymax)
            {
                info = $"\nWielokąt zawiera jądro.\nY min = {ymin} \n Y max = {ymax}";
            }
            else
            {
                info = $"\nWielokąt nie ma jądra\nY min = {ymin} \n Y max = {ymax}";
            }

            return info;
        }

        public static int InitMax(PolygonData[] polygon)
        {
            int max = polygon[0].y;
            for (int i = 1; i < polygon.Length; i++)
            {
                if (polygon[i].y < max)
                    max = polygon[i].y;
            }

            return max;
        }

        public static int InitMin(PolygonData[] polygon)
        {
            int min = polygon[0].y;
            for (int i = 1; i < polygon.Length; i++)
            {
                if (polygon[i].y > min)
                    min = polygon[i].y;
            }

            return min;
        }
    }
}
