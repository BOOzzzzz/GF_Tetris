using GameFramework;
using GameFramework.Event;

namespace GameMain.Scripts.Event
{
    public class GameOverEventArgs:GameEventArgs
    {
        public static readonly int EventId = typeof(GameOverEventArgs).GetHashCode();
        public override void Clear()
        {
            
        }
        
        public static GameOverEventArgs Create(object userData = null)
        {
            GameOverEventArgs gameOverEventArgs = ReferencePool.Acquire<GameOverEventArgs>();
            return gameOverEventArgs;
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