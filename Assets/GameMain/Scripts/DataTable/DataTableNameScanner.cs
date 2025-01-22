using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BOO
{
    public class DataTableNameScanner
    {
        public static List<string> GetDataTableNames()
        {
            List<string> dataTableNames = new List<string>();
            // 获取指定目录下所有的.txt文件
            string[] txtFiles = Directory.GetFiles("Assets/GameMain/DataTables", "*.txt");
            foreach (string file in txtFiles)
            {
                // 获取文件名（不包括扩展名）
                string fileName = Path.GetFileNameWithoutExtension(file);
                dataTableNames.Add(fileName);
            }

            return dataTableNames;
        }
    }
}