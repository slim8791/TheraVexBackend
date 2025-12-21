using System;
using System.Text;

namespace TheravexBackend.Helpers
{
    public static class FrenchNumberToWords
    {
        private static readonly string[] Units = {
            "zéro","un","deux","trois","quatre","cinq","six","sept","huit","neuf",
            "dix","onze","douze","treize","quatorze","quinze","seize"
        };

        private static string TensToWords(int n)
        {
            if (n < 17) return Units[n];
            if (n < 20) return "dix-" + Units[n - 10]; // 17-19
            if (n < 70)
            {
                int tens = (n / 10) * 10;
                int unit = n % 10;
                string tensWord = tens switch
                {
                    20 => "vingt",
                    30 => "trente",
                    40 => "quarante",
                    50 => "cinquante",
                    60 => "soixante",
                    _ => ""
                };

                if (unit == 0) return tensWord;
                if (unit == 1) return tensWord + " et un";
                return tensWord + "-" + Units[unit];
            }

            if (n < 80) // 70..79 => soixante-[10..19]
            {
                if (n == 71) return "soixante et onze";
                return "soixante-" + TensToWords(n - 60);
            }

            // 80..99
            if (n == 80) return "quatre-vingts";
            return "quatre-vingt-" + TensToWords(n - 80);
        }

        private static string LessThanOneThousandToWords(int n)
        {
            if (n < 100) return TensToWords(n);
            int hundreds = n / 100;
            int rest = n % 100;
            var sb = new StringBuilder();

            if (hundreds == 1)
                sb.Append("cent");
            else
                sb.Append(Units[hundreds] + " cent");

            if (rest == 0)
            {
                // plural 'cents' when exact hundreds and hundreds > 1
                if (hundreds > 1)
                    sb.Append("s");
                return sb.ToString();
            }

            sb.Append(" " + TensToWords(rest));
            return sb.ToString();
        }

        public static string ToWords(decimal amount)
        {
            if (amount < 0) return "moins " + ToWords(Math.Abs(amount));

            long intPart = (long)Math.Floor(amount);
            int millimes = (int)Math.Round((amount - intPart) * 1000); // three decimals (millimes)

            if (millimes == 1000)
            {
                intPart += 1;
                millimes = 0;
            }

            if (intPart == 0 && millimes == 0)
                return "Zéro dinar";

            var parts = new StringBuilder();

            if (intPart > 0)
            {
                long remainder = intPart;

                long milliards = remainder / 1_000_000_000;
                if (milliards > 0)
                {
                    parts.Append(WriteGroup((int)milliards) + (milliards > 1 ? " milliards" : " milliard"));
                    remainder %= 1_000_000_000;
                }

                long millions = remainder / 1_000_000;
                if (millions > 0)
                {
                    if (parts.Length > 0) parts.Append(" ");
                    parts.Append(WriteGroup((int)millions) + (millions > 1 ? " millions" : " million"));
                    remainder %= 1_000_000;
                }

                long milliers = remainder / 1000;
                if (milliers > 0)
                {
                    if (parts.Length > 0) parts.Append(" ");
                    if (milliers == 1)
                        parts.Append("mille");
                    else
                        parts.Append(WriteGroup((int)milliers) + " mille");
                    remainder %= 1000;
                }

                if (remainder > 0)
                {
                    if (parts.Length > 0) parts.Append(" ");
                    parts.Append(WriteGroup((int)remainder));
                }

                // dinar / dinars
                parts.Append(intPart > 1 ? " dinars" : " dinar");
            }

            if (millimes > 0)
            {
                if (parts.Length > 0) parts.Append(" et ");
                parts.Append(WriteGroup(millimes));
                parts.Append(millimes > 1 ? " millimes" : " millime");
            }

            // Capitalize first letter to match existing presentation
            if (parts.Length > 0)
            {
                parts[0] = char.ToUpper(parts[0]);
            }

            return parts.ToString();
        }

        private static string WriteGroup(int number)
        {
            return LessThanOneThousandToWords(number);
        }
    }
}