using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _master;
    [SerializeField] private string _type = "Master";

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(_type))
            LoadVolume();
        else
            SetVolume();
    }

    public void SetVolume()
    {
        float volume = _master.value;
        _mixer.SetFloat(_type, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(_type, volume);
    }

    private void LoadVolume()
    {
        _master.value = PlayerPrefs.GetFloat(_type);

        SetVolume();
    }
}
