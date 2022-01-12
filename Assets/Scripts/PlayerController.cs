using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    Vector3 movement;
    Vector3 lookPos;

    public Animator animator;

    public List<int> ammoByType;
    public List<GameObject> bulletsByType;

    private float nextFire;
    public Transform shootSpawn;

    public List<GameObject> guns;
    public int armaSelecionada;

    public float showFireRate;

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
    }


    void FixedUpdate()
    {
        Move();
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
        Animacoes(vertical, horizontal);

        transform.Translate(movement);
    }


    void LookMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            lookPos = hit.point;
        }

        Vector3 lookDir = lookPos - transform.position;
        lookDir.y = 0;

        transform.LookAt(transform.position + lookDir, Vector3.up);
    }


    void selecionarArma()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(KeyCode.Alpha1);
            armaSelecionada = 0;
            guns[1].SetActive(false);
            guns[0].SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            armaSelecionada = 1;
            guns[0].SetActive(false);
            guns[1].SetActive(true);
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
