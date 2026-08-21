using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioInfomation
{
    [SerializeField, Header("サウンドのクリップ")]
    private AudioClip _clip;
    public AudioClip Clip { get { return _clip; } }

    [SerializeField, Header("サウンドのボリューム"), Range(0.0f, 1.0f)]
    private float _volume;
    public float Volume { get { return _volume; } }

    [SerializeField, Header("ループするか")]
    private bool _loop;
    public bool Loop { get { return _loop; } }

    [SerializeField, Header("再生のオフセット"), Range(0.0f, 1.0f)]
    private float _ofset;
    public float Ofset { get { return _ofset; } }

}
