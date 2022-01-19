using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using MilkShake;

public class EnemyCube : MonoBehaviour
{
    public GameObject redBullet;
    public GameObject bullet;
    public GameObject greenBullet;
    public GameObject granade;
    public GameObject bullet1;
    public GameObject sniperBullet;
    public int maxZombiesPerfurar;
    [Space]
    public float life;
    public float damageArea;
    public float damage;
    public float tempoAteSerDestruido;

    public NavMeshAgent enemy;
    [Space]
    public Canvas canvas;
    public Slider health;
    [Space]
    public int pontos;

    Animator animator;

    GameObject player;
    Camera cam;
    CapsuleCollider capsule;

    bool isDead = false;

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
        if (life > 0)
        {
            PerseguirJogador();
            PosicaoDaHealthbar();
            HealthBar();
            IniciarAnimacao();
        }
        else
        {
            gameObject.tag = "Untagged";
            StartCoroutine(Dead(tempoAteSerDestruido));
        }


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
        //if (other.CompareTag("Store"))
        //{
        //    return;
        //}

        if (life > 0)
        {
            switch (other.tag)
            {
                case "Bullet":
                    life -= bullet.GetComponent<BulletMove>().damage;
                    Destroy(other.gameObject);
                    break;

                case "RedBullet":
                    life -= redBullet.GetComponent<BulletMove>().damage;
                    Destroy(other.gameObject);
                    break;

                case "GreenBullet":
                    life -= greenBullet.GetComponent<BulletMove>().damage;
                    Destroy(other.gameObject);
                    break;

                case "Granade":
                    life -= granade.GetComponent<BulletMove>().damage;
                    Destroy(other.gameObject);
                    break;
                case "Bullet1":
                    life -= bullet1.GetComponent<BulletMove>().damage;
                    Destroy(other.gameObject);
                    break;
                case "SniperBullet":
                    life -= sniperBullet.GetComponent<BulletMove>().damage;
                    //Sniper(other.gameObject);
                    break;
            }
        }
    }

    //IEnumerator Sniper(GameObject obj)
    //{
    //    yield return new WaitForSeconds(7);
    //    Destroy(obj);
    //}

    void PerseguirJogador()
    {
        if (life > 0)
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
        if (VerificarDistanciaDeAtaque() && life > 0)
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
        cam.GetComponent<ShakeCam>().Shake();
        player.GetComponent<PlayerController>().life -= damage;
    }

    IEnumerator Dead(float tempo)
    {
        if (!isDead)
        {
            player.GetComponent<PlayerController>().AddScore(pontos);
            isDead = true;
        }

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