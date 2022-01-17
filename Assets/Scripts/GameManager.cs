using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Canvas loja;
    public float tempoEntreWaves;

    int quantidadeDeInimigos;
    [Space]
    public List<GameObject> inimigos;
    [Space]
    public float MaxX;
    public float MaxZ;

    int quantidadeZombiesFast;
    int quantidadeZombiesNormal;
    int quantidadeZombiesBig;
    int wave;

    GameObject player;

    bool estado;
 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        wave = 1;
        quantidadeZombiesNormal = 2;
        quantidadeZombiesFast = 0;
        quantidadeZombiesBig = 0;
        estado = false;
        StartCoroutine(SpawnWave());
    }

    // Update is called once per frame
    void Update()
    {
        Loja();
    }

    bool FimDeWave()
    {
        quantidadeDeInimigos = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(quantidadeDeInimigos == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    Vector3 Posicao()
    {
        float x = Random.Range(-MaxX, MaxX);
        float z = Random.Range(-MaxZ, MaxZ);

        return new Vector3(x, 0, z);
    }

    IEnumerator SpawnWave()
    {
        while(player != null)
        {
            if (estado)
            {
                loja.gameObject.SetActive(!estado);
                estado = !estado;
            }

            if(wave != 1)
            {
                yield return new WaitWhile(FimDeWave);

                yield return new WaitForSeconds(tempoEntreWaves);
            }

            if (wave < 10)
            {
                Spawn();
                quantidadeZombiesNormal++;
            }
            else if (wave < 15)
            {
                quantidadeZombiesBig++;
                Spawn();
                quantidadeZombiesNormal++;
            }
            else
            {
                quantidadeZombiesBig++;
                quantidadeZombiesFast++;
                Spawn();
                quantidadeZombiesNormal++;
            }
            wave++;
        }
    }

    void Spawn()
    {
        int[] quantidades = { quantidadeZombiesNormal, quantidadeZombiesBig, quantidadeZombiesFast };
        for(int i = 0; i< quantidades.Length; i++)
        {
            for(int j = 0; j < quantidades[i]; j++)
            {
                Instantiate(inimigos[i], Posicao(), Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z));
            }
        }
    }

    void Loja()
    {
        if (Input.GetKeyDown(KeyCode.B) && !FimDeWave())
        {
            loja.gameObject.SetActive(!estado);
            estado = !estado;
        }
    }
}