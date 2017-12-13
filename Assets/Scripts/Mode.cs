using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class IMode //: MonoBehaviour
{
    public List<Vector3> m_path;

    public abstract void Execute(Player player);
}

public class ControlMode : IMode
{
    public override void Execute(Player player)
    {
        handleInput(player);
    }

    void handleInput(Player player)
    {
        if (Input.GetKey(KeyCode.W))
        {
            player.Forward.Execute(player, player.Forward);
        }

        if (Input.GetKey(KeyCode.S))
        {
            player.Back.Execute(player, player.Back);
        }

        if (Input.GetKey(KeyCode.D))
        {
            player.Right.Execute(player, player.Right);
        }

        if (Input.GetKey(KeyCode.A))
        {
            player.Left.Execute(player, player.Left);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            player.Plane.Execute(player, player.Plane);
        }
    }
}

public class AutoMode : IMode
{
    Vector3 currentTarget = Vector3.zero;
    public override void Execute(Player player)
    {
        if (m_path == null || m_path.Count <= 0)
            return;

        if(currentTarget == Vector3.zero)
            currentTarget = m_path[0];
        FollowPath(player);
    }

    void FollowPath(Player player)
    {
        Debug.Log(currentTarget + " | nb: " + m_path.Count);
        //currentTarget = m_path[0];
        float smoothTime = 0.2F;
        Vector3 velocity = Vector3.zero;

        float distanceRespectable = 2f;

        if (Vector3.Distance(player.transform.position, currentTarget) <= distanceRespectable)
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
            player.transform.position = Vector3.SmoothDamp(player.transform.position, currentTarget, ref velocity, smoothTime);
        }
    }
}

public class TrappedMode : IMode
{
    public override void Execute(Player player)
    {
    }
}
