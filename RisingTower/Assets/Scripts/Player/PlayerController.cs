using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    protected Player player;
    protected PlayerInput input_controller;
    protected NavMeshAgent nav_con;

    protected Target target;

    // Use this for initialization
    void Start () {
        player = GetComponent<Player>();
        input_controller = GetComponent<PlayerInput>();
        nav_con = GetComponent<NavMeshAgent>();

        input_controller.RegisterHandler(PlayerInput.Inputs.MouseRight, () => player.UseSkill(0, target));
        input_controller.RegisterHandler(PlayerInput.Inputs.Key1, () => player.UseSkill(1, target));
    }
	
    protected void UpdateMovement()
    {
        target = input_controller.MousePosition;

        if (input_controller.MouseLeft)
        {
            if (target != null)
            {
                nav_con.SetDestination(target.Position);
            }
        }

        if (nav_con.remainingDistance > nav_con.stoppingDistance)
            player.Move(nav_con.desiredVelocity, false, false);
        else
            player.Move(Vector3.zero, false, false);
    }

    void Update()
    {
        UpdateMovement();
    }
}
