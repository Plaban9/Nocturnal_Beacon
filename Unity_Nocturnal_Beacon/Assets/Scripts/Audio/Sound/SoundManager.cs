using UnityEngine;

namespace Minimalist.Audio.Sound
{
    /// <summary>
    /// Handles SFX.
    /// Contains methods to play types of SFX.
    /// </summary>
    internal class SoundManager : MonoBehaviour
    {
        internal static SoundManager Instance { get; private set; }

        [SerializeField] private AudioSource _sfxSource2D;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Plays sound using PlayOneShot(). 
        /// Mostly used in UI.
        /// </summary>
        /// <param name="audioClip"></param>
        internal void PlaySound2D(AudioClip audioClip, float volume)
        {
            _sfxSource2D.PlayOneShot(audioClip, volume * GameAttributes.Settings_SFXVolume * GameAttributes.Settings_MasterVolume);
        }

        /// <summary>
        /// Plays sound at a location using PlayClipAtPoint().
        /// Mostly used in gameplay.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="position"></param>
        internal void PlaySound3D(AudioClip audioClip, Vector3 position, float volume)
        {
            if (audioClip != null)
            {
                AudioSource.PlayClipAtPoint(audioClip, position, volume * GameAttributes.Settings_SFXVolume * GameAttributes.Settings_MasterVolume);
            }
        }

        internal void SetMasterVolume(float volumeToSet)
        {
            //if (_sfxSource2D != null)
            //{
            //    if (_sfxSource2D.isPlaying)
            //    {
            //        _sfxSource2D.volume = 1f * GameAttributes.Settings_MusicVolume * volumeToSet;
            //    }
            //}
        }

        internal void SetSFXVolume(float volumeToSet)
        {
            //if (_sfxSource2D != null)
            //{
            //    if (_sfxSource2D.isPlaying)
            //    {
            //        _sfxSource2D.volume = 1f * volumeToSet * GameAttributes.Settings_MasterVolume;
            //    }
            //}
        }
    }
}
