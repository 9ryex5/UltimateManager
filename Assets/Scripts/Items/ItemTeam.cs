using UnityEngine;
using UnityEngine.UI;

public class ItemTeam : MonoBehaviour
{

    private Team team;
    public Text textName;
    public Image imageNPlayers;
    public Text textNPlayers;
    public Image imageArrow;
    public Text textRanking;
    public GameObject layoutRanking;
    public Text textVictories;
    public Text textDefeats;
    public Text textSpirit;
    public Text textScoredPoints;
    public Text textSufferedPoints;

    private ItemTeamType type;

    public enum ItemTeamType
    {
        TEAM_TOURNAMENT,
        TEAM_RANKING
    }

    public void StartThis(Team _team, ItemTeamType _type)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(0, ManagerUI.MUI.sizeItem * Screen.height);

        team = _team;
        type = _type;

        textName.text = _team.GetName();
        textName.color = ManagerColors.MC.teamColors[_team.GetColorIndex()];

        switch (_type)
        {
            case ItemTeamType.TEAM_TOURNAMENT:
                textRanking.gameObject.SetActive(false);
                textNPlayers.text = _team.GetNPlayers().ToString();
                textVictories.gameObject.SetActive(false);
                layoutRanking.SetActive(false);
                break;
            case ItemTeamType.TEAM_RANKING:
                imageNPlayers.gameObject.SetActive(false);
                textNPlayers.gameObject.SetActive(false);
                imageArrow.gameObject.SetActive(false);
                layoutRanking.SetActive(true);
                textVictories.text = team.GetVictories().ToString();
                textDefeats.text = team.GetDefeats().ToString();
                textSpirit.text = team.GetTotalSpirit().ToString();
                textScoredPoints.text = team.GetPointsScored().ToString();
                textSufferedPoints.text = team.GetPointsSuffered().ToString();
                break;
        }
    }

    public void Clicked()
    {
        if (type == ItemTeamType.TEAM_TOURNAMENT)
            ManagerTournaments.MT.OpenTeamProfile(this);
    }

    public Team GetTeam()
    {
        return team;
    }

    public void SetRanking(int _rank)
    {
        textRanking.text = _rank + "º";
    }
}
