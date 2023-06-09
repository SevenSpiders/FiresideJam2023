using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// Generic Audio Manager
// Uses AudioSound.cs 
// Play sound clips with audioManager.Play("")

public class AudioManager : MonoBehaviour
{

    public List<AudioSound> sounds;

    
    void Awake() {


        foreach (AudioSound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.playOnAwake = false;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch == 0 ? 1f : s.pitch;
            s.source.loop = s.looping;
            
        }
    }
    


    
    public void Play(string name) {

        foreach (AudioSound s in sounds) {
            if (s.name != name) continue;
            s.source.Play(); 
        }
    }

    public void Play(string name, AudioParameters parameters) {
        foreach (AudioSound s in sounds) 
            if (s.name == name) {
                s.source.volume = parameters.volume;
                s.source.pitch = parameters.pitch;
                s.source.Play();
            }
    }

    public void Stop(string name, bool fade = false) {
        float fadeDuration = 0.3f;

        foreach (AudioSound s in sounds) 
            if (s.name == name) { 
                if (!fade) 
                    s.source.Stop();
                else 
                    s.source.DOFade(0f, fadeDuration).OnComplete(
                        () => {
                            s.source.volume = s.volume;
                            s.source.Stop();
                        }
                    );
            }
            
    }

}


[System.Serializable]
public struct AudioParameters {
    public float pitch;
    public float volume;

    public AudioParameters(float volume, float pitch) {
        this.volume = volume;
        this.pitch = pitch;
    }

    public static AudioParameters zero => new AudioParameters(0, 0);
    public static AudioParameters one => new AudioParameters(1f, 1f);
}
