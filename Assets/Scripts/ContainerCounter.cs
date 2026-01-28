using UnityEngine;

public class ContainerCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, GetKitchenObjectFollowTransform());
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
        else
        {
            //отдаем объект игроку в руки
            kitchenObject.SetKitchenObjectParent(player);
        }
    }
}
