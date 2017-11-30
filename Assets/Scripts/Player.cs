using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public new GameObject gameObject;
    public GameObject camera;

    public float forceSaut = 25f;
    public float maxSaut = 10f;
    private Action forward, back, right, left, jump;

    enum State
    {
        Standing,
        Walking
    }

    State m_state;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        m_timer = timer;
        m_state = State.Standing;

        forward = new Forward();
        back = new Back();
        right = new Right();
        left = new Left();
        jump = new Jump(forceSaut, maxSaut, this.GetComponents<Rigidbody>()[0]);
    }

    Vector3 prevPos;

    // Update is called once per frame
    void FixedUpdate()
    {
        handleKey();

        if(prevPos != transform.position)
        {
            prevPos = transform.position;
            m_state = State.Walking;
        }
        else
        {
            m_state = State.Standing;
        }

        //Vector3 pos = camera.transform.position;
        //pos.y += 2f * Time.deltaTime;

        //camera.transform.position = pos;

        //camera.transform.Rotate(0, 0, angleZ);

        m_timer -= Time.deltaTime;
        if (m_timer <= 0)
        {
            m_timer = timer;
            pasGauche = !pasGauche;
        }

        ocillation();
    }

    void ocillation()
    {
        Vector3 pos = camera.transform.position;
        if (m_state == State.Walking)
        {
            if (pasGauche)
            {
                pos.y = transform.position.y + angle;
            }
            else
            {
                pos.y = transform.position.y - angle;
            }

            camera.transform.position = pos;
        }
    }

    public void OnCollisionEnter(Collision col)
    {

        //Debug.Log (col.gameObject.name);
        //player.GetComponents<hello> () [0].salut ();

    }

    public float timer = 0.5f;
    float m_timer;
    bool pasGauche = false;
    public float angle = 5f;

    public void handleKey()
    {
        //public key binding ?
        if (Input.GetKey(KeyCode.W))
        {
            m_state = State.Walking;
            forward.Execute(this, forward);
        }

        if (Input.GetKey(KeyCode.S))
        {
            back.Execute(this, back);
        }

        if (Input.GetKey(KeyCode.D))
        {
            right.Execute(this, right);
        }

        if (Input.GetKey(KeyCode.A))
        {
            left.Execute(this, left);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jump.Execute(this, jump);
        }
    }
}
