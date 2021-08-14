using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TradeMap.Localization
{
    public class TextLocalizer : ITextLocalizer
    {
        private readonly Dictionary<string, string> _dataBase = new();


        public TextLocalizer(string[] filePath)
        {
            foreach (var path in filePath)
            {
                var lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    var kv = line.Split(':', 2);
                    if (kv.Length == 2)
                    {
                        _dataBase.Add(kv[0], kv[1]);
                    }
                }
            }
        }

        public string Expand(string text, Dictionary<string, object>? variables)
        {
            StringBuilder textBuilder = new(text);
            int start = -1;
            while (start < textBuilder.Length - 1)
            {
                start++;
                if (textBuilder[start] == '[')
                {
                    int end = start;
                    while (end < textBuilder.Length - 1)
                    {
                        end++;
                        if (textBuilder[end] == ']')
                        {
                            string key = textBuilder.ToString(start + 1, end - start - 1);
                            if (_dataBase.TryGetValue(key, out string? str))
                            {
                                textBuilder.Remove(start, end - start + 1);
                                textBuilder.Insert(start, str);
                                start = -1;
                            }
                            end = textBuilder.Length;
                        }
                    }
                }
            }

            if (variables != null)
            {
                start = -1;
                while (start < textBuilder.Length)
                {
                    start++;
                    if (textBuilder[start] == '{')
                    {
                        int end = start;
                        while (end < textBuilder.Length)
                        {
                            end++;
                            if (textBuilder[end] == '}')
                            {
                                string key = textBuilder.ToString(start + 1, end - start - 1);
                                if (variables.TryGetValue(key, out object? obj))
                                {
                                    textBuilder.Remove(start, end - start + 1);
                                    textBuilder.Insert(start, obj);
                                }
                                end = textBuilder.Length;
                            }
                        }
                    }
                }
            }

            return textBuilder.ToString();
        }

        public string Expand(string text)
        {
            return Expand(text, null);
        }
    }
}