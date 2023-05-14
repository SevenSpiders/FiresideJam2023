using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource music;
    public float t_wait = 60f;


    void Start() {
        // music.Play();
        InvokeRepeating(nameof(PlayMusic),0, t_wait);
    }


    void PlayMusic() {
        Debug.Log("play music");
        music.Play();
    }
}
