using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TrajectoryDisplayer : MonoBehaviourPun
{
    private CarBehaviour m_carBehaviour;

    private const string DOT_GUIDE = "DotGuide";

    public GameObject dotPrefab;
    private Transform dotGuide;
    List<GameObject> dotList = new List<GameObject>();


    public void Init()
    {
        CarInfo _info = GetComponent<CarInfo>();
        if (_info.ownerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            this.enabled = true;
            m_carBehaviour = GetComponent<CarBehaviour>();
            dotGuide = GameObject.Find(DOT_GUIDE).transform;
        }
    }

    void Update()
    {
        float _targetSpeed = PlayerController.instance.targetSpeed;
        float _rotAmount = PlayerController.instance.rotAmount;

        //Update dots if  the rotation or targetSpeed changed
        if (_rotAmount != m_carBehaviour.prevRotAmount || _targetSpeed != m_carBehaviour.prevTargetSpeed)
        {
            m_carBehaviour.prevRotAmount = _rotAmount;
            m_carBehaviour.prevTargetSpeed = _targetSpeed;
            UpdateDots();
        }
        //Remove all dots if is in move Phase
        if (GameManager.instance.isMovePhase)
        {
            RemoveDots();

        }
        else if (dotList.Count == 0)
        {
            UpdateDots();
        }
    }

    void UpdateDots()
    {
        RemoveDots();
        DrawTrajectory();
    }

    void RemoveDots()
    {
        if (dotList != null)
        {
            foreach (GameObject go in dotList)
            {
                Destroy(go);
            }
            dotList.Clear();
        }
    }

    void DrawTrajectory()
    {
        dotGuide.position = transform.position;
        dotGuide.rotation = transform.rotation;
        float spd = m_carBehaviour.speed;
        float rSpd = m_carBehaviour.rotSpeed;
        float _targetSpeed = PlayerController.instance.targetSpeed;
        float _rotAmount = PlayerController.instance.rotAmount;


        float amount = GameManager.instance.turnDuration / GameManager.instance.delta;


        for (int i = 0; i <= amount; i++)
        {
            float rest = i % 5;
            if (rest == 0)
            {
                GameObject go = Instantiate(dotPrefab, dotGuide.position, Quaternion.identity) as GameObject;
                dotList.Add(go);
            }
            //spd += m_acceleration * 0.02f;
            spd = Mathf.MoveTowards(spd, _targetSpeed, GameManager.instance.delta * m_carBehaviour.acceleration);

            if (_rotAmount > 0)
            {
                rSpd = _rotAmount - spd;
            }
            else if (_rotAmount < 0)
            {
                rSpd = _rotAmount + spd;
            }
            else
            {
                rSpd = 0f;
            }
            rSpd *= GameManager.instance.delta;
            Vector3 r = new Vector3(0f, 0f, rSpd);
            dotGuide.Rotate(r);
            dotGuide.position += dotGuide.up * spd;

        }
    }
}
