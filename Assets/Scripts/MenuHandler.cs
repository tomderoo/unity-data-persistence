using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(1000)]
public class MenuHandler : MonoBehaviour
{
    public InputField PlayerNameInputField;

    // Start is called before the first frame update
    void Start()
    {
        // Load any previously saved data and the input field to a saved player name.
        if (SaveDataHandler.Instance.CurrentPlayerName != null)
        {
            PlayerNameInputField.text = SaveDataHandler.Instance.CurrentPlayerName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        // If we have an entered name, start the game. Also save the entered name.
        if (PlayerNameInputField.text.Length > 0)
        {
            SaveDataHandler.Instance.CurrentPlayerName = PlayerNameInputField.text;
            SaveDataHandler.Instance.SavePlayerData();
            SceneManager.LoadScene(1);
        }
        // Else focus on the input field (consider adding a message or animation later).
        else
        {
            PlayerNameInputField.Select();
        }
    }

    public void ExitGame()
    {
        
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void ResetGame()
    {
        SaveDataHandler.Instance.ResetPlayerData();
        SceneManager.LoadScene(0);
    }
}
