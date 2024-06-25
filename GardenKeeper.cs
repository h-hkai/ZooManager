using System;
namespace ZooManager
{
    public class GardenKeeper : Animal
    {
        public GardenKeeper(string name)
        {
            emoji = "🚶‍♂️";
            species = "gardenKeeper";
            this.name = name;
            reactionTime = new Random().Next(6, 10); // reaction time 6 to 9 (slow)
        }
        

        
    }
}


