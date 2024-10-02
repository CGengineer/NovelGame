using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{

    private void Update()
    {
        
    }

    public void StartButtonClick()
    {
        FadeManager.Instance.LoadScene("2_PreviousScene", 1.5f);
    }
    public void ContinueButtonClick()
    {
        Debug.Log("nia2");
    }
    public void SettingButtonClick()
    {
        Debug.Log("true");
    }
}
