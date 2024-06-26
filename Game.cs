using System;
using System.Collections.Generic;

namespace ZooManager
{
    public static class Game
    {
        static public int numCellsX = 11;
        static public int numCellsY = 11;
        static private int maxCellsX = 11;
        static private int maxCellsY = 11;
        static private int roundCounter = 0;
        static public bool gameWon = false;

        static public List<List<Zone>> animalZones = new List<List<Zone>>();
        static public Zone holdingPen = new Zone(-1, -1, null);

        static public int coinsCounter = 0;

        static public void SetUpGame()
        {
            for (var y = 0; y < numCellsY; y++)
            {
                List<Zone> rowList = new List<Zone>();
                for (var x = 0; x < numCellsX; x++) rowList.Add(new Zone(x, y, null));
                animalZones.Add(rowList);
            }

            // Place the initial flower in the middle of the 3x3 grid
            animalZones[5][5].occupant = new Flowers();
            Console.WriteLine("Placed initial flower at 5,5");

            // Place initial mouse
            PlaceMouseInOuterGrid();
        }

        // Make sure that no other animals are present in the location of the spawning animal.
        static public Point getPositionInOuterGrid()
        {
            var random = new Random();
            int x, y;
            do
            {
                // Determine random outer grid position
                if (random.Next(2) == 0)
                {
                    // Place on top or bottom
                    y = random.Next(2) == 0 ? 0 : numCellsY - 1;
                    x = random.Next(numCellsX);
                }
                else
                {
                    // Place on left or right
                    x = random.Next(2) == 0 ? 0 : numCellsX - 1;
                    y = random.Next(numCellsY);
                }
            } while (animalZones[y][x].occupant != null);
            Point pos = new Point { x = x, y = y };
            return pos;
        }

        static public void PlaceMouseInOuterGrid()
        {
            int x, y;
            var mouse = new Mouse("Mouse" + roundCounter);
            mouse.location = getPositionInOuterGrid();
            x = mouse.location.x;
            y = mouse.location.y;
            animalZones[y][x].occupant = mouse;

            Console.WriteLine($"Placed mouse at {x},{y}");
        }

        static public void PlaceRabbitInOuterGrid()
        {
            int x, y;
            var rabbit = new Rabbit("Rabbit" + roundCounter);
            rabbit.location = getPositionInOuterGrid();
            x = rabbit.location.x;
            y = rabbit.location.y;
            animalZones[y][x].occupant = rabbit;

            Console.WriteLine($"Placed mouse at {x},{y}");
        }

        static public void ActivateAnimals()
        {
            Console.WriteLine("Activating animals...");
            for (var r = 1; r <= 10; r++) // reaction times from 1 to 10
            {
                for (var y = 0; y < numCellsY; y++)
                {
                    for (var x = 0; x < numCellsX; x++)
                    {
                        var zone = animalZones[y][x];
                        if (zone.occupant is Animal animal && animal.reactionTime == r && animal.round != roundCounter)
                        {
                            animal.round = roundCounter;
                            if (animal is Mouse mouse)
                            {
                                mouse.Move();
                            } else if (animal is Rabbit rabbit)
                            {
                                rabbit.Move();
                            } else if (animal is Cat cat)
                            {
                                cat .Move();
                            } else if (animal is GardenKeeper gardenKeeper)
                            {
                                gardenKeeper .Move();
                            }
                            else
                            {
                                animal.Activate();
                            }
                        }
                    }
                }
            }
        }

        static public void NextRound()
        {
            if (gameWon) return;

            ActivateAnimals();

            roundCounter++;
            Console.WriteLine($"Starting round {roundCounter}");

            if (roundCounter % 2 == 0)
            {
                PlaceFlowerInCenter();
            }

            // Add logic to spawn mice every 3 rounds
            if (roundCounter % 3 == 0)
            {
                PlaceMouseInOuterGrid();
            }

            // Add logic to spawn mice every 4 rounds
            if (roundCounter % 4 == 0)
            {
                PlaceRabbitInOuterGrid();
            }
            
            CheckWinCondition();
        }

        static private void PlaceFlowerInCenter()
        {
            // Define the center 3x3 grid (from 4,4 to 6,6 inclusive)
            for (var y = 4; y <= 6; y++)
            {
                for (var x = 4; x <= 6; x++)
                {
                    if (animalZones[y][x].occupant == null)
                    {
                        animalZones[y][x].occupant = new Flowers();
                        Console.WriteLine($"Placed a flower at {x},{y}");
                        return; // Place one flower per eligible round
                    }
                }
            }
        }

