//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ShootingStar.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class ListVector2Processor : GenericDataProcessor<List<Vector2>>
        {
            public override bool IsSystem
            {
                get
                {
                    return false;
                }
            }

            public override string LanguageKeyword
            {
                get
                {
                    return "List<Vector2>";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "List<vector2>",
                    "unityengine.List<vector2>"
                };
            }

            public override List<Vector2> Parse(string value)
            {
                List<Vector2> listV2 = new List<Vector2>();
                string[] splitedValue = value.Split(';');
                foreach (var t in splitedValue)
                {
                    string[] splitedVector2 = t.Trim().Split(',');
                    listV2.Add(new Vector2(float.Parse(splitedVector2[0]), float.Parse(splitedVector2[1])));
                }
                return listV2;
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                List<Vector2> listvector2 = Parse(value);
                binaryWriter.Write(listvector2.Count);
                for (int i = 0; i < listvector2.Count; i++)
                {
                    binaryWriter.Write(listvector2[i].x);
                    binaryWriter.Write(listvector2[i].y);
                }
            }
        }
    }
}
