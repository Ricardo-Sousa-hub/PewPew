using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    //public Vector3 targetOS;
    //public float speedCamera;

    public float alturaCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, player.transform.position + targetOS, speedCamera * Time.deltaTime);

        transform.position = new Vector3(player.transform.position.x, alturaCamera, player.transform.position.z - 5);
    }
}
