using UnityEngine;

public class ButtonSFXManager : MonoBehaviour {
    public void OnButtonHover() {
        SFXManager.Instance.PlaySound("Hover");
    }

    public void OnButtonConfirm() {
        SFXManager.Instance.PlaySound("Confirm");
    }
}
