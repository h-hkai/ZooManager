using System;
using System.Collections.Generic;

namespace ZooManager
{
    public class Cat : Animal
    {
        public Point targetZoon;

        public Stack<Point> temp = new Stack<Point>();

        private static Random random = new Random();

        public Cat(string name)
        {
            emoji = "🐱";
            species = "cat";
            this.name = name;
            reactionTime = new Random().Next(1, 6); // reaction time 1 (fast) to 5 (medium)Cat
        }

        new public void BFS()
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
                if (Game.animalZones[y][x].occupant is Mouse)
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
            Console.WriteLine($"Cat paths.size = {paths.Count}");
            int count = random.Next(1, paths.Count + 2);
            while (paths.Count > count)
            {
                if (paths.Count == 0) break ;
                paths.Pop();
            }
            if (paths.Count != 0) Step();
            paths.Clear();
        }

        public void Step()
        {
            int x = paths.Peek().x;
            int y = paths.Peek().y;
            Console.WriteLine($"Cat pos x = {x}, y = {y}");

            Game.animalZones[location.y][location.x].occupant = null;
            Game.animalZones[y][x].occupant = this;
        }

    }
}

