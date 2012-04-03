using System;
using System.Collections.Generic;

namespace MiniHawraman.Components
{
    public static class RandomCodeGenerator
    {
        public static string GenerateCode()
        {
            List<char> chars = new List<char>();

            chars.AddRange(GetLowerCaseChars(4));
            chars.AddRange(GetNumericChars(4));

            return GenerateCodeFromList(chars);
        }

        private static List<char> GetLowerCaseChars(int count)
        {
            List<char> result = new List<char>();

            Random random = new Random();

            for (int index = 0; index < count; index++)
            {
                result.Add(Char.ToLower(Convert.ToChar(random.Next(97, 122))));
            }

            return result;
        }

        private static List<char> GetNumericChars(int count)
        {
            List<char> result = new List<char>();

            Random random = new Random();

            for (int index = 0; index < count; index++)
            {
                result.Add(Convert.ToChar(random.Next(0, 9).ToString()));
            }

            return result;
        }

        private static string GenerateCodeFromList(List<char> chars)
        {
            string result = string.Empty;

            Random random = new Random();

            while (chars.Count > 0)
            {
                int randomIndex = random.Next(0, chars.Count);
                result += chars[randomIndex];
                chars.RemoveAt(randomIndex);
            }

            return result;
        }
    }
}