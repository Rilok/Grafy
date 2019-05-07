using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossingPoints
{
    public struct Points
    {
        public int x;
        public int y;
    }

    public struct Line
    {
        public int x;
        public int y1; // początek linii pionowej / wysokość prostej x
        public int y2; // koniec linii pionowej / ------//---------
        public string type;

        public Line(int x, int y, string type)
        {
            this.x = x;
            this.y1 = y;
            this.y2 = y;
            this.type = type;
        }

        public Line(int x, int y1, int y2, string type)
        {
            this.x = x;
            this.y1 = y1;
            this.y2 = y2;
            this.type = type;
        }
    }

    public class CrossingPoints
    {
        static void Main(string[] args)
        {
            int x1, x2, y1, y2;
            int linesNumber;
            List<Line> lineList = new List<Line>();
            List<Points> findingPoints = new List<Points>();

            Console.WriteLine("Podaj ilość prostych.");
            linesNumber = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < linesNumber; i++)
            {
                Console.Write($"Podaj X1[{i}]: ");
                x1 = Convert.ToInt32(Console.ReadLine());
                Console.Write($"Podaj X2[{i}]: ");
                x2 = Convert.ToInt32(Console.ReadLine());
                Console.Write($"Podaj Y1[{i}]: ");
                y1 = Convert.ToInt32(Console.ReadLine());
                Console.Write($"Podaj Y2[{i}]: ");
                y2 = Convert.ToInt32(Console.ReadLine());

                if (x1 == x2)
                {
                    lineList.Add(new Line(x1, y1, y2, "2vertical"));
                }
                else if (y1 == y2)
                {
                    if (x1 < x2)
                    {
                        lineList.Add(new Line(x1, y1, "1begin"));
                        lineList.Add(new Line(x2, y1, "3end"));
                    }
                    else
                    {
                        lineList.Add(new Line(x2, y1, "1begin"));
                        lineList.Add(new Line(x1, y1, "3end"));
                    }
                }
            }

            lineList = lineList.OrderBy(p => p.type).ThenBy(p => p.x).ToList();
            findingPoints = Sweeper(lineList);
            WritePoints(findingPoints);

            Console.ReadKey();
        }


        public static List<Points> Sweeper(List<Line> lineList)
        {
            List<Points> findingPoints = new List<Points>();
            List<int> horizontalLines = new List<int>();

            foreach (Line l in lineList)
            {
                int x = l.x;
                int y1 = l.y1;
                int y2 = l.y2;
                string type = l.type;

                if (type == "1begin")
                {
                    bool exists = false;
                    for (int i = 0; i < horizontalLines.Count; i++)
                    {
                        if (y1 < horizontalLines.ElementAt(i))
                        {
                            horizontalLines.Insert(i, y1);
                            exists = true;
                            break;
                        }
                    }

                    if (exists == false)
                    {
                        horizontalLines.Add(y1);
                    }
                }

                else if (type == "2vertical")
                {
                    foreach (int hL in horizontalLines)
                    {
                        if (isBetween(hL, y1, y2))
                        {
                            findingPoints.Add(new Points{x = x, y = hL});
                        }
                    }
                }

                else if (type == "3end")
                {
                    foreach (int hL in horizontalLines)
                    {
                        if (hL == y1)
                        {
                            horizontalLines.Remove(hL);
                            break;
                        }
                    }
                }
            }

            return findingPoints;
        }

        public static void WritePoints(List<Points> findingPoints)
        {
            Console.WriteLine("Znaleziono przecięcia w miejscach: ");
            foreach (Points p in findingPoints)
            {
                Console.WriteLine($"X: {p.x}, Y: {p.y}");
            }
        }

        public static bool isBetween(int num, int y1, int y2)
        {
            bool isBetween = false;
            if (y1 < y2)
            {
                if (num >= y1 && num <= y2)
                {
                    isBetween = true;
                }
            }
            else
            {
                if (num <= y1 && num >= y2)
                {
                    isBetween = true;
                }
            }

            return isBetween;
        }
    }
}
