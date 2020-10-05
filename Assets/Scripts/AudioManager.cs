using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    [SerializeField] private AudioSource audioSource;
    public float max = 0.5f;
    public bool isTrackEnabled = true;
    public bool isEffectsEnabled = true;

    private float _t = 0;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource.volume = max;
        }
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

    public void PlaySound(AudioClip audioClip)
    {
        if (!isEffectsEnabled) return;
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
