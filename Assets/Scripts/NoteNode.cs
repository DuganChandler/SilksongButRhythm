using UnityEngine;

public enum NoteType {
    Swat,
    Stomp,
    Spray,
    Bat
}

[System.Serializable]
public struct NoteData {
    public float beat;
    public int lane;
    public NoteType noteType;
}

public class NoteNode : MonoBehaviour {
    private static float beatsShownInAdvance;

    private Vector2 spawnPos;
    private Vector2 removePos;
    private Vector2 hitPos;
    private float beatOfThisNote;
    private float deathOffsetBeats;
    private float spawnBeat, hitBeat, deathBeat;

    void Update() {
        float currentBeat = Composer.songPosInBeats;

        if (currentBeat < hitBeat) {
            float t = Mathf.InverseLerp(spawnBeat, hitBeat, currentBeat);
            transform.position = Vector2.Lerp(spawnPos, hitPos, t);
        } else {
            float t2 = Mathf.InverseLerp(hitBeat, deathBeat, currentBeat);
            transform.position = Vector2.Lerp(hitPos, removePos, t2);

            if (currentBeat >= deathBeat) {
                gameObject.SetActive(false);
            }
        }
    }

    public void Init(Vector2 spawnPos, Vector2 hitPos, Vector2 removePos, float beatsShown, float beatOfNote, float deathOffsetBeats) {
        beatsShownInAdvance = beatsShown;
        this.spawnPos = spawnPos;
        this.removePos = removePos;
        this.beatOfThisNote = beatOfNote;
        this.deathOffsetBeats = deathOffsetBeats;

        spawnBeat = beatOfThisNote - beatsShownInAdvance;
        hitBeat   = beatOfThisNote;
        deathBeat = beatOfThisNote + deathOffsetBeats;

        transform.position = spawnPos;
    }
}
