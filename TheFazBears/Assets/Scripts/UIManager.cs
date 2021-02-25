using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public UIView settingsView;
    public UIView pauseView;
    public UIView gameOverView;

    public Slider masterVol;
    public Slider musticVol;
    public Slider SFXVol;

    public AudioMixer master;
    public AudioMixer music;
    public AudioMixer sfx;

    private void Start()
    {
        SetUpAudio();
    }

    private void SetUpAudio()
    {
        if (PlayerPrefs.HasKey("masterVol"))
        {
            float vol = PlayerPrefs.GetFloat("masterVol");
            master.SetFloat("MasterVol", Mathf.Log10(vol) * 20);
            masterVol.value = vol;
        }
        else
        {
            master.SetFloat("MasterVol", Mathf.Log10(1) * 20);
        }

        if (PlayerPrefs.HasKey("musicVol"))
        {
            float vol = PlayerPrefs.GetFloat("musicVol");
            music.SetFloat("MusicVol", Mathf.Log10(vol) * 20);
            musticVol.value = vol;
        }
        else
        {
            music.SetFloat("MasterVol", Mathf.Log10(1) * 20);
        }

        if (PlayerPrefs.HasKey("sfxVol"))
        {
            float vol = PlayerPrefs.GetFloat("sfxVol");
            sfx.SetFloat("SFXVol", Mathf.Log10(vol) * 20);
            SFXVol.value = vol;
        }
        else
        {
            sfx.SetFloat("MasterVol", Mathf.Log10(1) * 20);
        }


        masterVol.onValueChanged.AddListener(delegate { ChangeVol(masterVol); });
        musticVol.onValueChanged.AddListener(delegate { ChangeVol(musticVol); });
        SFXVol.onValueChanged.AddListener(delegate { ChangeVol(SFXVol); });
    }

    public void EscapeKeyPressed()
    {
        if (!pauseView.IsHidden)
        {
            if (settingsView.IsHidden)
            {
                pauseView.Hide();
                PauseToggle(false);
            }
            else
            {
                pauseView.Show();
                settingsView.Hide();
            }
            
        }
        else
        {
            pauseView.Show();
            PauseToggle(true);
        }
    }

    public void PauseToggle(bool pause)
    {
        if (pause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else if (!pause)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }

    public void Replay()
    {
        PauseToggle(false);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeVol(Slider slider)
    {
        float vol = slider.value;

        if(slider == masterVol)
        {
            PlayerPrefs.SetFloat("masterVol", vol);
            master.SetFloat("MasterVol", Mathf.Log10(vol) * 20);
        }
        else if(slider == musticVol)
        {
            PlayerPrefs.SetFloat("musicVol", vol);
            music.SetFloat("MusicVol", Mathf.Log10(vol) * 20);
        }
        else if(slider == SFXVol)
        {
            PlayerPrefs.SetFloat("sfxVol", vol);
            sfx.SetFloat("SFXVol", Mathf.Log10(vol) * 20);
        }
    }
}
