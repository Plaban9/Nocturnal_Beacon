using UnityEngine;

namespace Minimalist.Audio.Sound
{
    /// <summary>
    /// Contains data for a sound effect.
    /// Has multiple clips for randomness of the effect needed.
    /// </summary>
    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public SoundType soundType;
        public AudioClip[] clips;
        [Range(0f, 1f)]
        public float volume = 1f;

        public AudioClip GetRandomClip()
        {
            if (clips?.Length > 0)
            {
                return clips[Random.Range(0, clips.Length)];
            }

            return null;
        }
    }
}