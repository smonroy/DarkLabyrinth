using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu {
    public Mode previousMode;

    public Menu() {
        previousMode = Mode.Menu;
    }

    public void GetToast(NumpadKey numpadKey)
    {
        switch (numpadKey)
        {
            case NumpadKey.N1Key:
                Debug.Log("New game option");
                break;
            case NumpadKey.N2Key:
                if(previousMode == Mode.Menu) {
                    Debug.Log("There is no game to resume");
                } else {
                    Debug.Log("Resume game option");
                }
                break;
        }
    }
}
