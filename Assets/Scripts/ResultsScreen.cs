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
    [Space(5)]
    [SerializeField] private GameObject comboDisplayBox;
    [SerializeField] private TextMeshProUGUI comboTextBox;

    [Header("Settings")]
    [SerializeField] private float countScoreTime = 3;
    [SerializeField] private float timeBetweenSections = 0.5f;

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
            scoreTextBox.text = $"{Mathf.Round(totalScore * (timer / countScoreTime)):000000}";
            yield return null;
        }
        scoreTextBox.text = totalScore.ToString("000000");

        yield return new WaitForSeconds(timeBetweenSections);

        rankDisplayBox.SetActive(true);
        perfectTextBox.text = ScoreHolder.Instance.PerfectScoreAmount.ToString("000");
        goodTextBox.text = ScoreHolder.Instance.GoodScoreAmount.ToString("000");
        mehTextBox.text = ScoreHolder.Instance.MehScoreAmount.ToString("000");
        missTextBox.text = ScoreHolder.Instance.MissAmount.ToString("000");
        comboDisplayBox.SetActive(true);

        yield return new WaitForSeconds(timeBetweenSections);

        timer = 0;
        float maxCombo = ScoreHolder.Instance.MaxCombo;
        while (timer < countScoreTime)
        {
            timer += Time.fixedDeltaTime;
            comboTextBox.text = $"{Mathf.Round(maxCombo * (timer / countScoreTime)):000}";
            yield return null;
        }
        comboTextBox.text = maxCombo.ToString("000");
    }
}
