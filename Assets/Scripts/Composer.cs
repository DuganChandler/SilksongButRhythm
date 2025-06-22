using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Composer : MonoBehaviour {
    public static float songPosInBeats;

    public static float beatsShownInAdvance = 4f;

    [SerializeField] private float bpm;
    [SerializeField] private Transform[] noteSpawnPoints;
    [SerializeField] private Transform[] noteDeathPoints;
    [SerializeField] private Transform[] noteHitPoints;
    [SerializeField] private GameObject noteNodePrefab;
    [SerializeField] private ChartLoader chartLoader;

    [SerializeField] private GameObject songStartingPanel;
    [SerializeField] private TextMeshProUGUI songStartingText;

    public static event Action OnSongEnd;

    private AudioSource audioSource;
    private double dspStartTime;
    private float secPerBeat;
    private int nextIndex = 0;
    private List<NoteData> notes;// = {1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 12f, 13f, 14f, 15f, 17f, 18f, 19f, 20f};
    private Chart chart;
    private AudioClip song;

    public bool songStarted = false;
    void Awake() {
    }

    void OnEnable() {
        NoteNode.OnDeath += HandleNoteDeath;
    }

    void OnDisable() {
        NoteNode.OnDeath -= HandleNoteDeath;
    }

    void Start() {
        chart = GameManager.Instance.currentSelectedChart;
        song = chart.chartData.Song;
        // Debug.Log(chart);
        secPerBeat = 60f / chart.chartData.Bpm;
        notes = chart.notes;

        // Debug.Log(notes);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = song;

        StartCoroutine(DelaySongStart());        
    }

    IEnumerator DelaySongStart() {
        songStartingText.text = "Song is about to start!";
        songStartingPanel.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        
        songStartingText.text = "GO!";

        yield return new WaitForSeconds(0.75f);

        songStartingPanel.SetActive(false);

        audioSource.Play();
        dspStartTime = AudioSettings.dspTime;
        songStarted = true;
        yield return null;
    }

    void Update() {
        if (!songStarted) return;
        float songPosition = (float)(AudioSettings.dspTime - dspStartTime);
        songPosInBeats = songPosition / secPerBeat;

        if (nextIndex < notes.Count && notes[nextIndex].beat < songPosInBeats + beatsShownInAdvance) {
            var noteNode = Instantiate(noteNodePrefab).GetComponent<NoteNode>();
            noteNode.name = "Note " +  notes[nextIndex].beat.ToString();
            noteNode.noteData = notes[nextIndex];

            LaneManager.Instance.AddNoteToLane(noteNode);

            Vector2 spawnPos = noteSpawnPoints[noteNode.noteData.lane].position;
            Vector2 removePos = noteDeathPoints[noteNode.noteData.lane].position;
            Vector2 hitPos = noteHitPoints[noteNode.noteData.lane].position;
            noteNode.Init(spawnPos, hitPos, removePos, beatsShownInAdvance, noteNode.noteData.beat, 0.5f);

            nextIndex++;
        }

        if (songPosition > song.length) {
            songStarted = false;
            OnSongEnd?.Invoke();
            Debug.Log("Song is complete!");
        }
    }

    void HandleNoteDeath(Direction lane) {
        //Debug.Log("YOU GOT REKT LOSER");
    }
}
