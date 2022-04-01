using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    float movementFactor;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 5f;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f;
        float rawSinWave = Mathf.Sin(tau * cycles);
        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 movementOffset = movementFactor * movementVector;
        transform.position = startingPos + movementOffset;
    }
}
