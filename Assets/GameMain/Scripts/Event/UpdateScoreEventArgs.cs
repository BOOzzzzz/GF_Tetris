using GameFramework;
using GameFramework.Event;

namespace GameMain.Scripts.Event
{
    public class UpdateScoreEventArgs:GameEventArgs
    {
        public static readonly int EventId = typeof(UpdateScoreEventArgs).GetHashCode();
        public override void Clear()
        {
            
        }
        
        public int Score { get; private set; }
        
        public static UpdateScoreEventArgs Create(int score, object userData = null)
        {
            UpdateScoreEventArgs updateScoreEventArgs = ReferencePool.Acquire<UpdateScoreEventArgs>();
            updateScoreEventArgs.Score = score;
            return updateScoreEventArgs;
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