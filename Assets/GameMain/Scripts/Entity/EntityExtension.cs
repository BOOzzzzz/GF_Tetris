using UnityGameFramework.Runtime;

namespace BOO
{
    public static class EntityExtension
    {
        private static int serialID = 0; //

        public static int GenerateSerialID(this EntityComponent entityComponent)
        {
            return serialID++;
        }
    }
}