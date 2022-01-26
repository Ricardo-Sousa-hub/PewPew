using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject loja;
    public TextMeshProUGUI score;
    public GameObject endGame;
    public float tempoEntreWaves;
    public TextMeshProUGUI timer;
    float time;

    int quantidadeDeInimigos;
    [Space]
    public List<GameObject> inimigos;
    [Space]
    public float MaxX;
    public float MaxZ;
    [Space]
    Camera secondCam;

    int quantidadeZombiesFast;
    int quantidadeZombiesNormal;
    int quantidadeZombiesBig;
    public int wave;

    GameObject playerGO;
    PlayerController player;

    bool estado;
    int nightMode;
    public Light sun;
    public List<Light> luzes;

    // Start is called before the first frame update
    void Start()
    {
        time = tempoEntreWaves;

        Cursor.lockState = CursorLockMode.Confined;

        nightMode = PlayerPrefs.GetInt("Modo");
        if (nightMode == 0)
        {
            sun.enabled = false;
            foreach(Light light in luzes)
            {
                light.GetComponent<Animation>().enabled = false;
            }
        }

        score.SetText(PlayerPrefs.GetFloat("HighScore", 0).ToString());

        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<PlayerController>();
        estado = false;
        player.isStoreActive = estado;
        secondCam = GameObject.FindWithTag("SecondCamera").GetComponent<Camera>();
        secondCam.enabled = estado;

        wave = 1;
        quantidadeZombiesNormal = 2;
        quantidadeZombiesFast = 0;
        quantidadeZombiesBig = 0;

        StartCoroutine(SpawnWave());
    }

    // Update is called once per frame
    void Update()
    {
        if(playerGO != null)
        {
            UpdateScore();
            Loja();
            Timer();
        }
        else
        {
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enem in enemys)
            {
                enem.GetComponent<EnemyCube>().life = 0;
            }
            EndGame(true);
        }
    }

    bool FimDeWave()
    {
        quantidadeDeInimigos = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (quantidadeDeInimigos == 0)
        {
            timer.enabled = true;
            return false;
        }
        else
        {
            timer.enabled = false;
            time = tempoEntreWaves;
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
        while(playerGO != null)
        {
            if (estado)
            {
                loja.SetActive(!estado);
                player.isStoreActive = !estado;
                secondCam.enabled = !estado;
                estado = !estado;
            }

            if(wave == 1)
            {
                yield return new WaitForSeconds(5);
            }

            if (wave != 1)
            {
                yield return new WaitWhile(FimDeWave);
                yield return new WaitForSeconds(tempoEntreWaves);
            }
            if (!FimDeWave())
            {
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
    }

    void Spawn()
    {
        if(playerGO != null)
        {
            int[] quantidades = { quantidadeZombiesNormal, quantidadeZombiesBig, quantidadeZombiesFast };
            for (int i = 0; i < quantidades.Length; i++)
            {
                for (int j = 0; j < quantidades[i]; j++)
                {
                    Instantiate(inimigos[i], Posicao(), Quaternion.Euler(playerGO.transform.rotation.x, playerGO.transform.rotation.y, playerGO.transform.rotation.z));
                }
            }
        }
    }

    void Loja()
    {
        if (Input.GetKeyDown(KeyCode.B) && !FimDeWave() || (Input.GetKeyDown(KeyCode.Escape) && loja.activeInHierarchy))
        {
            Cursor.visible = !estado;
            playerGO.GetComponentInChildren<Animator>().SetFloat("Turn", 0);
            playerGO.GetComponentInChildren<Animator>().SetFloat("Forward", 0);
            secondCam.enabled = !estado;
            player.isStoreActive = !estado;
            loja.SetActive(!estado);
            estado = !estado;
        }
    }

    void UpdateScore()
    {
        score.SetText("Score: " + player.score.ToString());
    }

    void EndGame(bool est)
    {
        score.enabled = !est;
        endGame.SetActive(est);
        TextMeshProUGUI[] ui = endGame.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(TextMeshProUGUI element in ui)
        {
            if(element.name == "Score")
            {
                element.SetText("SCORE: " + player.score.ToString());
            }
            if(element.name == "BestScore")
            {
                if (player.score > PlayerPrefs.GetFloat("HighScore", 0))
                {
                    PlayerPrefs.SetFloat("HighScore", player.score);
                    element.SetText("BEST SCORE: " + PlayerPrefs.GetFloat("HighScore").ToString());
                }
                else
                {
                    element.SetText("BEST SCORE: " + PlayerPrefs.GetFloat("HighScore").ToString());
                }
            }
        }
    }

    public void Restart()
    {
        EndGame(false);

        //Instantiate(playerPrefab, transform.position, transform.rotation);

        score.SetText(PlayerPrefs.GetFloat("HighScore", 0).ToString());

        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Dead");
        foreach (GameObject enemy in enemys)
        {
            Destroy(enemy);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    void Timer()
    {
        if(time > 0 && !FimDeWave() && wave > 1)
        {
            time -= Time.deltaTime;
            timer.SetText(Mathf.FloorToInt(time % 60).ToString());
        }
        else
        {
            timer.SetText("");
        }
    }
}