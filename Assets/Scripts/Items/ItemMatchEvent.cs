using UnityEngine;
using UnityEngine.UI;

public class ItemMatchEvent : MonoBehaviour
{
    public Image imageType;
    public Sprite spritePoint;
    public Sprite spriteDefense;
    public Sprite spriteCallahan;
    public GameObject imageArrow;
    public Text whoAssisted;
    public Text whoPoint;
    public Text whoDefended;
    public Text textTimer;

    private MatchEvent myMatchEvent;

    public void StartThis(MatchEvent _matchEvent)
    {
        myMatchEvent = _matchEvent;
        GetComponent<RectTransform>().sizeDelta = new Vector2(0, ManagerUI.MUI.sizeItem * Screen.height);

        switch (_matchEvent.type)
        {
            case MatchEvent.MatchEventType.SCORE:
                imageType.sprite = spritePoint;
                whoAssisted.text = _matchEvent.whoAssist == null ? string.Empty : _matchEvent.whoAssist.GetName();
                whoPoint.text = _matchEvent.whoPoint == null ? string.Empty : _matchEvent.whoPoint.GetName();
                break;
            case MatchEvent.MatchEventType.TURNOVER:
                imageArrow.SetActive(false);
                imageType.sprite = spriteDefense;
                whoDefended.text = _matchEvent.whoDefend == null ? string.Empty : _matchEvent.whoDefend.GetName();
                break;
            case MatchEvent.MatchEventType.CALLAHAN:
                imageArrow.SetActive(false);
                imageType.sprite = spriteCallahan;
                whoDefended.text = _matchEvent.whoDefend.GetName();
                break;
        }

        textTimer.text = (_matchEvent.gameTime.Hours == 0 ? string.Empty : _matchEvent.gameTime.Hours.ToString("00") + ":") +
            _matchEvent.gameTime.Minutes.ToString("00") + ":" +
            _matchEvent.gameTime.Seconds.ToString("00");

        GetComponent<Image>().color = Color.Lerp(GetComponent<Image>().color, ManagerColors.MC.teamColors[_matchEvent.colorIndex], 0.5f);
    }

    public void Clicked()
    {
        string a = string.Empty;
        string b = string.Empty;

        for (int i = 0; i < myMatchEvent.playingA.Count; i++)
        {
            a += myMatchEvent.playingA[i].GetName() + " ";
        }

        for (int i = 0; i < myMatchEvent.playingB.Count; i++)
        {
            b += myMatchEvent.playingB[i].GetName() + " ";
        }

        Debug.Log((myMatchEvent.startedOffenceA ? "Offense: " : "Deffense: ") + a);
        Debug.Log((myMatchEvent.startedOffenceA ? "Defense: " : "Offense: ") + b);
    }
}
