using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        squidJump,
        Score,
        Lose,
        ButtonOver,
        ButtonClick,
    }

    public static void PlaySound(Sound sound)
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));   
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAsset.SoundAudioClip soundAudioClip in GameAsset.GetInstance().soundAudioClipArray)
        {
            if(soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

}
