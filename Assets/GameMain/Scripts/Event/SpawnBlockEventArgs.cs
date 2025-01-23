using GameFramework;
using GameFramework.Event;

namespace GameMain.Scripts.Event
{
    public class SpawnBlockEventArgs:GameEventArgs
    {
        public static readonly int EventId = typeof(SpawnBlockEventArgs).GetHashCode();
        public override void Clear()
        {
            
        }
        
        public static SpawnBlockEventArgs Create(object userData = null)
        {
            SpawnBlockEventArgs spawnBlockEventArgs = new SpawnBlockEventArgs();
            return spawnBlockEventArgs;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }
    }
}