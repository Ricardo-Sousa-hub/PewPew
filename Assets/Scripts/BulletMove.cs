using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public float damage;

    public int QuantidadeDeZombiesMax;
    public float RaioDeExplosao;

    public GameObject explosion;
    public AudioSource explosao;

    Camera cam;

    int counter;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        counter = 0;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(counter >= QuantidadeDeZombiesMax)
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
            if(gameObject.tag == "SniperBullet" && other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyCube>().life = 0;
                counter++;
            }
            if(gameObject.tag == "Grenade")
            {
                cam.GetComponent<ShakeCam>().Shake();

                Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
                Instantiate(explosion, pos, transform.rotation);
                List<GameObject> inimigos = new List<GameObject> ();
                inimigos.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
                
                foreach(GameObject enemy in inimigos)
                {
                    float distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
                    if (distance <= RaioDeExplosao/3)
                    {
                        //enemy.GetComponent<Rigidbody>().AddExplosionForce(1, transform.position, RaioDeExplosao);
                        enemy.GetComponent<EnemyCube>().life -= damage;
                    }
                    else if(distance <= RaioDeExplosao/2)
                    {
                        enemy.GetComponent<EnemyCube>().life -= damage/2;
                    }
                    else if(distance <= RaioDeExplosao)
                    {
                        enemy.GetComponent<EnemyCube>().life -= damage / 3;
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}
