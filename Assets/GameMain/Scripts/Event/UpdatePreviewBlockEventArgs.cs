using GameFramework;
using GameFramework.Event;

namespace GameMain.Scripts.Event
{
    public class UpdatePreviewBlockEventArgs:GameEventArgs
    {
        public static readonly int EventId = typeof(UpdatePreviewBlockEventArgs).GetHashCode();
        public override void Clear()
        {
            
        }
        
        public static UpdatePreviewBlockEventArgs Create(object userData = null)
        {
            UpdatePreviewBlockEventArgs updatePreviewBlockEventArgs = new UpdatePreviewBlockEventArgs();
            return updatePreviewBlockEventArgs;
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