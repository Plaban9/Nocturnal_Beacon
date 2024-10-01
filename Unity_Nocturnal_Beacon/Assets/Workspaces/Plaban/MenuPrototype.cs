using Minimalist.Audio;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class MenuPrototype : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.PlayMusic(Minimalist.Audio.Music.MusicType.Menu);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
