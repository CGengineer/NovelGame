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
        FadeManager.Instance.LoadScene("GameScene",1f);
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
