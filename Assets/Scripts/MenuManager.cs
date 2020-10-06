using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Toggle TrackToggle;
    [SerializeField] private Toggle EffectsToggle;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        //Debug.Log(AudioManager.Instance.isTrackEnabled);
        TrackToggle.isOn = AudioManager.Instance.isTrackEnabled;
        EffectsToggle.isOn = AudioManager.Instance.isEffectsEnabled;
        volumeSlider.value = AudioManager.Instance.max;
    }

    private void Update()
    {
        
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
