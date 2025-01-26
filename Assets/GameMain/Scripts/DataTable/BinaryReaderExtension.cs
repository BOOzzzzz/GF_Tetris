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
    }
}