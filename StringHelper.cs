using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Net;
using System.Xml;
using System.Linq;
using System.Globalization;

namespace CastelloBranco.StringExtensions;

public static partial class StringHelper
{
    public static string OnlyAsciiDigits(this string str)
    {
        StringBuilder sb = new(str.Length);

        foreach (char c in str)
        {
            if (char.IsAsciiDigit(c))
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    public static string OnlyAsciiLether(this string str)
    {
        StringBuilder sb = new(str.Length);

        foreach (char c in str)
        {
            if (char.IsAsciiLetter(c))
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    public static string OnlyAsciiLetherOrDigits(this string str)
    {
        StringBuilder sb = new(str.Length);

        foreach (char c in str)
        {
            if (char.IsAsciiLetterOrDigit(c))
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    public static string OnlyAllowed (this string str, string allowed_chars)
    {
        StringBuilder sb = new(str.Length);

        foreach (char c in str)
        {
            if (allowed_chars.Contains(c))
            {
                sb.Append (c);
            }
        }

        return sb.ToString();
    }

    private static readonly Dictionary<char, string> _Diacritics = new()
    {
        { 'ä', "ae" }, { 'æ', "ae" }, { 'ǽ', "ae" },
        { 'ö', "oe" }, { 'œ', "oe" },
        { 'ü', "ue" },
        { 'Ä', "Ae" },
        { 'Ü', "Ue" },
        { 'Ö', "Oe" },
        { 'À', "A" }, { 'Á', "A" }, { 'Â', "A" }, { 'Ã', "A" }, { 'Ä', "A" }, { 'Å', "A" }, { 'Ǻ', "A" }, { 'Ā', "A" }, { 'Ă', "A" }, { 'Ą', "A" }, { 'Ǎ', "A" }, { 'Α', "A" }, { 'Ά', "A" }, { 'Ả', "A" }, { 'Ạ', "A" }, { 'Ầ', "A" }, { 'Ẫ', "A" }, { 'Ẩ', "A" }, { 'Ậ', "A" }, { 'Ằ', "A" }, { 'Ắ', "A" }, { 'Ẵ', "A" }, { 'Ẳ', "A" }, { 'Ặ', "A" }, { 'А', "A" },
        { 'à', "a" }, { 'á', "a" }, { 'â', "a" }, { 'ã', "a" }, { 'å', "a" }, { 'ǻ', "a" }, { 'ā', "a" }, { 'ă', "a" }, { 'ą', "a" }, { 'ǎ', "a" }, { 'ª', "a" }, { 'α', "a" }, { 'ά', "a" }, { 'ả', "a" }, { 'ạ', "a" }, { 'ầ', "a" }, { 'ấ', "a" }, { 'ẫ', "a" }, { 'ẩ', "a" }, { 'ậ', "a" }, { 'ằ', "a" }, { 'ắ', "a" }, { 'ẵ', "a" }, { 'ẳ', "a" }, { 'ặ', "a" }, { 'а', "a" },
        { 'Б', "B" }, { 'б', "b" },
        { 'Ç', "C" }, { 'Ć', "C" }, { 'Ĉ', "C" }, { 'Ċ', "C" }, { 'Č', "C" },
        { 'ç', "c" }, { 'ć', "c" }, { 'ĉ', "c" }, { 'ċ', "c" }, { 'č', "c" },
        { 'Д', "D" }, { 'д', "d" },
        { 'Ð', "Dj" }, { 'Ď', "Dj" }, { 'Đ', "Dj" }, { 'Δ', "Dj" },
        { 'ð', "dj" }, { 'ď', "dj" }, { 'đ', "dj" }, { 'δ', "dj" },
        { 'È', "E" }, { 'É', "E" }, { 'Ê', "E" }, { 'Ë', "E" }, { 'Ē', "E" }, { 'Ĕ', "E" }, { 'Ė', "E" }, { 'Ę', "E" }, { 'Ě', "E" }, { 'Ε', "E" }, { 'Έ', "E" }, { 'Ẽ', "E" }, { 'Ẻ', "E" }, { 'Ẹ', "E" }, { 'Ề', "E" }, { 'Ế', "E" }, { 'Ễ', "E" }, { 'Ể', "E" }, { 'Ệ', "E" }, { 'Е', "E" }, { 'Э', "E" },
        { 'è', "e" }, { 'é', "e" }, { 'ê', "e" }, { 'ë', "e" }, { 'ē', "e" }, { 'ĕ', "e" }, { 'ė', "e" }, { 'ę', "e" }, { 'ě', "e" }, { 'έ', "e" }, { 'ε', "e" }, { 'ẽ', "e" }, { 'ẻ', "e" }, { 'ẹ', "e" }, { 'ề', "e" }, { 'ế', "e" }, { 'ễ', "e" }, { 'ể', "e" }, { 'ệ', "e" }, { 'е', "e" }, { 'э', "e" },
        { 'Ф', "F" }, { 'ф', "f" },
        { 'Ĝ', "G" }, { 'Ğ', "G" }, { 'Ġ', "G" }, { 'Ģ', "G" }, { 'Γ', "G" }, { 'Г', "G" }, { 'Ґ', "G" },
        { 'ĝ', "g" }, { 'ğ', "g" }, { 'ġ', "g" }, { 'ģ', "g" }, { 'γ', "g" }, { 'г', "g" }, { 'ґ', "g" },
        { 'Ĥ', "H" }, { 'Ħ', "H" },
        { 'ĥ', "h" }, { 'ħ', "h" },
        { 'Ì', "I" }, { 'Í', "I" }, { 'Î', "I" }, { 'Ï', "I" }, { 'Ĩ', "I" }, { 'Ī', "I" }, { 'Ĭ', "I" }, { 'Ǐ', "I" }, { 'Į', "I" }, { 'İ', "I" }, { 'Η', "I" }, { 'Ή', "I" }, { 'Ί', "I" }, { 'Ι', "I" }, { 'Ϊ', "I" }, { 'Ỉ', "I" }, { 'Ị', "I" }, { 'И', "I" }, { 'Ы', "I" },
        { 'ì', "i" }, { 'í', "i" }, { 'î', "i" }, { 'ï', "i" }, { 'ĩ', "i" }, { 'ī', "i" }, { 'ĭ', "i" }, { 'ǐ', "i" }, { 'į', "i" }, { 'ı', "i" }, { 'η', "i" }, { 'ή', "i" }, { 'ί', "i" }, { 'ι', "i" }, { 'ϊ', "i" }, { 'ỉ', "i" }, { 'ị', "i" }, { 'и', "i" }, { 'ы', "i" },
        { 'Ĵ', "J" }, { 'ĵ', "j" },
        { 'Ķ', "K" }, { 'К', "K" }, { 'Κ', "K" },
        { 'ķ', "k" }, { 'к', "k" }, { 'κ', "k" },
        { 'Ĺ', "L" }, { 'Ļ', "L" }, { 'Ľ', "L" }, { 'Ŀ', "L" }, { 'Ł', "L" }, { 'Λ', "L" }, { 'Л', "L" },
        { 'ĺ', "l" }, { 'ļ', "l" }, { 'ľ', "l" }, { 'ŀ', "l" }, { 'ł', "l" }, { 'λ', "l" }, { 'л', "l" },
        { 'М', "M" }, { 'м', "m" },
        { 'Ñ', "N" }, { 'Ń', "N" }, { 'Ņ', "N" }, { 'Ň', "N" }, { 'Ν', "N" }, { 'Н', "N" },
        { 'ñ', "n" }, { 'ń', "n" }, { 'ņ', "n" }, { 'ň', "n" }, { 'ŉ', "n" }, { 'ν', "n" }, { 'н', "n" },
        { 'Ò', "O" }, { 'Ó', "O" }, { 'Ô', "O" }, { 'Õ', "O" }, { 'Ō', "O" }, { 'Ŏ', "O" }, { 'Ǒ', "O" }, { 'Ő', "O" }, { 'Ơ', "O" }, { 'Ø', "O" }, { 'Ǿ', "O" }, { 'Ο', "O" }, { 'Ό', "O" }, { 'Ω', "O" }, { 'Ώ', "O" }, { 'Ỏ', "O" }, { 'Ọ', "O" }, { 'Ồ', "O" }, { 'Ố', "O" }, { 'Ỗ', "O" }, { 'Ổ', "O" }, { 'Ộ', "O" }, { 'Ờ', "O" }, { 'Ớ', "O" }, { 'Ỡ', "O" }, { 'Ở', "O" }, { 'Ợ', "O" }, { 'О', "O" },
        { 'ò', "o" }, { 'ó', "o" }, { 'ô', "o" }, { 'õ', "o" }, { 'ō', "o" }, { 'ŏ', "o" }, { 'ǒ', "o" }, { 'ő', "o" }, { 'ơ', "o" }, { 'ø', "o" }, { 'ǿ', "o" }, { 'º', "o" }, { 'ο', "o" }, { 'ό', "o" }, { 'ω', "o" }, { 'ώ', "o" }, { 'ỏ', "o" }, { 'ọ', "o" }, { 'ồ', "o" }, { 'ố', "o" }, { 'ỗ', "o" }, { 'ổ', "o" }, { 'ộ', "o" }, { 'ờ', "o" }, { 'ớ', "o" }, { 'ỡ', "o" }, { 'ở', "o" }, { 'ợ', "o" }, { 'о', "o" },
        { 'П', "P" }, { 'п', "p" },
        { 'Ŕ', "R" }, { 'Ŗ', "R" }, { 'Ř', "R" }, { 'Ρ', "R" }, { 'Р', "R" },
        { 'ŕ', "r" }, { 'ŗ', "r" }, { 'ř', "r" }, { 'ρ', "r" }, { 'р', "r" },
        { 'Ś', "S" }, { 'Ŝ', "S" }, { 'Ş', "S" }, { 'Ș', "S" }, { 'Š', "S" }, { 'Σ', "S" }, { 'С', "S" },
        { 'ś', "s" }, { 'ŝ', "s" }, { 'ş', "s" }, { 'ș', "s" }, { 'š', "s" }, { 'ſ', "s" }, { 'σ', "s" }, { 'ς', "s" }, { 'с', "s" },
        { 'Ț', "T" }, { 'Ţ', "T" }, { 'Ť', "T" }, { 'Ŧ', "T" }, { 'τ', "T" }, { 'Т', "T" },
        { 'ț', "t" }, { 'ţ', "t" }, { 'ť', "t" }, { 'ŧ', "t" }, { 'т', "t" },
        { 'Ù', "U" }, { 'Ú', "U" }, { 'Û', "U" }, { 'Ũ', "U" }, { 'Ū', "U" }, { 'Ŭ', "U" }, { 'ů', "U" }, { 'Ű', "U" }, { 'Ų', "U" }, { 'Ư', "U" }, { 'Ǔ', "U" }, { 'Ǖ', "U" }, { 'Ǘ', "U" }, { 'Ǚ', "U" }, { 'Ǜ', "U" }, { 'Ũ', "U" }, { 'Ủ', "U" }, { 'Ụ', "U" }, { 'Ừ', "U" }, { 'Ứ', "U" }, { 'Ữ', "U" }, { 'Ử', "U" }, { 'Ự', "U" }, { 'У', "U" },
        { 'ù', "u" }, { 'ú', "u" }, { 'û', "u" }, { 'ũ', "u" }, { 'ū', "u" }, { 'ŭ', "u" }, { 'ů', "u" }, { 'ű', "u" }, { 'ų', "u" }, { 'ư', "u" }, { 'ǔ', "u" }, { 'ǖ', "u" }, { 'ǘ', "u" }, { 'ǚ', "u" }, { 'ǜ', "u" }, { 'υ', "u" }, { 'ύ', "u" }, { 'ϋ', "u" }, { 'ủ', "u" }, { 'ụ', "u" }, { 'ừ', "u" }, { 'ứ', "u" }, { 'ữ', "u" }, { 'ử', "u" }, { 'ự', "u" }, { 'у', "u" },
        { 'Ý', "Y" }, { 'Ÿ', "Y" }, { 'Ŷ', "Y" }, { 'Υ', "Y" }, { 'Ύ', "Y" }, { 'Ϋ', "Y" }, { 'Ỳ', "Y" }, { 'Ỹ', "Y" }, { 'Ỷ', "Y" }, { 'Ỵ', "Y" }, { 'Й', "Y" },
        { 'ý', "y" }, { 'ÿ', "y" }, { 'ŷ', "y" }, { 'ỳ', "y" }, { 'ỹ', "y" }, { 'ỷ', "y" }, { 'ỵ', "y" }, { 'й', "y" },
        { 'В', "V" }, { 'в', "v" },
        { 'Ŵ', "W" }, { 'ŵ', "w" },
        { 'Ź', "Z" }, { 'Ż', "Z" }, { 'Ž', "Z" }, { 'Ζ', "Z" }, { 'З', "Z" },
        { 'ź', "z" }, { 'ż', "z" }, { 'ž', "z" }, { 'ζ', "z" }, { 'з', "z" },
        { 'Æ', "AE" }, { 'Ǽ', "AE" },
        { 'ß', "ss" },
        { 'Ĳ', "IJ" }, { 'ĳ', "ij" },
        { 'Œ', "OE" },
        { 'ƒ', "f" },
        { 'ξ', "ks" },
        { 'π', "p" },
        { 'β', "v" },
        { 'μ', "m" },
        { 'ψ', "ps" },
        { 'Ё', "Yo" }, { 'ё', "yo" },
        { 'Є', "Ye" }, { 'є', "ye" },
        { 'Ї', "Yi" },
        { 'Ж', "Zh" }, { 'ж', "zh" },
        { 'Х', "Kh" }, { 'х', "kh" },
        { 'Ц', "Ts" }, { 'ц', "ts" },
        { 'Ч', "Ch" }, { 'ч', "ch" },
        { 'Ш', "Sh" }, { 'ш', "sh" },
        { 'Щ', "Shch" }, { 'щ', "shch" },
        { 'Ъ', "" }, { 'ъ', "" }, { 'Ь', "" }, { 'ь', "" },
        { 'Ю', "Yu" }, { 'ю', "yu" },
        { 'Я', "Ya" }, { 'я', "ya" },
    };

    public static string RemoveDiacritics(this char c)
    {
        return _Diacritics.TryGetValue(c, out string? replacement) ? replacement : c.ToString();
    }

    public static string RemoveDiacritics(this string s)
    {
        if (string.IsNullOrEmpty(s)) return s;

        StringBuilder sb = new StringBuilder(s.Length);

        foreach (char c in s)
        { 
            sb.Append(c.RemoveDiacritics());
        }

        return sb.ToString();
    }

    public static string Base64StringEncode(this string input, System.Text.Encoding? encoding = null)
    {
        byte[] encbuff = (encoding ?? System.Text.Encoding.UTF8).GetBytes(input);

        return Convert.ToBase64String(encbuff);
    }

    public static string Base64StringDecode(this string input, System.Text.Encoding? encoding = null)
    {
        byte[] decbuff = Convert.FromBase64String(input);

        return (encoding ?? System.Text.Encoding.UTF8).GetString(decbuff);
    }

    public static string CaseInsensitivePatternReplace(this string original, string pattern, string replacement)
    {
        int count, position0, position1;

        count = position0 = 0;

        string upperString = original.ToUpper();

        string upperPattern = pattern.ToUpper();

        int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);

        char[] chars = new char[original.Length + Math.Max(0, inc)];

        while ((position1 = upperString.IndexOf(upperPattern,
                                          position0)) != -1)
        {
            for (int i = position0; i < position1; ++i)
                chars[count++] = original[i];

            for (int i = 0; i < replacement.Length; ++i)
                chars[count++] = replacement[i];

            position0 = position1 + pattern.Length;
        }

        if (position0 == 0) return original;

        for (int i = position0; i < original.Length; ++i)
            chars[count++] = original[i];

        return new string(chars, 0, count);
    }

    public static string FilterWords(this string input, params string[] filterWords)
    {
        return StringHelper.FilterWords(input, char.MinValue, filterWords);
    }

    public static string FilterWords(this string input, char mask, params string[] filterWords)
    {
        string stringMask = mask == char.MinValue ? string.Empty : mask.ToString();

        string totalMask = stringMask;

        foreach (string s in filterWords)
        {
            Regex regEx = new(s, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            if (stringMask.Length > 0)
            {
                for (int i = 1; i < s.Length; i++)
                    totalMask += stringMask;
            }

            input = regEx.Replace(input, totalMask);

            totalMask = stringMask;
        }

        return input;
    }

    public static MatchCollection HasWords(this string input, params string[] hasWords)
    {
        StringBuilder sb = new(hasWords.Length + 50);
        //sb.Append("[");

        foreach (string s in hasWords)
        {
            sb.AppendFormat("({0})|", StringHelper.HtmlSpecialEntitiesEncode(s.Trim()));
        }

        string pattern = sb.ToString();

        pattern = pattern.TrimEnd('|'); // +"]";

        Regex regEx = new(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        return regEx.Matches(input);
    }

    public static string HtmlSpecialEntitiesEncode(this string input)
    {
        return WebUtility.HtmlEncode(input);
    }

    public static string HtmlSpecialEntitiesDecode(this string input)
    {
        return WebUtility.HtmlDecode(input);
    }

    public static string MD5String(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("Input cannot be null or empty.", nameof(input));
        }

        byte[] data = MD5.HashData(Encoding.UTF8.GetBytes(input));

        return Convert.ToHexString(data).ToLowerInvariant(); // ToLowerInvariant to match "x2" formatting
    }

    public static bool MD5VerifyString(this string input, string hash)
    {
        string hashOfInput = StringHelper.MD5String(input);

        return string.Equals(hashOfInput, hash, StringComparison.OrdinalIgnoreCase);
    }

    public static string SHA256String(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("Input cannot be null or empty.", nameof(input));
        }

        using SHA256 sha256 = SHA256.Create();

        byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

        return Convert.ToHexString(data).ToLowerInvariant(); // ToLowerInvariant to match "x2" formatting
    }

    public static bool SHA256VerifyString(this string input, string hash)
    {
        string hashOfInput = SHA256String(input);

        return string.Equals(hashOfInput, hash, StringComparison.OrdinalIgnoreCase);
    }

    public static string SHA512String(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("Input cannot be null or empty.", nameof(input));
        }

        using SHA512 sha512 = SHA512.Create();

        byte[] data = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));

        return Convert.ToHexString(data).ToLowerInvariant(); // ToLowerInvariant to match "x2" formatting
    }

    public static bool SHA512VerifyString(this string input, string hash)
    {
        // Hash the input.
        string hashOfInput = SHA512String(input);

        // Compare the hashes.
        return string.Equals(hashOfInput, hash, StringComparison.OrdinalIgnoreCase);
    }

    public static string Reverse(this string input)
    {
        var length = input.Length;

        if (length == 0)
            return "";

        int srcIndex;

        //CONSIDER: Get System.String to add a surrogate aware Reverse method

        //Detect if there are any graphemes that need special handling
        for (srcIndex = 0; srcIndex <= length - 1; srcIndex++)
        {
            var ch = input[srcIndex];

            var uc = char.GetUnicodeCategory(ch);

            if (uc == UnicodeCategory.Surrogate || uc == UnicodeCategory.NonSpacingMark || uc == UnicodeCategory.SpacingCombiningMark || uc == UnicodeCategory.EnclosingMark)
            {
                //Need to use special handling
                return InternalStrReverse(input, srcIndex, length);
            }
        }

        var chars = input.ToCharArray();

        Array.Reverse(chars);

        return new string(chars);

        //    StringBuilder sb = new StringBuilder(chars.Length);

        //    for (int i = chars.Length - 1; i > -1; i--)
        //        sb.Append(chars[i]);

        //    return sb.ToString();
    }

    ///<remarks>This routine handles reversing Strings containing graphemes
    /// GRAPHEME: a text element that is displayed as a single character</remarks>
    private static string InternalStrReverse(string expression, int srcIndex, int length)
    {
        //This code can only be hit one time
        var sb = new StringBuilder(length) { Length = length };

        var textEnum = StringInfo.GetTextElementEnumerator(expression, srcIndex);

        //Init enumerator position
        if (!textEnum.MoveNext())
        {
            return "";
        }

        var lastSrcIndex = 0;
        var destIndex = length - 1;

        //Copy up the first surrogate found
        while (lastSrcIndex < srcIndex)
        {
            sb[destIndex] = expression[lastSrcIndex];
            destIndex -= 1;
            lastSrcIndex += 1;
        }

        //Now iterate through the text elements and copy them to the reversed string
        var nextSrcIndex = textEnum.ElementIndex;

        while (destIndex >= 0)
        {
            srcIndex = nextSrcIndex;

            //Move to next element
            nextSrcIndex = (textEnum.MoveNext()) ? textEnum.ElementIndex : length;
            lastSrcIndex = nextSrcIndex - 1;

            while (lastSrcIndex >= srcIndex)
            {
                sb[destIndex] = expression[lastSrcIndex];
                destIndex -= 1;
                lastSrcIndex -= 1;
            }
        }

        return sb.ToString();
    }

    public static string SentenceCase(this string input)
    {
        if (input.Length < 1)
            return input;

        string sentence = input.Trim().ToLower();

        return $"{char.ToUpper(sentence[0])}{sentence[1..]}";
    }

    public static string SpaceToHtmlNbsp(this string input)
    {
        return input.Replace(" ", "&nbsp;");
    }

    public static string StripTags(this string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty; // Early exit for null or empty input

        var result = new StringBuilder();
        bool insideTag = false;
        int length = input.Length;

        for (int i = 0; i < length; i++)
        {
            char currentChar = input[i];

            if (currentChar == '<') // Starting a tag
            {
                insideTag = true;
            }
            else if (currentChar == '>') // Ending a tag
            {
                insideTag = false;

                continue; // Skip adding '>'
            }

            if (!insideTag) // Only add characters that are not inside a tag
            {
                result.Append(currentChar);
            }
        }

        return result.ToString();
    }

    public static string TitleCase(this string input, string[]? ignoreWords = null)
    {
        string[] tokens = input.Split(' ');
        StringBuilder sb = new(input.Length);
        foreach (string s in tokens)
        {
            if (s != tokens[0] && ignoreWords != null && ignoreWords.Contains(s.ToLower()))
            {
                sb.Append(s.ToLower() + " ");
            }
            else
            {
                sb.Append(char.ToUpper(s[0]));
                sb.Append(s[1..].ToLower());
                sb.Append(' ');
            }
        }

        return sb.ToString().Trim();
    }

    public static string TrimIntraWords(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input; 

        return string.Join(" ", input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
    }

    public static string RemoveNewLines(this string input, string replaceWith = "")
    {
        if (string.IsNullOrEmpty(input)) return input;

        input = input.Replace("\r\n", replaceWith)
                     .Replace("\n\r", replaceWith)
                     .Replace("\r", replaceWith)
                     .Replace("\n", replaceWith);

        return input;
    }

    public static string EncodeNewLineToHtmlBrTag(this string input)
    {
        return RemoveNewLines(input, "<br/>");
    }

    public static string WordWrap(this string input, int charCount, bool cutOff = false, string? breakText = null)
    {
        if (charCount <= 0) throw new ArgumentOutOfRangeException(nameof(charCount), "Character count must be greater than zero.");

        StringBuilder sb = new();

        var words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (cutOff)
        {
            foreach (var word in words)
            {
                for (int i = 0; i < word.Length; i += charCount)
                {
                    sb.Append(word.AsSpan(i, Math.Min(charCount, word.Length - i))).Append(breakText ?? Environment.NewLine);
                }
            }
        }
        else
        {
            List<string> currentLine = new();

            int currentLength = 0;

            foreach (var word in words)
            {
                if (currentLength + word.Length + currentLine.Count > charCount)
                {
                    sb.Append(string.Join(" ", currentLine)).Append(breakText ?? Environment.NewLine);
                    currentLine.Clear();
                    currentLength = 0;
                }
                currentLine.Add(word);
                currentLength += word.Length;
            }

            if (currentLine.Count > 0)
            {
                sb.Append(string.Join(" ", currentLine));
            }
        }

        return sb.ToString().TrimEnd(); 
    }

    public static string Left(this string text, int numberOfCharacters)
    {
        return text.Length > numberOfCharacters ? text[..numberOfCharacters] : text;
    }

    public static string Right(this string text, int numberOfCharacters)
    {
        return numberOfCharacters >= text.Length ? text : text[^numberOfCharacters..];
    }

    public static string ToCamel(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        var s = value.Trim().ToLower();

        string[] separators = { "_", " " };

        string[] parts = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
            return value;

        return parts[0] + string.Concat(parts.Skip(1).Select(p => char.ToUpper(p[0]) + p[1..]));
    }

    public static string ToPascal(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        var s = value.Trim().ToLower();
        string[] separators = { "_", " " };
        string[] parts = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        return string.Concat(parts.Select(p => char.ToUpper(p[0]) + p[1..]));
    }

    public static string Capitalize(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        string[] parts = value.ToLower().Split('.', StringSplitOptions.RemoveEmptyEntries);

        return string.Join(". ", parts.Select(p => p.Trim()[..1].ToUpper() + p.Trim()[1..])) + ".";
    }

    public static string CapitalizeAll(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        return string.Join(" ", value.Trim().ToLower()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(word => char.ToUpper(word[0]) + word[1..]));
    }

    private static readonly char[] pathSplitCharacters = { '/', '\\' };
    public static string PathCombine(this string basePath, params string[] additional)
    {
        var segments = new List<string>();

        if (! string.IsNullOrWhiteSpace(basePath))
        {
            if (!string.IsNullOrWhiteSpace(basePath))
            {
                segments.AddRange(basePath.Split(pathSplitCharacters, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        foreach (var additionalPath in additional)
        {
            if (!string.IsNullOrWhiteSpace(additionalPath))
            {
                segments.AddRange(additionalPath.Split(pathSplitCharacters, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        return System.IO.Path.Combine(segments.ToArray());
    }

    private const int KeySize = 32; // AES-256
    private const int IvSize = 16;   // AES block size
    private const int Iterations = 100_000; // PBKDF2 iterations

    public static string Encrypt(this string str, string password, string salt)
    {
        // Convert salt to byte array
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

        // Generate the key using PBKDF2 with SHA-256
        using var keyGenerator = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);

        byte[] key = keyGenerator.GetBytes(KeySize);
        byte[] iv = new byte[IvSize];

        RandomNumberGenerator.Fill(iv);

        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var ms = new MemoryStream();

        ms.Write(iv, 0, iv.Length); // Prepend IV to the output

        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            using var sw = new StreamWriter(cs);

            sw.Write(str);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(this string encryptedStr, string password, string salt)
    {
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

        using var keyGenerator = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);

        byte[] key = keyGenerator.GetBytes(KeySize);

        byte[] fullCipher = Convert.FromBase64String(encryptedStr);

        byte[] iv = new byte[IvSize];

        Array.Copy(fullCipher, 0, iv, 0, iv.Length);

        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var ms = new MemoryStream(fullCipher, IvSize, fullCipher.Length - IvSize);
        using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    public static byte[] FromBase64 (this string str)
    {
        return System.Convert.FromBase64String (str);
    }

    public static string ToBase64(byte [] data)
    {
        return System.Convert.ToBase64String(data);
    }

    public static string ToHexString(byte[] data)
    {
        return Convert.ToHexString(data).ToLowerInvariant();
    }
    public static byte[] FromHexString(this string str)
    {
        return Convert.FromHexString(str);
    }
}
