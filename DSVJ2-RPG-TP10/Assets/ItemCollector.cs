using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    //En vez de devolver gameObject devolveria un Item(ScrpitableObject)
    private GameObject itemPickedUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemPickedUp = other.gameObject;
        }
    }
    public bool ItemPicked()
    {
        if (itemPickedUp != null)
            return true;
        else
            return false;
    }
    public GameObject ReturnItemToPlayer()
    {
        return itemPickedUp;
    }
}
