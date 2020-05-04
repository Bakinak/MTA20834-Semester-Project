using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager 
{

    public enum Sound
    {
        backgroundMusic,
        splash,
        fishReel,
        fishCaughtFailed,
        fishCaughtSucces,
        waves,
        rain,
        moveHook,
        questComplete,
        fishBiteHook,
    }

    public static void PlaySound(Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound)); // plays the sound that we are looking for in the array once
        audioSource.volume = 0.05f;
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray) //cycle through the gameassets (the sounds we have in the array)
        {
            if(soundAudioClip.sound == sound) //if the soundaudioclip equals the sound that we are looking for, then play the audioclip 
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound" + sound + " not found");
        return null;
    }
}
