using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCube : MonoBehaviour
{
    public float life;
    public GameObject redBullet;
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(life > 0)
        {
            if (other.CompareTag("RedBullet"))
            {
                life = life - redBullet.GetComponent<BulletMove>().damage;
            }
            if (other.CompareTag("Bullet"))
            {
                
                life = life - bullet.GetComponent<BulletMove>().damage;
            }
            Destroy(other.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
