using Minimalist.Audio.Music;
using Minimalist.Audio.Sound;

using UnityEngine;

namespace Minimalist.Audio
{
    /// <summary>
    /// Contains data for SFX and Music.
    /// It is a scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "Audio Library", menuName = "Minimalist/Audio Library", order = 1)]
    public class AudioLibrary : ScriptableObject
    {
        [Header("Music")]
        [SerializeField]
        public MusicTrack[] musicLibrary;

        [Header("SFX/Sound")]
        [SerializeField]
        public SoundEffect[] soundLibrary;

        #region Music
        public MusicTrack GetMusicFromName(string name)
        {
            foreach (var track in musicLibrary)
            {
                if (track.name.Equals(name))
                {
                    return track;
                }
            }

            return null;
        }

        public MusicTrack GetMusicFromType(MusicType musicType)
        {
            foreach (var track in musicLibrary)
            {
                if (track.musicType == musicType)
                {
                    return track;
                }
            }

            return null;
        }
        #endregion

        #region Sound
        public SoundEffect GetSFXFromName(string name)
        {
            foreach (var track in soundLibrary)
            {
                if (track.name.Equals(name))
                {
                    return track;
                }
            }

            return null;
        }

        public SoundEffect GetSFXFromType(SoundType soundType)
        {
            foreach (var track in soundLibrary)
            {
                if (track.soundType == soundType)
                {
                    return track;
                }
            }

            return null;
        }
        #endregion
    }
}
