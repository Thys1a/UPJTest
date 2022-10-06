using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffPanel : BaseUIForm
{
    // Start is called before the first frame update
    public void Close()
    {
        UIManager.Instance().CloseUIForms("StaffPanel");
    }
}
