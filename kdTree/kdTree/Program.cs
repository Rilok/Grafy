using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdTree
{
    public class KDTree
    {
        static void Main(string[] args)
        {
            Node tree = new Node();
            PointList[] area = new PointList[2];

            Console.WriteLine("Podaj liczbę elementów: ");
            int n = Convert.ToInt32(Console.ReadLine());
            PointList[] userPoints = new PointList[n];

            for (int i = 0; i < n; i++)
            {
                Console.Write($"Podaj element X[{i}]: ");
                userPoints[i].x = Convert.ToInt32(Console.ReadLine());

                Console.Write($"Podaj element Y[{i}]: ");
                userPoints[i].y = Convert.ToInt32(Console.ReadLine());
            }

            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    Console.Write("Podaj Xmin: ");
                    area[i].x = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Podaj Ymin: ");
                    area[i].y = Convert.ToInt32(Console.ReadLine());
                }

                else
                {
                    Console.Write("Podaj Xmax: ");
                    area[i].x = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Podaj Ymax: ");
                    area[i].y = Convert.ToInt32(Console.ReadLine());
                }

            }

            List<PointList> sortedX = new List<PointList>();
            List<PointList> sortedY = new List<PointList>();

            sortedX.AddRange(userPoints);
            sortedX.OrderBy(p => p.x);
            sortedY.AddRange(userPoints);
            sortedY.OrderBy(p => p.y);

            tree = BuildTree(sortedX, sortedY, 0);
            tree.Print();
            //WriteThatTree(tree);
            SearchTree(tree, area);

            Console.ReadKey();
        }

        public class Node
        {
            public Node before;
            public Node left;
            public Node right;
            public PointList[] area;
            public int? x;
            public int? y;
            public int depth;
            public string type;

            public Node()
            {
                before = null;
                left = null;
                right = null;
                x = null;
                y = null;
                area = new PointList[2];
                depth = 0;
                type = "";
            }
        }

        public struct PointList
        {
            public int x;
            public int y;
        }

        private List<Node> tree = new List<Node>();

        public static Node BuildTree(List<PointList> sortedX, List<PointList> sortedY, int depth)
        {
            Node node = null;
            int pivot = 0;

            List<PointList> leftSideX = null;
            List<PointList> rightSideX = null;
            List<PointList> leftSideY = null;
            List<PointList> rightSideY = null;

            if (sortedX.Count == 1)
            {
                return new Node{x = sortedX.First().x, y = sortedX.First().y, before = null, left = null, right = null, depth = depth, type = "point"};
            }
            else
            {
                node = new Node();
                if (depth % 2 == 0)
                {
                    leftSideX = new List<PointList>();
                    rightSideX = new List<PointList>();
                    leftSideY = new List<PointList>();
                    rightSideY = new List<PointList>();

                    int itemCount = sortedX.Count / 2;
                    int counter = 0;

                    leftSideX = sortedX.Take(sortedX.Count / 2).ToList();
                    rightSideX = sortedX.Skip(sortedX.Count / 2).ToList();

                    pivot = leftSideX.Last().x;
                    //pivot = Pivot(leftSideX, depth);

                    foreach (PointList point in sortedY)
                    {
                        if (point.x <= pivot) { leftSideY.Add(point);}
                        else if (point.x > pivot) { rightSideY.Add(point);}
                    }

                    node.x = pivot;
                    node.depth = depth;
                    node.type = "vertical";

                }
                else if (depth % 2 != 0)
                {
                    leftSideX = new List<PointList>();
                    rightSideX = new List<PointList>();
                    leftSideY = new List<PointList>();
                    rightSideY = new List<PointList>();

                    int itemCount = sortedY.Count / 2;
                    int counter = 0;

                    pivot = leftSideY.Last().x;
                    //pivot = Pivot(leftSideY, depth);

                    node.y = pivot;
                    node.depth = depth;
                    node.type = "horizontal";

                    node.left = BuildTree(leftSideX, leftSideY, depth + 1);
                    node.left.before = node;

                    node.right = BuildTree(rightSideX, rightSideY, depth + 1);
                    node.right.before = node;
                }

                node.left = BuildTree(leftSideX, leftSideY, depth + 1);
                node.left.before = node;

                node.right = BuildTree(rightSideX, rightSideY, depth + 1);
                node.right.before = node;
            }

            return node;
        }

        /*public static void SearchTree(Node root, PointList[] area)
        {
            if (root.type == "point")
            {
                if (root.x >= area[0].x && root.x <= area[1].x && root.y >= area[0].y && root.y <= area[1].y)
                {
                    Console.WriteLine($"Point {root.x}, {root.y} is included in question area.");
                }
            }
            else
            {
                if (root.type == "vertical")
                {
                    if (root.x >= area[0].x) { SearchTree(root.left, area);}
                    if (root.x <= area[1].x) { SearchTree(root.right, area);}
                }

                if (root.type == "horizontal")
                {
                    if (root.y >= area[0].y) { SearchTree(root.left, area);}
                    if (root.y <= area[1].y) { SearchTree(root.right, area);}
                }
            }
        }*/

        public static void SearchTree(Node root, PointList[] area)
        {
            if (root.type == "point")
            {
                if (root.x >= area[0].x && root.x <= area[1].x && root.y >= area[0].y && root.y <= area[1].y)
                {
                    Console.WriteLine($"Point {root.x}, {root.y} is included in question area.");
                }
            }
            else
            {
                NodeArea(root.left, "left");
                if (isContained(area, root.left.area))
                    ReportSubtree(root.left);
                else if (isIntersected(area, root.left.area))
                    SearchTree(root.left, area);

                NodeArea(root.right, "right");
                if (isContained(area, root.right.area))
                    ReportSubtree(root.right);
                else if (isIntersected(area, root.right.area))
                    SearchTree(root.right, area);
            }
        }

        public static bool isContained(PointList[] bigArea, PointList[] smallArea)
        {
            return (bigArea[0].y <= smallArea[0].y &&
                    bigArea[1].y >= smallArea[1].y &&
                    bigArea[0].x <= smallArea[0].x &&
                    bigArea[1].x >= smallArea[1].x);

        }

        public static bool isIntersected(PointList[] area, PointList[] nodeArea)
        {
            if ((nodeArea[0].x < area[0].x && area[0].x <= nodeArea[1].x
                || nodeArea[0].x < area[1].x && area[1].x <= nodeArea[1].x
                || area[0].x <= nodeArea[0].x && nodeArea[1].x <= area[1].x
                || nodeArea[0].y < area[0].y && area[0].y <= nodeArea[1].y
                || nodeArea[0].y < area[1].y && area[1].y <= nodeArea[1].y
                || area[0].y <= nodeArea[0].y && nodeArea[1].y <= area[1].y))
                return true;
            else
                return false;
        }

        public static void ReportSubtree(Node root)
        {
            if (root.type == "point")
            {
                Console.WriteLine($"Point {root.x}, {root.y} found in subtree.");
            }
            else
            {
                ReportSubtree(root.left);
                ReportSubtree(root.right);
            }   
        }
        public static void NodeArea(Node node, string sonType)
        {
            PointList[] parentArea = null;

            if (node.before != null && node != null)
            {
                parentArea = node.before.area;

                node.area[0].x = parentArea[0].x;
                node.area[1].x = parentArea[1].x;
                node.area[0].y = parentArea[0].y;
                node.area[1].y = parentArea[1].y;

                if (node.depth % 2 == 0) // horizontal
                {
                    if (sonType == "left")
                        node.area[1].y = (int)node.before.y;
                    if (sonType == "right")
                        node.area[0].y = (int)node.before.y;
                }
                else // vertical
                {
                    if (sonType == "left")
                        node.area[1].x = (int)node.before.x;
                    if (sonType == "right")
                        node.area[0].x = (int)node.before.x;
                }
            }
            else
            {
                
            }
        }

        public static int Pivot(List<PointList> points, int depth)
        {
            
            if (depth % 2 == 0)
            {
                return (int)points.Last().x;
            }
            else
            {
                return (int)points.Last().y;
            }
        }

        public static void WriteThatTree(Node root)
        {
            if (root != null)
            {
                WriteThatTree(root.left);
                Console.Write($"{root.x}, {root.y} | {root.type} | d = {root.depth} \t");
                WriteThatTree(root.right);
            }
        }
    }

    public static class BTreePrinter
    {
        class NodeInfo
        {
            public KDTree.Node Node;
            public string Text;
            public int StartPos;
            public int Size { get { return Text.Length; } }
            public int EndPos { get { return StartPos + Size; } set { StartPos = value - Size; } }
            public NodeInfo Parent, Left, Right;
        }

        public static void Print(this KDTree.Node root, int spacing = 2, int topMargin = 2, int leftMargin = 2)
        {
            if (root == null) return;
            int rootTop = Console.CursorTop + topMargin;
            var last = new List<NodeInfo>();
            var next = root;
            for (int level = 0; next != null; level++)
            {
                var item = new NodeInfo { Node = next, Text = ($"{next.x},{next.y}")};
                if (level < last.Count)
                {
                    item.StartPos = last[level].EndPos + spacing;
                    last[level] = item;
                }
                else
                {
                    item.StartPos = leftMargin;
                    last.Add(item);
                }
                if (level > 0)
                {
                    item.Parent = last[level - 1];
                    if (next == item.Parent.Node.left)
                    {
                        item.Parent.Left = item;
                        item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos - 1);
                    }
                    else
                    {
                        item.Parent.Right = item;
                        item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos + 1);
                    }
                }
                next = next.left ?? next.right;
                for (; next == null; item = item.Parent)
                {
                    int top = rootTop + 2 * level;
                    Print(item.Text, top, item.StartPos);
                    if (item.Left != null)
                    {
                        Print("/", top + 1, item.Left.EndPos);
                        Print("_", top, item.Left.EndPos + 1, item.StartPos);
                    }
                    if (item.Right != null)
                    {
                        Print("_", top, item.EndPos, item.Right.StartPos - 1);
                        Print("\\", top + 1, item.Right.StartPos - 1);
                    }
                    if (--level < 0) break;
                    if (item == item.Parent.Left)
                    {
                        item.Parent.StartPos = item.EndPos + 1;
                        next = item.Parent.Node.right;
                    }
                    else
                    {
                        if (item.Parent.Left == null)
                            item.Parent.EndPos = item.StartPos - 1;
                        else
                            item.Parent.StartPos += (item.StartPos - 1 - item.Parent.EndPos) / 2;
                    }
                }
            }
            Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
        }

        private static void Print(string s, int top, int left, int right = -1)
        {
            Console.SetCursorPosition(left, top);
            if (right < 0) right = left + s.Length;
            while (Console.CursorLeft < right) Console.Write(s);
        }
    }
}
