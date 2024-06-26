using System;
namespace ZooManager
{
	public class Rabbit : Animal
	{
		public Rabbit(string name)
		{
			this.emoji = "🐇";
			this.species = "rabbit";
			this.name = name;
            this.speed = 4;
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
                    Step();
                    break;
                case 3:
                    paths.Pop();
                    Step();
                    if (this.speed <= 2) break;
                    paths.Pop();
                    Step();
                    break;
                case 4:
                    paths.Pop();
                    Step();
                    if (this.speed <= 2) break;
                    paths.Pop();
                    Step();
                    if (this.speed <= 3) break;
                    paths.Pop();
                    Step();
                    break;
                default:
                    paths.Pop();
                    Step();
                    if (this.speed <= 2) break;
                    paths.Pop();
                    Step();
                    if (this.speed <= 3) break;
                    paths.Pop();
                    Step();
                    if (this.speed <= 4) break;
                    paths.Pop();
                    Step();
                    break;
            }

            paths.Clear();
        }

        public void Step()
        {
            int x = paths.Peek().x;
            int y = paths.Peek().y;

            var trapZone = Game.animalZones[y][x];
            if (trapZone.occupant is Trap)
            {
                this.speed = this.speed - 1;
                Game.animalZones[location.y][location.x].occupant = null;
                trapZone.occupant = null;
                return;
            }

            var targetZone = Game.animalZones[y][x];
            if (targetZone.occupant is Flowers)
            {
                // Mouse eats the flower and disappears
                Console.WriteLine($"{name} ate a flower at {x},{y} and disappeared.");
                targetZone.occupant = this;
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

            Killed();
        }

        public void Killed()
        {
            for (var y = 0; y < Game.numCellsY; y++)
            {
                for (var x = 0; x < Game.numCellsX; x++)
                {
                    var zone = Game.animalZones[y][x];
                    if (zone.occupant is Animal animal && animal is GardenKeeper gardenKeeper)
                    {
                        var h = System.Math.Abs(this.location.y - gardenKeeper.location.y);
                        var w = System.Math.Abs(this.location.x - gardenKeeper.location.x);
                        var distance = System.Math.Sqrt(h * h + w * w);
                        if (distance < 2)
                        {
                            Game.animalZones[location.y][location.x].occupant = null;
                        }
                    }
                }
            }
        }

    }
}

