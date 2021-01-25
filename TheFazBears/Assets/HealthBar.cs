using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    private float AnimationTime = 1;
    private RectTransform transform;
    public RectTransform healthBar;

    void Start()
    {
        transform = gameObject.GetComponent<RectTransform>();
       // Shrink();
    }

    private void Shrink()
    {
        LeanTween.scale(transform, new Vector3(.5f, .5f, .5f), AnimationTime);
    }

    private void Grow()
    {
        LeanTween.scale(transform, new Vector3(1f, 1f, 1f), AnimationTime);
    }

    private void UpdateValue(float healthDecrease, float totalHealth)
    {
        
    }
}
