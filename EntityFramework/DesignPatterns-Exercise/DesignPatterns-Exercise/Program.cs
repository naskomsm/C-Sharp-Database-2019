namespace DesignPatterns_Exercise
{
    using System;

    public class Program
    {
        public static void Main()
        {
            // First Task
            SandwichMenu sandwichMenu = new SandwichMenu();

            // Initialize the default sandwiches
            sandwichMenu["BLT"] = new Sandwich("Wheat", "Bacon", "", "Lectture, Tomato");
            sandwichMenu["PB&J"] = new Sandwich("White", "", "", "Peanut Butter, Jelly");
            sandwichMenu["Turkey"] = new Sandwich("Rye", "Turkey", "Swiss", "Lectture, Onion, Tomato");

            // Deli manager adds custome sandwiches :D
            sandwichMenu["LoadedBLT"] = new Sandwich("Wheat", "Turkey, Beacon", "American", "Lettuce, Tomato, Onion, Olives");
            sandwichMenu["ThreeMeatCombo"] = new Sandwich("Rye", "Turkey, Ham, Salami", "Provolone", "Lettuce, Onion");
            sandwichMenu["Vegetarian"] = new Sandwich("Wheat", "", "", "Lettuce, Onion, Tomato, Olives, Spinach");

            // now we can clone the sandwiches
            var sandwich1 = sandwichMenu["BLT"].Clone() as Sandwich;
            var sandwich2 = sandwichMenu["ThreeMeatCombo"].Clone() as Sandwich;
            var sandwich3 = sandwichMenu["Vegetarian"].Clone() as Sandwich;

            // Second Task
            var phone = new SingleGift("Phone", 256);
            phone.CalculateTotalPrice();
            Console.WriteLine();

            var rootBox = new CompositeGift("RootBox", 0);
            var truckToy = new SingleGift("TruckToy", 289);
            var plainToy = new SingleGift("PlainToy", 587);

            rootBox.Add(truckToy);
            rootBox.Add(plainToy);

            var childBox = new CompositeGift("ChildBox", 0);
            var soldierToy = new SingleGift("SoldierToy", 200);

            childBox.Add(soldierToy);
            rootBox.Add(childBox);

            Console.WriteLine($"Total price of this composite present is: {rootBox.CalculateTotalPrice()}");

            // Third Task
            var sourdough = new SourDough();
            sourdough.Make();

            var twelveGrain = new TwelveGrain();
            twelveGrain.Make();

            var wholeWheat = new WholeWheat();
            wholeWheat.Make();
        }
    }
}