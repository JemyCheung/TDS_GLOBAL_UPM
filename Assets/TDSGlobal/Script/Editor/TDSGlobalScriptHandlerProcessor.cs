using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TDSGlobalEditor
{
    public class TDSGlobalScriptStreamWriterHelper : System.IDisposable
    {
        private string filePath;

        public TDSGlobalScriptStreamWriterHelper(string fPath)
        {
            filePath = fPath;
            if (!System.IO.File.Exists(filePath))
            {
                Debug.LogError(filePath + "路径下文件不存在");
                return;
            }
        }

        public void WriteBelow(string below, string text)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string all = streamReader.ReadToEnd();
            streamReader.Close();
            int beginIndex = all.IndexOf(below, StringComparison.Ordinal);
            if (beginIndex == -1)
            {
                Debug.LogError(filePath + "中没有找到字符串" + below);
                return;
            }

            int endIndex = all.LastIndexOf("\n", beginIndex + below.Length, StringComparison.Ordinal);
            all = all.Substring(0, endIndex) + "\n" + text + "\n" + all.Substring(endIndex);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(all);
            streamWriter.Close();
        }

        public void Replace(string below, string newText)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string all = streamReader.ReadToEnd();
            streamReader.Close();
            int beginIndex = all.IndexOf(below, StringComparison.Ordinal);
            if (beginIndex == -1)
            {
                Debug.LogError(filePath + "中没有找到字符串" + below);
                return;
            }

            all = all.Replace(below, newText);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(all);
            streamWriter.Close();
        }

        public void Dispose()
        {
        }
    }

    public class TDSGlobalFileHelper
    {
        public static void CopyAndReplaceDirectory(string srcPath, string dstPath)
        {
            if (Directory.Exists(dstPath))
                Directory.Delete(dstPath);
            if (File.Exists(dstPath))
                File.Delete(dstPath);

            Directory.CreateDirectory(dstPath);

            foreach (var file in Directory.GetFiles(srcPath))
                File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));

            foreach (var dir in Directory.GetDirectories(srcPath))
                CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
        }
    }

}
