using UnityEngine;

public class NoteNode : MonoBehaviour {
    private static float beatsShownInAdvance;

    private Vector2 spawnPos;
    private Vector2 removePos;
    private float beatOfThisNote;

    void Update() {
        float currentBeat = Composer.songPosInBeats;

        float t = Mathf.InverseLerp(
            beatOfThisNote - beatsShownInAdvance,
            beatOfThisNote,
            currentBeat
        );

        transform.position = Vector2.Lerp(spawnPos, removePos, t);

        if (t >= 1f) {
            gameObject.SetActive(false);
        }
    }

    public void Init(Vector2 spawnPos, Vector2 removePos, float beatsShown, float beatOfNote, float songPosInBeats) {
        beatsShownInAdvance = beatsShown;
        this.spawnPos = spawnPos;
        this.removePos = removePos;
        this.beatOfThisNote = beatOfNote;

        transform.position = spawnPos;
    }
}
