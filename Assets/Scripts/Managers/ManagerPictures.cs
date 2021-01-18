using UnityEngine;

public class ManagerPictures : MonoBehaviour
{

    public static ManagerPictures MP;

    public Sprite[] pictures;

    private void Awake()
    {
        MP = this;
    }

    public Sprite GetPicture(int _index)
    {
        if (_index == -1)
            return null;

        return pictures[_index];
    }
}
