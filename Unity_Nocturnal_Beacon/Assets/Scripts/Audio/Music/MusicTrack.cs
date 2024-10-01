using UnityEngine;

namespace Minimalist.Audio.Music
{
    /// <summary>
    /// Contains data for a Music.
    /// </summary>
    [System.Serializable]
    public class MusicTrack
    {
        public string name;
        public MusicType musicType;
        public AudioClip audioClip;
        [Range(0f, 1f)]
        public float volume = 1f;
        public bool shouldLoop;
    }
}
