using System;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float noteCheckThreshold = 3;
    [SerializeField] private float perfectDistance = 0.25f;
    [SerializeField] private float goodDistance = 0.5f;
    [SerializeField] private float mehDistance = 1f;

    private Queue<NoteNode> northLane = new();
    private Queue<NoteNode> southLane = new();
    private Queue<NoteNode> eastLane = new();
    private Queue<NoteNode> westLane = new();

    public static event Action<Rank> NoteCompletedAction;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void OnEnable()
    {
        NoteNode.OnDeath += RemoveNodeFromLane;
        NoteNode.OnHitFood += CountMiss;
    }

    private void OnDisable()
    {
        NoteNode.OnDeath -= RemoveNodeFromLane;
        NoteNode.OnHitFood -= CountMiss;
    }

    #region Lane Methods
    public void AddNoteToLane(NoteNode note)
    {
        switch (note.noteData.LaneDirection)
        {
            case Direction.North:
                northLane.Enqueue(note);
                break;

            case Direction.South:
                southLane.Enqueue(note);
                break;

            case Direction.East:
                eastLane.Enqueue(note);
                break;

            case Direction.West:
                westLane.Enqueue(note);
                break;
        };
    }

    private void RemoveNodeFromLane(Direction lane)
    {
        switch (lane)
        {
            case Direction.North:
                if (northLane.Count > 0) northLane.Dequeue();
                break;

            case Direction.South:
                if (southLane.Count > 0) southLane.Dequeue();
                break;

            case Direction.East:
                if (eastLane.Count > 0) eastLane.Dequeue();
                break;

            case Direction.West:
                if (westLane.Count > 0) westLane.Dequeue();
                break;
        };
    }

    private Queue<NoteNode> GetLane(Direction direction) => direction switch
    {
        Direction.North => northLane,
        Direction.South => southLane,
        Direction.East => eastLane,
        Direction.West => westLane,
        _ => throw new System.NotImplementedException(),
    };

    private void PrintLane(Direction direction)
    {
        Queue<NoteNode> lane = GetLane(direction);

        string s = "";
        foreach (NoteNode note in lane)
        {
            s += note.noteData.beat + ", ";
        }

        print(s);
    }
    #endregion

    #region Checking Hits

    public void CheckHit(Direction lane, NoteType action)
    {
        // Getting the note in the right lane
        NoteNode noteToCheck;
        bool canHitSomething = lane switch
        {
            Direction.North => northLane.TryPeek(out noteToCheck),
            Direction.South => southLane.TryPeek(out noteToCheck),
            Direction.East => eastLane.TryPeek(out noteToCheck),
            Direction.West => westLane.TryPeek(out noteToCheck),
            _ => throw new System.NotImplementedException(),
        };

        // Return if no note in lane
        if (!canHitSomething)
        {
            return;
        }

        // Check if note is within threshold
        float distanceToNote = Mathf.Abs(Composer.songPosInBeats - noteToCheck.noteData.beat);
        if (distanceToNote > noteCheckThreshold) 
        {
            return;
        }

        // Check action
        if (action != noteToCheck.noteData.noteType)
        {
            //NoteCompletedAction?.Invoke(Rank.Miss);
            //Destroy(noteToCheck.gameObject);
            //return;
        }

        //// Check accuracy
        if (distanceToNote <= perfectDistance)
        {
            NoteCompletedAction?.Invoke(Rank.Perfect);
            print("Perfect: " + distanceToNote);
        }
        else if (distanceToNote <= goodDistance)
        {
            NoteCompletedAction?.Invoke(Rank.Good);
            print("Good: " + distanceToNote);
        }
        else if (distanceToNote <= mehDistance)
        {
            NoteCompletedAction?.Invoke(Rank.Meh);
            print("Meh: " + distanceToNote);
        }
        else
        {
            NoteCompletedAction?.Invoke(Rank.Miss);
            print("Miss:" + distanceToNote);
        }

        Destroy(noteToCheck.gameObject);
    }

    private void CountMiss()
    {
        NoteCompletedAction?.Invoke(Rank.Miss);
        print("Miss: Late");
    }

    #endregion
}
