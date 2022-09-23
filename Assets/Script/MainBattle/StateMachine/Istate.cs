using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Istate
{
    GameController Controller { get; set; }
    void OnStateEnter(GameController controller);
    void OnStateStay();
    void OnStateExit();
}
