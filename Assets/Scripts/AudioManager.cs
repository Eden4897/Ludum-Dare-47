using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;
    [SerializeField] private AudioSource audioSource;
    public float max = 0.5f;
    public bool isTrackEnabled = true;
    public bool isEffectsEnabled = true;

    public AudioClip towerBuildAudio;
    public AudioClip towerBreakAudio;
    public AudioClip enemyDeathAudio;
    public AudioClip menuHoverAudio;
    public AudioClip menuSelectAudio;

    // public int audioSourcePoolLimit = 5;
    private float _t = 0;
    // private AudioSource[] _audioSourcesPool;
    // private int _audioSourcesPoolIndex;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource.volume = max;
        }
    }

    // private void OnEnable()
    // {
    //     _audioSourcesPool = new AudioSource[audioSourcePoolLimit];
    //     for (int i = 0; i < audioSourcePoolLimit; i++)
    //     {
    //         _audioSourcesPool[i] = gameObject.AddComponent<AudioSource>();
    //     }
    // }

    public void PlayOne(AudioClip clip)
    {
        if (!isEffectsEnabled)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
        // _audioSourcesPool[_audioSourcesPoolIndex].PlayOneShot(clip);
        // _audioSourcesPoolIndex = (_audioSourcesPoolIndex + 1) % _audioSourcesPool.Length;
    }

    private void Update()
    {
        _t += Time.deltaTime;
    }

    public void SetMax(float value)
    {
        float originalPercentageInGame = audioSource.volume / max;
        max = value;
        audioSource.volume = Mathf.Lerp(0, max, originalPercentageInGame);
    }

    [Obsolete("Use PlayOne instead", true)]
    public void PlaySound(AudioClip audioClip)
    {
        AudioSource source = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        source.clip = audioClip;
        source.volume = max;
        source.Play();
        StartCoroutine(DeleteSource(source));
    }

    private IEnumerator DeleteSource(AudioSource audioSource)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(audioSource);
    }

    public void SetTrackActive(bool state)
    {
        if (_t <= 0.01) return;
        isTrackEnabled = state;
        if (state)
        {
            audioSource.volume = max;
        }
        else
        {
            audioSource.volume = 0;
        }
    }

    public void SetEffectsActive(bool state)
    {
        if (_t <= 0.01) return;
        isEffectsEnabled = state;
    }
}
