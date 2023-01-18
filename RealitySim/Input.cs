using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealitySim
{
    static class Input
    {
        public static string GetInput(string prompt)
        {
            string? readLine = null;
            while (readLine == null)
            {
                Console.WriteLine(prompt);
                readLine = Console.ReadLine();
            }
            return readLine.Trim().ToUpper();
        }

        public static string GetInput(string prompt, string[] validInput)
        {
            string? input = null;

            while (input == null)
            {
                input = GetInput(prompt);
                if (!validInput.Contains(input))
                {
                    input = null;
                }
            }
            return input;
        }
    }
}
