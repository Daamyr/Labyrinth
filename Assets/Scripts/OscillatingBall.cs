using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatingBall : MonoBehaviour
{

    public float m_amplitude = 1;
    public float velocity = 0.025f;
    float angle = 0;
    public Vector3 offset;

    GameObject m_rotateAround;


    public GameObject Target
    {
        get { return m_rotateAround; }
        set { m_rotateAround = value; }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = new Vector3();

        pos = m_rotateAround.transform.position + offset;

        transform.rotation = m_rotateAround.transform.rotation;

        float y = m_amplitude * Mathf.Sin(angle);
        angle += velocity;
        pos.y += y;// Mathf.Sin(rotateAround.transform.position.y * m_speed) * m_amplitude;
        transform.position = pos;

    }

}
