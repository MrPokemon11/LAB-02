using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryPathController : MonoBehaviour
{
    [SerializeField]
    public PathManager pathManager;

    List<Waypoint> thePath;
    Waypoint target;

    public Animator animator;
    bool isWalking;

    public float MoveSpeed;
    public float RotateSpeed;

    public bool isMerc1;
    public bool isMerc2;
    public bool isTech;

    public GameObject techTargetObj;
    Vector3 techTarget;

    // Start is called before the first frame update
    void Start()
    {
        techTarget = techTargetObj.transform.position;

        thePath = pathManager.GetPath();
        if (thePath != null && thePath.Count > 0)
        {
            target = thePath[0];
        }

        isWalking = false;
        animator.SetBool("isWalking", isWalking);
    }



    void rotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void rotateTowardsTarget(Vector3 tempTarg)
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = tempTarg - transform.position;
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

    bool triggerCheck()
    {
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, target.pos);
        bool isWrongTrigger;
        if (distanceToTarget > stepSize + MoveSpeed)
        {
            isWrongTrigger = true;
        }
        else
        {
            isWrongTrigger = false;
        }
        return isWrongTrigger;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerCheck())
        {
            return;
        }

        target = pathManager.GetNextTarget();

    }

    // Update is called once per frame
    void Update()
    {
        if(isTech && Input.GetKey(KeyCode.KeypadEnter))
        {
            rotateTowardsTarget(techTarget);
        }
        if ((isMerc1 && Input.GetKeyDown(KeyCode.Keypad1)) || (isMerc2 && Input.GetKeyDown(KeyCode.Keypad3)) || (isTech && Input.GetKeyDown(KeyCode.Keypad2)))
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
