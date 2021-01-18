using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerColors : MonoBehaviour
{

    public static ManagerColors MC;

    public Color titles;
    public Color buttonText;
    public Color itemText;
    public Color regularText;
    public Color fieldText;
    public Color warningText;
    public Color gray;

    public Color[] teamColors;

    public GameObject prefabTitle;
    public GameObject prefabButton;
    public GameObject prefabRegularText;
    public GameObject prefabField;
    public GameObject prefabWarningText;

    public GameObject prefabItemPlayer;
    public GameObject prefabItemTournament;

    private void Awake()
    {
        MC = this;
    }

    private void Start()
    {
        prefabTitle.GetComponent<TextMeshProUGUI>().color = titles;
        prefabButton.GetComponentInChildren<TextMeshProUGUI>().color = buttonText;
        prefabRegularText.GetComponent<TextMeshProUGUI>().color = regularText;
        prefabField.GetComponentInChildren<Text>().color = fieldText;
        prefabField.GetComponentsInChildren<Text>()[1].color = fieldText;
        prefabWarningText.GetComponentInChildren<Text>().color = warningText;

        prefabItemPlayer.GetComponentInChildren<ItemPlayer>().textName.color = itemText;
        prefabItemTournament.GetComponentInChildren<ItemTournament>().textName.color = itemText;
    }
}
