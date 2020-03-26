using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class Program
    {
        static Dictionary<char, int> dict = new Dictionary<char, int>()
        {
            {'А', 10}, {'Б', 11}, {'В', 12},
            {'Г', 13}, {'Д', 14}, {'Е', 15},
            {'Ж', 16}, {'З', 17}, {'И', 18},
            {'Й', 19}, {'К', 20}, {'Л', 21},
            {'М', 22}, {'Н', 23}, {'О', 24},
            {'П', 25}, {'Р', 26}, {'С', 27},
            {'Т', 28}, {'У', 29}, {'Ф', 30},
            {'Х', 31}, {'Ц', 32}, {'Ч', 33},
            {'Ш', 34}, {'Щ', 35}, {'Ъ', 36},
            {'Ы', 37}, {'Ь', 38}, {'Э', 39},
            {'Ю', 40}, {'Я', 41}, {' ', 99}
        };

        static int p = 107;
        static int q = 241;
        static int n = p * q;
        static int e, d;

        static void Main(string[] args)
        {
            Console.WriteLine("Выберите ключ: ");
            Dictionary<int, int> keys = generateKeys();
            int count = 1;
            foreach (var pair in keys)
            {
                Console.WriteLine(count + ". e = " + pair.Key + "; d = " + pair.Value);
                count++;
            }
            string c = Console.ReadLine();
            if (c == "1")
            {
                e = keys.ElementAt(0).Key;
                d = keys.ElementAt(0).Value;
            }
            else if (c == "2")
            {
                e = keys.ElementAt(1).Key;
                d = keys.ElementAt(1).Value;
            }
            else
            {
                e = keys.ElementAt(2).Key;
                d = keys.ElementAt(2).Value;
            }
            Console.Write("Введите сообщение: ");
            int[] enc = Encrypt(Console.ReadLine());
            Console.WriteLine("Зашифрованное сообщение:");
            foreach (int i in enc) Console.Write(i + " ");
            Console.WriteLine();
            string dec = Decrypt(enc);
            Console.WriteLine("Расшифрованное сообщение: " + dec);
            Console.ReadLine();
        }

        static int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b) a %= b;
                else b %= a;
            }
            return a == 0 ? b : a;
        }

        static bool isCoprime(int a, int b)
        {
            return GCD(a, b) == 1 ? true : false;
        }

        static string toNumbers(string s)
        {
            s = s.ToUpper();
            return dict.Aggregate(s, (result, pair) => result.Replace(pair.Key.ToString(), pair.Value.ToString()));
        }

        static string toLetters(string s)
        {
            string result = "";
            for (int i = 0; i <= s.Length - 2; i += 2)
            {
                int num = Convert.ToInt32(s.Substring(i, 2));
                result += dict.FirstOrDefault(x => x.Value == num).Key.ToString();
            }
            return result;
        }

        static int[] toNumberBlocks(string str)
        {
            List<int> array = new List<int>();
            str = toNumbers(str);
            while (str.Length != 0)
            {
                for (int i = str.Length - 1; i >= 0; i--)
                {
                    if (str.Length <= n.ToString().Length && Convert.ToInt32(str) < n)
                    {
                        array.Add(Convert.ToInt32(str));
                        str = string.Empty;
                        break;
                    }
                    else
                    {
                        if (Convert.ToInt32(str.Substring(i, str.Length - i)) >= n)
                        {
                            if (Convert.ToInt32(str.Substring(i + 1, str.Length - i - 1)) == 0)
                            {
                                array.Add(Convert.ToInt32(str.Substring(i + 2, str.Length - i + 1)));
                                str = str.Remove(i + 2, str.Length - i - 1);
                                break;
                            }
                            else
                            {
                                array.Add(Convert.ToInt32(str.Substring(i + 2, str.Length - i - 2)));
                                str = str.Remove(i + 2, str.Length - i - 2);
                                break;
                            }
                        }
                    }

                }
            }
            array.Reverse();
            return array.ToArray();
        }

        static Dictionary<int, int> generateKeys()
        {
            Dictionary<int, int> pairs = new Dictionary<int, int>();
            int ed = (p - 1) * (q - 1) + 1;
            while (true)
            {
                for (int i = 2; i < ed; i++)
                {
                    if (ed % i == 0)
                    {
                        int e = i;
                        int d = ed / i;
                        if (isCoprime(e, d)) pairs.Add(e, d);
                        if (pairs.Count == 3) return pairs;
                    }
                }
                ed += (p - 1) * (q - 1);
            }
        }

        static int[] Encrypt(string s)
        {
            int[] to_enc = toNumberBlocks(s);
            List<int> result = new List<int>();
            foreach (int i in to_enc)
            {
                int num = (int)BigInteger.ModPow(i, e, n);
                result.Add(num);
            }
            return result.ToArray();
        }

        static string Decrypt(int[] to_dec)
        {
            string result = "";
            foreach (int i in to_dec)
            {
                int num = (int)BigInteger.ModPow(i, d, n);
                result += num.ToString();
            }
            return toLetters(result);
        }
    }
}
