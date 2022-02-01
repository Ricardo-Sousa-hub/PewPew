using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{

    public Animation anim;
    [Space]
    public GameObject startMenu;
    [Space]
    public GameObject mapas;
    [Space]
    public GameObject modos;
    [Space]
    public GameObject options;

    int mapa;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void LoadMapOptions()
    {
        startMenu.SetActive(false);
        anim.Play();
    }

    public void LoadModOptions(int mapaEscolhido)
    {
        mapa = mapaEscolhido;
        mapas.SetActive(false);
        modos.SetActive(true);
    }

    public void LoadSceneWithMode(int modo)
    {
        PlayerPrefs.SetInt("Modo", modo);
        SceneManager.LoadScene(mapa);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void AtivarEscolha()
    {
        mapas.SetActive(true);
    }

    public void LoadOptions()
    {
        startMenu.SetActive(false);
        options.SetActive(true);
    }

    public void LoadMenu()
    {
        options.SetActive(false);
        startMenu.SetActive(true);
    }
}
