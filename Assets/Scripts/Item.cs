using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isHealth;
    public int health;
    [Space]
    public bool isAmmo;
    public int arma;
    public int ammoType;
    public int quantidadeDeMunicao;
    [Space]
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isAmmo)
            {
                player.GetComponent<PlayerController>().ammoByType[ammoType] = quantidadeDeMunicao;
            }
            else if (isHealth)
            {
                player.GetComponent<PlayerController>().life = health;
            }
            else
            {
                player.GetComponent<PlayerController>().armasDesbloqueadas[arma] = true;
                player.GetComponent<PlayerController>().ammoByType[arma] = quantidadeDeMunicao;
            }
            
            Destroy(gameObject);
        }
    }
}
