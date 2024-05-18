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

namespace NLine.Library;

public class FileFinder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="arguments"></param>
    /// <returns>the file if one was provided in the list of arguments; returns null otherwise.</returns>
    public static string? FindFileName(string[] arguments)
    {
        foreach (string arg in arguments)
        {
            if (arg.EndsWith(".txt") ||  arg.EndsWith(".rtf") || arg[arg.Length - 3].Equals('.'))
            {
                return arg;
            }
        }

        return null;
    }

    public static bool FoundAFileInArgs(string[] arguments)
    {
        foreach (string arg in arguments)
        {
            if (arg.EndsWith(".txt") || arg.EndsWith(".rtf") || arg[arg.Length - 3].Equals('.'))
            {
                return true;
            }
        }

        return false;
    }
}