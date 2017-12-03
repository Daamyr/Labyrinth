using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{

    public Vector3 currentTarget;//à setter pour débuter

    List<Vector3> m_path;
    public float smoothTime = 0.3F;
    public Vector3 velocity = Vector3.zero;
    public float distanceRespectable = 3.0f;

    public OscillatingBall ballPrefab;
    OscillatingBall ball;

    public List<Vector3> Path
    {
        set { m_path = value; }
    }

    public int m_nbNode;

    // Use this for initialization
    void Start()
    {
        ball = Instantiate(ballPrefab);
        ball.Target = gameObject;
        //m_path = new List<Vector3>();
    }


    // Update is called once per frame
    void Update()
    {
        //if (m_path.Count <= 0)
        //    tourne = false;
        //if (tourne)
        //{
        //    if (Vector3.Distance(transform.position, currentTarget) <= distanceRespectable)
        //    {
        //        try
        //        {
        //            List<Vector3> tmp = m_path;
        //            tmp.RemoveAt(0);
        //            m_path = tmp;
        //            currentTarget = m_path[0];
        //        }
        //        catch { }
        //    }
        //    else
        //    {
        //        transform.position = Vector3.SmoothDamp(transform.position, currentTarget, ref velocity, smoothTime);
        //    }
        //}
    }

    IEnumerator FollowPath()
    {
        currentTarget = m_path[0];
        while (m_path.Count > 0)
        {
            WaitForFixedUpdate delay = new WaitForFixedUpdate();

            if (Vector3.Distance(transform.position, currentTarget) <= distanceRespectable)
            {
                try
                {
                    List<Vector3> tmp = m_path;
                    tmp.RemoveAt(0);
                    m_path = tmp;
                    currentTarget = m_path[0];
                }
                catch { }
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, currentTarget, ref velocity, smoothTime);
            }

            yield return null;
        }

        Destroy(ball.gameObject);
        Destroy(gameObject);
    }

    void fakePath()
    {
        List<Vector3> tmpStack = new List<Vector3>();

        float hauteur = 10;

        tmpStack.Add(new Vector3(0, hauteur, 0));
        tmpStack.Add(new Vector3(3, hauteur, 0));
        tmpStack.Add(new Vector3(3, hauteur, 3));
        tmpStack.Add(new Vector3(0, hauteur, 3));
        tmpStack.Add(new Vector3(0, hauteur, 0));

        m_path = tmpStack;
    }
}
