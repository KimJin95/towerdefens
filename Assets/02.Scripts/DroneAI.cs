using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    public enum DroneState { Idle, Move, Attack, Damage, Die }
    DroneState state = DroneState.Idle;

    float idleDelayTime = 2f;
    float currTime;

    NavMeshAgent myAgent;
    float moveSpeed;
    Transform tower;

    float attackRange = 3;

    private void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.speed = moveSpeed;
        myAgent.enabled = false;

        tower = FindObjectOfType<Tower>().transform;
    }


    private void Update()
    {
        switch (state)
        {
            case DroneState.Idle: Idle(); break;
            case DroneState.Move: Move(); break;
            case DroneState.Attack: Attack(); break;
            case DroneState.Damage: Damage(); break;
            case DroneState.Die: Die(); break;

        }
    }

    private void Idle()
    {
        currTime += Time.deltaTime;
        if (currTime >= idleDelayTime)
        {
            state = DroneState.Move;
            myAgent.enabled = true;
        }
    }

    private void Move()
    {
        myAgent.SetDestination(tower.position);

        float dist = Vector3.Distance(tower.position, transform.position);
        if (dist <= attackRange)
        {
            state = DroneState.Attack;
            myAgent.enabled = false;
        }
    }

    private void Attack()
    {

    }

    private void Damage()
    {

    }

    private void Die()
    {

    }
}