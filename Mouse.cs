using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

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
            this.speed = 2;
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
            Console.WriteLine($"{name} moved.");
            
            BFS();
            switch (paths.Count)
            {
                case 0:
                    return;
                case 1:
                    break;
                case 2:
                    Step(paths.Pop());
                    break;
                default:
                    Step(paths.Pop());
                    if (this.isLive) { 
                        Step(paths.Pop()); 
                    }
                    break;
            }

            paths.Clear();
        }

        public void Step(Point last)
        {
            int x = paths.Peek().x;
            int y = paths.Peek().y;

            var trapZone = Game.animalZones[y][x];
            if (trapZone.occupant is Trap trap)
            {
                this.isLive = false;
                Game.animalZones[location.y][location.x].occupant = null;
                trapZone.occupant = null;
                return;
            }

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

            Scared(last);
        }

        public void Scared(Point last)
        {
            for (var y = 0; y < Game.numCellsY; y++)
            {
                for (var x = 0; x < Game.numCellsX; x++)
                {
                    var zone = Game.animalZones[y][x];
                    if (zone.occupant is Animal animal1 && animal1 is GardenKeeper gardenKeeper)
                    {
                        var h = System.Math.Abs(this.location.y - gardenKeeper.location.y);
                        var w = System.Math.Abs(this.location.x - gardenKeeper.location.x);
                        var distance = System.Math.Sqrt(h * h + w * w);
                        if (distance <= 2)
                        {
                            paths.Push(last);
                        }
                    } else if (zone.occupant is Animal animal2 && animal2 is Cat cat)
                    {
                        var h = System.Math.Abs(this.location.y - cat.location.y);
                        var w = System.Math.Abs(this.location.x - cat.location.x);
                        var distance = System.Math.Sqrt(h * h + w * w);
                        if (distance <= 1)
                        {
                            paths.Push(last);
                        }
                    }
                    
                }
            }
        }
    }
}

