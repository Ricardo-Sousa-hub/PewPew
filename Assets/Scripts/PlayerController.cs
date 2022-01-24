using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    Vector3 movement;
    Vector3 lookPos;
    [Space]
    public Animator animator;
    [Space]
    public List<GameObject> guns;
    public List<bool> armasDesbloqueadas;
    public List<int> ammoByType;
    public List<GameObject> bulletsByType;
    [Space]
    public List<Image> gunsImage;
    [Space]
    //public int[] maxAmmo;
    public int[] precoArmas;

    float nextFire;
    [Space]
    public Transform shootSpawn;
    [Space]
    int armaSelecionada;

    public float life;

    Vector3 lookDir;
    [Space]
    public float score;
    [Space]
    public bool isStoreActive;

    [Space]
    public int pelletCount;
    public float spreadAngle;
    //shootspawn
    List<Quaternion> pellets;

    public List<AudioClip> audiosDisparos;
    AudioSource audioDisparo;

    // Start is called before the first frame update
    void Start()
    {
        audioDisparo = GetComponent<AudioSource>();
        score = 0;
        armaSelecionada = 0;
    }

    void Awake()
    {
        pellets = new List<Quaternion>(new Quaternion[pelletCount]);
    }


    // Update is called once per frame
    void Update()
    {
        if (!isStoreActive)
        {
            LookMousePos();
            Shoot();
            verificarVida();
            SelecionarArma();
        }
    }


    void FixedUpdate()
    {
        if (!isStoreActive)
        {
            Move();
        }
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
        if (Physics.Raycast(ray, out hit))
        {
            lookPos = hit.point;
        }

        lookDir = lookPos - transform.position;
        lookDir.y = 0;

        transform.LookAt(transform.position + lookDir, Vector3.up);
    }


    void SelecionarArma()
    {
        switch (Input.inputString) // recolher o input em string
        {
            case "1":
                TrocarDeArma(0);
                break;
            case "2":
                TrocarDeArma(1);
                break;
            case "3":
                TrocarDeArma(2);
                break;
            case "4":
                TrocarDeArma(3);
                break;
            case "5":
                TrocarDeArma(4);
                break;
            case "6":
                TrocarDeArma(5);
                break;
            case "7":
                TrocarDeArma(6);
                break;
        }
    }

    void TrocarDeArma(int arma)
    {
        if (armasDesbloqueadas[arma])
        {
            gunsImage[armaSelecionada] = gunsImage[armaSelecionada].GetComponent<Image>();
            gunsImage[armaSelecionada].color = new Color(gunsImage[armaSelecionada].color.r, gunsImage[armaSelecionada].color.g, gunsImage[armaSelecionada].color.b, 0.25f);
            guns[armaSelecionada].SetActive(false);

            guns[arma].SetActive(true);
            gunsImage[arma] = gunsImage[arma].GetComponent<Image>();
            gunsImage[arma].color = new Color(gunsImage[arma].color.r, gunsImage[arma].color.g, gunsImage[arma].color.b, 1);
            armaSelecionada = arma;
        }
    }


    void Shoot()
    {
        Gun gun = guns[armaSelecionada].GetComponent<Gun>();
        if (Input.GetButton("Fire1") && Time.time > nextFire && (ammoByType[armaSelecionada] != 0 || armaSelecionada == 0))
        {
            audioDisparo.clip = audiosDisparos[armaSelecionada];
            audioDisparo.Play();

            nextFire = Time.time + gun.fireRate;
            if (gun.isShotgun)
            {
                ShootgunShoot();
            }
            else
            {
                Instantiate(bulletsByType[armaSelecionada], shootSpawn.position, shootSpawn.rotation); //Quaternion.Euler(x, y, z) porque o prefab estava a spawnar com a rotacao 0,0,0
            }

            if (armaSelecionada != 0)
            {
                ammoByType[armaSelecionada] = ammoByType[armaSelecionada] - 1;
            }
        }
    }

    void ShootgunShoot()
    {
        int i = 0;
        foreach(Quaternion quat in pellets.ToArray())
        {
            pellets[i] = Random.rotation;
            GameObject p = Instantiate(bulletsByType[armaSelecionada], shootSpawn.position, shootSpawn.rotation);
            p.transform.rotation = Quaternion.RotateTowards(p.transform.rotation, pellets[i], spreadAngle);
            p.GetComponent<Rigidbody>().AddForce(p.transform.right * bulletsByType[armaSelecionada].GetComponent<BulletMove>().speed);
            i++;
        }
    }

    public void DesbloquearArmaAndAmmo(int arma)
    {
        if(score >= precoArmas[arma] && !armasDesbloqueadas[arma])
        {
            score -= precoArmas[arma];
            armasDesbloqueadas[arma] = true;
            TrocarDeArma(arma);
            ammoByType[arma] = guns[arma].GetComponent<Gun>().maxAmmo;
        }
        else if(score >= (precoArmas[arma] / 2) && armasDesbloqueadas[arma] && ammoByType[arma] < guns[arma].GetComponent<Gun>().maxAmmo)
        {
            score -= (precoArmas[arma] / 2);
            ammoByType[arma] = guns[arma].GetComponent<Gun>().maxAmmo;
        }
    }

    public void Health()
    {
        life = 100;
    }

    public void AddScore(int pontos)
    {
        score += pontos;
    }
}
