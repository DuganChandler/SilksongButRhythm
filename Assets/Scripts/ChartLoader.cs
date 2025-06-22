using System;
using System.Collections.Generic;
using UnityEngine;

public class ChartLoader : MonoBehaviour {
    public string pathToCharts = "Charts/";
    // [SerializeField] private List<string> chartFileNames;
    [SerializeField] private ChartLibrary chartLibrary; 

    public Dictionary<string, Chart> charts = new();

    void Awake() {
        LoadCharts();     
    }

    public void LoadCharts() {
        foreach(ChartData chart in chartLibrary.Charts) {
            string fileName = chart.Name;
            var csv = Resources.Load<TextAsset>($"{pathToCharts}{fileName}");
            if (csv == null) {
                Debug.LogError($"Missing CSV: {fileName}");
                continue;
            }

            Chart newChart = new() {
                notes = ParseCsv(csv),
                chartData = GetChartDataByName(fileName)
            };

            charts[fileName] = newChart;

            // foreach (var note in newChart.notes) {
            //     Debug.Log(note.beat);
            // }
        }
    }

    List<NoteData> ParseCsv(TextAsset csv) {
        var list = new List<NoteData>();
        var lines = csv.text.Split(new[] {'\n','\r'}, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < lines.Length; i++) {
        var c = lines[i].Split(',');
        if (float.TryParse(c[0], out float b)
            && int.TryParse(c[1], out int l)
            && Enum.TryParse(c[2].Trim(), out NoteType t)) {
                list.Add(new NoteData { beat=b, lane=l, noteType=t });
            }
        }
        return list;
    }

    public Chart GetChartByName(string name) {
        return charts[name];
    }

    public ChartData GetChartDataByName(string name) {
        foreach(ChartData chartData in chartLibrary.Charts) {
            if (chartData.Name == name) {
                return chartData;
            }
        }
        Debug.LogError($"Chart of name {name}, does not have chartData");
        return null;
    }
}
