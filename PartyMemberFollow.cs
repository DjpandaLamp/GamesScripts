using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberFollow : MonoBehaviour
{
    private PlayerOverworldManager player;
    public Animator animator;
    public Rigidbody2D rigidbody2d;
    public float yVector;
    public float xVector;
    public string currentAnimation;
    public int moveTimer;

    public int memberIndex; //Which Party Member is in Slot
    public int memberPos; //Which position the party member is at.

    public Vector2 movement;
    public Vector2 playerPos;
    public Vector2 dir;
    public Vector2 offsetPos; //The Offset
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerOverworldManager>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.xVector != 0 || player.yVector != 0)
        {
            movement = player.transform.position;
            switch (player.dir)
            {
                case 0:
                    if (memberPos == 1)
                    {
                        offsetPos = new Vector2(.05f, .05f);
                    }
                    else if (memberPos == 2)
                    {
                        offsetPos = new Vector2(.05f, -.05f);
                    }
                    movement += new Vector2(0.5f, 0);
                    break;
                case 1:
                    if (memberPos == 1)
                    {
                        offsetPos = new Vector2(-.05f, .05f);
                    }
                    else if (memberPos == 2)
                    {
                        offsetPos = new Vector2(-.05f, -.05f);
                    }
                    movement += new Vector2(-0.5f, 0);
                    break;
                case 2:
                    if (memberPos == 1)
                    {
                        offsetPos = new Vector2(-.05f, .05f);
                    }
                    else if (memberPos == 2)
                    {
                        offsetPos = new Vector2(.05f, .05f);
                    }
                    movement += new Vector2(0, 0.5f);
                    break;
                case 3:
                    if (memberPos == 1)
                    {
                        offsetPos = new Vector2(-.05f, -.05f);
                    }
                    else if (memberPos == 2)
                    {
                        offsetPos = new Vector2(.05f, -.05f);
                    }
                    movement += new Vector2(0, -0.5f);
                    break;
            }
            dir = -Vector2.MoveTowards(transform.localPosition, transform.InverseTransformPoint(movement), 1);
            movement = Vector2.Lerp(transform.position, movement, 0.07f);
            MovePlayer();
        }
       else
        {
            dir = Vector2.zero;
            
        }
       
        //dist = Vector2.Distance()

        
        SetAnimation();
    }

    void MovePlayer()
    {
        //playerRigidbody.AddForce(movement);
        transform.position = movement + offsetPos;
    }

    void SetAnimation()
    {
        xVector = dir.x;
        yVector = dir.y;
        if (animator != null)
        {
           

            if (yVector < 0.4f && yVector > -0.4f)
            {
                yVector = 0;
            }
            if (yVector == 0)
            {
                if (xVector < 0)
                {
                    animator.SetTrigger("Left");
                    currentAnimation = "AnimationDevWalkLeft";
                }
                if (xVector > 0)
                {
                    animator.SetTrigger("Right");
                    currentAnimation = "AnimationDevWalkRight";
                }
            }


            if (yVector < 0)
            {
                animator.SetTrigger("Down");
                currentAnimation = "AnimationDevWalkDown";
            }
            if (yVector > 0)
            {
                animator.SetTrigger("Up");
                currentAnimation = "AnimationDevWalkUp";
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
