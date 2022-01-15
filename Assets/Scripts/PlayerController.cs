using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    Vector3 movement;
    Vector3 lookPos;

    public Animator animator;
    [Space]
    public List<GameObject> guns;
    public List<bool> armasDesbloqueadas;
    public List<int> ammoByType;
    public List<GameObject> bulletsByType;

    float nextFire;
    [Space]
    public Transform shootSpawn;

    int armaSelecionada;

    public float life;

    Vector3 lookDir;

    // Start is called before the first frame update
    void Start()
    {
        armaSelecionada = 0;
    }


    // Update is called once per frame
    void Update()
    {
        LookMousePos();
        Shoot();
        selecionarArma();
        verificarVida();
    }


    void FixedUpdate()
    {
        Move();
    }

    void verificarVida()
    {
        if(life <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Animacoes(float vertical, float horizontal)
    {
        animator.SetFloat("Forward", vertical);
        animator.SetFloat("Turn", horizontal);
    }


    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        horizontal = horizontal * Time.deltaTime * moveSpeed;
        vertical = vertical * Time.deltaTime * moveSpeed;

        movement = new Vector3(horizontal, 0, vertical);
        transform.Translate(movement, Space.World); //Space world, em relação ao mundo

        Vector3 local = transform.InverseTransformDirection(movement); // passar movement do mundo para local
        Animacoes(local.z, local.x);
    }


    void LookMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            lookPos = hit.point;
        }

        lookDir = lookPos - transform.position;
        lookDir.y = 0;

        transform.LookAt(transform.position + lookDir, Vector3.up);
    }


    void selecionarArma()
    {
        switch (Input.inputString) // recolher o input em string
        {
            case "1":
                guns[0].SetActive(true);
                guns[armaSelecionada].SetActive(false);
                armaSelecionada = 0;
                break;
            case "2":
                if (armasDesbloqueadas[1])
                {
                    guns[1].SetActive(true);
                    guns[armaSelecionada].SetActive(false);
                    armaSelecionada = 1;
                }
                break;
            case "3":
                if (armasDesbloqueadas[2])
                {
                    guns[2].SetActive(true);
                    guns[armaSelecionada].SetActive(false);
                    armaSelecionada = 2;
                }
                break;
        }
    }


    void Shoot()
    {
        Gun gun = guns[armaSelecionada].GetComponent<Gun>();
        if (Input.GetButton("Fire1") && Time.time > nextFire && (ammoByType[armaSelecionada] != 0 || armaSelecionada == 0))
        {
            nextFire = Time.time + gun.fireRate;
            Instantiate(bulletsByType[armaSelecionada], shootSpawn.position, shootSpawn.rotation); //Quaternion.Euler(x, y, z) porque o prefab estava a spawnar com a rotacao 0,0,0

            if (armaSelecionada != 0)
            {
                ammoByType[armaSelecionada] = ammoByType[armaSelecionada] - 1;
            }
        }
    }
}
