using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu {
    public Mode previousMode;
    private AudioManager audioManager;

    public Menu(AudioManager audioManager) {
        previousMode = Mode.Menu;
        this.audioManager = audioManager;
    }

    public void GetToast(NumpadKey numpadKey)
    {
        switch (numpadKey)
        {
            case NumpadKey.N1Key:
                audioManager.Play("new-game-option");
                break;
            case NumpadKey.N2Key:
                if(previousMode == Mode.Menu) {
                    audioManager.Play("there-is-no-game-to-resume");
                } else {
                    audioManager.Play("resume-game-option");
                }
                break;
        }
    }
}
