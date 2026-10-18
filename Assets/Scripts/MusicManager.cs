using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    private const string MusicVolumeKey = "MusicVolume";
    private GUIManager guiManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        guiManager = GameObject.Find("GUIManager").GetComponent<GUIManager>();
    }

    private void Update()
    {
        if (guiManager.esc)
        {
            audioSource.pitch = 0;
        }else if (!guiManager.esc)
        {
            audioSource.pitch = 1;
        }
    }

    private void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat(MusicVolumeKey, 50f) / 100f;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume / 100f);
    }
}
