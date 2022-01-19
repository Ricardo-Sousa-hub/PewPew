using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestruirAfterTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestruirAfterTime()
    {
        transform.localPosition += new Vector3(0, 1.5f, 0);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
