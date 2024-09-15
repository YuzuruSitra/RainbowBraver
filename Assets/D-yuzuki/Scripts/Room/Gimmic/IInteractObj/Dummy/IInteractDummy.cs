using UnityEngine;

public class IInteractDummy : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Oh Pasta.");
    }
}
