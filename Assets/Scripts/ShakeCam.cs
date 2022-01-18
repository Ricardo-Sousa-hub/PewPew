using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

public class ShakeCam : MonoBehaviour
{
    public Shaker shake;
    public ShakePreset shakePreset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake()
    {
        shake.Shake(shakePreset);
    }
}
