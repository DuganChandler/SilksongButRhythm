using System.Collections.Generic;
using UnityEngine;

public class Composer : MonoBehaviour {
    public static float songPosInBeats;

    public static float beatsShownInAdvance = 4f;

    [SerializeField] private float bpm;
    [SerializeField] private AudioClip song;
    [SerializeField] private Transform[] noteSpawnPoints;
    [SerializeField] private Transform[] noteDeathPoints;
    [SerializeField] private GameObject noteNodePrefab;

    private AudioSource audioSource;
    private double dspStartTime;
    private float secPerBeat;
    private int nextIndex = 0;
    private List<NoteData> notes;// = {1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 12f, 13f, 14f, 15f, 17f, 18f, 19f, 20f};
    private Chart chart;

    void Awake() {
        chart = GameManager.Instance.currentSelectedChart;
    }

    void Start() {
        secPerBeat = 60f / chart.chartData.Bpm;
        notes = chart.notes;

        Debug.Log(notes);

        dspStartTime = AudioSettings.dspTime;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = song;
        audioSource.Play();
    }

    void Update() {
        float songPosition = (float)(AudioSettings.dspTime - dspStartTime);
        songPosInBeats = songPosition / secPerBeat;

        if (nextIndex < notes.Count && notes[nextIndex].beat < songPosInBeats + beatsShownInAdvance) {
            var noteNode = Instantiate(noteNodePrefab).GetComponent<NoteNode>();
            Vector2 spawnPos = noteSpawnPoints[0].position;
            Vector2 removePos = noteDeathPoints[0].position;
            noteNode.Init(spawnPos, removePos, beatsShownInAdvance, notes[nextIndex].beat, songPosInBeats);

            nextIndex++;
        }
    }
}
