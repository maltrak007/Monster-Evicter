using TMPro;
using UnityEngine;
using System.Collections;

public class DeathPanelScript : MonoBehaviour
{
    public TextMeshProUGUI deathText;  // Asignar desde el inspector
    public float startFontSize = 0f;
    public float targetFontSize = 120f;
    public float duration = 1.5f;

    void Start()
    {
        deathText.fontSize = startFontSize;
    }
    
    public void ShowDeathText()
    {
        StartCoroutine(AnimateDeathText());
    }

    IEnumerator AnimateDeathText()
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            t = Mathf.SmoothStep(0f, 1f, t);  // Suaviza la animación
            deathText.fontSize = Mathf.Lerp(startFontSize, targetFontSize, t);
            yield return null;
        }

        // Asegura que el tamaño final sea el exacto
        deathText.fontSize = targetFontSize;
    }
}