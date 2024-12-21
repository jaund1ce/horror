using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : SceneBase
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.Instance.Show<EndUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
