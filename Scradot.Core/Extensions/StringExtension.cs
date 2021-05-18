using Scradot.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Scradot.Core.Extensions
{
    public static class StringExtension
    {
        public static Regex _clearSpaceRegex = new(@"\s+");
        public static string Truncate(this string value, int start, int end)
        {
            if (start >= end && start < value.Length) return value[start..];
            if (value.Length > end) return value[start..end];
            if (value.Length > start) return value[start..];
            return value;
        }

        public static string RemoveAccents(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string ReFirst(this string text, string pattern, RegexOptions regexOptions = RegexOptions.Multiline)
        {
            if (text is null) return null;
            var value = Regex.Match(text, pattern, regexOptions).Value;
            if (string.IsNullOrEmpty(value)) return null;
            return value;
        }

        public static string ClearSpaces(this string text)
        {
            if (text is null) return null;
            return _clearSpaceRegex.Replace(text, " ").Trim();
        }
        public static T Deserialize<T>(this string text) => JsonSerializer.Deserialize<T>(text, JsonConfig.Options);
        public static string SeparateByDash(this string text) => string.Concat(text.Select((x, j) => j > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString()));
    }
}
