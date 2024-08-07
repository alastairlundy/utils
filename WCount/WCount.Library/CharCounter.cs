﻿/*
     Copyright 2024 Alastair Lundy

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;

using WCount.Library.Localizations;

namespace WCount.Library;

public static class CharCounter
{
    /// <summary>
    /// Get the number of characters in a string.
    /// </summary>
    /// <param name="s">The string to be searched.</param>
    /// <returns>the number of characters in a string.</returns>
    public static ulong CountChars(this string s)
    {
        ulong totalChars = Convert.ToUInt64(s.ToCharArray().Length);

        return totalChars;
    }

    /// <summary>
    /// Gets the number of characters in a file.
    /// </summary>
    /// <param name="filePath">The file path of the file to be searched.</param>
    /// <returns>the number of characters in the file specified.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file specified could not be found.</exception>
    public static ulong CountCharsInFile(this string filePath)
    {
        if (File.Exists(filePath))
        {
            ulong totalChars = 0;
            
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                totalChars += CountChars(line.Split(' '));
            }

            return totalChars;
        }

        throw new FileNotFoundException(Resources.Exceptions_FileNotFound_Message, filePath);
    }

    /// <summary>
    /// Gets the number of characters in an IEnumerable of strings.
    /// </summary>
    /// <param name="enumerable">The IEnumerable to be searched.</param>
    /// <returns>the number of characters in the specified IEnumerable.</returns>
    public static ulong CountChars(this IEnumerable<string> enumerable)
    {
        ulong totalChars = 0;

        foreach (string s in enumerable)
        {
            totalChars += Convert.ToUInt64(s.ToCharArray().Length);
        }

        return totalChars;
    }
}