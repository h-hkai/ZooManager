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
                case 3:
                    paths.Pop();
                    paths.Pop();
                    break;
                case 4:
                    paths.Pop();
                    paths.Pop();
                    paths.Pop();
                    break;
                default:
                    paths.Pop();
                    paths.Pop();
                    paths.Pop();
                    paths.Pop();
                    break;
            }
            int x = paths.Peek().x;
            int y = paths.Peek().y;

            paths.Clear();

            Game.animalZones[location.y][location.x].occupant = null;
            Game.animalZones[y][x].occupant = this;
        }

    }
}

