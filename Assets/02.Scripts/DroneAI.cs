using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    public enum DroneState { Idle, Move, Attack, Damage, Die }
    public DroneState state = DroneState.Idle;

    float idleDelayTime = 2f;
    float currTime;

    NavMeshAgent myAgent;
    float moveSpeed = 1;
    Transform tower;

    float attackRange = 4;
    float attackDelayTime = 2;

    int hp = 3;

    bool isDead;
    Transform explosion;

    private void Start()
    {
        explosion = GameObject.Find("Explosion").transform;

        myAgent = GetComponent<NavMeshAgent>();
        myAgent.speed = moveSpeed;
        myAgent.enabled = false;

        tower = GameObject.Find("Tower").transform;

        //StartCoroutine(CheckState()); //->StopCoroutine 불가능
        //StartCoroutine("CheckState"); ->StopCoroutine 가능 
    }


    private void Update()
    {
        if (isDead) return;

        switch (state)
        {
            case DroneState.Idle: Idle(); break;
            case DroneState.Move: Move(); break;
            case DroneState.Attack: Attack(); break;
            case DroneState.Damage: break;
            case DroneState.Die: Die(); break;
        }
    }

    private void Idle()
    {
        print(currTime);
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
        currTime += Time.deltaTime;
        if (currTime > attackDelayTime)
        {
            tower.GetComponent<Tower>().Hp--;
            currTime = 0;
        }
    }

    IEnumerator Damage()
    {
        myAgent.enabled = false;

        Material mat = GetComponentInChildren<MeshRenderer>().material;
        Color orignColor = mat.color;

        mat.color = Color.red;

        yield return new WaitForSeconds(0.1f);
        mat.color = orignColor;

        state = DroneState.Idle;
        currTime = 0;
    }

    private void Die()
    {
        isDead = true;

        explosion.position = transform.position;
        explosion.GetComponent<ParticleSystem>().Play();
        explosion.GetComponent<AudioSource>().Play();

        Destroy(gameObject);
    }

    public void OnDamageProcess()
    {
        hp--;
        if (hp > 0)
        {
            state = DroneState.Damage;
            StopCoroutine("Damage");
            StartCoroutine("Damage");
        }
        else
        {
            state = DroneState.Die;
        }
    }
}