using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelBlocker : MonoBehaviour
{
    public Button[] buttons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel",1);
        for (int i = 0; i< buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i< unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    // Update is called once per frame
}
