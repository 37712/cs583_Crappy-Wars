﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public EnemyUnit enemy_unit;
    public Transform player_tranform;
    public Animator animator;
    public NavMeshAgent nav;
    public bool running = false,
                 init_rot = false,
                 init_mov = false;

    public float rotSpeed; // normaly 90f

    public bool attack = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        nav.stoppingDistance = 3f;
        player_tranform = player_tranform.GetComponent<Transform>();
    }

    private void Update()
    {
        enemyMove();
    }


    /* Private methods */
    private void enemyMove()
    {
        //if(BattleManager.state == BattleState.ENEMYMOVEMENT)
        //{
            move_enemy(enemy_unit.move_enemy);
            rotate_unit();
            move_to_direction();

            /* Keep the player's tranformation updated */
            //player_tranform = UnitManager.Instance.playerUnit.transform;
            
       // }
    }

    private void move_enemy(bool move)
    {
        if (move)
        {
            init_rot = true;

            //deactivate movement
            enemy_unit.move_enemy = false;

            //move unit
            nav.SetDestination(enemy_unit.destination);

            /***** THIS PART IS FOR ANIMATION **********/

            //animation
            running = true;

            /**** END OF ANIMATION SECTION *************/

            // set animation state
            animator.SetBool("running", running);
        }
    }

    private void rotate_unit()
    {
        if(init_rot)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, enemy_unit.destination - transform.position), rotSpeed * Time.deltaTime);

        if (Quaternion.LookRotation(Vector3.forward, enemy_unit.destination - transform.position) == transform.rotation && init_rot)
        {
            init_rot = false;
            init_mov = true;
        }

    }

    private void move_to_direction()
    {
        if (init_mov)
            nav.SetDestination(enemy_unit.destination);
            //nav.SetDestination(Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime));

        //Is the enemy unit close enough to the player unit?
        if (Vector3.Distance(player_tranform.transform.position, transform.position) <= nav.stoppingDistance)
        {
            init_mov = false;
            running = false;

            
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            attack = true;
            animator.SetBool("attack", attack);
        }
    }
}
