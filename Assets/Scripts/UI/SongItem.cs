using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongItem : MonoBehaviour {
    [SerializeField] public TextMeshProUGUI songName;
    [SerializeField] public TextMeshProUGUI bpm;
    [SerializeField] public TextMeshProUGUI composer;
    [SerializeField] public Image albumArt;

    public void OnButtonHover() {
        SFXManager.Instance.PlaySound("Hover");
    }

    public void OnButtonConfirm() {
        SFXManager.Instance.PlaySound("Confirm");
    }
}
