using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToolGood.Bedrock.Utils
{
    public static class StringSimilarityUtil
    {
        public static int LevenshteinDistance(string source, string target)
        {
            int columnSize = source.Length;
            int rowSize = target.Length;
            if (columnSize == 0) {
                return rowSize;
            }
            if (rowSize == 0) {
                return columnSize;
            }

            int[,] matrix = new int[rowSize + 1, columnSize + 1];
            for (int i = 0; i <= columnSize; i++) {
                matrix[0, i] = i;
            }
            for (int j = 1; j <= rowSize; j++) {
                matrix[j, 0] = j;
            }

            for (int i = 0; i < rowSize; i++) {
                for (int j = 0; j < columnSize; j++) {
                    int sign;
                    if (source[j].Equals(target[i]))
                        sign = 0;
                    else
                        sign = 1;
                    matrix[i + 1, j + 1] = Math.Min(Math.Min(matrix[i, j] + sign, matrix[i + 1, j] + 1), matrix[i, j + 1] + 1);
                }
            }

            return matrix[rowSize, columnSize];
        }

        public static double StringSimilarity(string source, string target)
        {
            int distance = LevenshteinDistance(source, target);
            double maxLength = Math.Max(source.Length, target.Length);

            return (maxLength - distance) / maxLength;
        }

        /// <summary>
        /// Compares the two strings based on letter pair matches
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns>The percentage match from 0.0 to 1.0 where 1.0 is 100%</returns>
        public static double CompareStrings(string str1, string str2)
        {
            List<string> pairs1 = WordLetterPairs(str1.ToUpper());
            List<string> pairs2 = WordLetterPairs(str2.ToUpper());

            int intersection = 0;
            int union = pairs1.Count + pairs2.Count;

            for (int i = 0; i < pairs1.Count; i++) {
                for (int j = 0; j < pairs2.Count; j++) {
                    if (pairs1[i] == pairs2[j]) {
                        intersection++;
                        pairs2.RemoveAt(j);//Must remove the match to prevent "GGGG" from appearing to match "GG" with 100% success
                        break;
                    }
                }
            }
            return (2.0 * intersection) / union;
        }

        /// <summary>
        /// Gets all letter pairs for each
        /// individual word in the string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static List<string> WordLetterPairs(string str)
        {
            List<string> AllPairs = new List<string>();

            // Tokenize the string and put the tokens/words into an array
            string[] Words = Regex.Split(str, @"\s");

            // For each word
            for (int w = 0; w < Words.Length; w++) {
                if (!string.IsNullOrEmpty(Words[w])) {
                    // Find the pairs of characters
                    String[] PairsInWord = LetterPairs(Words[w]);

                    for (int p = 0; p < PairsInWord.Length; p++) {
                        AllPairs.Add(PairsInWord[p]);
                    }
                }
            }
            return AllPairs;
        }

        /// <summary>
        /// Generates an array containing every
        /// two consecutive letters in the input string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string[] LetterPairs(string str)
        {
            int numPairs = str.Length - 1;

            string[] pairs = new string[numPairs];

            for (int i = 0; i < numPairs; i++) {
                pairs[i] = str.Substring(i, 2);
            }
            return pairs;
        }




    }
}
