# CastelloBranco.StringExtensions Library

## Overview

`CastelloBranco.StringExtensions` is a utility library that provides a variety of helper functions for string manipulation, extending the capabilities of C#'s `String` class. Inspired by popular string functions in PHP, this library is designed to simplify common tasks, such as filtering strings, encoding/decoding, and modifying text formatting.

It supercedes CBT.StringExtensions that are now obsolete

This library includes functions that are missing in .NET, offering robust solutions for string manipulation in your .NET projects.

---

## Features

- **String Filters**
  - `OnlyAsciiLether()`        : Removes all non-alphabetical characters.
  - `OnlyAsciiLetherOrDigits()`: Removes all non-alphabetical and non-digit characters.
  - `OnlyAsciiDigits()`        : Retains only numeric characters.
  - `OnlyAllowed ()`           : Retain only allowed caracters.
  - `RemoveDiacritics()`: Removes diacritics from strings (e.g., "ã" becomes "a").
  - `FilterWords()` Filter Words
  - `HasWords()`: get regexp MatchCollection for words

- **Base64 Encoding/Decoding**
  - `ToBase64()`: Encodes a string in Base64 using UTF-8 or any other encodig.
  - `FromBase64()`: Decodes a Base64 encoded string using UTF-8 or any other encodig.
  - `ToHex()`: Encodes a string in Hex using UTF-8 or any other encodig.
  - `FromHex()`: Decodes a hex encoded string using UTF-8 or any other encodig.
   
- **Case Insensitive Functions**
  - `CaseInsensitivePatternReplace()`: Replaces substrings matching a pattern, ignoring case sensitivity.

- **Html Handling**
  - `HtmlSpecialEntitiesEncode()`: Encodes special HTML characters.
  - `HtmlSpecialEntitiesDecode()`: Decodes HTML-encoded special characters.
  - `PadLeftHtmlSpaces()` / `PadRightHtmlSpaces()`: Pads using HTML non-breaking spaces (`&nbsp;`).
  - `SpaceToHtmlNbsp()`: Replace spaces with & nbsp;
  - `StripTags()`: Removes HTML tags from a string.
  - `HtmlEncodeNewLine()`: Encode all new lines to < b r\ > 

- **Hashing and Encryption**
  - `MD5String()`: Generates an MD5 hash for a given string.
  - `MD5VerifyString()`: Verifies if a string matches a provided MD5 hash.
  - `SHA256String()`: Generates an SHA256 hash for a given string.
  - `SHA256VerifyString()`: Verifies if a string matches a provided SHA256 hash.
  - `SHA512String()`: Generates an SHA512 hash for a given string.
  - `SHA512VerifyString()`: Verifies if a string matches a provided SHA512 hash.
  - `Encrypt()`: Encrypt an string
  - `Decrypt()`: Dencrypt an string

- **Text Formatting**
  - `SentenceCase()`: Converts a string to sentence case.
  - `TitleCase()`: Converts a string to title case, with an option to ignore short words.
  - `TrimIntraWords()`: Removes extra spaces between words.
  - `Reverse()` : Reverse string
  - `ToCamel()` : Cemelize an string 
  - `ToPascal()`: Pascal Case an string
  - `Capitalize()`: Captilize first lether of sentence
  - `CapitalizeAll()` : Captilize first lether of words
  
- **String Wrapping and Trimming**
  - `WordWrap()`: Wraps text based on a specified character length.
  - `RemoveNewLines()`: Removes or replaces newlines and carriage returns.
  - `Left()` : Get left part of string 
  - `Right()`: Get right part of string 

- **Path Operations**
  - `PathCombine()`: Combines paths, handling both forward and backward slashes.

---

## Package

Nuget CastelloBranco.StringExtensions - https://www.nuget.org/packages/CastelloBranco.StringExtensions/

## Installation

To use this library in your .NET project, include CastelloBranco.StringExtensions package from nuget or or download the source code and compile it with your application

---

## Usage Examples

### Removing Non-Alphabetical Characters
```csharp
string original = "abc123!@#";
string result = original.OnlyAlpha();  // result: "abc"
```

### Base64 Encoding and Decoding
```csharp
string text = "Hello World!";
string encoded = text.Base64StringEncode();  // Encodes to Base64
string decoded = encoded.Base64StringDecode();  // Decodes back to original string
```

### MD5 Hashing
```csharp
string hash = "mystring".MD5String();  // Generate MD5 hash
bool isValid = "mystring".MD5VerifyString(hash);  // Verify the hash
```

### Path Combining
```csharp
string combinedPath = "C:\\MyFolder".PathCombine("SubFolder", "File.txt");
// combinedPath: "C:\MyFolder\SubFolder\File.txt"
```

---

## Contributing

If you wish to contribute to this project, please fork the repository, make your changes, and submit a pull request.

---

## License

This project is licensed under the MIT License. For more details, please see the LICENSE file.

---

## Contact

For any queries or feedback, please contact **Cesar Castello Branco Filho**.

---

This library is designed to make string manipulation easier for .NET developers by providing simple, reusable, and PHP-inspired string extension methods.

