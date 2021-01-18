using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerHistory : MonoBehaviour
{

    private Tournament cTournament;
    private ItemMatch itemMatch;
    private Match match;

    //TEAMS
    public GameObject panelTeams;
    public TextMeshProUGUI textTeamA;
    public TextMeshProUGUI textTeamB;
    public Transform parentTeamA;
    public Transform parentTeamB;
    public ItemPlayer prefabItemPlayer;
    public TextMeshProUGUI textScoreboard;

    //EVENTS
    public GameObject panelEvents;
    public Transform parentMatchEvents;
    public ItemMatchEvent prefabItemMatchEvent;

    //SPIRIT
    public GameObject panelSpirit;
    public TextMeshProUGUI textSpiritTeamA;
    public TextMeshProUGUI textSpiritTeamB;
    public TextMeshProUGUI[] textsSpiritA;
    public TextMeshProUGUI[] textsSpiritB;
    public TextMeshProUGUI textSpiritTotalA;
    public TextMeshProUGUI textSpiritTotalB;

    private void Start()
    {
        parentTeamA.GetComponent<GridLayoutGroup>().cellSize = new Vector2(ManagerUI.MUI.sizePlayerGrid * Screen.height, ManagerUI.MUI.sizePlayerGrid * Screen.height);
        parentTeamB.GetComponent<GridLayoutGroup>().cellSize = new Vector2(ManagerUI.MUI.sizePlayerGrid * Screen.height, ManagerUI.MUI.sizePlayerGrid * Screen.height);
    }

    public void StartThis(ItemMatch _itemMatch)
    {
        itemMatch = _itemMatch;
        match = _itemMatch.GetMatch();

        //Teams
        textTeamA.text = match.GetTeamA().GetName();
        textTeamB.text = match.GetTeamB().GetName();
        textTeamA.color = ManagerColors.MC.teamColors[match.GetTeamA().GetColorIndex()];
        textTeamB.color = ManagerColors.MC.teamColors[match.GetTeamB().GetColorIndex()];
        textScoreboard.text = match.GetScoreA() + " - " + match.GetScoreB();

        for (int i = 0; i < parentTeamA.childCount; i++)
            Destroy(parentTeamA.GetChild(i).gameObject);

        for (int i = 0; i < parentTeamB.childCount; i++)
            Destroy(parentTeamB.GetChild(i).gameObject);

        for (int i = 0; i < match.GetTeamA().GetNPlayers(); i++)
        {
            ItemPlayer ip = Instantiate(prefabItemPlayer, parentTeamA);
            ip.StartThis(match.GetTeamA().GetPlayerID(i), ItemPlayer.ItemPlayerType.NOTHING, false, float.MinValue);
        }

        for (int i = 0; i < match.GetTeamB().GetNPlayers(); i++)
        {
            ItemPlayer ip = Instantiate(prefabItemPlayer, parentTeamB);
            ip.StartThis(match.GetTeamB().GetPlayerID(i), ItemPlayer.ItemPlayerType.NOTHING, false, float.MinValue);
        }

        ButtonTeams();

        //Events
        for (int i = 0; i < parentMatchEvents.childCount; i++)
            Destroy(parentMatchEvents.GetChild(i).gameObject);

        for (int i = 1; i < match.GetNEvents(); i++)
        {
            ItemMatchEvent ime = Instantiate(prefabItemMatchEvent, parentMatchEvents);
            ime.StartThis(match.GetEvent(i));
        }

        //Spirit
        textSpiritTeamA.text = match.GetTeamA().GetName();
        textSpiritTeamB.text = match.GetTeamB().GetName();
        textSpiritTeamA.color = ManagerColors.MC.teamColors[match.GetTeamA().GetColorIndex()];
        textSpiritTeamB.color = ManagerColors.MC.teamColors[match.GetTeamB().GetColorIndex()];

        for (int i = 0; i < textsSpiritA.Length; i++)
            textsSpiritA[i].text = match.GetSpiritA(i).ToString();

        textSpiritTotalA.text = match.GetTotalSpiritA().ToString();

        for (int i = 0; i < textsSpiritB.Length; i++)
            textsSpiritB[i].text = match.GetSpiritB(i).ToString();

        textSpiritTotalB.text = match.GetTotalSpiritB().ToString();
    }

    public void ButtonTeams()
    {
        panelTeams.SetActive(true);
        panelEvents.SetActive(false);
        panelSpirit.SetActive(false);
    }

    public void ButtonEvents()
    {
        panelTeams.SetActive(false);
        panelEvents.SetActive(true);
        panelSpirit.SetActive(false);
    }

    public void ButtonSpirit()
    {
        panelTeams.SetActive(false);
        panelEvents.SetActive(false);
        panelSpirit.SetActive(true);
    }
}
