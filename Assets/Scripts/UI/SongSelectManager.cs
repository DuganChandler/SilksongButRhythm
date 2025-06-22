using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongSelectManager : MonoBehaviour {
    public GameObject buttonPrefab;
    public Transform contentPanel;

    public List<GameObject> buttonObjects = new();
    private List<(Button, ChartData)> buttonList = new();

    public ChartLoader chartLoader;

    public GameObject confirmPanel;
    public Chart currentSelectedChart;

    public void ClearButtons() {
        foreach (var button in buttonObjects) {
            if (button) {
                Destroy(button);
            }
        }
        buttonList.Clear();
        buttonObjects.Clear();
    }

    void Start() {
       if (chartLoader != null) PopulateSongs(chartLoader.charts);
    }

    public void PopulateSongs(Dictionary<string, Chart> charts) {
        ClearButtons();

        var chartCount = charts.Count;

        if (chartCount <= 0) return;

        int i = 0;
        foreach (KeyValuePair<string, Chart> chart in charts) {
            ++i;
            ChartData currentChart = chart.Value.chartData;
            GameObject buttonObj = Instantiate(buttonPrefab, contentPanel);
            buttonObjects.Add(buttonObj);

            SongItem songItem = buttonObj.GetComponent<SongItem>();
            Transform buttonTransform = buttonObj.GetComponent<Transform>();

            songItem.songName.text = currentChart.Name;
            songItem.albumArt.sprite = currentChart.AlbumArt;

            Button currentButton = buttonObj.GetComponent<Button>();
            currentButton.onClick.AddListener(() => OnSongButtonClicked(currentChart.Name, i));

            buttonList.Add((currentButton, currentChart));
        }
        i = 0;
    }

    void OnSongButtonClicked(string name, int buttonSelected) {
        confirmPanel.SetActive(true);
        currentSelectedChart = chartLoader.GetChartByName(name);
    }

    public void OnModeSelect(string mode) {
        GameManager.Instance.currentSelectedChart = currentSelectedChart;
        GameManager.Instance.mode = mode;
        confirmPanel.SetActive(false);
        SceneManager.LoadScene("ChartPlayer");
    }

    public void OnCancel() {
        currentSelectedChart = null;
        confirmPanel.SetActive(false);
    }
}
