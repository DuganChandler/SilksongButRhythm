using System.Collections;
using TMPro;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    [Header("In-Scene Elements")]
    [SerializeField] private TextMeshProUGUI scoreTextBox;
    [Space(5)]
    [SerializeField] private GameObject rankDisplayBox;
    [SerializeField] private TextMeshProUGUI perfectTextBox;
    [SerializeField] private TextMeshProUGUI goodTextBox;
    [SerializeField] private TextMeshProUGUI mehTextBox;
    [SerializeField] private TextMeshProUGUI missTextBox;

    [Header("Settings")]
    [SerializeField] private float countScoreTime;
    [SerializeField] private float timeBetweenRanks;

    private Coroutine countTotalCoroutine;

    private void Start()
    {
        if (countTotalCoroutine != null) StopCoroutine(countTotalCoroutine);

        countTotalCoroutine = StartCoroutine(CountTotalCoroutine());
    }

    private IEnumerator CountTotalCoroutine()
    {
        float timer = 0;
        float totalScore = ScoreHolder.Instance.TotalScore;

        while (timer < countScoreTime)
        {
            timer += Time.fixedDeltaTime;
            scoreTextBox.text = $"{Mathf.Round(totalScore * (timer / countScoreTime))}";
            yield return null;
        }
        scoreTextBox.text = totalScore.ToString();

        rankDisplayBox.SetActive(true);
        perfectTextBox.text = ScoreHolder.Instance.PerfectScoreAmount.ToString();
        goodTextBox.text = ScoreHolder.Instance.GoodScoreAmount.ToString();
        mehTextBox.text = ScoreHolder.Instance.MehScoreAmount.ToString();
        missTextBox.text = ScoreHolder.Instance.MissAmount.ToString();

    }
}
