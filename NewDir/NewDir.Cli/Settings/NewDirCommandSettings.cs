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

using System.ComponentModel;

using Spectre.Console.Cli;

namespace NewDir.Cli.Settings;

public class NewDirCommandSettings : CommandSettings 
{
        [CommandArgument(0, "<directory_name>")]
        [DefaultValue(null)]
        public string? DirectoryName { get; init; }
        
        [CommandOption("-p|--parents")]
        [DefaultValue(false)]
        public bool CreateParentDirectories { get; init; }
        
        [CommandOption("-m|--mode")]
        [DefaultValue(null)]
        public string? Mode { get; init; }
        
        [CommandOption("--debug|--debugging|--verbose")]
        [DefaultValue(false)]
        public bool UseDebugging { get; init; }
}