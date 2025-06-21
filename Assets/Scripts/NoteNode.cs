using System;
using UnityEngine;

public enum NoteType {
    Swat,
    Stomp,
    Spray,
    Bat
}

[System.Serializable]
public struct NoteData
{
    public float beat;
    public int lane;
    public NoteType noteType;

    private const int EASTLANE_NUM = 0;
    private const int SOUTHLANE_NUM = 1;
    private const int WESTLANE_NUM = 2;
    private const int NORTHLANE_NUM = 3;

    public readonly Direction LaneDirection => lane switch
    {
        NORTHLANE_NUM => Direction.North,
        EASTLANE_NUM => Direction.East,
        SOUTHLANE_NUM => Direction.South,
        WESTLANE_NUM => Direction.West,
        _ => throw new NotImplementedException(),
    };
}

public class NoteNode : MonoBehaviour {
    private static float beatsShownInAdvance;

    private Vector2 spawnPos;
    private Vector2 removePos;
    private Vector2 hitPos;
    private float beatOfThisNote;
    private float deathOffsetBeats;
    private float spawnBeat, hitBeat, deathBeat;
    
    public NoteData noteData;

    public static event Action<Direction> OnDeath;

    void Update() {
        float currentBeat = Composer.songPosInBeats;

        if (currentBeat < hitBeat) {
            float t = Mathf.InverseLerp(spawnBeat, hitBeat, currentBeat);
            transform.position = Vector2.Lerp(spawnPos, hitPos, t);
        } else {
            float t2 = Mathf.InverseLerp(hitBeat, deathBeat, currentBeat);
            transform.position = Vector2.Lerp(hitPos, removePos, t2);

            if (currentBeat >= deathBeat) {
                Destroy(gameObject);
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

    void OnDestroy() {
        OnDeath?.Invoke(noteData.LaneDirection);
    }
}
