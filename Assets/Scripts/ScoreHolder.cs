using UnityEngine;

public class ScoreHolder : MonoBehaviour
{
    private int perfectScores;
    private int goodScores;
    private int mehScores;
    private int misses;

    private void OnEnable()
    {
        LaneManager.NoteCompletedAction += RegisterNote;
    }

    private void OnDisable()
    {
        LaneManager.NoteCompletedAction -= RegisterNote;
    }

    private void RegisterNote(Rank rank)
    {
        switch (rank)
        {
            case Rank.Perfect:
                perfectScores++;
                break;
            case Rank.Good:
                goodScores++;
                break;
            case Rank.Meh:
                mehScores++;
                break;
            case Rank.Miss:
                misses++;
                break;
        }
}

[ContextMenu("Print Scores")]
    private void PrintScores()
    {
        print("Perfect: " + perfectScores +
            "\nGood: " + goodScores +
            "\nMeh: " + mehScores +
            "\nMiss: " + misses);
    }
}

public enum Rank
{
    Perfect,
    Good,
    Meh,
    Miss
}