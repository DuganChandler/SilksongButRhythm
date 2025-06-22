using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChartData", menuName = "Charts/ChartData")]
public class ChartData : ScriptableObject {
    [SerializeField] int bpm;
    [SerializeField] new string name;
    [SerializeField] string composer;
    [SerializeField] string length;
    [SerializeField] AudioClip song;

    public int Bpm => bpm;
    public string Name => name;
    public string Composer => composer;
    public string Length => length;
    public AudioClip Song => song;

}
