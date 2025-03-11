using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberFollow : MonoBehaviour
{
    private PlayerOverworldManager player;
    public Animator animator;
    public Rigidbody2D rigidbody2d;
    public GameObject perst;
    public GlobalPersistantScript GlobalPersistant;

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
    public int spacing;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerOverworldManager>();
        perst = GameObject.FindWithTag("Persistant");
        GlobalPersistant = perst.GetComponent<GlobalPersistantScript>();
        if (spacing <= 0)
        {
            spacing = 1;
        }
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
                    movement += new Vector2(0.5f, 0);
                    break;
                case 1:

                    movement += new Vector2(-0.5f, 0);
                    break;
                case 2:

                    movement += new Vector2(0, 0.5f);
                    break;
                case 3:

                    movement += new Vector2(0, -0.5f);
                    break;
            }
            dir = -Vector2.MoveTowards(transform.localPosition, transform.InverseTransformPoint(movement), 1);
            

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
        bool hasMoved = false;
        //playerRigidbody.AddForce(movement);
        if (player.positionArray.ToArray().Length > spacing*memberIndex)
        {
            transform.position = player.positionArray[spacing * memberIndex - 1];
            hasMoved = true;
        }
        if (memberIndex == 3 && hasMoved)
        {
            player.positionArray.Remove(player.positionArray[player.positionArray.ToArray().Length - 1]);
        }

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
