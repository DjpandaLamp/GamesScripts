 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOverworldManager : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D enemyRigidbody;
    public CircleCollider2D circleCollider2D;

    public GameObject perst;
    public GlobalPersistantScript GlobalPersistant;

    public float xVector;
    public float yVector;
    public float movetimer;
    public float moveSpd = 10;
    public bool isChasing;
    public PlayerOverworldManager player;



    public int id;

    public string currentAnimation;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerOverworldManager>();
        perst = GameObject.FindWithTag("Persistant");
        GlobalPersistant = perst.GetComponent<GlobalPersistantScript>();
        moveSpd = moveSpd * Random.Range(0.8f, 1.2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
        
    }

    void GetInput()
    {
        if (!GlobalPersistant.isPaused)
        {
            if (!isChasing)
            {
                if (movetimer <= 0)
                {
                    int dir = Random.Range(0, 9);

                    switch (dir)
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
                    MovePlayer();
                    SetAnimation();
                    movetimer = (2 + (0.1f * (Random.Range(-2, 4))));


                }

            }
            else
            {
                if (movetimer <= 1)
                {
                    UnityEngine.Vector2 direction = UnityEngine.Vector3.Normalize((transform.position - player.transform.position));


                    xVector = -direction.x;
                    yVector = -direction.y;
                    movetimer = (0.5f + (0.1f * (Random.Range(-2, 4))));
                }

                MovePlayer();
                SetAnimation();
            }
            movetimer -= 1f * Time.deltaTime;
        }
        else
        {
            enemyRigidbody.velocity = Vector2.zero;
        }
    }

    void MovePlayer()
    {
        UnityEngine.Vector2 movement = new UnityEngine.Vector2(xVector, yVector).normalized;


        
        movement *= Time.deltaTime * moveSpd;

        xVector = movement.x;
        yVector = movement.y;

        if (isChasing)
        {
            movement *= 1.5f;
        }

        //playerRigidbody.AddForce(movement);
        enemyRigidbody.velocity = movement;
    }

    void SetAnimation()
    {
        if (animator != null)
        {

            if (yVector < 0.4f && yVector > -0.4f)
            {
                yVector = 0;
            }

            if (yVector == 0)
            {
                if (xVector > 0)
                {
                    animator.SetInteger("dir", 0);
                    //animator.Play(currentAnimation, -1, 0.60f);
                    //currentAnimation = "zomb_1_left";
                    Debug.Log("left");            
                }
                if (xVector < 0)
                {
                    animator.SetInteger("dir", 1);
                    //animator.Play(currentAnimation, -1, 0.60f);
                    //currentAnimation = "zomb_1_right";
                    Debug.Log("right");

                }
            }


            if (yVector < 0)
            {
                animator.SetInteger("dir", 2);

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
