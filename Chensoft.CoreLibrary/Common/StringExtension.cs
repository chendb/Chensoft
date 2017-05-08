
using System;
using System.Collections.Generic;
using System.Text;

namespace Chensoft.Common
{
    public static class StringExtension
    {
        public static readonly string InvalidCharacters = "`~!@#$%^&*()-+={}[]|\\/?:;'\"\t\r\n ";

        public static int GetStringHashCode(string text)
        {
            if (text == null || text.Length < 1)
                return 0;

            unsafe
            {
                fixed (char* src = text)
                {
                    int hash1 = 5381;
                    int hash2 = hash1;

                    int c;
                    char* s = src;
                    while ((c = s[0]) != 0)
                    {
                        hash1 = ((hash1 << 5) + hash1) ^ c;
                        c = s[1];

                        if (c == 0)
                            break;

                        hash2 = ((hash2 << 5) + hash2) ^ c;
                        s += 2;
                    }

                    return hash1 + (hash2 * 1566083941);
                }
            }
        }

        public static bool ContainsCharacters(this string text, string characters)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(characters))
                return false;

            foreach (char character in characters)
            {
                foreach (char item in text)
                {
                    if (character == item)
                        return true;
                }
            }

            return false;
        }

        public static string RemoveCharacters(this string text, string invalidCharacters)
        {
            return RemoveCharacters(text, invalidCharacters, 0);
        }

        public static string RemoveCharacters(this string text, string invalidCharacters, int startIndex)
        {
            return RemoveCharacters(text, invalidCharacters, startIndex, -1);
        }

        public static string RemoveCharacters(this string text, string invalidCharacters, int startIndex, int count)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(invalidCharacters))
                return text;

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex");

            if (count < 1)
                count = invalidCharacters.Length - startIndex;

            if (startIndex + count > invalidCharacters.Length)
                throw new ArgumentOutOfRangeException("count");

            string result = text;

            for (int i = startIndex; i < startIndex + count; i++)
                result = text.Replace(invalidCharacters[i].ToString(), "");

            return result;
        }

        public static bool IsNullOrSpace(this string text)
        {
            string temp;
            return IsNullOrSpace(text, out temp);
        }

        public static bool IsNullOrSpace(this string text, out string trimmedString)
        {
            bool result = string.IsNullOrEmpty(text);

            if (result)
            {
                trimmedString = text;
                return result;
            }

            trimmedString = text.Trim();
            return (trimmedString.Length == 0);
        }

        public static string TrimString(this string text, string trimString)
        {
            return TrimString(text, trimString, StringComparison.OrdinalIgnoreCase);
        }

        public static string TrimString(this string text, string trimString, StringComparison comparisonType)
        {
            return TrimStringEnd(
                TrimStringStart(text, trimString, comparisonType),
                trimString,
                comparisonType);
        }

        public static string TrimString(this string text, string prefix, string suffix)
        {
            return TrimString(text, prefix, suffix, StringComparison.OrdinalIgnoreCase);
        }

        public static string TrimString(this string text, string prefix, string suffix, StringComparison comparisonType)
        {
            return text
                    .TrimStringStart(prefix, comparisonType)
                    .TrimStringEnd(suffix, comparisonType);
        }

        public static string TrimStringEnd(this string text, string trimString)
        {
            return TrimStringEnd(text, trimString, StringComparison.OrdinalIgnoreCase);
        }

        public static string TrimStringEnd(this string text, string trimString, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(trimString))
                return text;

            while (text.EndsWith(trimString, comparisonType))
                text = text.Remove(text.Length - trimString.Length);

            return text;
        }

        public static string TrimStringStart(this string text, string trimString)
        {
            return TrimStringStart(text, trimString, StringComparison.OrdinalIgnoreCase);
        }

        public static string TrimStringStart(this string text, string trimString, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(trimString))
                return text;

            while (text.StartsWith(trimString, comparisonType))
                text = text.Remove(0, trimString.Length);

            return text;
        }

        public static bool In(this string text, IEnumerable<string> collection, StringComparison comparisonType)
        {
            if (collection == null)
                return false;

            foreach (var item in collection)
            {
                if (string.Equals(item, text, comparisonType))
                    return true;
            }

            return false;
        }
    }
}
