using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class LightFlicker : MonoBehaviour
{
    public float intensityHigh = 1f;
    public float intensityLow = 0.7f;
    public float maxJump = 0.05f;

    private float currentIntensity = 0.9f;
    private Light2D light;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {

        currentIntensity = Random.Range(Mathf.Max(intensityLow, currentIntensity - maxJump), Mathf.Min(intensityHigh, currentIntensity + maxJump));

        light.intensity = currentIntensity;
    }
}
