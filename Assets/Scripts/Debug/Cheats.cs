using Chrio;
using UnityEngine;

public class Cheats : SharksBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.PageUp)) GlobalState.Game.Money[0] += 500;
        if (Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.M) && Input.GetKey(KeyCode.PageUp)) GlobalState.Game.Money[1] += 500;
    }
}
