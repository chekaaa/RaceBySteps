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
        float _delta = (maxGas / 2f) * GameManager.instance.delta;
        float _maxTargetGas;

        if (m_isGasPedalDown)
        {
            _maxTargetGas = GameManager.instance.localCar.speed +
             ((GameManager.instance.localCar.acceleration * GameManager.instance.delta)
              * GameManager.instance.totalSteps);

            _maxTargetGas = Mathf.Clamp(_maxTargetGas, 0f, maxGas);

            // Debug.Log("Pedal clicked");
            float tmpSpeed = targetSpeed + _delta;
            targetSpeed = Mathf.Clamp(tmpSpeed, 0, _maxTargetGas);
        }
        else if (m_isBreakPedalDown)
        {
            _maxTargetGas = GameManager.instance.localCar.speed -
             (((GameManager.instance.localCar.acceleration * GameManager.instance.localCar.breakMultiplier) * GameManager.instance.delta)
             * GameManager.instance.totalSteps);

            _maxTargetGas = Mathf.Clamp(_maxTargetGas, 0f, maxGas);

            float tmpSpeed = targetSpeed - _delta;
            targetSpeed = Mathf.Clamp(tmpSpeed, _maxTargetGas, maxGas);
        }


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
