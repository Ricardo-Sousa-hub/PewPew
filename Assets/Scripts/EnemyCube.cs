using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyCube : MonoBehaviour
{
    public GameObject redBullet;
    public GameObject bullet;
    public GameObject greenBullet;
    [Space]
    public float life;

    public float damageRate;
    public float zombieDamage;
    private float nextFire;
    public float damageArea;

    public NavMeshAgent enemy;

    public Slider health;

    public Animator animator;
    [Space]
    public GameObject player;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        health.value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(life <= 0)
        {
            DropReward();
            Destroy(gameObject);
        }

        PerseguirJogador();
        PosicaoDaHealthbar();
        HealthBar();
        DarDano();
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(transform.position, new Vector3(damageArea, 2, damageArea));
    //}

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

    void PerseguirJogador()
    {
        if(life > 0)
        {
            enemy.SetDestination(player.GetComponent<Transform>().position);
        }
    }

    void DarDano()
    {
        float distace = Vector3.Distance(player.GetComponent<Transform>().position, transform.position);

        if (distace < damageArea)
        {
            animator.SetBool("Andar", false);
            if(Time.time > nextFire) 
            {
                animator.SetBool("Atack", true);
                nextFire = Time.time + damageRate;
                player.GetComponent<PlayerController>().life -= zombieDamage;
            }
        }
        else
        {
            animator.SetBool("Andar", true);
            animator.SetBool("Atack", false);
        }
    }

    void HealthBar()
    {
        health.value = life;
    }

    void PosicaoDaHealthbar() // manter helathbar virada para a camara
    {
        health.transform.rotation = cam.transform.rotation;
    }

    void DropReward()
    {

    }
}
