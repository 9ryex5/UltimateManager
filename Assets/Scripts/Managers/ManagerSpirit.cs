using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerSpirit : MonoBehaviour
{
    public GameObject manager;
    private SaveFile SF;

    private ItemMatch itemMatch;
    private Match match;

    public TextMeshProUGUI textTeamA;
    public TextMeshProUGUI textTeamB;
    public TextMeshProUGUI textSpiritboard;
    public Transform parentTeamA;
    public Transform parentTeamB;
    public ItemPlayer prefabItemPlayer;
    public TextMeshProUGUI textTitleTeam;
    public Image[] buttons0;
    public Image[] buttons1;
    public Image[] buttons2;
    public Image[] buttons3;
    public Image[] buttons4;
    private int[] buttonsIndexes;
    public Sprite spriteChecked;
    public Sprite spriteUnchecked;

    private Team teamA;
    private Team teamB;
    private bool currentTeamA;

    private void Awake()
    {
        SF = manager.GetComponent<SaveFile>();
    }

    private void Start()
    {
        parentTeamA.GetComponent<GridLayoutGroup>().cellSize = new Vector2(ManagerUI.MUI.sizePlayerGrid * Screen.height, ManagerUI.MUI.sizePlayerGrid * Screen.height);
        parentTeamB.GetComponent<GridLayoutGroup>().cellSize = new Vector2(ManagerUI.MUI.sizePlayerGrid * Screen.height, ManagerUI.MUI.sizePlayerGrid * Screen.height);
    }

    public void StartThis(ItemMatch _itemMatch)
    {
        itemMatch = _itemMatch;
        match = _itemMatch.GetMatch();

        textTeamA.text = match.GetTeamA().GetName();
        textTeamB.text = match.GetTeamB().GetName();
        textTeamA.color = ManagerColors.MC.teamColors[match.GetTeamA().GetColorIndex()];
        textTeamB.color = ManagerColors.MC.teamColors[match.GetTeamB().GetColorIndex()];
        textSpiritboard.text = match.GetTotalSpiritA() + " - " + match.GetTotalSpiritB();

        buttonsIndexes = new int[5];

        for (int i = 0; i < parentTeamA.childCount; i++)
            Destroy(parentTeamA.GetChild(i).gameObject);

        for (int i = 0; i < parentTeamB.childCount; i++)
            Destroy(parentTeamB.GetChild(i).gameObject);

        teamA = match.GetTeamA();
        teamB = match.GetTeamB();

        for (int i = 0; i < teamA.GetNPlayers(); i++)
        {
            ItemPlayer ip = Instantiate(prefabItemPlayer, parentTeamA);
            ip.StartThis(teamA.GetPlayerID(i), ItemPlayer.ItemPlayerType.PLAYER_PLAYING, false, float.MinValue);
        }

        for (int i = 0; i < teamB.GetNPlayers(); i++)
        {
            ItemPlayer ip = Instantiate(prefabItemPlayer, parentTeamB);
            ip.StartThis(teamB.GetPlayerID(i), ItemPlayer.ItemPlayerType.PLAYER_PLAYING, false, float.MinValue);
        }
    }

    public void ButtonOpenSpiritA()
    {
        currentTeamA = true;

        textTitleTeam.text = match.GetTeamA().GetName();
        textTitleTeam.color = ManagerColors.MC.teamColors[match.GetTeamA().GetColorIndex()];

        for (int i = 0; i < buttonsIndexes.Length; i++)
            buttonsIndexes[i] = match.GetSpiritA(i);

        FlushSpirit();
    }

    public void ButtonOpenSpiritB()
    {
        currentTeamA = false;

        textTitleTeam.text = match.GetTeamB().GetName();
        textTitleTeam.color = ManagerColors.MC.teamColors[match.GetTeamB().GetColorIndex()];

        for (int i = 0; i < buttonsIndexes.Length; i++)
            buttonsIndexes[i] = match.GetSpiritB(i);

        FlushSpirit();
    }

    public void ButtonSpirit0(int _value)
    {
        if (currentTeamA)
            match.SetSpiritA(0, _value);
        else
            match.SetSpiritB(0, _value);

        buttonsIndexes[0] = _value;

        FlushSpirit();
    }

    public void ButtonSpirit1(int _value)
    {
        if (currentTeamA)
            match.SetSpiritA(1, _value);
        else
            match.SetSpiritB(1, _value);

        buttonsIndexes[1] = _value;

        FlushSpirit();
    }

    public void ButtonSpirit2(int _value)
    {
        if (currentTeamA)
            match.SetSpiritA(2, _value);
        else
            match.SetSpiritB(2, _value);

        buttonsIndexes[2] = _value;

        FlushSpirit();
    }

    public void ButtonSpirit3(int _value)
    {
        if (currentTeamA)
            match.SetSpiritA(3, _value);
        else
            match.SetSpiritB(3, _value);

        buttonsIndexes[3] = _value;

        FlushSpirit();
    }

    public void ButtonSpirit4(int _value)
    {
        if (currentTeamA)
            match.SetSpiritA(4, _value);
        else
            match.SetSpiritB(4, _value);

        buttonsIndexes[4] = _value;

        FlushSpirit();
    }

    private void FlushSpirit()
    {
        for (int i = 0; i < buttons0.Length; i++)
            buttons0[i].sprite = spriteUnchecked;

        for (int i = 0; i < buttons1.Length; i++)
            buttons1[i].sprite = spriteUnchecked;

        for (int i = 0; i < buttons2.Length; i++)
            buttons2[i].sprite = spriteUnchecked;

        for (int i = 0; i < buttons3.Length; i++)
            buttons3[i].sprite = spriteUnchecked;

        for (int i = 0; i < buttons4.Length; i++)
            buttons4[i].sprite = spriteUnchecked;

        if (buttonsIndexes[0] != -1) buttons0[buttonsIndexes[0]].sprite = spriteChecked;
        if (buttonsIndexes[1] != -1) buttons1[buttonsIndexes[1]].sprite = spriteChecked;
        if (buttonsIndexes[2] != -1) buttons2[buttonsIndexes[2]].sprite = spriteChecked;
        if (buttonsIndexes[3] != -1) buttons3[buttonsIndexes[3]].sprite = spriteChecked;
        if (buttonsIndexes[4] != -1) buttons4[buttonsIndexes[4]].sprite = spriteChecked;

        textSpiritboard.text = match.GetTotalSpiritA() + " - " + match.GetTotalSpiritB();
    }

    public void ButtonSaveSpirit()
    {
        bool spiritUnfinished = false;

        for (int i = 0; i < buttonsIndexes.Length; i++)
        {
            if (match.GetSpiritA(i) == -1)
            {
                spiritUnfinished = true;
                break;
            }
        }

        for (int i = 0; i < buttonsIndexes.Length; i++)
        {
            if (match.GetSpiritB(i) == -1)
            {
                spiritUnfinished = true;
                break;
            }
        }

        if (!spiritUnfinished)
            itemMatch.SetStatus(Match.MatchStatus.FINISHED);

        ManagerTournaments.MT.VerifyEndTournament();

        SF.SaveTournamentsData();
    }
}
