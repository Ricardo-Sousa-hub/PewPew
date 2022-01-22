using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using MilkShake;
using TMPro;

public class EnemyCube : MonoBehaviour
{
    public GameObject bullet;
    public GameObject redBullet;
    public GameObject shootgunBullet;
    public GameObject greenBullet;
    public GameObject granade;
    public GameObject bullet1;
    [Space]
    public GameObject sniperBullet;
    [Space]
    public float life;
    public float damageArea;
    public float damage;
    public float tempoAteSerDestruido;

    public NavMeshAgent enemy;
    [Space]
    public Canvas canvas;
    public Slider health;
    public GameObject text;
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
        if (life > 0 && player != null)
        {
            PerseguirJogador();
            PosicaoDaHealthbar();
            HealthBar();
            IniciarAnimacao();
        }
        else
        {
            gameObject.tag = "Dead";
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
                    ShowFloatingText(bullet.GetComponent<BulletMove>().damage);
                    life -= bullet.GetComponent<BulletMove>().damage;
                    Destroy(other.gameObject);
                    break;
                case "RedBullet":
                    ShowFloatingText(redBullet.GetComponent<BulletMove>().damage);
                    life -= redBullet.GetComponent<BulletMove>().damage;
                    Destroy(other.gameObject);
                    break;
                case "BulletShotgun":
                    ShowFloatingText(shootgunBullet.GetComponent<BulletMove>().damage);
                    life -= shootgunBullet.GetComponent<BulletMove>().damage;
                    Destroy(other.gameObject);
                    break;
                case "GreenBullet":
                    ShowFloatingText(greenBullet.GetComponent<BulletMove>().damage);
                    life -= greenBullet.GetComponent<BulletMove>().damage;
                    Destroy(other.gameObject);
                    break;

                case "Granade":
                    ShowFloatingText(granade.GetComponent<BulletMove>().damage);
                    break;
                case "Bullet1":
                    ShowFloatingText(bullet1.GetComponent<BulletMove>().damage);
                    life -= bullet1.GetComponent<BulletMove>().damage;
                    Destroy(other.gameObject);
                    break;
                case "SniperBullet":
                    ShowFloatingText(sniperBullet.GetComponent<BulletMove>().damage);
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
        if(player != null)
        {
            cam.GetComponent<ShakeCam>().Shake();
            player.GetComponent<PlayerController>().life -= damage;
        }
    }

    IEnumerator Dead(float tempo)
    {
        if (!isDead && player != null)
        {
            player.GetComponent<PlayerController>().AddScore(pontos);
            isDead = true;
        }
        else if(life <= 0)
        {
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

    void ShowFloatingText(float damage)
    {
        var go = Instantiate(text, transform.position, Quaternion.identity, transform);
        string damageTaken = "-" + damage;
        go.GetComponent<TextMesh>().text = damageTaken;
    }
}