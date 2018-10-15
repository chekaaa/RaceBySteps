using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryDisplayer : MonoBehaviour
{
    private CarBehaviour m_carBehaviour;

    public GameObject dotPrefab;
    public Transform dotGuide;
    List<GameObject> dotList = new List<GameObject>();

    private void Awake()
    {
        m_carBehaviour = GetComponent<CarBehaviour>();
    }

    void Update()
    {


        if (m_carBehaviour.rotAmount != m_carBehaviour.prevRotAmount || m_carBehaviour.targetSpeed != m_carBehaviour.prevTargetSpeed)
        {
            m_carBehaviour.prevRotAmount = m_carBehaviour.rotAmount;
            m_carBehaviour.prevTargetSpeed = m_carBehaviour.targetSpeed;
            UpdateDots();
        }
        if (m_carBehaviour.isMoving)
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

        float amount = m_carBehaviour.turnDuration / m_carBehaviour.delta;


        for (int i = 0; i <= amount; i++)
        {
            float rest = i % 5;
            if (rest == 0)
            {
                GameObject go = Instantiate(dotPrefab, dotGuide.position, Quaternion.identity) as GameObject;
                dotList.Add(go);
            }
            //spd += m_acceleration * 0.02f;
            spd = Mathf.MoveTowards(spd, m_carBehaviour.targetSpeed, m_carBehaviour.delta * m_carBehaviour.acceleration);
            spd = Mathf.Clamp(spd, -0.5f, m_carBehaviour.maxSpeed);

            if (m_carBehaviour.rotAmount > 0)
            {
                rSpd = m_carBehaviour.rotAmount - spd;
            }
            else if (m_carBehaviour.rotAmount < 0)
            {
                rSpd = m_carBehaviour.rotAmount + spd;
            }
            else
            {
                rSpd = 0f;
            }
            rSpd *= m_carBehaviour.delta;
            Vector3 r = new Vector3(0f, 0f, rSpd);
            dotGuide.Rotate(r);
            dotGuide.position += dotGuide.up * spd;

        }
    }
}
