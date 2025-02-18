using GameFramework.DataTable;
using GameFramework.Sound;
using Unity.Burst.CompilerServices;
using UnityGameFramework.Runtime;

namespace BOO
{
    public static class SoundExtension
    {
        private static int? musicSerialsID;
        private static int? soundSerialsID;

        public static int? PlayMusic(this SoundComponent soundComponent, int musicId, object userData = null)
        {
            IDataTable<DRMusic> dtMusic = GameEntry.DataTable.GetDataTable<DRMusic>();
            var drMusic = dtMusic.GetDataRow(musicId);
            if (drMusic == null)
            {
                Log.Warning("Can not load music '{0}' from data table.", musicId.ToString());
                return null;
            }

            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 64;
            playSoundParams.Loop = true;
            playSoundParams.VolumeInSoundGroup = 1f;
            playSoundParams.SpatialBlend = 0f;
            musicSerialsID = soundComponent.PlaySound(AssetUtility.GetMusicAsset(drMusic.AssetName), "Music", 0,
                playSoundParams, userData);
            return musicSerialsID;
        }

        public static void StopMusic(this SoundComponent soundComponent)
        {
            if (!musicSerialsID.HasValue)
                return;
            soundComponent.StopSound(musicSerialsID.Value);
        }

        public static int? PlaySound(this SoundComponent soundComponent, int soundId, Entity bindEntity = null,
            object userData = null)
        {
            IDataTable<DRSound> dtSound = GameEntry.DataTable.GetDataTable<DRSound>();
            var drSound = dtSound.GetDataRow(soundId);
            if (drSound == null)
            {
                Log.Warning("Can not load sound '{0}' from data table.", soundId.ToString());
                return null;
            }

            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 64;
            playSoundParams.Loop = false;
            playSoundParams.VolumeInSoundGroup = 1f;
            playSoundParams.SpatialBlend = 0f;
            return soundComponent.PlaySound(AssetUtility.GetSoundAsset(drSound.AssetName), "Sound", 0,
                playSoundParams, bindEntity, userData);
        }
        
        public static int? PlayUISound(this SoundComponent soundComponent, int soundId, 
            object userData = null)
        {
            IDataTable<DRSound> dtSound = GameEntry.DataTable.GetDataTable<DRSound>();
            var drSound = dtSound.GetDataRow(soundId);
            if (drSound == null)
            {
                Log.Warning("Can not load sound '{0}' from data table.", soundId.ToString());
                return null;
            }

            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 64;
            playSoundParams.Loop = false;
            playSoundParams.VolumeInSoundGroup = 1f;
            playSoundParams.SpatialBlend = 0f;
            return soundComponent.PlaySound(AssetUtility.GetUISoundAsset(drSound.AssetName), "UISound", 0,
                playSoundParams, userData);
        }
    }
}