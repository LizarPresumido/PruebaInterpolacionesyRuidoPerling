using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public AnimationCurve noiseConverter;

    public enum NoiseState {Up, Down };
    NoiseState noiseState;

    /// <summary>
    /// noise limits
    /// </summary>
    public Vector2 limitsX, limitsY;

    float fila, columnaX, columnaY, columnaZ;

    float lerpA;

    /// <summary>
    /// Noise values
    /// </summary>
    [HideInInspector]
    public float noiseFrecuency, noiseAmplitude;
    /// <summary>
    /// transition move speed
    /// </summary>
    [HideInInspector]
    public float moveSpeed;

    [HideInInspector]
    public bool inMovement;

    Transform cubeTransform;
    [HideInInspector]
    public Vector3 position, movePosition;
    Vector3 offset;

    void Start()
    {
        lerpA = 0;
        noiseState = NoiseState.Up;
        columnaZ = Random.Range(limitsX.x, limitsX.y);
        cubeTransform = transform;
        position = cubeTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float ruidoX = Mathf.PerlinNoise(columnaX, fila);
        float ruidoY = Mathf.PerlinNoise(columnaY, fila);
        float ruidoZ = Mathf.PerlinNoise(columnaZ, fila);

        ruidoX = noiseConverter.Evaluate(ruidoX);
        ruidoY = noiseConverter.Evaluate(ruidoY);
        ruidoZ = noiseConverter.Evaluate(ruidoZ);

        offset = new Vector3(ruidoX * noiseAmplitude, ruidoY * noiseAmplitude, ruidoZ * noiseAmplitude);
        cubeTransform.position = position + offset;
        

        #region Noise management section
        if (noiseState == NoiseState.Up)
        {
            //Incrementa la fila
            fila += Time.deltaTime * noiseFrecuency;
            //Pregunta si se ha llegado al limite superior
            if (fila >= limitsY.y)
                //Cambia de estado
                noiseState = NoiseState.Down;
        }
        else if (noiseState == NoiseState.Down)
        {
            //Decrementa la fila
            fila -= Time.deltaTime * noiseFrecuency;
            //Pregunta si se ha llegado al limite inferior
            if (fila <= limitsY.x)
                //Cambia de estado
                noiseState = NoiseState.Up;

        }
        #endregion
    }

    public void move(Vector3 move)
    {
        inMovement = true;
        movePosition = move;
        StartCoroutine(_move());
    }

    IEnumerator _move()
    {
        do
        {
            lerpA += Time.deltaTime * moveSpeed;
            if (lerpA > 1)
            {
                lerpA = 1;
            }
            cubeTransform.position = Vector3.Lerp(cubeTransform.position, movePosition, lerpA);
            yield return 0;

            if (lerpA == 1)
            {
                lerpA = 0;
                inMovement = false;
            }
        } while (inMovement);
        
    }
}
