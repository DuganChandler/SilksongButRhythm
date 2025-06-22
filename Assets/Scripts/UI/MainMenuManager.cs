using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    private void Awake() {
        Screen.SetResolution(1920, 1080, true);
    }
    public void OnStart() {
       SceneManager.LoadScene("SongSelect"); 
    }

    public void OnQuit() {
        Application.Quit(); 
    }
}
