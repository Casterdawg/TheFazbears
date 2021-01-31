﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    private float animationTime = 0.25f;
    public Transform healthBar;

    public void UpdateValue(float currentHealth, float totalHealth)
    {
        if (currentHealth > 0)
        {
            Vector3 newScale = new Vector3((currentHealth / totalHealth), 1, 1);
            LeanTween.scale(healthBar.gameObject, newScale, animationTime);
        }
        else
        {
            healthBar.localScale = Vector3.zero;
        }
    }
}
