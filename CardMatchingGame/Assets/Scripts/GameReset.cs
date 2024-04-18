using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameReset : MonoBehaviour
{
    private Button btnObj;

    // Start is called before the first frame update
    void Start()
    {
        btnObj = GetComponent<Button>();
        btnObj.onClick.AddListener(() => {
            GameRestart();
        });
    }

    private void GameRestart()
    {
        GamaManager.instance.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
