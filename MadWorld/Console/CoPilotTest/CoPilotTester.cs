using System;

namespace Console.CoPilotTest
{
    //This is my Github Co-Pilot test class 
    public class CoPilotTester 
    {
        public static CoPilotTester Tester = new();
        public static void Start() {
            var f = Tester.ConvertCelsiusToFahrenheit(30.0);
            System.Console.WriteLine("30.0 degrees Celsius is {0} degrees Fahrenheit", f);
        }

        public double ConvertCelsiusToFahrenheit(double celsius) {
            return (celsius * 9 / 5) + 32;
        }
    }
}