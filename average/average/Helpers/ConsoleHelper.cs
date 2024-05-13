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

using System.Reflection;

using AlastairLundy.Extensions.System.AssemblyExtensions;
using AlastairLundy.Extensions.System.VersionExtensions;

namespace average.Helpers;

internal class ConsoleHelper
{
    public static void PrintUnformattedVersion()
    {
        Console.WriteLine($"{Assembly.GetExecutingAssembly().GetProjectName()} {Assembly.GetExecutingAssembly().GetProjectVersion().GetFriendlyVersionToString()}");
    }

    public static void PrintUnformattedLicense()
    {
        foreach (string line in File.ReadAllLines($"Environment.CurrentDirectory{Path.DirectorySeparatorChar}LICENSE.txt"))
        {
            Console.WriteLine(line);
        }

        Console.WriteLine();
    }
}