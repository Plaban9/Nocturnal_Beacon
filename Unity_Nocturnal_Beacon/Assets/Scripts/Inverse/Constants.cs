namespace Minimalist.Inverse
{
    /// <summary>
    /// Contains constants for various aspects of the game.
    /// </summary>
    public class Constants
    {
        public class Gameplay
        {
            public class Player
            {
                public const float JUMP_HEIGHT = 20f;
            }
        }

        public class General
        {
            public class Audio
            {
                public const float MAX_MASTER_VOLUME = 1f;
                public const float MAX_MUSIC_VOLUME = 1f;
                public const float MAX_SOUND_VOLUME = 1f;
            }
        }

        public class SaveSystem
        {
            #region GameStats
            public const string STAT_LAUNCH_COUNT = "launch_count";
            public const string STAT_TUTORIAL_COMPLETED = "tutorial_completed";
            #endregion


            #region GAMEPLAY_CONSTANTS
            public const string GAMEPLAY_LAST_LEVEL_UNLOCKED = "last_level_unlocked";
            public const string GAMEPLAY_LAST_LEVEL_PLAYED = "last_level_played";
            #endregion

            #region SETTINGS_CONSTANTS
            public const string SETTINGS_MASTER_VOLUME = "volume_master";
            public const string SETTINGS_SOUND_VOLUME = "volume_sound";
            public const string SETTINGS_MUSIC_VOLUME = "volume_music";
            #endregion
        }
    }
}
