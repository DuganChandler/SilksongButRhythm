using UnityEngine;

public class SFXManager : MonoBehaviour {
    [SerializeField] private SoundLibrary soundLibrary;
    [SerializeField] private AudioSource audioSource;

    public static SFXManager Instance { get; private set; }
    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }

    public void PlaySound(string soundName) {
        audioSource.PlayOneShot(soundLibrary.GetClipFromName(soundName));
    }

    public void PlaySoundByAudioClip(AudioClip sound) {
        audioSource.PlayOneShot(sound);
    }
}
