using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [HideInInspector] public float targetSpeed = 0f;
    [HideInInspector] public float rotAmount;
    public float maxGas = 0.1f;
    public float maxRot = 50f;

    public Slider gasSlider;

    private bool m_isGasPedalDown = false, m_isBreakPedalDown = false;

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
        if (GameManager.instance.isMovePhase)
            return;

        rotAmount = -SimpleInput.GetAxis("Horizontal") * maxRot;
        //targetSpeed = PlayerController.instance.gasSlider.value;

        if (m_isGasPedalDown)
        {
            // Debug.Log("Pedal clicked");
            float tmpSpeed = targetSpeed + (maxGas / 2f) * Time.deltaTime;
            targetSpeed = Mathf.Clamp(tmpSpeed, 0, maxGas);
        }
        else if (m_isBreakPedalDown)
        {
            float tmpSpeed = targetSpeed - (maxGas / 2f) * Time.deltaTime;
            targetSpeed = Mathf.Clamp(tmpSpeed, 0, maxGas);
        }

        Debug.Log("Target speed: " + targetSpeed);
    }

    public void OnClickBreakPedal(bool isBreakPedalDown)
    {
        m_isBreakPedalDown = isBreakPedalDown;
    }

    public void OnClickGasPedal(bool isGasPedaDown)
    {
        m_isGasPedalDown = isGasPedaDown;
        //   Debug.Log("Pedal clicked");
    }

}
