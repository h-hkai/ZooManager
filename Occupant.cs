using System;
namespace ZooManager
{
    public class Occupant
    {
        public string emoji;
        public string species;

        public Point location;

        public void ReportLocation()
        {
            Console.WriteLine($"I am at {location.x},{location.y}");
        }

        // If the obj are in the map.
        public bool isInMap(Point next)
        {
            int x = next.x; 
            int y = next.y;
            if (0 <= x && x < Game.numCellsX && 0 <= y && y < Game.numCellsY) return true;
            return false;
        }

        // If the obj are in the garden.
        public bool isInGarden(Point next)
        {
            int x = this.location.x;
            int y = this.location.y;
            if (3 <= x && x <= 7 && 3 <= y && y <= 7) return true;
            return false;
        }
    }
}
