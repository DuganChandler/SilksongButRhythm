using UnityEngine;

public class Composer : MonoBehaviour {
    float songPosition;
    float songPosInBeats;
    float secPerBeat;
    // how much time (in secs) has passed since song started
    float dsptimesong;

    // song info
    [SerializeField] private float bpm;
    float[] notes = {1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 12f, 13f, 14f, 15f, 17f, 18f, 19f, 20f};
    int nextIndex = 0;

    public AudioClip song;

    public static float beatsShownInAdvance = 1f;

    private AudioSource audioSource;

    void Start() {
        secPerBeat = 60f / bpm;

        dsptimesong = (float) AudioSettings.dspTime;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = song;
        audioSource.Play();
    }

    void Update() {
        songPosition = (float) (AudioSettings.dspTime - dsptimesong); 

        songPosInBeats = songPosition / secPerBeat;

        if (nextIndex < notes.Length && notes[nextIndex] < songPosInBeats + beatsShownInAdvance) {
            // Instantiated
            Debug.Log(notes[nextIndex]);
            nextIndex++;
        }
    }
}
