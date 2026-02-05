using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField]
    private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    /// <summary>
    /// принимает объект игрока, описывает взаимодействие игрока с текущим counter-ом
    /// </summary>
    /// <param name="player"> игрок </param>
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact() была вызвана");
    }
    // <summary>
    /// принимает объект игрока, описывает взаимодействие игрока с текущим counter-ом для нарезки овощей
    /// </summary>
    /// <param name="player"> игрок </param>
    public virtual void InteractAlternative(Player player) { }

    /// <summary>
    /// получение точки спавна кухонного объекта на текущем counter-е
    /// </summary>
    /// <returns> точка спавна </returns>
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    /// <summary>
    /// принимает кухонный объект и присваивает его значение кухонному объекту текущему counter-у
    /// </summary>
    /// <param name="kitchenObject"> кухонный объект </param>
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    /// <summary>
    /// получение кухонного объекта текущего counter-а
    /// </summary>
    /// <returns> кухонный объект </returns>
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    /// <summary>
    /// удаление текущего counter-а
    /// </summary>
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    /// <summary>
    /// проверяет, есть ли на текущем counter-е кухонный объект
    /// </summary>
    /// <returns> есть ли кухонный объект </returns>
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
