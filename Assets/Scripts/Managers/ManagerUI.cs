using System.Collections;
using UnityEngine;

public class ManagerUI : MonoBehaviour
{
    public static ManagerUI MUI;

    public Animator firstMenu;
    private Animator currentMenu;

    [Range(0, 1)]
    public float sizePlayerGrid;
    [Range(0, 1)]
    public float sizeItem;

    private void Awake()
    {
        MUI = this;
    }

    private void Start()
    {
        currentMenu = firstMenu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentMenu == firstMenu)
            Application.Quit();
    }

    public void GoRight(Animator _menu)
    {
        StartCoroutine(SetInactive(currentMenu.gameObject, 0.5f));
        _menu.gameObject.SetActive(true);
        currentMenu.Play("CenterLeft");
        _menu.Play("RightCenter");
        currentMenu = _menu;
    }

    public void GoLeft(Animator _menu)
    {
        StartCoroutine(SetInactive(currentMenu.gameObject, 0.5f));
        _menu.gameObject.SetActive(true);
        currentMenu.Play("CenterRight");
        _menu.Play("LeftCenter");
        currentMenu = _menu;
    }

    public void Appear(Animator _menu)
    {
        _menu.gameObject.SetActive(true);
        _menu.Play("Appear");
    }

    public void Disappear(Animator _menu)
    {
        _menu.GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(SetInactive(_menu.gameObject, 0.5f));
        _menu.Play("Disappear");
    }

    IEnumerator SetInactive(GameObject _menu, float time)
    {
        yield return new WaitForSeconds(time);
        if (_menu.GetComponent<CanvasGroup>() != null) _menu.GetComponent<CanvasGroup>().interactable = true;
        _menu.SetActive(false);
    }
}
