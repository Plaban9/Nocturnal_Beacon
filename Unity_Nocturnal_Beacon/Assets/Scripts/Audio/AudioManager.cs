using Minimalist.Audio.Music;
using Minimalist.Audio.Sound;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Minimalist.Audio
{
    /// <summary>
    /// Handles audio data (SFX and Music) for a scene.
    /// It is a non-persistant singleton.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region Instance Dictionary for scenes
        private static AudioManager _currentInstance;
        private static Dictionary<UnityEngine.SceneManagement.Scene, AudioManager> _sceneToAudioManagerDictionary;
        #endregion

        public static AudioManager Instance { get; private set; }

        [SerializeField] private bool _stopMusicOnSceneExit = false;
        [SerializeField] private AudioLibrary _audioLibrary;

        private void Awake()
        {
            //For a scenario where we loaded scene music in memory.
            //InitializeSceneDictionary();

            Instance = this;
        }

        private void OnDisable()
        {
            if (_stopMusicOnSceneExit)
            {
                try
                {
                    MusicManager.Instance?.StopMusic();
                }
                catch (Exception exception)
                {
                    D("Error in stopping Music: " + exception.Message);
                }
            }

            Instance = null;
            _currentInstance = null;
        }

        private void Start()
        {
            if (_audioLibrary == null)
            {
                D("Audio Library is not set");
            }
        }

        public static void SetMasterVolume(float volumeToSet)
        {
            try
            {
                MusicManager.Instance?.SetMasterVolume(volumeToSet);
            }
            catch (Exception exception)
            {
                D("Error in setting Master Volume for Music: " + exception.Message);
            }

            try
            {
                SoundManager.Instance?.SetMasterVolume(volumeToSet);
            }
            catch (Exception exception)
            {
                D("Error in setting Master Volume for Music: " + exception.Message);
            }
        }

        #region Music
        /// <summary>
        /// Plays Music by supplied Music Type.
        /// Note: Has additional default parameters.
        /// </summary>
        /// <param name="musicType"></param>
        /// <param name="fadeDuration"></param>
        /// <param name="loop"></param>
        public static void PlayMusic(MusicType musicType, float fadeDuration = 0.5f, bool loop = true)
        {
            if (!PlayMusic(Instance._audioLibrary.GetMusicFromType(musicType), fadeDuration, loop))
            {
                D("Unable to play Music Track with type: " + musicType.ToString());
            }
        }

        /// <summary>
        /// Plays Music by supplied music name.
        /// Note: Has additional default parameters.
        /// </summary>
        /// <param name="musicName"></param>
        /// <param name="fadeDuration"></param>
        /// <param name="loop"></param>
        public static void PlayMusic(string musicName, float fadeDuration = 0.5f, bool loop = true)
        {
            if (!PlayMusic(Instance._audioLibrary.GetMusicFromName(musicName), fadeDuration, loop))
            {
                D("Unable to play Music Track with name: " + musicName);
            }
        }

        private static bool PlayMusic(MusicTrack musicTrack, float fadeDuration = 0.5f, bool loop = true)
        {
            if (musicTrack != null)
            {
                try
                {
                    MusicManager.Instance?.PlayMusic(musicTrack.audioClip, fadeDuration, loop ? loop : musicTrack.shouldLoop, musicTrack.volume);
                    return true;
                }
                catch (Exception exception)
                {
                    D("Error in playing Music: " + exception.Message);
                }
            }

            return false;
        }

        public static void SetMusicVolume(float volumeToSet)
        {
            try
            {
                MusicManager.Instance?.SetMusicVolume(volumeToSet);
            }
            catch (Exception exception)
            {
                D("Error in setting Volume for Music: " + exception.Message);
            }
        }
        #endregion

        #region SFX
        /// <summary>
        /// Plays SFX by supplied Sound Type.
        /// </summary>
        /// <param name="sfxType"></param>
        public static void PlaySFX(SoundType sfxType)
        {
            if (!PlaySFX(Instance._audioLibrary.GetSFXFromType(sfxType), false, Vector3.zero))
            {
                D("Unable to play SFX with type: " + sfxType.ToString());
            }
        }

        /// <summary>
        /// Plays SFX by supplied SFX name.
        /// </summary>
        /// <param name="sfxName"></param>
        public static void PlaySFX(string sfxName)
        {
            if (!PlaySFX(Instance._audioLibrary.GetSFXFromName(sfxName), false, Vector3.zero))
            {
                D("Unable to play SFX with name: " + sfxName);
            }
        }

        /// <summary>
        /// Plays SFX by supplied Sound Type at given position.
        /// </summary>
        /// <param name="sfxType"></param>
        /// <param name="position"></param>
        public static void PlaySFX3D(SoundType sfxType, Vector3 position)
        {
            if (!PlaySFX(Instance._audioLibrary.GetSFXFromType(sfxType), true, position))
            {
                D("Unable to play 3D SFX with type: " + sfxType.ToString());
            }
        }

        /// <summary>
        /// Plays SFX by supplied Sound name at given position.
        /// </summary>
        /// <param name="sfxName"></param>
        /// <param name="position"></param>
        public static void PlaySFX3D(string sfxName, Vector3 position)
        {
            if (!PlaySFX(Instance._audioLibrary.GetSFXFromName(sfxName), true, position))
            {
                D("Unable to play 3D SFX with name: " + sfxName);
            }
        }

        private static bool PlaySFX(SoundEffect soundEffect, bool play3D, Vector3 position)
        {
            if (soundEffect != null)
            {
                try
                {
                    AudioClip audioClip = soundEffect.GetRandomClip();

                    if (audioClip != null)
                    {
                        if (play3D)
                        {
                            SoundManager.Instance?.PlaySound3D(audioClip, position, soundEffect.volume);
                        }
                        else
                        {
                            SoundManager.Instance?.PlaySound2D(audioClip, soundEffect.volume);
                        }

                        return true;
                    }
                }
                catch (Exception exception)
                {
                    D("Error in playing SFX: " + exception.Message);
                }
            }

            return false;
        }

        public static void SetSFXVolume(float volumeToSet)
        {
            try
            {
                SoundManager.Instance?.SetSFXVolume(volumeToSet);
            }
            catch (Exception exception)
            {
                D("Error in setting Volume for Music: " + exception.Message);
            }
        }
        #endregion

        #region Utility
        private void InitializeSceneDictionary()
        {
            if (_sceneToAudioManagerDictionary == null)
            {
                _sceneToAudioManagerDictionary = new Dictionary<UnityEngine.SceneManagement.Scene, AudioManager>();
            }

            if (_sceneToAudioManagerDictionary.ContainsKey(gameObject.scene))
            {
                _currentInstance = _sceneToAudioManagerDictionary[gameObject.scene];
                Destroy(this.gameObject);
            }
            else
            {
                _sceneToAudioManagerDictionary[gameObject.scene] = this;
                _currentInstance = _sceneToAudioManagerDictionary[gameObject.scene];
            }
        }

        public AudioManager GetInstanceBasedOnScene(UnityEngine.SceneManagement.Scene scene)
        {
            if (_sceneToAudioManagerDictionary.ContainsKey(scene))
            {
                return _sceneToAudioManagerDictionary[scene];
            }

            return null;
        }
        #endregion

        private static void D(string message)
        {
            Debug.Log("<<AudioManager>> " + message);
        }

    }
}
