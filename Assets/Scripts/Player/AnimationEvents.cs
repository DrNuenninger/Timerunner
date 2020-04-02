using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update
    private bool stepLeft = true;


    //[1 = right | 0 = left]
    public void PlayPlayerStep()
    {
        print("sadas");
        if (GetComponent<Controller2D>().collissions.below)
        {
            if (stepLeft)
            {
                FindObjectOfType<SoundManager>().Play("PlayerStepLeft");
            }
            else
            {
                FindObjectOfType<SoundManager>().Play("PlayerStepRight");
            }

            stepLeft = (stepLeft) ? false : true;
        }

        
    }
}
