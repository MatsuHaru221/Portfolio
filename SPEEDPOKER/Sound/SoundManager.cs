using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-90)]
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    protected override bool dontDestroyOnLoad => true;

    [SerializeField, Header("BGMリスト")]
    private List<AudioInfomation> _bgmList = new List<AudioInfomation>();
    public int BGMCount { get { return _bgmList.Count; } }

    [SerializeField, Header("SEリスト")]
    private List<AudioInfomation> _seList = new List<AudioInfomation>();
    public int SECount { get { return _seList.Count; } }

    [SerializeField, Header("親ミキサー")]
    private AudioMixer _mixer;

    [SerializeField, Header("BGMミキサー")]
    private AudioMixerGroup _bgmMixier;

    [SerializeField, Header("SEミキサー")]
    private AudioMixerGroup _seMixier;

    [SerializeField, Header("BGMフェードアウト時間"), Range(0.0f, 5.0f)]
    private float _fadeoutRate = 0.0f;

    [SerializeField, Header("BGMフェードイン時間"), Range(0.0f, 5.0f)]
    private float _fadeinRate = 0.0f;

    [SerializeField]
    private List<AudioSource> _bgmSource = new List<AudioSource>();
    [SerializeField]
    private List<AudioSource> _seSource = new List<AudioSource>();

    private float _bgmVolume = 1;
    public void SetBGMVolume(float volume)
    {
        _bgmVolume = volume;
    }
    public float _GetBGMVolume { get { return _bgmVolume; } }
    private float _seVolume = 1;
    public void SetSEVolume(float volume)
    {
        _seVolume = volume;
    }
    public float _GetSEVolume { get { return _seVolume; } }

    private bool _onBGM = true;
    public void OnBGM() { _onBGM = true; }
    public void OffBGM() { _onBGM = false; }
    private bool _onSE = true;
    public void OnSE() { _onSE = true; }
    public void OffSE() { _onSE = false; }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _bgmList.Count; i++)
        {
            _bgmSource.Add(gameObject.AddComponent<AudioSource>());
            _bgmSource[i].clip = _bgmList[i].Clip;
            _bgmSource[i].loop = _bgmList[i].Loop;
            _bgmSource[i].volume = _bgmList[i].Volume;
            _bgmSource[i].time = _bgmList[i].Ofset;
            _bgmSource[i].outputAudioMixerGroup = _bgmMixier;
        }
        for (int i = 0; i < _seList.Count; i++)
        {
            _seSource.Add(gameObject.AddComponent<AudioSource>());
            _seSource[i].clip = _seList[i].Clip;
            _seSource[i].loop = _seList[i].Loop;
            _seSource[i].volume = _seList[i].Volume;
            _seSource[i].time = _seList[i].Ofset;
            _seSource[i].outputAudioMixerGroup = _seMixier;
        }
    }

    public void Update()
    {
        _mixer.SetFloat("BGM", _bgmVolume);
        _mixer.SetFloat("SE", _seVolume);
    }

    public void PlaySE(int index)   // 効果音を鳴らす(単発)
    {
        _seSource[index].Play();
    }

    public void PlaySELoop(int index)   // 効果音を鳴らす(ループ)
    {
        StartCoroutine(FadeInSE(index));
    }

    public void StopSELoop(int index)
    {
        StartCoroutine(FadeOutSE(index));
    }

    public void PlayBGM(int index)  // BGMを再生
    {
        StartCoroutine(FadeInBGM(index));
    }

    public void PlayBGMWithoutFade(int index)   //フェードインなしでBGM再生
    {
        _bgmSource[index].Play();
    }

    public void StopBGM()
    {
        StartCoroutine(FadeOutBGM(SerachPlayBGM()));
    }

    public void StopBGMAbs()
    {
        _bgmSource[SerachPlayBGM()].Stop();
    }

    public void StopSEAbs()
    {
        List<int> list = SerachPlaySE();
        for (int i = 0; i < list.Count; i++)
        {
            _seSource[list[i]].Stop();
        }
    }

    public void ChangeBGM(int index)    // BGMを変更
    {
        StartCoroutine(ChangeClip(SerachPlayBGM(), index));
    }

    private IEnumerator ChangeClip(int stopIndex, int playIndex)
    {
        if (stopIndex != -1)
        {
            Debug.Log("変更フェードアウト");
            float stoptimeCnt = 0;
            while (stoptimeCnt <= _fadeoutRate)
            {
                stoptimeCnt += Time.deltaTime;
                _bgmSource[stopIndex].volume = _bgmList[stopIndex].Volume - (_bgmList[stopIndex].Volume * (stoptimeCnt / _fadeoutRate));
                yield return null;
            }
            _bgmSource[stopIndex].volume = 0;
            _bgmSource[stopIndex].Stop();
        }

        _bgmSource[playIndex].volume = 0;
        _bgmSource[playIndex].Play();
        float timeCnt = 0;
        while (timeCnt <= _fadeoutRate)
        {
            timeCnt += Time.deltaTime;
            _bgmSource[playIndex].volume = _bgmList[playIndex].Volume * (timeCnt / _fadeoutRate);
            yield return null;
        }
    }

    private IEnumerator FadeOutBGM(int index)
    {
        if (index == -1) yield break;
        float timeCnt = 0;
        while (timeCnt <= _fadeoutRate)
        {
            timeCnt += Time.deltaTime;
            _bgmSource[index].volume = _bgmList[index].Volume - (_bgmList[index].Volume * (timeCnt / _fadeoutRate));
            yield return null;
        }

        _bgmSource[index].volume = 0;
        _bgmSource[index].Stop();
    }

    private IEnumerator FadeInBGM(int index)
    {
        _bgmSource[index].volume = 0;
        _bgmSource[index].Play();
        float timeCnt = 0;
        while (timeCnt <= _fadeinRate)
        {
            timeCnt += Time.deltaTime;
            _bgmSource[index].volume = _bgmList[index].Volume * (timeCnt / _fadeinRate);
            yield return null;
        }
    }
    private IEnumerator FadeOutSE(int index)
    {
        if (index == -1) yield break;
        float timeCnt = 0;
        while (timeCnt <= _fadeoutRate)
        {
            timeCnt += Time.deltaTime;
            _seSource[index].volume = _seList[index].Volume - (_seList[index].Volume * (timeCnt / _fadeoutRate));
            yield return null;
        }

        _seSource[index].volume = 0;
        _seSource[index].Stop();
    }

    private IEnumerator FadeInSE(int index)
    {
        _seSource[index].volume = 0;
        _seSource[index].Play();
        float timeCnt = 0;
        while (timeCnt <= _fadeinRate)
        {
            timeCnt += Time.deltaTime;
            _seSource[index].volume = _seList[index].Volume * (timeCnt / _fadeinRate);
            yield return null;
        }
    }

    public int SerachPlayBGM() // 再生中のBGMを探す(Listのインデックスを返し、ない場合は-1を返す)
    {
        for (int i = 0; i < _bgmSource.Count; i++)
        {
            Debug.Log("検索中");
            if (_bgmSource[i].isPlaying)
            {
                Debug.Log(i);
                return i;
            }
        }
        Debug.Log(-1);
        return -1;
    }

    public List<int> SerachPlaySE()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < _seSource.Count; i++)
        {
            Debug.Log("検索中");
            if (_seSource[i].isPlaying)
            {
                Debug.Log(i);
                list.Add(i);
            }
        }
        Debug.Log(-1);
        return list;
    }
}
