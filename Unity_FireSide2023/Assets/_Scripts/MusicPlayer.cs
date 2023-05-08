using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip MainTheme;
    public AudioClip BattleTheme;

    [Range(0f, 1f)]
    public float maxVolume = .1f;

    public float fadeTime = 3;
    public bool isAttacked = false;

    private bool mainThemePlaying = false;

    private AudioSource mainThemeSource;
    private AudioSource battleThemeSource;

    private void Awake() {
        if (MainTheme == null || BattleTheme == null)
            return;
        
        AddAudioSources();
    }

    private void Update() {
        if (MainTheme == null || BattleTheme == null)
            return;


        if (!isAttacked && !mainThemePlaying) {
            StopAllCoroutines();
            StartCoroutine(FadeInOut(mainThemeSource,battleThemeSource));
            mainThemePlaying = true;
        }

        if (isAttacked && mainThemePlaying) {
            StopAllCoroutines();
            StartCoroutine(FadeInOut(battleThemeSource, mainThemeSource));
            mainThemePlaying = false;
        }

    }

    private void AddAudioSources() {
        GameObject MainThemeGO = new("Main Theme Music");
        MainThemeGO.transform.parent = transform;
        mainThemeSource = MainThemeGO.AddComponent<AudioSource>();
        mainThemeSource.Stop();
        mainThemeSource.playOnAwake = false;
        mainThemeSource.volume = 0;
        mainThemeSource.loop = true;
        mainThemeSource.clip = MainTheme;

        GameObject BattleThemeGO = new("Battle Theme Music");
        BattleThemeGO.transform.parent = transform;
        battleThemeSource = BattleThemeGO.AddComponent<AudioSource>();
        battleThemeSource.Stop();
        battleThemeSource.playOnAwake = false;
        battleThemeSource.volume = 0;
        battleThemeSource.loop = true;
        battleThemeSource.clip = BattleTheme;
    }

    private IEnumerator FadeInOut(AudioSource fadeIn,AudioSource fadeOut) {
        
        fadeIn.volume = 0;
        fadeIn.Play();

        float t = 0;

        while (t < fadeTime) {
            t += Time.deltaTime;
            fadeIn.volume = Mathf.Lerp(0, maxVolume, t / fadeTime);
            fadeOut.volume = Mathf.Lerp(maxVolume,0,t / fadeTime);
            yield return null;
        }

        fadeOut.volume = 0;
        fadeOut.Stop();
    }

    
}
