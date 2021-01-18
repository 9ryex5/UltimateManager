using UnityEngine;
using UnityEngine.UI;

public class ItemMatch : MonoBehaviour
{
    private Match match;

    public Text dateTime;
    public Image imageStatus;
    public Sprite spritePlaying;
    public Sprite spriteSpirit;
    public Sprite spriteValid;
    public Text textTeamA;
    public Text textTeamB;
    public Text textScoreA;
    public Text textScoreB;
    public Text textDash;
    public Image imageArrow;
    private bool showedHalfTime;
    private bool showedFullTime;
    private bool secondHalf;

    public void StartThis(Match _match)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(0, ManagerUI.MUI.sizeItem * Screen.height);

        match = _match;
        Refresh();
    }

    public void Clicked()
    {
        ManagerTournaments.MT.ButtonOpenItemMatch(this);
    }

    public Match GetMatch()
    {
        return match;
    }

    public void SetStatus(Match.MatchStatus _status)
    {
        match.SetStatus(_status);
        Refresh();
    }

    public void Refresh()
    {
        textTeamA.text = match.GetNameA();
        textTeamB.text = match.GetNameB();
        textTeamA.color = match.GetTeamA() == null ? ManagerColors.MC.gray : ManagerColors.MC.teamColors[match.GetTeamA().GetColorIndex()];
        textTeamB.color = match.GetTeamB() == null ? ManagerColors.MC.gray : ManagerColors.MC.teamColors[match.GetTeamB().GetColorIndex()];

        switch (match.GetStatus())
        {
            case Match.MatchStatus.NOTHING:
                dateTime.text = match.GetDate().Hour.ToString("00") + " : " + match.GetDate().Minute.ToString("00");
                imageArrow.gameObject.SetActive(false);
                break;
            case Match.MatchStatus.READY:
                dateTime.text = match.GetDate().Hour.ToString("00") + " : " + match.GetDate().Minute.ToString("00");
                imageArrow.gameObject.SetActive(true);
                break;
            case Match.MatchStatus.RUNNING:
                dateTime.gameObject.SetActive(false);
                imageStatus.sprite = spritePlaying;
                textScoreA.text = match.GetScoreA().ToString();
                textScoreB.text = match.GetScoreB().ToString();
                textDash.text = "-";
                imageArrow.gameObject.SetActive(true);
                break;
            case Match.MatchStatus.SPIRIT:
                dateTime.gameObject.SetActive(false);
                imageStatus.sprite = spriteSpirit;
                textScoreA.text = match.GetScoreA().ToString();
                textScoreB.text = match.GetScoreB().ToString();
                textDash.text = "-";
                imageArrow.gameObject.SetActive(true);
                break;
            case Match.MatchStatus.FINISHED:
                dateTime.gameObject.SetActive(false);
                imageStatus.sprite = spriteValid;
                textScoreA.text = match.GetScoreA().ToString();
                textScoreB.text = match.GetScoreB().ToString();
                textDash.text = "-";
                imageArrow.gameObject.SetActive(true);
                break;
        }

    }

    public bool GetShowedHalfTime()
    {
        return showedHalfTime;
    }

    public bool GetShowedFullTime()
    {
        return showedFullTime;
    }

    public bool GetSecondHalf()
    {
        return secondHalf;
    }

    public void SetShowedHalfTime(bool _value)
    {
        showedHalfTime = _value;
    }

    public void SetShowedFullTime(bool _value)
    {
        showedFullTime = _value;
    }

    public void SetSecondHalf()
    {
        secondHalf = true;
    }
}
