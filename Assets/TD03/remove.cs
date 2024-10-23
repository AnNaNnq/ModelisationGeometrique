using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remove : MonoBehaviour
{
    public int potentiel;
    MeshRenderer monCube;
    public Material violet, rouge, orange, jaune, vert;


    private void Start()
    {
        monCube = GetComponent<MeshRenderer>();
        int random = Random.Range(1, 256);
        potentiel = random;
        color();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            if (other.GetComponent<SphereDrag>().effacer)
            {
                potentiel -= 51;
            }
            else
            {
                potentiel += 51;
                
            }
            color();
        }
    }

    private void color()
    {
        monCube.enabled = true;
        if (potentiel <= 0)
        {
            monCube.enabled = false;
        }
        else if (potentiel < 51)
        {
            monCube.material = violet;
        }
        else if (potentiel < 102)
        {
            monCube.material = rouge;
        }
        else if (potentiel < 153)
        {
            monCube.material = orange;
        }
        else if (potentiel < 204)
        {
            monCube.material = jaune;
        }
        else if (potentiel <= 255)
        {
            monCube.material = vert;
        }
    }
}
