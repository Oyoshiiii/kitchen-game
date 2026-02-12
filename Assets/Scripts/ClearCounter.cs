using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //на тумбе ничего не лежит
            if (player.HasKitchenObject())
            {
                //у игрока есть в руках объект кладем на тумбочку
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            // на тумбе уже лежит объект
            if(player.HasKitchenObject())
            {
                //у игрока есть объект в руках
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //игрок держит тарелку
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                //у игрока ничего нет
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
