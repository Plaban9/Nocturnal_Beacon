using System.Collections;

using UnityEngine;

namespace Minimalist.Audio.Music
{
    /// <summary>
    /// Handles Music.
    /// Contains methods to play types of Music.
    /// </summary>
    internal class MusicManager : MonoBehaviour
    {
        private float _currentTrackVolume = 1.0f;

        internal static MusicManager Instance { get; private set; }

        [SerializeField] private AudioSource _musicSource;

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
        /// Plays Music. Tip: Call with audioClip as null to give a fade out with no audio.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="fadeDuration"></param>
        /// <param name="loop"></param>
        internal void PlayMusic(AudioClip audioClip, float fadeDuration = 0.5f, bool loop = true, float volume = 1f)
        {
            StartCoroutine(AnimateMusicCrossFade(audioClip, fadeDuration, loop, volume));
        }

        internal void StopMusic()
        {
            if (_musicSource != null)
            {
                _musicSource.Stop();
            }
        }

        internal void PauseMusic()
        {
            if (_musicSource != null)
            {
                _musicSource.Pause();
            }
        }

        private IEnumerator AnimateMusicCrossFade(AudioClip nextTrack, float fadeDuration = 0.5f, bool loop = true, float volume = 1f)
        {
            float percent = 0; // Used as intermediate variable for lerping

            while (percent < 1)
            {
                percent += Time.deltaTime * 1 / fadeDuration;
                _musicSource.volume = Mathf.Lerp(_musicSource.volume * GameAttributes.Settings_MusicVolume * GameAttributes.Settings_MasterVolume, 0, percent);

                yield return null;
            }

            if (nextTrack == null)
            {
                yield break;
            }

            _currentTrackVolume = volume;
            _musicSource.clip = nextTrack;
            _musicSource.loop = loop;
            _musicSource.Play();

            percent = 0;

            while (percent < 1)
            {
                percent += Time.deltaTime * 1 / fadeDuration;
                _musicSource.volume = Mathf.Lerp(0f, volume * GameAttributes.Settings_MusicVolume * GameAttributes.Settings_MasterVolume, percent);

                yield return null;
            }
        }

        internal void SetMasterVolume(float volumeToSet)
        {
            if (_musicSource != null)
            {
                if (_musicSource.isPlaying)
                {
                    _musicSource.volume = _currentTrackVolume * volumeToSet;
                }
            }
        }

        internal void SetMusicVolume(float volumeToSet)
        {
            if (_musicSource != null)
            {
                if (_musicSource.isPlaying)
                {
                    _musicSource.volume = _currentTrackVolume * volumeToSet;
                }
            }
        }
    }
}
