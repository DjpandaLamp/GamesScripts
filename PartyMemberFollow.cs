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
    public ParticleSystem particleSystem;

    public float yVector;
    public float xVector;
    public string currentAnimation;
    public int moveTimer;

    public int memberIndex; //Which Party Member is in Slot
    public int memberPos; //Which position the party member is at.

    public Vector2 movement;
    public Vector2 playerPos;
    public int dir;
    public Vector2 offsetPos; //The Offset
    public int spacing;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerOverworldManager>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
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
           /* switch (player.dir)
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
            } */
          
            

            MovePlayer();
        }
        SetAnimation();
    }

    void MovePlayer()
    {
        bool hasMoved = false;
        //playerRigidbody.AddForce(movement);
        if (player.positionArray.ToArray().Length > spacing*memberIndex)
        {
            transform.position = player.positionArray[spacing * memberIndex - 1];
            dir = player.directionArray[spacing * memberIndex - 1];
            hasMoved = true;
        }
        if (memberIndex == 3 && hasMoved)
        {
            player.positionArray.Remove(player.positionArray[player.positionArray.ToArray().Length - 1]);
            player.directionArray.Remove(player.directionArray[player.directionArray.ToArray().Length - 1]);
        }

    }

    void SetAnimation()
    {
        if (animator != null)
        {


            if (dir == 0)
                {
                    animator.SetTrigger("Left");
                    currentAnimation = "AnimationDevWalkLeft";
                }
                if (dir == 1)
                {
                    animator.SetTrigger("Right");
                    currentAnimation = "AnimationDevWalkRight";
            }



            if (dir == 2)
            {
                animator.SetTrigger("Down");
                currentAnimation = "AnimationDevWalkDown";
            }
            if (dir == 3)
            {
                animator.SetTrigger("Up");
                currentAnimation = "AnimationDevWalkUp";
            }




            if (player.playerRigidbody.velocity.x == 0 && player.playerRigidbody.velocity.y == 0)
            {

                animator.Play(currentAnimation, -1, 0.60f);
                animator.speed = 0f;
                if (particleSystem.isPlaying)
                {
                    particleSystem.Stop();
                }
            }
            else
            {
                animator.speed = 1f;
                if (particleSystem.isStopped)
                {
                    particleSystem.Play();
                }

            }
        }

    }
}
