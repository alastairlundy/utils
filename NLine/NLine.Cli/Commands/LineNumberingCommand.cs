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
using System.ComponentModel;
using System.IO;
using System.Linq;

using CliUtilsLib;

using NLine.Cli.Localizations;
using NLine.Library;

using Spectre.Console;
using Spectre.Console.Cli;

namespace NLine.Cli.Commands;

public class LineNumberingCommand : Command<LineNumberingCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<File>")]
        public string? File { get; init; }
        
        [CommandOption("-b")]
        public string? BodyLinesOption { get; init; }
        
        [CommandOption("-s")]
        public string? AppendLineNumber { get; init; }
        
        [CommandOption("-l")]
        [DefaultValue(1)]
        public int GroupOfEmptyLinesCountedAsOne { get; init; }
        
        [CommandOption("-w")]
        [DefaultValue(0)]
        public int ColumnNumber { get; init; }
        
        [CommandOption("-i")]
        [DefaultValue(1)]
        public int LineIncrementor { get; init; }
        
        [CommandOption("-v")]
        [DefaultValue(1)]
        public int LineStartingNumber { get; init; }
        
        [CommandOption("-n")]
        public string? NumberFormatting { get; init; }
        
        [CommandOption("--output")]
        public string? OutputFile { get; init; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        string? searchString = null;
        bool assignEmptyLinesNumbers = false;
        
        if (settings.BodyLinesOption != null)
        {
            if (settings.BodyLinesOption.Contains('a'))
            {
                assignEmptyLinesNumbers = true;
            }
            else if (settings.BodyLinesOption.Contains('p'))
            {
                searchString = settings.BodyLinesOption.Remove(0);
            }
        }

        string[]? input = null;
        
        if (settings.File == null && !FileArgumentFinder.FoundAFileInArgs(context.Remaining.Raw.ToArray()))
        {
            input = Console.OpenStandardInput().ToString()?.Split(Environment.NewLine) ?? null;
        }
        else if (settings.File == null && FileArgumentFinder.FoundAFileInArgs(context.Remaining.Raw.ToArray()))
        {
            IEnumerable<string>? files = FileArgumentFinder.FindFileNamesInArgs(context.Remaining.Raw.ToArray());

            if (files == null)
            {
                input = Console.OpenStandardInput().ToString()?.Split(Environment.NewLine) ?? null;
            }
            else
            {
                input = File.ReadAllLines(files.First());
            }

        }
        else if(settings.File != null)
        {
            input = File.ReadAllLines(settings.File!);
        }

        if (input == null)
        {
            AnsiConsole.WriteException(new ArgumentException());
            return -1;
        }

        bool addLeadingZeroes = false;
        bool tabSpaceAfterLineNumber = false;
        int columnNumbers = settings.ColumnNumber;
        
        if (settings.ColumnNumber == 1)
        {
            if (settings.NumberFormatting!.Contains("rn"))
            {
                columnNumbers = 5;
            }
            else if (settings.NumberFormatting.Contains("ln"))
            {
                tabSpaceAfterLineNumber = true;
            }
            else if (settings.NumberFormatting.Contains("rz"))
            {
                addLeadingZeroes = true;
            }
        }
        
        string stringAppender = string.Empty;

        if (settings.AppendLineNumber != null)
        {
            stringAppender = settings.AppendLineNumber;
        }
        
        string[] results = LineNumberer.AddLineNumbers(input, settings.LineIncrementor, settings.LineStartingNumber,
            stringAppender, assignEmptyLinesNumbers, settings.GroupOfEmptyLinesCountedAsOne, columnNumbers, tabSpaceAfterLineNumber, addLeadingZeroes, searchString).ToArray();

        if (settings.OutputFile != null)
        {
            try
            {
                File.WriteAllLines(settings.OutputFile, results);
                AnsiConsole.WriteLine(Resources.File_Saved_Succes);
                return 0;
            }
            catch(Exception exception)
            {
                AnsiConsole.WriteLine(Resources.File_Saved_Failure);
                AnsiConsole.WriteException(exception);
                return -1;
            }
        }
        // ReSharper disable once RedundantIfElseBlock
        else
        {
            foreach (string res in results)
            {
                AnsiConsole.WriteLine(res);
            }

            return 0;
        }
    }
}