using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private FMOD.Studio.EventInstance levelMusic;
    private FMOD.Studio.EventInstance pauseMusic;
    public FMODUnity.EventReference levelMusicEvent;
    [SerializeField] private FMODUnity.StudioEventEmitter pauseMusicEmitter;

    void Awake() {
        levelMusic = FMODUnity.RuntimeManager.CreateInstance(levelMusicEvent);
        levelMusic.setParameterByName("Character Switch", 0);
    }

    void OnEnable()
    {
        levelMusic.start();
        levelMusic.setPaused(false);
    }

    void OnDisable()
    {
        levelMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        pauseMusicEmitter.Stop();
    }

    public void SwitchToPauseMusic() {
        levelMusic.setPaused(true);
        pauseMusicEmitter.Play();
    }

    public void SwitchToLevelMusic() {
        levelMusic.setPaused(false);
        pauseMusicEmitter.Stop();
    }

    public void SwitchActiveCharacterMusic(bool isSkeleton) {
        if (isSkeleton) {
            levelMusic.setParameterByName("Character Switch", 1);
        } else {
            levelMusic.setParameterByName("Character Switch", 0);
        }
    }
}
