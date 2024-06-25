using System;
using System.Collections;
using System.Collections.Generic;

namespace ZooManager
{
    public class Mouse : Animal
    {
        private static Random random = new Random();
        private const int MinReactionTime = 1;
        private const int MaxReactionTime = 3;
        
        static private List<List<bool>> animalZonesVisited = new List<List<bool>>();
        private Stack<Point> paths = new Stack<Point>();

        public int round = -1;

        public Mouse(string name)
        {
            emoji = "🐭";
            species = "mouse";
            this.name = name;
            reactionTime = random.Next(MinReactionTime, MaxReactionTime + 1);

            // init
            for (var y = 0; y < Game.numCellsY; y++)
            {
                List<bool> rowList = new List<bool>();
                for (var x = 0; x < Game.numCellsX; x++) rowList.Add(false);
                animalZonesVisited.Add(rowList);
            }
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
                if (Game.animalZones[y][x].occupant is Flowers)
                {
                    paths.Push(new Point { x = x, y = y });
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

        public void Move()
        {
            BFS();
            if (paths.Count == 0) return;
            else if (paths.Count == 2) paths.Pop();
            else if (paths.Count > 2)
            {
                paths.Pop();
                paths.Pop();
            }
            int x = paths.Peek().x;
            int y = paths.Peek().y;

            paths.Clear();

            var targetZone = Game.animalZones[y][x];
            if (targetZone.occupant is Flowers)
            {
                // Mouse eats the flower and disappears
                Console.WriteLine($"{name} ate a flower at {x},{y} and disappeared.");
                targetZone.occupant = null;
                Game.animalZones[location.y][location.x].occupant = null;
                
            }
            else if (targetZone.occupant == null)
            {
                // Move to the new zone
                Game.animalZones[location.y][location.x].occupant = null;
                this.location = new Point { x = x, y = y };
                Game.animalZones[y][x].occupant = this;
                Console.WriteLine($"{name} moved to {x},{y}");
            }
        }
    }
}

