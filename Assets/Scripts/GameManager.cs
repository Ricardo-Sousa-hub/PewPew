using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject loja;
    public TextMeshProUGUI score;
    public GameObject endGame;
    public float tempoEntreWaves;
    public TextMeshProUGUI timer;
    float time;

    public GameObject pauseMenu;
    [Space]
    public GameObject optionsMenu;
    [Space]
    public AudioMixer audioMixer;
    public Slider volume;
    public TMP_Dropdown quality;

    int quantidadeDeInimigos;
    [Space]
    public List<GameObject> inimigos;
    [Space]
    public float MaxX;
    public float MinX;
    public float MaxZ;
    public float MinZ;
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

    [Space]
    public List<TextMeshProUGUI> precos;

    // Start is called before the first frame update
    void Start()
    {
        
        quality.value = PlayerPrefs.GetInt("Quality");
        QualitySettings.SetQualityLevel(quality.value);
        
        volume.value = PlayerPrefs.GetFloat("Volume");
        audioMixer.SetFloat("Volume", volume.value);

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
            PauseMenu();
            Loja();
            Timer();
            
            SetVolume();
            SetQuality();
            if (loja.activeInHierarchy) //Os valores da loja apenas atualizam se a loja estiver aberta, otimização
            {
                UpdatePrecos();
            }
        }
        else
        {
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enem in enemys)
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
        float x = Random.Range(MinX, MaxX);
        float z = Random.Range(MinZ, MaxZ);

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
            playerGO.GetComponentInChildren<Animator>().SetFloat("Turn", 0);
            playerGO.GetComponentInChildren<Animator>().SetFloat("Forward", 0);
            secondCam.enabled = !estado;
            player.isStoreActive = !estado;
            loja.SetActive(!estado);
            estado = !estado;
        }
    }

    void PauseMenu()
    {
        if(!loja.activeInHierarchy && !optionsMenu.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!player.isPaused);
            if (player.isPaused)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0.0f;
            }
            player.isPaused = !player.isPaused;
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadOptions()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 0.0f;
        optionsMenu.SetActive(true);
    }

    public void SetVolume()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
        audioMixer.SetFloat("Volume", volume.value);
    }

    public void SetQuality()
    {
        PlayerPrefs.SetInt("Quality", quality.value);
        QualitySettings.SetQualityLevel(quality.value);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void LoadPause()
    {
        optionsMenu.SetActive(false);
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
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

    void UpdatePrecos()
    {
        for(int i = 1; i < precos.Count; i++)
        {
            precos[i].SetText(player.precoArmasComTaxa[i].ToString());
        }
    }
}