using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarBehaviour : MonoBehaviour
{
    public float rotAmount;
    public float maxRot = 50f;
    [HideInInspector] public float prevRotAmount = 0;
    public float targetSpeed = 0f;
    [HideInInspector] public float prevTargetSpeed = 0f;
    public float rotSpeed = 0f;
    public float rotLock = 0.05f;
    public float speed = 0f;
    public float acceleration = .05f;
    public float maxSpeed = 1f;
    public bool isMoving = false;
    public float turnDuration = 5f;
    [HideInInspector] public float steps = 0f;
    public float delta = 0.02f;

    public Slider gasSlider;
    public Button turnButton;





    void Update()
    {
        rotAmount = -SimpleInput.GetAxis("Horizontal") * maxRot;
        targetSpeed = gasSlider.value;




        if (isMoving)
        {
            Move();
            steps += delta;
            if (steps >= turnDuration)
            {
                isMoving = false;
                turnButton.interactable = true;
                steps = 0f;
            }
        }

    }

    public void CompleteTurn()
    {
        isMoving = true;
        turnButton.interactable = false;
    }



    void Move()
    {
        //m_speed += m_acceleration * Time.deltaTime;
        speed = Mathf.MoveTowards(speed, targetSpeed, acceleration * delta);
        //m_speed = Mathf.Clamp(m_speed, -0.5f, m_maxSpeed);

        if (rotAmount > 0)
        {
            rotSpeed = rotAmount - speed;
        }
        else if (rotAmount < 0)
        {
            rotSpeed = rotAmount + speed;
        }
        else
        {
            rotSpeed = 0f;
        }

        rotSpeed *= delta;
        Vector3 rot = new Vector3(0f, 0f, rotSpeed);
        transform.position += transform.up * speed;
        if (speed > rotLock)
            transform.Rotate(rot);
    }


}
