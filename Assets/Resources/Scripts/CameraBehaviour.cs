using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehaviour : MonoBehaviour
{

    public static CameraBehaviour instance;

    public Transform target;
    [SerializeField] float m_followSpeed = .5f;

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

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, m_followSpeed * Time.deltaTime);
    }
}
