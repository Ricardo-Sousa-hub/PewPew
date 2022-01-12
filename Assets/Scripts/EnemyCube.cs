using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCube : MonoBehaviour
{
    public float life;

    public GameObject redBullet;
    public GameObject bullet;
    public GameObject greenBullet;

    public float damageRate;
    public GameObject player;
    public float zombieDamage;

    private float nextFire;
    public float damageArea;

    public NavMeshAgent enemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(life <= 0)
        {
            Destroy(gameObject);
        }

        DetetarJogador();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(damageArea, 2, damageArea));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }

        if(life > 0)
        {
            switch (other.tag)
            {
                case "Bullet":
                    life -= bullet.GetComponent<BulletMove>().damage;
                    break;

                case "RedBullet":
                    life -= redBullet.GetComponent<BulletMove>().damage;
                    break;

                case "GreenBullet":
                    life -= bullet.GetComponent<BulletMove>().damage;
                    break;
            }
            Destroy(other.gameObject);
        }
    }

    void DetetarJogador()
    {
        float distace = Vector3.Distance(player.GetComponent<Transform>().position, transform.position);

        enemy.SetDestination(player.GetComponent<Transform>().position);

        if (distace < damageArea && Time.time > nextFire)
        {
            nextFire = Time.time + damageRate;
            player.GetComponent<PlayerController>().life -= zombieDamage;
        }
    }
}
