using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flicker : MonoBehaviour
{
    Light2D Lighting;
    float diff;
    [SerializeField] float diffMultiplier = 0.3f;
    [SerializeField] float min = 0.6f;
    [SerializeField] float max = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Lighting = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Lighting.intensity += diff * Time.deltaTime;

        if (Lighting.intensity <= min)
        {
            diff = diffMultiplier;
        }
        else if (Lighting.intensity >= max)
        {
            diff = -diffMultiplier;
        }

    }
}
