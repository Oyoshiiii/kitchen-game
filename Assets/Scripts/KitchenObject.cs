using Unity.VisualScripting;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    /// <summary>
    /// пинимает родителя кухонного объекта и присваивает его текущему кухонному объекту
    /// </summary>
    /// <param name="kitchenObjectParent"> родитель кухонного объекта </param>
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    /// <summary>
    /// уничтожает текущий кухонный объект у родителя и в целоом
    /// </summary>
    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    /// <summary>
    /// принимает scriptableObject кухонного объекта, родителя кухонного объекта и спавнит соответствующий текущему scriptableObject текущий кухонный объект у родителя
    /// </summary>
    /// <param name="kitchenObjectSO"> scriptableObject кухонный объект </param>
    /// <param name="kitchenObjectParent"> родитель кухонного объекта </param>
    /// <returns> кухонный объект, принадлежащий родителю </returns>
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
