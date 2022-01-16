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
    public int maxAmmo;
    
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
                IsAmmo();
            }
            else if (isHealth)
            {
                print("Med");
                IsHealth();
            }
            else
            {
                player.GetComponent<PlayerController>().armasDesbloqueadas[arma] = true;
                player.GetComponent<PlayerController>().ammoByType[arma] = maxAmmo;
                Destroy(gameObject);
            }
        }
    }

    void IsAmmo()
    {
        if (player.GetComponent<PlayerController>().ammoByType[ammoType] <= maxAmmo - quantidadeDeMunicao) //60 < 100-50=50 false
        {
            player.GetComponent<PlayerController>().ammoByType[ammoType] += quantidadeDeMunicao;
            Destroy(gameObject);
        }
        else if(player.GetComponent<PlayerController>().ammoByType[ammoType] == maxAmmo)
        {
            return;
        }
        else
        {
            player.GetComponent<PlayerController>().ammoByType[ammoType] = maxAmmo;
            Destroy(gameObject);
        }
    }

    void IsHealth()
    {
        if(player.GetComponent<PlayerController>().life <= 100 - health)
        {
            player.GetComponent<PlayerController>().life += health;
            Destroy(gameObject);
        }
        else if(player.GetComponent<PlayerController>().life == health)
        {
            return;
        }
        else
        {
            player.GetComponent<PlayerController>().life = health;
            Destroy(gameObject);
        }
    }
}

