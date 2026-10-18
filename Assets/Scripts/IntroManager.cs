using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject mainIntro;
    [SerializeField] private GameObject setting;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_InputField inputTextSensitivity;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TMP_InputField inputTextMusic;

    private Animator[] buttonAnimators;
    private TMP_Text[] buttonTexts;
    private Player player;
    [SerializeField] private int selectedIndex = 0;

    private void Awake()
    {
        Button[] buttons = { startButton, settingButton, exitButton };
        buttonAnimators = new Animator[buttons.Length];
        buttonTexts = new TMP_Text[buttons.Length];
        player = GameObject.Find("Player").GetComponent<Player>();

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null)
            {
                buttonAnimators[i] = buttons[i].GetComponent<Animator>();
                buttonTexts[i] = buttons[i].GetComponentInChildren<TMP_Text>();
            }
        }
    }

    private const string SensitivityKey = "MouseSensitivity";
    private const string MusicVolumeKey = "MusicVolume";

    private void Start()
    {
        SetupSensitivity();
        SetupMusicVolume();
        UpdateSelection();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            selectedIndex = Mathf.Max(0, selectedIndex - 1);
            UpdateSelection();

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            selectedIndex = Mathf.Min(buttonAnimators.Length - 1, selectedIndex + 1);
            UpdateSelection();

        }

        PlayerEnterKey();
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < buttonAnimators.Length; i++)
        {
            if (buttonTexts[i] != null && i != selectedIndex)
                buttonTexts[i].fontSize = 80f;

            if (buttonAnimators[i] != null)
            {
                buttonAnimators[i].enabled = i == selectedIndex;

                if (i == selectedIndex)
                {
                    buttonAnimators[i].Rebind();
                    buttonAnimators[i].Play("IntroGUIAnim", 0, 0f);
                    buttonAnimators[i].Update(0f);
                }
            }
        }
    }

    private void PlayerEnterKey() 
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            switch (selectedIndex)
            {
                case 1:
                    mainIntro.SetActive(false);
                    setting.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                case 2:
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #else
                        Application.Quit();
                    #endif
                    break;
                default:
                    SceneManager.LoadScene("SampleScene");
                    break;
            }
        }   
    }

    public void SettingOutButton()
    {
        mainIntro.SetActive(true);
        setting.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetupMusicVolume()
    {
        musicSlider.wholeNumbers = true;

        float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 50f);
        savedVolume = Mathf.Clamp(savedVolume, musicSlider.minValue, musicSlider.maxValue);

        musicSlider.value = savedVolume;
        inputTextMusic.text = Mathf.RoundToInt(savedVolume).ToString();

        musicSlider.onValueChanged.AddListener(value =>
        {
            int volume = Mathf.RoundToInt(value);

            inputTextMusic.text = volume.ToString();
            PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        });

        inputTextMusic.onEndEdit.AddListener(text =>
        {
            if (int.TryParse(text, out int volume))
            {
                volume = Mathf.Clamp(
                    volume,
                    Mathf.CeilToInt(musicSlider.minValue),
                    Mathf.FloorToInt(musicSlider.maxValue)
                );

                musicSlider.value = volume;
                inputTextMusic.text = volume.ToString();
                PlayerPrefs.SetFloat(MusicVolumeKey, volume);
            }
            else
            {
                inputTextMusic.text = Mathf.RoundToInt(musicSlider.value).ToString();
            }
        });
    }

    private void SetupSensitivity()
    {
        sensitivitySlider.wholeNumbers = true;

        float savedValue = PlayerPrefs.GetFloat(SensitivityKey, 5f);
        savedValue = Mathf.Clamp(savedValue, sensitivitySlider.minValue, sensitivitySlider.maxValue);

        sensitivitySlider.value = savedValue;
        inputTextSensitivity.text = Mathf.RoundToInt(savedValue).ToString();

        sensitivitySlider.onValueChanged.AddListener(value =>
        {
            int intValue = Mathf.RoundToInt(value);
            inputTextSensitivity.text = intValue.ToString();
            PlayerPrefs.SetFloat(SensitivityKey, intValue);
        });

        inputTextSensitivity.onEndEdit.AddListener(text =>
        {
            if (int.TryParse(text, out int value))
            {
                value = Mathf.Clamp(
                    value,
                    Mathf.CeilToInt(sensitivitySlider.minValue),
                    Mathf.FloorToInt(sensitivitySlider.maxValue)
                );

                sensitivitySlider.value = value;
                inputTextSensitivity.text = value.ToString();
                PlayerPrefs.SetFloat(SensitivityKey, value);
            }
            else
            {
                inputTextSensitivity.text = Mathf.RoundToInt(sensitivitySlider.value).ToString();
            }
        });
    }

}
