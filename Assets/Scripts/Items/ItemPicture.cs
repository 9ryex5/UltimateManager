using UnityEngine;
using UnityEngine.UI;

public class ItemPicture : MonoBehaviour
{
    private int index;
    private Image myImage;

    public void StartThis(int _index, Sprite _sprite)
    {
        index = _index;
        myImage = GetComponent<Image>();
        myImage.sprite = _sprite;
    }

    public void Clicked()
    {
        ManagerPlayers.MP.PickPicture(index);
    }
}
