using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjects : MonoBehaviour
{
    [SerializeField] GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        CreatCube();
    }

    void CreatCube()
    {
        for (int x = -200; x <= 200; x += 40)
        {
            for (int z = -200; z <= 200; z += 40)
            {
                GameObject obj = Instantiate(cube, cube.transform.parent);
                obj.transform.position = new Vector3(x, 0, z);
            }
        }

        Destroy(cube);
    }
}
