//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2025-01-26 16:00:22.230
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace BOO
{
    /// <summary>
    /// 方块表。
    /// </summary>
    public class DREntity : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取属性编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取资源组。
        /// </summary>
        public string AssetGroup
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取起始位置。
        /// </summary>
        public Vector2 OriginPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取旋转中心点。
        /// </summary>
        public Vector2 Pivot
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取颜色。
        /// </summary>
        public Color Color
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            AssetName = columnStrings[index++];
            AssetGroup = columnStrings[index++];
            OriginPosition = DataTableExtension.ParseVector2(columnStrings[index++]);
            Pivot = DataTableExtension.ParseVector2(columnStrings[index++]);
            Color = DataTableExtension.ParseColor(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    AssetName = binaryReader.ReadString();
                    AssetGroup = binaryReader.ReadString();
                    OriginPosition = binaryReader.ReadVector2();
                    Pivot = binaryReader.ReadVector2();
                    Color = binaryReader.ReadColor();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
