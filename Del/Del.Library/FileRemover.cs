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

using Del.Library.Localizations;

namespace Del.Library;

public class FileRemover
{
    public event EventHandler<string> FileDeleted;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public bool TryDeleteFile(string file)
    {
        try
        {
            DeleteFile(file);
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
    /// <param name="file"></param>
    /// <exception cref="FileNotFoundException"></exception>
    public void DeleteFile(string file)
    {
        if (File.Exists(file))
        {
            File.Delete(file);
            FileDeleted?.Invoke(this, Resources.File_Deleted.Replace("{x}", file));
        }

        throw new FileNotFoundException(Resources.Exceptions_FileNotFound.Replace("{x}", file));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="files"></param>
    public void DeleteFiles(IEnumerable<string> files)
    {
        foreach (string file in files)
        {
            DeleteFile(file);
        }
    }
}