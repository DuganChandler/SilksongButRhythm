using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public void OnStart() {
       SceneManager.LoadScene("SongSelect"); 
    }

    public void OnQuit() {
        Application.Quit(); 
    }
}
