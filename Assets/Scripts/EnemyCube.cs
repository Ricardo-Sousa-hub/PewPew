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
    public float tempoAteSerDestruido;

    public NavMeshAgent enemy;
    [Space]
    public Canvas canvas;
    public Slider health;

    Animator animator;
    
    GameObject player;
    Camera cam;
    CapsuleCollider capsule;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
        health.value = 100;
        capsule = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(life <= 0)
        {
            StartCoroutine(Dead(tempoAteSerDestruido));
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
        if(VerificarDistanciaDeAtaque() && life > 0)
        {
            animator.SetBool("Andar", false);
            animator.SetBool("Atack", true);
            transform.LookAt(player.GetComponent<Transform>().position);

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

    IEnumerator Dead(float tempo)
    {
        canvas.enabled = false;
        animator.SetBool("Dead", true);
        capsule.enabled = false;
        enemy.enabled = false;
        yield return new WaitForSeconds(tempo);
        Destroy(gameObject);
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
