using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pascal 
{
    [System.Serializable]
    public class AudioSound
    {
        
        string Name;
        public string name;
        public AudioClip clip;

        // [Range(0f, 1f)]
        public float volume = 1f;

        [HideInInspector] public bool looping;
        [HideInInspector] public float pitch;

        [HideInInspector] public AudioSource source;

    }
}
