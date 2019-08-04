using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManetteButtons : MonoBehaviour
{
    private bool[] _areButtonsIn = new bool[4];

    public void RegisterPlayerButton(PlayerLinker playerLinker)
    {
        if (playerLinker.PlayerManager.Id >= 0 && playerLinker.PlayerManager.Id < 4)
        {
            _areButtonsIn[playerLinker.PlayerManager.Id] = true;
            if (AreAllButtonsIn())
            {
                Pickable pickable = GetComponent<Pickable>();
                pickable.SetPickableInput(pickableinput.manette);
            }
        }
    }

    private bool AreAllButtonsIn()
    {
        bool allButtonsIn = true;
        foreach (bool buttonIn in _areButtonsIn)
        {
            allButtonsIn &= buttonIn;
        }
        return allButtonsIn;
    }


}
