using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilPathController : MonoBehaviour
{
    [SerializeField]
    public PathManager pathManager;

    List<Waypoint> thePath;
    Waypoint target;

    public Animator animator;
    bool isWalking;

    public float MoveSpeed;
    public float RotateSpeed;

    public int unidleDelay = 60;

    // Start is called before the first frame update
    void Start()
    {
        thePath = pathManager.GetPath();
        if (thePath != null && thePath.Count > 0)
        {
            target = thePath[0];
        }

        isWalking = true;
        animator.SetBool("isWalking", isWalking);
    }



    void rotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void moveForward()
    {
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, target.pos);
        if (distanceToTarget < stepSize)
        {
            return;
        }
        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);
    }

    private void OnTriggerStay(Collider collision)
    {
        //check if collider is the hero
        if(collision.name == "RPGHeroHP")
        {
            isWalking = false;
            animator.SetBool("isWalking", isWalking);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.name == "RPGHeroHP")
        {
            for (int i = unidleDelay; i > 0; i--)
            {
                continue;
            }
            isWalking = true;
            animator.SetBool("isWalking", isWalking);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name != "RPGHeroHP")
        {
            target = pathManager.GetNextTarget();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            //toggle if any key is pressed
            isWalking = !isWalking;
            animator.SetBool("isWalking", isWalking);
        }
        if (isWalking)
        {
            rotateTowardsTarget();
            moveForward();
        }

    }
}
