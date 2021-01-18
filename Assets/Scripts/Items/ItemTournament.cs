using UnityEngine;
using TMPro;

public class ItemTournament : MonoBehaviour
{

    private Tournament tournament;
    public TextMeshProUGUI textName;

    private ItemTournamentType type;

    public enum ItemTournamentType
    {
        PLAYER_IN_TOURNAMENT,
        TOURNAMENT
    }

    public void StartThis(Tournament _tournament, ItemTournamentType _type)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(0, ManagerUI.MUI.sizeItem * Screen.height);

        type = _type;
        tournament = _tournament;
        textName.text = _tournament.GetName();
    }

    public void Clicked()
    {
        switch (type)
        {
            case ItemTournamentType.TOURNAMENT:
                ManagerTournaments.MT.OpenTournamentProfile(this);
                break;
            case ItemTournamentType.PLAYER_IN_TOURNAMENT:
                ManagerPlayers.MP.OpenPlayerInTournamentProfile(this);
                break;
        }
    }

    public Tournament GetTournament()
    {
        return tournament;
    }
}
