using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimatorManager : MonoBehaviour {

    #region _STATES


    #endregion

    /// <summary>
    /// Demo que coloca posiciona al azar los animales cargados.
    /// </summary>
    /// <param name="bundlesLoaded"></param>
    public void Init(List<GameObject> bundlesLoaded)
    {
        float position = 0;
        foreach (GameObject bundle in bundlesLoaded)
        {            
            float randomRotation = Random.Range(-90, 90);
            bundle.transform.position = new Vector3(position, 0, 0);
            bundle.transform.rotation = Quaternion.Euler(0, randomRotation, 0);

            position += 5;
        }
    }

}
