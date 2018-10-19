using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [HideInInspector] public float targetSpeed = 0f;
    [HideInInspector] public float rotAmount;
    public float maxRot = 50f;

    public Slider gasSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        rotAmount = -SimpleInput.GetAxis("Horizontal") * maxRot;
        targetSpeed = PlayerController.instance.gasSlider.value;
    }

}
