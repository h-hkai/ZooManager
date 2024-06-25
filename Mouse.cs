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

        public void Move()
        {
            BFS();
            switch (paths.Count)
            {
                case 0:
                    return;
                case 1:
                    break;
                case 2:
                    paths.Pop();
                    break;
                default:
                    paths.Pop();
                    paths.Pop();
                    break;
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

