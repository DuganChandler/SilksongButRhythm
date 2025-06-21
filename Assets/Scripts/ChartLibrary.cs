using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChartLibrary", menuName = "Charts/ChartLibrary")]
public class ChartLibrary : ScriptableObject {
   [SerializeField] List<ChartData> charts; 

   public List<ChartData> Charts => charts;
}
