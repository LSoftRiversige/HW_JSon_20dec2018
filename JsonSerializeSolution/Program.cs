using System;
using Newtonsoft.Json;

namespace JsonSerializeSolution
{
    public class Car
    {
        public string ModelName { get; set; }
        public decimal Price { get; set; }
        public double HorcePower { get; set; }
        public bool IsStock { get; set; }
        public DateTime DateOfIssue { get; set; }
        //[MyJsonName("MyLetterInfo")]
        public char Letter { get; set; }
        public int[] Sizes { get; set; }
        public string[] Colors { get; set; }
        public class Engine
        {
            public string Vendor { get; set; }
            public int CylinderCount { get; set; }
            public char[] SerialNumber { get; set; }
        }
        public Engine EngineOfCar { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var car = new Car()
            {
                ModelName = "Land Cruiser LC 200",
                Price = 2500000.45m,
                IsStock = false,
                DateOfIssue = DateTime.Parse("2018/12/20"),
                HorcePower = 249d,
                Letter = 'A',
                Sizes = new int[] { 5000, 3000, 1500, 35 },
                Colors = new string[] { "Red", "Yellow" },
                EngineOfCar = new Car.Engine()
                {
                    CylinderCount = 8,
                    Vendor="Toyeta Motors corp.",
                    SerialNumber=new char[] { 'X', '5','1','8', 'Y', '2', 'Z'}
                }

            };
            string myJson = Json.ToJson(car);
            string newtonJson = JsonConvert.SerializeObject(car);

            Console.WriteLine(myJson);
            Console.WriteLine("-------------------");
            Console.WriteLine(newtonJson);
            Console.WriteLine("-------------------");

            bool isEqual = myJson.Equals(newtonJson);

            Console.Write("Equality check: ");
            Console.WriteLine(isEqual);
            Console.WriteLine();
            if (isEqual)
            {
                var car1 = JsonConvert.DeserializeObject<Car>(myJson);
                Console.WriteLine("DeserializeObject=OK");
                Console.WriteLine();
            }
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
