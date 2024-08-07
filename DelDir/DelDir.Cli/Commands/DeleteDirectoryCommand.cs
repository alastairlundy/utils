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
using System.ComponentModel;
using System.IO;
using System.Linq;
using AlastairLundy.Extensions.System;
using Del.Library;
using Del.Library.Extensions;
using DelDir.Cli.Localizations;

using Spectre.Console;
using Spectre.Console.Cli;

namespace DelDir.Cli.Commands;

public class DeleteDirectoryCommand : Command<DeleteDirectoryCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<Directory>")]
        public string? DirectoryToBeDeleted { get; init; }
        
        [CommandOption("-v|--verbose")]
        [DefaultValue(false)]
        public bool Verbose { get; init; }
        
        [CommandOption("-p")]
        [DefaultValue(false)]
        public bool RemoveEmptyParentDirectories { get; init; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        ExceptionFormats exceptionFormats;

        if (settings.Verbose)
        {
            exceptionFormats = ExceptionFormats.Default;
        }
        else
        {
            exceptionFormats = ExceptionFormats.NoStackTrace;
        }
        
        if (settings.DirectoryToBeDeleted == null)
        {
            AnsiConsole.WriteException(new ArgumentNullException(nameof(settings.DirectoryToBeDeleted), Resources.Exception_NoArgumentsProvided), exceptionFormats);
            return -1;
        }

        try
        {
            if (settings.DirectoryToBeDeleted.Equals("/"))
            {
                throw new ArgumentException(Resources.Exception_InvalidSlashArgument, settings.DirectoryToBeDeleted);
            }
            if (settings.DirectoryToBeDeleted.Equals("*"))
            {

                if (settings.DirectoryToBeDeleted.AreSubdirectoriesEmpty())
                {
                    DirectoryRemover directoryRemover = new DirectoryRemover();
                    directoryRemover.DirectoryDeleted += OnDeleted;
                    directoryRemover.FileDeleted += OnDeleted;

                    void OnDeleted(object? sender, string e)
                    {
                        if (settings.Verbose)
                        {
                            AnsiConsole.WriteLine(e);
                        }
                    }
                    
                    directoryRemover.DeleteRecursively(settings.DirectoryToBeDeleted, true);
                    
                    if (settings.RemoveEmptyParentDirectories)
                    {
                        directoryRemover.DeleteParentDirectory(settings.DirectoryToBeDeleted, settings.RemoveEmptyParentDirectories);
                    }

                    return 0;
                }
                else
                {
                    throw new ArgumentException(Resources.Exception_NonEmptyDirectory.Replace("{x}", settings.DirectoryToBeDeleted));
                }
            }
            if (Directory.Exists(settings.DirectoryToBeDeleted))
            {
                if (!Directory.GetDirectories(settings.DirectoryToBeDeleted).Any() &&
                    !Directory.GetFiles(settings.DirectoryToBeDeleted).Any())
                {
                    DirectoryRemover directoryRemover = new DirectoryRemover();
                    directoryRemover.DirectoryDeleted += DirectoryRemoverOnDirectoryDeleted;

                    void DirectoryRemoverOnDirectoryDeleted(object? sender, string e)
                    {
                        if (settings.Verbose)
                        {
                            AnsiConsole.WriteLine(e);
                        }
                    }
                    
                    directoryRemover.DeleteDirectory(settings.DirectoryToBeDeleted, true);

                    if (settings.RemoveEmptyParentDirectories)
                    {
                        directoryRemover.DeleteParentDirectory(settings.DirectoryToBeDeleted, settings.RemoveEmptyParentDirectories);
                    }
                    
                    return 0;
                }
                else
                {
                    throw new ArgumentException(Resources.Exception_NonEmptyDirectory.Replace("{x}", settings.DirectoryToBeDeleted));
                }
            }
            else
            {
                throw new DirectoryNotFoundException(Resources.Exception_DirectoryNotFound.Replace("{x}", settings.DirectoryToBeDeleted));
            }
        }
        catch(Exception exception)
        {
            AnsiConsole.WriteException(exception, exceptionFormats);
            return -1;
        }
    }
}