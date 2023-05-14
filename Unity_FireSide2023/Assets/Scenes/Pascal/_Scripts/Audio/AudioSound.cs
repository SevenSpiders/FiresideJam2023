using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public class AudioSound
    {
        
        string Name;
        public string name;
        public AudioClip clip;

        // [Range(0f, 1f)]
        public float volume = 1f;

        [HideInInspector] public bool looping;
        public float pitch = 1f;

        public void Reset() {
            source.volume = volume;
            source.pitch = pitch;
        }

        [HideInInspector] public AudioSource source;

    }

