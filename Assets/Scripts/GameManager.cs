using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject loja;
    public TextMeshProUGUI score;
    public float tempoEntreWaves;

    int quantidadeDeInimigos;
    [Space]
    public List<GameObject> inimigos;
    [Space]
    public float MaxX;
    public float MaxZ;
    [Space]
    public Camera secondCam;

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
        player.GetComponent<PlayerController>().isStoreActive = estado;
        secondCam.enabled = estado;
        StartCoroutine(SpawnWave());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
        Loja();
    }

    bool FimDeWave()
    {
        quantidadeDeInimigos = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (quantidadeDeInimigos == 0)
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
        while (player != null)
        {
            if (estado)
            {
                loja.SetActive(!estado);
                player.GetComponent<PlayerController>().isStoreActive = !estado;
                secondCam.enabled = !estado;
                estado = !estado;
            }

            if (wave != 1)
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
        for (int i = 0; i < quantidades.Length; i++)
        {
            for (int j = 0; j < quantidades[i]; j++)
            {
                Instantiate(inimigos[i], Posicao(), Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z));
            }
        }
    }

    void Loja()
    {
        if (Input.GetKeyDown(KeyCode.B) && !FimDeWave())
        {
            player.GetComponentInChildren<Animator>().SetFloat("Turn", 0);
            player.GetComponentInChildren<Animator>().SetFloat("Forward", 0);
            secondCam.enabled = !estado;
            player.GetComponent<PlayerController>().isStoreActive = !estado;
            loja.SetActive(!estado);
            estado = !estado;
        }
    }

    void UpdateScore()
    {
        score.SetText("Score: "+player.GetComponent<PlayerController>().score.ToString());
    }
}