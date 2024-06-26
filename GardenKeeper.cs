using System;
namespace ZooManager
{
    public class GardenKeeper : Animal
    {
        private static Random random = new Random();
        public GardenKeeper(string name)
        {
            emoji = "🚶‍♂️";
            species = "gardenKeeper";
            this.name = name;
            reactionTime = new Random().Next(6, 10); // reaction time 6 to 9 (slow)
        }
        
        public void Move()
        {
            int x, y;
            do
            {
                x = random.Next(3, 7);
                y = random.Next(3, 7);
                Console.WriteLine($"Game.animalZones[{y}][{x}].occupant = {Game.animalZones[y][x].occupant}");
            } while ((x == this.location.x && y == this.location.y) || Game.animalZones[y][x].occupant != null);
            Game.animalZones[location.y][location.x].occupant = null;
            this.location = new Point { x = x, y = y };
            Console.WriteLine($"GardenKeeper y = {y}, x = {x}");
            Game.animalZones[y][x].occupant = this;

            Shoot();
            Scare();
        }

        public void Shoot()
        {
            for (var y = 0; y < Game.numCellsY; y++)
            {
                for (var x = 0; x < Game.numCellsX; x++)
                {
                    var zone = Game.animalZones[y][x];
                    if (zone.occupant is Animal animal && animal is Rabbit rabbit)
                    {
                        var h = System.Math.Abs(this.location.y - rabbit.location.y);
                        var w = System.Math.Abs(this.location.x - rabbit.location.x);
                        var distance = System.Math.Sqrt(h * h + w * w);
                        if (distance <= 2)
                        {
                            zone.occupant = null;
                        }
                    }
                }
            }
        }

        public void Scare()
        {
            for (var y = 0; y < Game.numCellsY; y++)
            {
                for (var x = 0; x < Game.numCellsX; x++)
                {
                    var zone = Game.animalZones[y][x];
                    if (zone.occupant is Animal animal && animal is Mouse mouse)
                    {
                        var h = System.Math.Abs(this.location.y - mouse.location.y);
                        var w = System.Math.Abs(this.location.x - mouse.location.x);
                        var distance = System.Math.Sqrt(h * h + w * w);
                        if (distance <= 2)
                        {
                            zone.occupant = null;
                            Game.animalZones[y - 1][x - 1].occupant = mouse;
                        }
                    }
                }
            }
        }
    }
}


