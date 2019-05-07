using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steiner
{
    class Program
    {
        public struct Point
        {
            private int x;
            private int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public int GetX()
            {
                return x;
            }

            public int GetY()
            {
                return y;
            }

            public int Dist()
            {
                return Math.Abs(x) + Math.Abs(y);
            }

            public int Dist(Point p)
            {
                return (Math.Abs(x - p.GetX()) + Math.Abs(y - p.GetY()));
            }
        }

        public struct Candidate
        {
            private Point p1;
            private Point p2;
            private Point candidatePoint;

            public Candidate(Point p1, Point p2)
            {
                this.p1 = p1;
                this.p2 = p2;
                candidatePoint = new Point(Math.Min(p1.GetX(), p2.GetX()), Math.Min(p1.GetY(), p2.GetY()));
            }

            public Point GetP1()
            {
                return p1;
            }

            public Point GetP2()
            {
                return p2;
            }

            public Point GetCandidatePoint()
            {
                return candidatePoint;
            }

            public int Dist()
            {
                return candidatePoint.GetX() + candidatePoint.GetY();
            }

            public void PrintCandidate()
            {
                Console.WriteLine($"Candidate: {candidatePoint.GetX()}, {candidatePoint.GetY()}");
            }
        }

        public struct Line
        {
            private Point p1;
            private Point p2;

            public Line(Point p1, Point p2)
            {
                this.p1 = p1;
                this.p2 = p2;
            }
            
            public void PrintLine()
            {
                Console.WriteLine($"Line: {p1.GetX()}, {p1.GetY()} -> {p2.GetX()}, {p2.GetY()}.");
            }
        }

        static void Main(string[] args)
        {
            List<Line> lineList = new List<Line>();

            Console.Write("Podaj ilość elementów: ");
            int n = Convert.ToInt32(Console.ReadLine());

            List<Point> pointList = DataSet(n).OrderBy(p => p.GetX()).ThenBy(q => q.GetY()).ToList();

            SteinerTree(pointList, lineList);
            PrintLines(lineList);

            Console.ReadKey();
        }

        static List<Candidate> GenerateCandidates(List<Point> pointsList)
        {
            List<Candidate> candidates = new List<Candidate>();

            for (int i = 0; i < pointsList.Count; i++)
            {
                for (int j = 0; j < pointsList.Count; j++)
                {
                    if (i != j)
                    {
                        candidates.Add(new Candidate(pointsList[i], pointsList[j]));
                    }
                }
            }

            return candidates;
        }

        static Candidate ChooseCandidate(List<Candidate> candidates)
        {
            Candidate chosenCandidate = candidates.First();

            for (int i = 1; i < candidates.Count; i++)
            {
                if(chosenCandidate.GetCandidatePoint().GetY() < candidates[i].GetCandidatePoint().GetY())
                    if(chosenCandidate.Dist() < candidates[i].Dist())
                        chosenCandidate = candidates[i];
            }

            return chosenCandidate;
        }

        static void SteinerTree(List<Point> pointsList, List<Line> linesList)
        {
            while (pointsList.Count > 1)
            {
                List<Candidate> candidates = new List<Candidate>();
                candidates = GenerateCandidates(pointsList);

                //Print(candidates);

                Candidate chosenCandidate = ChooseCandidate(candidates);

                chosenCandidate.PrintCandidate();

                pointsList.Remove(chosenCandidate.GetP1());
                pointsList.Remove(chosenCandidate.GetP2());

                Point pq = new Point(chosenCandidate.GetCandidatePoint().GetX(), chosenCandidate.GetCandidatePoint().GetY());

                bool exists = false;
                for (int i = 0; i < pointsList.Count; i++)
                {
                    if (!ComparePoints(pq, pointsList[i]))
                    {
                        pointsList.Insert(i, pq);
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    pointsList.Add(pq);
                }
                AddLines(chosenCandidate.GetP1(), pq, linesList);
                AddLines(chosenCandidate.GetP2(), pq, linesList);
            }

            Point r = new Point(0, 0);
            AddLines(r, pointsList.First(), linesList);
        }

        static List<Point> DataSet(int n)
        {
            int x, y;
            List<Point> pointList = new List<Point>();
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Podaj X [{i}]: ");
                x = Convert.ToInt32(Console.ReadLine());
                Console.Write($"Podaj Y [{i}]: ");
                y = Convert.ToInt32(Console.ReadLine());

                pointList.Add(new Point(x, y));
            }

            return pointList;
        }

        static bool ComparePoints(Point p1, Point p2)
        {
            if (p1.Dist() != p2.Dist())
            {
                if (p1.Dist() > p2.Dist())
                    return false;
                else
                    return true;
            }
            else
            {
                if (p1.GetX() > p2.GetX())
                    return true;
                if (p1.GetY() > p2.GetY())
                    return true;
                else
                    return false;
            }
        }

        static void AddLines(Point p1, Point p2, List<Line> lineList)
        {
            Line newLine;

            if (p1.GetX() == p2.GetX() && p1.GetY() == p2.GetY())
                return;
            if (p1.GetX() == p2.GetX() || p1.GetY() == p2.GetY())
            {
                newLine = new Line(p1, p2);
                lineList.Add(newLine);
                return;
            }

            Point edgePoint = new Point(Math.Max(p1.GetX(), p2.GetX()), Math.Min(p1.GetY(), p2.GetY()));
            AddLines(p1, edgePoint, lineList);
            AddLines(p2, edgePoint, lineList);
        }

        static void PrintLines(List<Line> linesList)
        {
            foreach (Line l in linesList)
            {
                l.PrintLine();
            }
        }

        static void Print(List<Candidate> candidates)
        {
            foreach (Candidate c in candidates)
            {
                Console.WriteLine($"TMP Candidate: {c.GetCandidatePoint().GetX()}, {c.GetCandidatePoint().GetY()}");
            }
        }
    }
}
