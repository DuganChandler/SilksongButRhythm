// CounterRotate.cs
using UnityEngine;

[ExecuteAlways]
public class CounterRotate : MonoBehaviour {
  void LateUpdate() {
    // Keeps this Transform level, no matter its parent’s rotation
    transform.rotation = Quaternion.identity;
  }
}
