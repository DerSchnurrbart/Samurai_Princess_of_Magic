var Distance;
var Target : Transform;
var lookAtDistance = 25.0;
var chaseRange = 24.0;
var attackRange = 3.0;
var moveSpeed = 8.0;
var Damping = 6.0;
var attackRepeatTime = 1;

var theDamage = 10;

private var attackTime : float;

var controller : CharacterController;
var gravity : float = 20.0;
private var MoveDirection : Vector3 = Vector3.zero;

function Start ()
{
    attackTime = Time.time;
}

function Update ()
{
    Distance = Vector3.Distance(Target.position, transform.position);

    //if player is within enemy detection range, enemy turns toward you and turns yellow;
    //TO DO: Play a SFX to show that the enemy detected you
    if (Distance < lookAtDistance)
    {
        
        lookAt();
    }

    //if player is outside of enemy detection, enemy is green
    if (Distance > lookAtDistance)
    {
        GetComponent.<Renderer>().material.color = Color.green;
    }

    if (Distance < attackRange)
    {
        attack();
    }

    //if player is within enemy attack range, enemy turns red and starts chasing you;
    //TO DO: Play a SFX to show enemy is now aggro
    if (Distance < chaseRange)
    {
        
        chase ();
    }
}

function lookAt ()
{
    GetComponent.<Renderer>().material.color = Color.yellow;
    var rotation = Quaternion.LookRotation(Target.position - transform.position);
    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);
}

function chase ()
{
    GetComponent.<Renderer>().material.color = Color.red;
    
    /* More advanced movement, unecessary

    moveDirection = transform.forward;
    moveDirection *= moveSpeed;

    moveDirection.y -= gravity * Time.deltaTime;
    controller.Move(moveDirection * Time.deltaTime);
    */

    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
}

function attack ()
{
    if (Time.time > attackTime)
    {
        Target.SendMessage("damagePlayer", theDamage, SendMessageOptions.DontRequireReceiver);
        Debug.Log("You have been attacked");
        attackTime = Time.time + attackRepeatTime;
    }
}
