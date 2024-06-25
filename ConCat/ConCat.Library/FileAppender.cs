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
using System.Linq;

using AlastairLundy.Extensions.System.IEnumerableExtensions;

using CliUtilsLib;

using ConCat.Library.Localizations;
// ReSharper disable RedundantIfElseBlock

namespace ConCat.Library;

/// <summary>
/// 
/// </summary>
public class FileAppender
{
    protected List<string> AppendedFileContents;
    
    public FileAppender()
    {
        AppendedFileContents = new List<string>();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileToBeAppended">The file to have its contents appended to the existing file contents. If no existing file contents exists, this will become the contents appended to in the future.</param>
    /// <returns>true if the files where successfully appended; returns false otherwise.</returns>
    public bool TryAppendFileContents(string fileToBeAppended)
    {
        try
        {
            AppendFileContents(fileToBeAppended);

            return true;
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileToBeAppended"></param>
    /// <exception cref="Exception"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public void AppendFileContents(string fileToBeAppended)
    {
        if (FileFinder.IsAFile(fileToBeAppended) || File.Exists(fileToBeAppended))
        {
            try
            {
                string[] fileContents = File.ReadAllLines(fileToBeAppended);

                AppendedFileContents = AppendedFileContents.Combine(fileContents).ToList();
            }
            catch (UnauthorizedAccessException exception)
            {
                throw new Exception(exception.Message, exception);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception);
            }
        }
        else
        {
            throw new FileNotFoundException(Resources.Exceptions_FileNotFound, fileToBeAppended);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileContents"></param>
    /// <exception cref="Exception"></exception>
    public void AppendFileContents(string[] fileContents)
    {
        try
        {
            AppendedFileContents = AppendedFileContents.Combine(fileContents).ToList();
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message, exception);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> ToEnumerable()
    {
        return AppendedFileContents;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string[] ToArray()
    {
        return AppendedFileContents.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public void WriteToFile(string fileName)
    {
        if (FileFinder.IsAFile(fileName))
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                File.WriteAllLines(fileName, AppendedFileContents);
            }
            catch (UnauthorizedAccessException exception)
            {
                throw new UnauthorizedAccessException(exception.Message, exception);
            }
            catch (FileNotFoundException exception)
            {
                throw new FileNotFoundException(exception.Message, fileName, exception);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception);
            }
        }
        else
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(Resources.Exceptions_FileNotFound, fileName);
            }
        }
    }
}