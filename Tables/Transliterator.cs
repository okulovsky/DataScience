using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    public static class Transliterator
    {
        static string[] lat_up = { "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh", "Ts", "Ch", "Sh", "Shch", "\"", "Y", "'", "E", "Yu", "Ya" };
        static string[] lat_low = { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "\"", "y", "'", "e", "yu", "ya" };
        static string[] rus_up = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
        static string[] rus_low = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };

        static string badDelimiters = " ,";

        static Dictionary<char, string> Dict = new Dictionary<char, string>();
        static Transliterator()
        {
            for (int i = 0; i < 33; i++)
            {
                Dict[rus_up[i][0]] = lat_up[i];
                Dict[rus_low[i][0]] = lat_low[i];
            }
        }

        public static string Transliterate(string str)
        {
            if (str == null) return null;
            var b = new StringBuilder();
            foreach (var e in str)
            {
                if (Dict.ContainsKey(e))
                    b.Append(Dict[e]);
                else if (badDelimiters.Contains(e)) { }
                else b.Append(e);
            }
            return b.ToString();
        }
    }
}
