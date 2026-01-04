using System;
using System.Collections.Generic;
using System.Globalization;
using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Enemies.EnemyComponents;
using TMPro;
using UnityEngine;

public class DamagePopUpGenerator : MonoBehaviour
{
    public GameObject damagePopUpPrefab;

    public enum DamageType
    {
        Normal,
        Weakness,
        Resistance
    }
    private DamageType damageType;

    private Dictionary<DamageType, float> damageTypeTextSize;
    
    private void Start()
    {
        damageTypeTextSize = new Dictionary<DamageType, float>
        {
            { DamageType.Normal, 4f },
            { DamageType.Weakness, 6f },
            { DamageType.Resistance, 2f }
        };
    }

    public void GeneratePopUp(float damage, DamageType damageType, Affinities affinitiesColor, Vector3 worldPosition)
    {
        GameObject popUp = Instantiate(damagePopUpPrefab, worldPosition, Quaternion.identity, transform);
        popUp.transform.localScale = Vector3.one;
        TextMeshProUGUI textMesh = popUp.GetComponentInChildren<TextMeshProUGUI>();

        if (textMesh != null)
        {
            textMesh.text = damage.ToString(CultureInfo.CurrentCulture);
            
            if (damageTypeTextSize.TryGetValue(damageType, out float fontSize))
            {
                textMesh.fontSize = fontSize;
            }

            switch (affinitiesColor)
            {
                case Affinities.Electric:
                    textMesh.color = Color.magenta;
                    break;
                case Affinities.Fire:
                    textMesh.color = Color.red;
                    break;
                case Affinities.Water:
                    textMesh.color = Color.blue;
                    break;
            }
        }
        StartCoroutine(FadeOutAndDestroy(popUp, 1.0f));
    }
    
    private System.Collections.IEnumerator FadeOutAndDestroy(GameObject popUp, float duration)
    {
        TextMeshProUGUI textMesh = popUp.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh == null)
        {
            yield break;
        }

        float startAlpha = textMesh.color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0, elapsed / duration);
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
            yield return null;
        }

        Destroy(popUp);
    }
}
