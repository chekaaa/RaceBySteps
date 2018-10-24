using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CarBehaviour : MonoBehaviourPunCallbacks
{

    [HideInInspector] public float prevRotAmount = 0;
    [HideInInspector] public float prevTargetSpeed = 0f;
    [HideInInspector] public float rotSpeed = 0f;
    public float rotLock = 0.05f;
    [HideInInspector] public float speed = 0f;
    public float acceleration = .05f;
    private float m_rotAmount = 0f;

    private float m_targetSpeed = 0f;


    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     if (stream.IsWriting)
    //     {
    //         stream.SendNext(rotSpeed);
    //         stream.SendNext(speed);

    //     }
    //     else
    //     {
    //         rotSpeed = (float)stream.ReceiveNext();
    //         speed = (float)stream.ReceiveNext();

    //     }
    // }




    public void Init()
    {
        // if (PhotonNetwork.IsMasterClient)
        // {
        this.enabled = true;
        // }
    }

    void Update()
    {
        if (GameManager.instance.isMovePhase)
        {
            Move();
        }
    }

    void Move()
    {
        //m_speed += m_acceleration * Time.deltaTime;
        speed = Mathf.MoveTowards(speed, m_targetSpeed, acceleration * GameManager.instance.delta);
        //m_speed = Mathf.Clamp(m_speed, -0.5f, m_maxSpeed);

        if (m_rotAmount > 0)
        {
            rotSpeed = m_rotAmount - speed;
        }
        else if (m_rotAmount < 0)
        {
            rotSpeed = m_rotAmount + speed;
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

    public void SetMoveValues(float _rotAmount, float _targetSpeed)
    {

        m_rotAmount = _rotAmount;
        m_targetSpeed = _targetSpeed;
    }

    public void StopCar()
    {
        m_rotAmount = 0f;
        m_targetSpeed = 0f;
        speed = 0f;
        rotSpeed = 0f;
    }


}
