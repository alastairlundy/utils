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

using System.IO;

using Del.Library.Localizations;

namespace Del.Library.Extensions;

public static class IsEmptyExtensions
{
    
    /// <summary>
    /// Checks if a Directory is empty or not.
    /// </summary>
    /// <param name="directory">The directory to be searched.</param>
    /// <returns>true if the directory is empty; returns false otherwise.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown if the directory does not exist.</exception>
    public static bool IsDirectoryEmpty(this string directory)
    {
        if (Directory.Exists(directory))
        {
            return Directory.GetDirectories(directory).Length == 0 && Directory.GetFiles(directory).Length == 0;
        }
        else
        {
            throw new DirectoryNotFoundException($"{Resources.Exceptions_DirectoryNotFound.Replace("{x}", directory)}");
        }
    }
}