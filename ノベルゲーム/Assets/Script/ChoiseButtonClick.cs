using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiseButtonClick : MonoBehaviour
{


    public void ChoiseButtonOneClick()
    {
        MainStory.OneChoiseClickFlag = true;
    }


    public void ChoiseButtonTwoClick()
    {
        MainStory.TwoChoiseClickFlag = true;
    }
}
