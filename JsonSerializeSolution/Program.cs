using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonSerializeSolution
{
    [Serializable]
    public class Car
    {
        public DriverCategory DriverCategory { get; set; }
        public List<Wheel> Wheels { get; set; }
        public string ModelName { get; set; }
        public decimal Price { get; set; }
        public double HorcePower { get; set; }
        public bool IsStock { get; set; }
        public DateTime DateOfIssue { get; set; }

        [MyJsonName("Letter")]
        public char Letter { get; set; }
        
        public int[] Sizes { get; set; }
        public string[] Colors { get; set; }
        public Engine EngineOfCar { get; set; }

        //public Dictionary<string, string> Parts { get; set; }

        public class Engine
        {
            public string Vendor { get; set; }
            public int CylinderCount { get; set; }
            public char[] SerialNumber { get; set; }
        }
        
        public class Wheel
        {
            public string ModelName { get; set; }
            public int Size { get; set; }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var car = new Car()
            {
                DriverCategory=DriverCategory.B,
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
                },
                Wheels=new List<Car.Wheel>()
                {
                    new Car.Wheel()
                    {
                        ModelName ="Goodyear",
                        Size=85
                    },
                    new Car.Wheel()
                    {
                        ModelName ="Goodyear",
                        Size=85
                    },
                    new Car.Wheel()
                    {
                        ModelName ="Goodyear",
                        Size=85
                    },
                    new Car.Wheel()
                    {
                        ModelName ="Goodyear",
                        Size=85
                    }
                }

                //,Parts = new Dictionary<string, string>()
                //{
                //    ["cr"] = "Кардан",
                //    ["kb"] = "Карбюратор"
                //}
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

    public enum DriverCategory
    {
        Undefined, 
        A, B, C, D, E
    }
}