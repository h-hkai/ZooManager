using System;
namespace ZooManager
{
	public class Flowers : Occupant
	{
		static int count = 0;
		public Flowers()
		{
			count++;
			this.emoji = "🌸";
			this.species = "flowers";
		}

		static public int getCounter()
		{
			return count;
		}
	}
}

