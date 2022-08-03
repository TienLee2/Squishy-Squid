using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameAsset : MonoBehaviour
{
    private static GameAsset instance;

    public static GameAsset GetInstance()
    {
        return instance;
    }

    private void Awake()
    { 
        instance = this;
    }

    public Sprite pipeHeadSprite;
    public Transform pfPipeHead;
    public Transform pfPipeBody;

    public SoundAudioClip[] soundAudioClipArray;

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
