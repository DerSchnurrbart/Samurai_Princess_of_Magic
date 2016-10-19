#pragma strict

var HP = 50;

function Update ()
{
    if (HP <= 0)
    {
        Dead();
    }
}

function ApplyDamage (Damage : int)
{
    HP -= Damage;
}

function Dead()
{
    Destroy (gameObject);
}