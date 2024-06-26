using System;
using System.Collections.Generic;
namespace ZooManager
{
    public class Animal : Occupant
    {
        public string name;
        public int reactionTime = 5; // default reaction time for animals (1 - 10)

        static public List<List<bool>> animalZonesVisited = new List<List<bool>>();
        public Stack<Point> paths = new Stack<Point>();

        public int speed;
        public int round = -1;
        public bool isLive = true;

        virtual public void Activate()
        {
            Console.WriteLine($"Animal {name} at {location.x},{location.y} activated");
        }

        // Use BFS to find the path to nearest flower.
        public void BFS()
        {
            int x, y, nx, ny;

            // init
            for (y = 0; y < Game.numCellsY; y++)
            {
                for (x = 0; x < Game.numCellsX; x++)
                {
                    animalZonesVisited[y][x] = false;
                }
            }

            int[] dirs = { 0, 1, 0, -1, 0 };

            var que = new Queue<Point>();

            que.Enqueue(location);
            while (que.Count != 0)
            {
                var curPoint = que.Dequeue();
                x = curPoint.x;
                y = curPoint.y;
                if (Game.animalZones[y][x].occupant is Flowers flowers)
                {
                    paths.Push(new Point { x = x, y = y });
                    flowers.hazardIndex = System.Math.Min(flowers.hazardIndex, System.Math.Abs(x - location.x) + System.Math.Abs(y - location.y));
                    break;
                }
                for (int i = 0; i < 4; i++)
                {
                    nx = x + dirs[i];
                    ny = y + dirs[i + 1];

                    var nextPoint = new Point { x = nx, y = ny };
                    if (isInMap(nextPoint) && !animalZonesVisited[ny][nx])
                    {
                        animalZonesVisited[ny][nx] = true;
                        Game.animalZones[ny][nx].pre = new Point { x = x, y = y };
                        que.Enqueue(nextPoint);
                    }
                }
            }
            // Use pre location to find path.
            do
            {
                if (paths.Count == 0) break;
                var topPoint = paths.Peek();
                x = topPoint.x;
                y = topPoint.y;
                if (x == this.location.x && y == this.location.y) break;
                paths.Push(Game.animalZones[y][x].pre);
            } while (true);
        }


    }
}
