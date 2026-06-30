using UnityEngine;

public class SetupPhaseReadyButton : MonoBehaviour
{
    public void Ready()
    {
        RunManager.Instance.LoadCombat();
    }
}
