using UnityEditor;
using UnityEngine;
using System.Collections;

public class HealthToUI : MonoBehaviour
{
    HasHealth HasHealthScript;

    void Start()
    {
        HasHealthScript = GetComponent<HasHealth>();
    }

    void OnGUI()
    {
        if (HasHealthScript == null) {
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.0f, 2.0f, 0.0f));
        GUI.HorizontalSlider(new Rect(screenPos.x - 50, (Screen.height - screenPos.y), 100, 0), HasHealthScript.CurrentHealth, HasHealthScript.DeadUnder, HasHealthScript.MaxHealth);
    }
}
