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

            Game.animalZones[location.y][location.x].occupant = null;
            Game.animalZones[y][x].occupant = this;
        }

    }
}

