using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BOO
{
    public static class BinaryReaderExtension
    {
        public static Vector2 ReadVector2(this BinaryReader binaryReader)
        {
            return new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }
        
        public static Color ReadColor(this BinaryReader binaryReader)
        {
            return new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }
        
        public static List<Vector2> ReadListVector2(this BinaryReader binaryReader)
        {
            List<Vector2> list = new List<Vector2>();
            int count = binaryReader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                list.Add(new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle()));
            }

            return list;
        }
    }
}