﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolGood.HtmlExtract.HtmlAgilityPack
{
    public class Token
    {
        public string Filter { get; set; }
        public IList<Token> SubTokens { get; set; }

        public Token(string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentNullException(nameof(word));

            var tokens = SplitTokens(word).ToList();

            Filter = tokens.First();
            SubTokens = tokens.Skip(1).Select(i => new Token(i)).ToList();
        }

        private static IList<string> SplitTokens(string token)
        {
            Func<char, bool> isNameToken = (c) => char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '*';
            var rt = new List<string>();
           
            int start = 0;
            bool isPrefix = true;
            bool isOpeningBracket = false;
            char closeBracket = '\0';
            for (int i = 0; i < token.Length; i++)
            {
                if (isOpeningBracket)
                {
                    if (token[i] == closeBracket)
                    {
                        isOpeningBracket = false;
                        isPrefix = true;
                        rt.Add(token.Substring(start, i - start + 1));
                        start = i + 1;
                    }

                    continue;
                }

                if (token[i] == '(')
                {
                    closeBracket = ')';
                    isOpeningBracket = true;
                }
                else if (token[i] == '[')
                {
                    closeBracket = ']';
                    if (i != start)
                    {
                        rt.Add(token.Substring(start, i - start));
                        start = i;
                    }
                    isOpeningBracket = true;
                }
                else if (i == token.Length - 1)
                {
                    rt.Add(token.Substring(start, i - start + 1));
                }
                else if (!isNameToken(token[i]) && !isPrefix)
                {
                    rt.Add(token.Substring(start, i - start));
                    start = i;
                }
                else if (isNameToken(token[i]))
                    isPrefix = false;
            }

            return rt;
        }
    }
}