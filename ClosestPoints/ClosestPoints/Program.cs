using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ClosestPoints
{
    struct PointsData
    {
        public int X;
        public int Y;
    }

    class Segment
    {
        public Segment(PointsData p1, PointsData p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public readonly PointsData P1;
        public readonly PointsData P2;

        public float Distance()
        {
            return (float)Math.Sqrt(Math.Pow((P1.X - P2.X), 2) + Math.Pow((P1.Y - P2.Y), 2));
        }

        public static float Distance(PointsData p1, PointsData p2)
        {
            return (float)Math.Sqrt(Math.Pow((p1.X - p2.X), 2) + Math.Pow((p1.Y - p2.Y), 2));
        }
    }
    class ClosestPoints
    {
        static Segment ClosestPoints_Brute(PointsData[] points)
        {
            int n = points.Length;
            var result = Enumerable.Range(0, n - 1)
                .SelectMany(i => Enumerable.Range(i + 1, n - (i + 1))
                    .Select(j => new Segment(points[i], points[j])))
                .OrderBy(seg => seg.Distance())
                .First();
            return result;
        }

        private static Segment ClosestPoints2(PointsData[] points)
        {

            int count = points.Length;
            if (count <= 3)
                return ClosestPoints_Brute(points);

            PointsData[] leftSide = points.Take(count / 2).ToArray();
            PointsData[] rightSide = points.Skip(count / 2).ToArray();

            var leftResult = ClosestPoints2(leftSide);
            var rightResult = ClosestPoints2(rightSide);

            var result = rightResult.Distance() < leftResult.Distance() ? rightResult : leftResult;

            var midX = leftSide.Last().X;
            var bandWidth = result.Distance();
            var inBandByX = points.Where(p => Math.Abs(midX - p.X) <= bandWidth);

            var inBandByY = inBandByX.ToArray();

            int iLast = inBandByY.Length - 1;
            for (int i = 0; i < iLast; i++)
            {
                var pLower = inBandByY[i];
                for (int j = i + 1; j <= iLast; j++)
                {
                    var pUpper = inBandByY[j];
                    if ((Math.Abs(pUpper.Y - pLower.Y)) >= result.Distance())
                        continue;

                    if (Segment.Distance(pUpper, pLower) < result.Distance())
                        result = new Segment(pLower, pUpper);
                }
            }
            return result;

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Podaj ilość elementów.");
            int n = Convert.ToInt32(Console.ReadLine());

            PointsData[] S = new PointsData[n];
            Segment result;

            FillData(S, n);

            S = S.OrderBy(p => p.X).ThenBy(p => p.Y).ToArray();

            //SortingData(S, 0, n-1);
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Sorted by X: {i} X = {S[i].X}, Y = {S[i].Y};");
            }

            result = ClosestPoints2(S);

            Console.WriteLine($"Closest points:");
            Console.WriteLine($"X1 = {result.P1.X}, Y1 = {result.P1.Y}");
            Console.WriteLine($"X2 = {result.P2.X}, Y2 = {result.P2.Y}");
            Console.WriteLine($"{result.Distance()}");

            //Console.WriteLine($"{5/2}");
            Console.ReadKey();
        }

        public static void FillData(PointsData[] data, int n)
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write($"[{i}] X = ");
                data[i].X = Convert.ToInt32(Console.ReadLine());
                Console.Write($"[{i}] Y = ");
                data[i].Y = Convert.ToInt32(Console.ReadLine());
            }
        }

        public static void SortingData(PointsData[] data, int left, int right)
        {
            int i = left, j = right;
            var pivot = data[(left + right) / 2].X;
            while (i < j)
            {
                while (data[i].X < pivot) i++;
                while (data[j].X > pivot) j--;
                if (i <= j)
                {
                    var tmp = data[i];
                    data[i++] = data[j];
                    data[j--] = tmp;
                }
            }

            if (left < j) SortingData(data, left, j);
            if (i < right) SortingData(data, i, right);
        }
    }
}
