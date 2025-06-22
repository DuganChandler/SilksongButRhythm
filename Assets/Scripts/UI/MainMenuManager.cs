using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip; 

    private void Awake() {
        Screen.SetResolution(1920, 1080, true);
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    public void OnStart() {
       SceneManager.LoadScene("SongSelect"); 
       audioSource.Stop();
    }

    public void OnQuit() {
        Application.Quit(); 
    }
}
