using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftWx.Dna {
    /// <summary>
    /// Extension methods for strings.
    /// </summary>
    public static class StringExtensions {
        /// <summary>
        /// Similar to String.Split, but a bit faster.
        /// </summary>
        /// <param name="text">The string to be split.</param>
        /// <param name="delimiter">The delimiter character that denotes where to split.</param>
        /// <param name="maxWords">The maximum number of string parts that are wanted.</param>
        /// <returns>An array of strings split from the source text string.</returns>
        public static string[] FastSplit(this string text, char delimiter, int maxWords) {
            string[] result = new string[maxWords];
            FastSplit(text, delimiter, result);
            return result;
        }

        /// <summary>
        /// Similar to String.Split but a bit faster.
        /// </summary>
        /// <param name="text">The string to be split.</param>
        /// <param name="delimiter">The delimiter character that denotes where to split.</param>
        /// <param name="result">String array that will receive the strings split from 
        /// the source text string. The Length of the array will be used as a limit on
        /// the number of strings that will be split from the source.</param>
        public static int FastSplit(this string text, char delimiter, string[] result) {
            if (result == null) throw new ArgumentNullException("result array cannot be null.");

            int max = result.Length;
            int len, startPos, pos, idx;
            idx = pos = 0;
            if (text == null) text = "";
            while ((idx < max) && (pos < text.Length)) {
                startPos = pos;
                pos = text.IndexOf(delimiter, pos, text.Length - pos);
                if (pos >= 0) {
                    len = pos - startPos;
                    pos++;
                    result[idx++] = (len != 0) ? text.Substring(startPos, len) : null;
                } else {
                    result[idx++] = text.Substring(startPos);
                    break;
                }
            }
            int colCount = idx;
            for (; idx < max; ) result[idx++] = null;
            return colCount;
        }
    }
}