        static private void CheckWinCondition()
        {
            int flowerCount = 0;
            // Define the center 3x3 grid (from 4,4 to 6,6 inclusive)
            for (var y = 4; y <= 6; y++)
            {
                for (var x = 4; x <= 6; x++)
                {
                    if (animalZones[y][x].occupant is Flowers)
                    {
                        flowerCount++;
                    }
                }
            }

            if (flowerCount >= 9)
            {
                gameWon = true;
                Console.WriteLine("You win! 9 flowers placed in the center.");
            }
        }

        static public void AddZones(Direction d)
        {
            if (d == Direction.down || d == Direction.up)
            {
                if (numCellsY >= maxCellsY) return; // hit maximum height!
                List<Zone> rowList = new List<Zone>();
                for (var x = 0; x < numCellsX; x++)
                {
                    rowList.Add(new Zone(x, numCellsY, null));
                }
                numCellsY++;
                if (d == Direction.down) animalZones.Add(rowList);
                // if (d == Direction.up) animalZones.Insert(0, rowList);
            }
            else // must be left or right...
            {
                if (numCellsX >= maxCellsX) return; // hit maximum width!
                for (var y = 0; y < numCellsY; y++)
                {
                    var rowList = animalZones[y];
                    // if (d == Direction.left) rowList.Insert(0, new Zone(null));
                    if (d == Direction.right) rowList.Add(new Zone(numCellsX, y, null));
                }
                numCellsX++;
            }
        }

        static public void ZoneClick(Zone clickedZone)
        {
            Console.WriteLine($"Click x = {clickedZone.location.x} y = {clickedZone.location.y}");
            if (gameWon) return;

            Console.Write("Got animal ");
            Console.WriteLine(clickedZone.emoji == "" ? "none" : clickedZone.emoji);
            Console.Write("Held animal is ");
            Console.WriteLine(holdingPen.emoji == "" ? "none" : holdingPen.emoji);
            if (clickedZone.occupant != null) clickedZone.occupant.ReportLocation();
            if (holdingPen.occupant == null && clickedZone.occupant != null)
            {
                // take animal from zone to holding pen
                Console.WriteLine("Taking " + clickedZone.emoji);
                holdingPen.occupant = clickedZone.occupant;
                holdingPen.occupant.location = new Point { x = -1, y = -1 };
                clickedZone.occupant = null;
                ActivateAnimals();
            }
            else if (holdingPen.occupant != null && clickedZone.occupant == null)
            {
                // put animal in zone from holding pen
                Console.WriteLine("Placing " + holdingPen.emoji);
                clickedZone.occupant = holdingPen.occupant;
                clickedZone.occupant.location = clickedZone.location;
                holdingPen.occupant = null;
                Console.WriteLine("Empty spot now holds: " + clickedZone.emoji);
                //ActivateAnimals();
            }
            else if (holdingPen.occupant != null && clickedZone.occupant != null)
            {
                Console.WriteLine("Could not place animal.");
                // Don't activate animals since user didn't get to do anything
            }
        }

        static public void AddToHolding(string occupantType)
        {
            if (gameWon) return;
            if (holdingPen != null && holdingPen.occupant != null) return;
            if (occupantType == "flowers") return; // prevent players from adding flowers manually
            Console.WriteLine($"occupantType = {occupantType}");
            if (holdingPen.occupant == null)
            {
                Occupant obj = null;
                switch(occupantType)
                {
                    case "cat":
                        if (coinsCounter < 5) return;
                        obj = new Cat("Cat" + roundCounter);
                        coinsCounter -= 5;
                        break;
                    case "gardenKeeper":
                        if (coinsCounter < 10) return;
                        obj = new GardenKeeper("GardeKeeper" +  roundCounter);
                        coinsCounter -= 10;
                        break;
                    case "trap":
                        if (coinsCounter < 2) return;
                        obj = new Trap();
                        coinsCounter -= 2;
                        break;
                    default:
                        return;
                }
                Console.WriteLine("Taking " + obj.emoji);
                holdingPen.occupant = obj;
                //ActivateAnimals();
            }
            Console.WriteLine($"Holding pen occupant at {holdingPen.occupant.location.x},{holdingPen.occupant.location.y}");
        }

      
    }
}

