using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void ToRecreatedScene() {
        SceneManager.LoadScene(1);
    }

    public void ToDIScene() {
        SceneManager.LoadScene(2);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
