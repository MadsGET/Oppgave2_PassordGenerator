using System;
using System.Collections.Generic;

namespace Oppgave2_PassordGenerator
{
    class Program
    {
        private static Random random = new Random();
        private static readonly int lowerCaseStart = 97, lowerCaseEnd = 123;
        private static readonly int upperCaseStart = 65, upperCaseEnd = 91;
        private static readonly int symbolStart = 33, symbolEnd = 48;
        private static readonly int digitStart = 48, digitEnd = 58;
        private static readonly string helpText = "\n" +
        "l = lower case letter \n" +
        "L = upper case letter \n" +
        "d = digit \n" +
        "s = special characters \n\n" +
        "-> Command: (length) + (min character value)\n" +
        "-> Example: 4 lLsd\n" +
        "-> Equals: oV/8";

        private static int lengthCount = 0, lowercaseCount = 0, uppercaseCount = 0, digitCount = 0, symbolCount = 0;

        static void Main(string[] args)
        {
            // Check if length and command is valid.
            if (args.Length == 2 && ValidateCommand(args[0], args[1]))
            {
                Console.WriteLine(GeneratePassword(lengthCount, lowercaseCount, uppercaseCount, digitCount, symbolCount));
            }
            else 
            {
                ThrowError();
            }
        }

        static void ThrowError() 
        {
            Console.WriteLine(helpText);
        }

        static bool ValidateCommand(string length, string minCharValue) 
        {
            // Paramater length check
            foreach (char c in length)
            {
                if (!char.IsDigit(c)) return false;
            }

            // Add length to length count.
            lengthCount = int.Parse(length);

            // Parameter minCharValue check
            foreach (char c in minCharValue)
            {
                // Validate characters
                if (!c.Equals('l') && !c.Equals('L') && !c.Equals('d') && !c.Equals('s')) return false;

                switch (c) 
                {
                    case 'l': lowercaseCount++; continue;
                    case 'L': uppercaseCount++; continue;
                    case 's': symbolCount++; continue;
                    case 'd': digitCount++; continue;
                }
            }

            // Output amount.
            //Console.WriteLine("\nl: " + lowercaseCount + "\nL: " + uppercaseCount + "\nd: " + digitCount + "\nd: " + symbolCount);

            return true;
        }

        static string GeneratePassword(int length, int lowerCase, int upperCase, int digits, int symbols)
        {
            // Min character range.
            int minChar = lowerCase + upperCase + digits + symbols;

            // If 0 passed in user can specify completly without any randomness.
            if (length == 0) length = minChar;

            if (minChar > length)
            {
                return "-> Operation failed: Minimum requirements exceeded total length.";
            }
            else
            {
                // Generate pools.
                int poolSize = 64;
                string[] lowerCasePool = GenerateUnicode(lowerCaseStart, lowerCaseEnd, poolSize);
                string[] upperCasePool = GenerateUnicode(upperCaseStart, upperCaseEnd, poolSize);
                string[] symbolPool = GenerateUnicode(symbolStart, symbolEnd, poolSize);
                string[] digitPool = GenerateUnicode(digitStart, digitEnd, poolSize);

                // Remaining types that needs to be generated.
                List<string> poolTypes = new List<string>();    // User input pool

                // Add ranges to base pool
                poolTypes.AddRange(GeneratePoolType(length - minChar, "random"));
                poolTypes.AddRange(GeneratePoolType(lowerCase, "lowercase"));
                poolTypes.AddRange(GeneratePoolType(upperCase, "uppercase"));
                poolTypes.AddRange(GeneratePoolType(symbols, "symbols"));
                poolTypes.AddRange(GeneratePoolType(digits, "digits"));

                // Password assembly.
                string result = "";

                string[] typeArray = { "lowercase", "uppercase", "symbols", "digits" };
                int count = 0;

                // Loop through and deal with each category randomly.
                while (count < length)
                {
                    // Fetch a random pool type
                    int randomNumber = random.Next(0, poolTypes.Count);
                    string type = poolTypes[randomNumber];
                    poolTypes.RemoveAt(randomNumber);

                    if (type == "random")
                    {
                        // Replace type with a random one.
                        type = typeArray[random.Next(0, typeArray.Length - 1)];
                    }

                    // Take a pool type and assign a value to result.
                    switch (type)
                    {
                        case "lowercase": result += lowerCasePool[random.Next(0, lowerCasePool.Length - 1)]; break;
                        case "uppercase": result += upperCasePool[random.Next(0, upperCasePool.Length - 1)]; break;
                        case "symbols": result += symbolPool[random.Next(0, symbolPool.Length - 1)]; break;
                        case "digits": result += digitPool[random.Next(0, digitPool.Length - 1)]; break;
                    }

                    count++;
                }

                // Take a pool type and assign a value to result.
                return result;
            }
        }

        // Generates a random unicode string.
        static string[] GenerateUnicode(int start, int end, int length)
        {
            Random random = new Random();
            string[] result = new string[length];

            for (int i = 0; i < length; i++)
            {
                result[i] += (char)random.Next(start, end);
            }

            return result;
        }

        // Generates an array with pool times times its size.
        static string[] GeneratePoolType(int size, string name)
        {
            string[] result = new string[size];

            for (int i = 0; i < size; i++)
            {
                result[i] = name;
            }

            return result;
        }
    }
}
