using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class MenuManager : MonoBehaviour
{
    [SerializeField] private Toggle TrackToggle;
    [SerializeField] private Toggle EffectsToggle;
    [SerializeField] private Slider volumeSlider;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        //Debug.Log(AudioManager.Instance.isTrackEnabled);
        TrackToggle.isOn = AudioManager.Instance.isTrackEnabled;
        EffectsToggle.isOn = AudioManager.Instance.isEffectsEnabled;
        volumeSlider.value = AudioManager.Instance.max;
    }

    private void Update()
    {
        // "Esc" disables full-screen in WebGL, so we also support "P"
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            _canvas.enabled = !_canvas.enabled;
        }
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ToggleTrack()
    {
        AudioManager.Instance.SetTrackActive(!AudioManager.Instance.isTrackEnabled);
    }

    public void ToggleEffect()
    {
        AudioManager.Instance.SetEffectsActive(!AudioManager.Instance.isEffectsEnabled);
    }

    public void OnValueChanged()
    {
        AudioManager.Instance.SetMax(volumeSlider.value);
    }
}
