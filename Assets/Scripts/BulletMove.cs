using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    
}
