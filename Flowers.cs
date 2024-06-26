using System;
namespace ZooManager
{
	public class Flowers : Occupant
	{
		public int hazardIndex = 10000;
		public Flowers()
		{
			this.emoji = "🌸";
			this.species = "flowers";
		}
	}
}

