 using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnemyOverworldManager : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D enemyRigidbody;

    public float xVector;
    public float yVector;
    public float movetimer;
    public float moveSpd = 10;

    public int id;

    public string currentAnimation;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
        MovePlayer();
    }

    void GetInput()
    {
        if (movetimer <= 0)
        {
            int dir = Random.Range(0, 9);

            switch(dir)
            {
                case 0: //no movement
                    xVector = 0;
                    yVector = 0;
                    break;
                case 1: //right
                    xVector = 1;
                    yVector = 0;
                    break;
                case 2: //left
                    xVector = -1;
                    yVector = 0;
                    break;
                case 3: //up
                    xVector = 0;
                    yVector = 1;
                    break;
                case 4: //down
                    xVector = 0;
                    yVector = -1;
                    break;
                case 5: //upright
                    xVector = 1;
                    yVector = 1;
                    break;
                case 6: //upleft
                    xVector = -1;
                    yVector = 1;
                    break;
                case 7: //downright
                    xVector = 1;
                    yVector = -1;
                    break;
                case 8: //downleft
                    xVector = -1;
                    yVector = -1;
                    break;
                    
            }
            SetAnimation();
            movetimer = (2 + (0.1f * (Random.Range(-2, 4))));

            
        }
        movetimer -= 1f * Time.deltaTime;
    }

    void MovePlayer()
    {
        UnityEngine.Vector2 movement = new UnityEngine.Vector2(xVector, yVector);
        movement *= Time.deltaTime * moveSpd;


        //playerRigidbody.AddForce(movement);
        enemyRigidbody.velocity = movement;
    }

    void SetAnimation()
    {
        if (animator != null)
        {
            if (yVector == 0)
            {
                if (xVector < 0)
                {
                    animator.SetInteger("dir", 0);
                    animator.Play(currentAnimation, -1, 0.60f);
                    currentAnimation = "zomb_1_left";
                    Debug.Log("left");
                }
                if (xVector > 0)
                {
                    animator.SetInteger("dir", 1);
                    animator.Play(currentAnimation, -1, 0.60f);
                    currentAnimation = "zomb_1_right";
                    Debug.Log("right");
                }
            }


            if (yVector < 0)
            {
                animator.SetInteger("dir", 2);
                animator.Play(currentAnimation, -1, 0.60f);
                currentAnimation = "zomb_1_down";
                Debug.Log("down");
            }
            if (yVector > 0)
            {
                animator.SetInteger("dir", 3);
                currentAnimation = "zomb_1_up";
                Debug.Log("up");
            }




            if (xVector == 0 && yVector == 0)
            {

                animator.Play(currentAnimation, -1, 0.60f);
                animator.speed = 0f;

            }
            else
            {
                animator.speed = 1f;
            }
        }

    }
}
