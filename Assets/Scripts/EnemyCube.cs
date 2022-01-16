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
    public float damageArea;
    public float damage;

    public NavMeshAgent enemy;
    [Space]
    public Slider health;

    public Animator animator;
    
    GameObject player;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
        health.value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(life <= 0)
        {
            Destroy(gameObject);
        }

        PerseguirJogador();
        PosicaoDaHealthbar();
        HealthBar();
        IniciarAnimacao();
        //VerificarDistanciaDeAtaque();

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
        if (other.CompareTag("Powerup"))
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

    public bool VerificarDistanciaDeAtaque()
    {
        float distace = Vector3.Distance(player.GetComponent<Transform>().position, transform.position);

        return distace < damageArea;
    }

    void IniciarAnimacao()
    {
        if(VerificarDistanciaDeAtaque())
        {
            animator.SetBool("Andar", false);
            
            animator.SetBool("Atack", true);
            
        }
        else
        {
            animator.SetBool("Andar", true);
            animator.SetBool("Atack", false);
        }
        
    }

    void DarDano()
    {
        player.GetComponent<PlayerController>().life -= damage;
    }

    void HealthBar()
    {
        health.value = life;
    }

    void PosicaoDaHealthbar() // manter helathbar virada para a camara
    {
        health.transform.rotation = cam.transform.rotation;
    }
}
