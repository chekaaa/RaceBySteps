using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehaviour : MonoBehaviour
{

    public static CameraBehaviour instance;

    private Animator m_animator;

    [HideInInspector] public Transform target;
    public Transform spectatorTarget;
    [SerializeField] float targetSizePlaying = 10, targetSizeSpectator = 15;
    private float actualTargetSize = 10;
    [SerializeField] float m_followSpeed = .5f;

    private void Awake()
    {
        actualTargetSize = targetSizePlaying;

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
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, actualTargetSize
        , m_followSpeed * Time.deltaTime);
    }

    public void ChangeSpectateCamera()
    {
        actualTargetSize = targetSizeSpectator;
        target = spectatorTarget;

    }


}
