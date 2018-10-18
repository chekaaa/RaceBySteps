using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarBehaviour : MonoBehaviour
{

    [HideInInspector] public float prevRotAmount = 0;
    [HideInInspector] public float prevTargetSpeed = 0f;
    [HideInInspector] public float rotSpeed = 0f;
    public float rotLock = 0.05f;
    [HideInInspector] public float speed = 0f;
    public float acceleration = .05f;



    void Update()
    {
        if (GameManager.instance.isMovePhase)
        {
            Move();
        }
    }

    void Move()
    {
        float _targetSpeed = PlayerController.instance.targetSpeed;
        float _rotAmount = PlayerController.instance.rotAmount;
        //m_speed += m_acceleration * Time.deltaTime;
        speed = Mathf.MoveTowards(speed, _targetSpeed, acceleration * GameManager.instance.delta);
        //m_speed = Mathf.Clamp(m_speed, -0.5f, m_maxSpeed);

        if (_rotAmount > 0)
        {
            rotSpeed = _rotAmount - speed;
        }
        else if (_rotAmount < 0)
        {
            rotSpeed = _rotAmount + speed;
        }
        else
        {
            rotSpeed = 0f;
        }

        rotSpeed *= GameManager.instance.delta;
        Vector3 rot = new Vector3(0f, 0f, rotSpeed);
        transform.position += transform.up * speed;
        if (speed > rotLock)
            transform.Rotate(rot);
    }


}
