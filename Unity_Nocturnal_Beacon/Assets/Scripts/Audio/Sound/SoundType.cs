namespace Minimalist.Audio.Sound
{
    /// <summary>
    /// Catalog of Sounds used in game.
    /// Note: Actions are separted by _ with tags/type.
    /// </summary>
    public enum SoundType
    {
        #region Player
        Player_Spawn,
        Player_Jump,
        Player_Hit,
        Player_Death,
        #endregion

        #region Companion
        Companion_DogBark,
        Companion_DogInteract,
        Companion_DogDetect,
        #endregion

        #region Enemy
        Enemy_Death,
        Enemy_Attack,
        Enemy_Detect,
        Enemy_Footsteps,
        Enemy_Chase,
        Enemy_Howl,
        #endregion

        #region Gameplay
        Gameplay_RealmChange,
        Gameplay_LevelStart,
        Gameplay_LevelComplete,
        Gameplay_LevelOver,
        Gameplay_LevelRestart,
        #endregion

        #region UI
        UI_Click,
        UI_Quit,
        UI_Hover,
        #endregion

        #region Transition
        Transition_Clap
        #endregion
    }
}
