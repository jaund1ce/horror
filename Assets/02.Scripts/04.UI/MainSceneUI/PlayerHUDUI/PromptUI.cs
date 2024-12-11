using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PromptUI : BaseUI
{
    [field:SerializeField]public TextMeshProUGUI text {  get; private set; }
    

    public override void OpenUI()
    {
        this.gameObject.SetActive(true);
        Debug.Log("Open");
    }

    public override void CloseUI()
    {
        this.gameObject.SetActive(false);
    }

    public void SetPromptText(String txt)
    {
        text.text = txt;
    }
}

