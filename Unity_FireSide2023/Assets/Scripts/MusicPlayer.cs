using System;
using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip MainTheme;
    public AudioClip BattleTheme;
    public AudioClip AtmosphereLoop;
    public bool startWithMusic = false;
    private AudioSource mainThemeSource;
    private AudioSource battleThemeSource;
    private AudioSource atmosphereSource;


    private bool mainThemePlaying = false;


    private void Awake()
    {
        if (MainTheme == null || BattleTheme == null || AtmosphereLoop == null)
            return;

        AddAudioSources();
    }

    private void Update()
    {
        if (MainTheme == null || BattleTheme == null || !startWithMusic)
            return;


        if (!PlayerAttributes.isAttacked && !mainThemePlaying)
        {
            StopAllCoroutines();
            StartCoroutine(FadeInOut(mainThemeSource, battleThemeSource));
            mainThemePlaying = true;
        }

        if (PlayerAttributes.isAttacked && mainThemePlaying)
        {
            StopAllCoroutines();
            StartCoroutine(FadeInOut(battleThemeSource, mainThemeSource));
            mainThemePlaying = false;
        }

    }

    private void AddAudioSources()
    {
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

        GameObject AtmoLoopGO = new("Atmosphere");
        AtmoLoopGO.transform.parent = transform;
        atmosphereSource = AtmoLoopGO.AddComponent<AudioSource>();
        atmosphereSource.Stop();
        atmosphereSource.playOnAwake = false;
        atmosphereSource.volume = PlayerAttributes.atmosphereVolume;
        atmosphereSource.loop = true;
        atmosphereSource.clip = AtmosphereLoop;
        atmosphereSource.Play();

    }

    private IEnumerator FadeInOut(AudioSource fadeIn, AudioSource fadeOut)
    {

        fadeIn.volume = 0;
        fadeIn.Play();

        float t = 0;

        while (t < PlayerAttributes.musicFadeTime)
        {
            t += Time.deltaTime;
            fadeIn.volume = Mathf.Lerp(0, PlayerAttributes.musicMaxVolume, t / PlayerAttributes.musicFadeTime);
            fadeOut.volume = Mathf.Lerp(PlayerAttributes.musicMaxVolume, 0, t / PlayerAttributes.musicFadeTime);
            yield return null;
        }

        fadeOut.volume = 0;
        fadeOut.Stop();
    }


}