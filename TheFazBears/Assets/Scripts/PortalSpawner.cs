using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;

public class PortalSpawner : InteractionBase
{
    private int buttonsInteracted = 0;
    private int buttonCount = 4;
    public GameObject portal;
    public UIView winScreen;
    public override void InteractSucessful()
    {
        base.InteractSucessful();
        buttonsInteracted++;
        if(buttonsInteracted == buttonCount)
        {
            portal.SetActive(true);
            winScreen.Show();
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
