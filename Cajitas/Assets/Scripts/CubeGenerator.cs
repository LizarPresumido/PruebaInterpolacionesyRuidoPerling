using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public int numCubesMoving;
    public Vector2 noiseFrecuencyRange;
    public Vector2 noiseAmplitudeRange;
    /// <summary>
    /// Alturas que va a tener el cubo de cubos
    /// </summary>
    public int height;

    /// <summary>
    /// proporcion de cubos en un lado
    /// </summary>
    public int sideCubes;

    /// <summary>
    /// Cubo de referencia
    /// </summary>
    public GameObject cube;

    private List<GameObject> cubos;
    private int numCubes = 0;

    private Movement move;


    private Color[] colores  = { new Color(0f,0f,0f,0.2f), new Color(0f,0f,1f, 0.2f), new Color(1f,0f,1f, 0.2f), new Color(0f,1f,1f, 0.2f), new Color(1f,0.92f,0.016f, 0.2f), new Color(0.5f,0.5f,0.5f, 0.2f), new Color(0f,1f,0f, 0.2f), new Color(1f,1f,1f, 0.2f) };
    // Start is called before the first frame update
    void Start()
    {
        move = cube.GetComponent<Movement>();
        cubos = new List<GameObject>();
        StartCoroutine(setCubes(height));
    }

    IEnumerator setCubes(int h)
    {
        GameObject cubeDummy;
        //bucle para las alturas
        for (int y = 0; y <= height; ++y)
        {
            //bucle para cada fila
            for (int z = 0; z <= sideCubes; ++z)
            {
                if(z == 0 || z == sideCubes)
                {
                    for(int x = 0; x <= sideCubes; ++x)
                    {
                        move.noiseFrecuency = Random.Range(noiseFrecuencyRange.x, noiseFrecuencyRange.y);
                        move.noiseAmplitude = Random.Range(noiseAmplitudeRange.x, noiseAmplitudeRange.y);
                        cubeDummy = Instantiate(cube);
                        cubeDummy.transform.position = new Vector3(x,y,z);
                        cubeDummy.GetComponent<MeshRenderer>().material.color = colores[Random.Range(0,colores.Length)];
                        cubos.Add(cubeDummy);
                        ++numCubes;
                        yield return 0;
                    }
                        
                }
                else
                {
                    for(int x = 0; x <= sideCubes; x += sideCubes)
                    {
                        move.noiseFrecuency = Random.Range(noiseFrecuencyRange.x, noiseFrecuencyRange.y);
        move.noiseAmplitude = Random.Range(noiseAmplitudeRange.x, noiseAmplitudeRange.y);
                        cubeDummy = Instantiate(cube);
                        cubeDummy.transform.position = new Vector3(x, y, z);
                        cubeDummy.GetComponent<MeshRenderer>().material.color = colores[Random.Range(0, colores.Length)];
                        cubos.Add(cubeDummy);
                        ++numCubes;
                        yield return 0;
                    }
                }
            }
        }
        StopCoroutine("setCubes");
        Invoke("moveCubes", Random.Range(0, 3));
    }

    void moveCubes()
    {
        Movement cube1, cube2;
        do
        {
            cube1 = cubos[Random.Range(0, numCubes)].GetComponent<Movement>();
        } while (cube1.inMovement);
        do
        {
            cube2 = cubos[Random.Range(0, numCubes)].GetComponent<Movement>();
        } while (cube2.inMovement);
        float moveSpeed = Random.Range(0,0.5f);
        cube1.moveSpeed = moveSpeed;
        cube2.moveSpeed = moveSpeed;
        cube1.move(cube2.position);
        cube2.move(cube1.position);
        
        Invoke("moveCubes", Random.Range(0,0.5f));
    }
}
