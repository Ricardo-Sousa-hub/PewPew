using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public float damage;
    int counter;
    public int QuantidadeDeZombiesMax;
    public float RaioDeExplosao;

    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "Sniper" && counter >= QuantidadeDeZombiesMax)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            return;
        }
        else
        {
            if(gameObject.tag == "SniperBullet")
            {
                if (counter >= QuantidadeDeZombiesMax)
                {
                    other.GetComponent<EnemyCube>().life = 0;
                }
                else counter++;
            }
            if(gameObject.tag == "Granade")
            {
                Instantiate(explosion, transform.position, transform.rotation);
                List<GameObject> inimigos = new List<GameObject> ();
                inimigos.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
                
                foreach(GameObject enemy in inimigos)
                {
                    float distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
                    if (distance <= RaioDeExplosao)
                    {
                        //enemy.GetComponent<Rigidbody>().AddExplosionForce(1, transform.position, RaioDeExplosao);
                        enemy.GetComponent<EnemyCube>().life = 0;
                    }
                }
            }
        }
    }
}
