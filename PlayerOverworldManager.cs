using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOverworldManager : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody2D playerRigidbody;
    public Animator animator;
    public JSONSave JSONSave;
    public TextStartEnd textSE;
    public OverworldMenuManager overworldMenuManager;
    public Camera Camera;

    public string currentAnimation;

    public bool canMove;

    public int dir = 0;
    public float xVector;
    public float yVector;
    public Vector2 pastPos;
    public float moveSpd;
    public List<Vector2> positionArray;

    public float disSinceLastEncounter;

    // Start is called before the first frame update
    void Start()
    {
        
        textSE = GameObject.FindObjectOfType<TextStartEnd>(true);
        overworldMenuManager = GameObject.FindObjectOfType<OverworldMenuManager>(true);
        animator = GetComponent<Animator>();
        JSONSave = GameObject.Find("PersistantObject").GetComponent<JSONSave>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (textSE.isActiveAndEnabled == false && overworldMenuManager.isUp == false && overworldMenuManager.yPos < -440)

        {
            GetInput(false, 0, 0);
            if (xVector != 0 || yVector != 0)
            {

                if (yVector == 0)
                {
                    if (xVector < 0)
                    {
                        dir = 0; //left
                    }
                    if (xVector > 0)
                    {
                        dir = 1; //right
                    }
                }


                if (yVector < 0)
                {
                    dir = 2; //up
                }
                if (yVector > 0)
                {
                    dir = 3; //down
                }
            }
            MovePlayer();
        }
        else
        {
            GetInput(true, 0, 0);
            MovePlayer();
        }

        SetAnimation();

    }

    void GetInput(bool overrule, float oXVector, float oYVector)
    {
        if (overrule)
        {
            xVector = oXVector;
            yVector = oYVector;
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                xVector = Mathf.Sign(Input.GetAxis("Horizontal"));
            }
            else
            {
                xVector = 0;
            }
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                yVector = Mathf.Sign(Input.GetAxis("Vertical"));
            }
            else
            {
                yVector = 0;
            }
        }
    }

    public void setPastPos()
    {
        pastPos = transform.position;
    }

    void MovePlayer()
    {
        Vector2 movement = new Vector2(xVector, yVector);
        movement *= Time.deltaTime * moveSpd;
        disSinceLastEncounter += Vector2.Distance(Vector2.zero, movement) / 15;

        //playerRigidbody.AddForce(movement);
        playerRigidbody.velocity = movement;
        if (movement != Vector2.zero)
        {
            if (positionArray.ToArray().Length == 0)
            {
                positionArray.Insert(0, transform.position);
            }
            if ((Vector2)transform.position != positionArray[0])
            {
                positionArray.Insert(0, transform.position);
            }
            
        }

    }
    void SetAnimation()
    {
        if (animator != null)
        {
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

                animator.Play(currentAnimation, -1, 0f);
                animator.speed = 0f;

            }
            else
            {
                animator.speed = 1f;
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyOverworldManager>() != null)
        {
            if (textSE.isActiveAndEnabled == false && overworldMenuManager.isUp == false && overworldMenuManager.yPos < -440)
            {
                StartCoroutine(EnemyBattle());
            }

        }

    }
    IEnumerator EnemyBattle()
    {
       
        for (int i = 0; i < 50; i++)
        {
            Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, 0.01f, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        JSONSave.SaveToJSON(1);
        SceneManager.LoadScene(1);
    }

}





