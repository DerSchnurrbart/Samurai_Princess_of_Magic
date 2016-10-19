#pragma strict

private var theCollider : String;


function OnTriggerEnter (other : Collider)
    {
        
        theCollider = other.tag;
        if (theCollider == "Player")
        { 
            GetComponent.<AudioSource>().Play();
            GetComponent.<AudioSource>().loop = true;
        }

    }

function OnTriggerExit (other : Collider)
    {
        /*This code stops the music if you exit the music zone

        theCollider = other.tag;
        if (theCollider == "Player")
        {
            GetComponent.<AudioSource>().Stop();
            GetComponent.<AudioSource>().loop = false;
        }
        */
    }