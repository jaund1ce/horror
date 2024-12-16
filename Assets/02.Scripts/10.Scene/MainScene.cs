using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : SceneBase
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.Show<MainUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
