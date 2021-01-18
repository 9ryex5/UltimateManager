using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPlayer : MonoBehaviour
{
    private SaveFile SF;

    private Image myImage;

    private int playerID;
    public Image picture;
    public TextMeshProUGUI textName;
    public GameObject imageAssist;
    public GameObject imagePoint;
    public GameObject imageDefense;
    public GameObject imageCallahan;
    public Image imageValue;
    public Text textValue;
    private bool selected;

    private ItemPlayerType type;

    public enum ItemPlayerType
    {
        NOTHING,
        PLAYER_PLAYERS,
        PLAYER_TEAMS,
        PLAYER_GENERATOR,
        PLAYER_TOURNAMENT,
        PLAYER_TOURNAMENT_ADD,
        PLAYER_PLAYING,
        PLAYER_RANKING
    }

    public void StartThis(int _playerID, ItemPlayerType _type, bool _selectable, float _rankingValue)
    {
        SF = SaveFile.SF;
        Player p = SF.GetPlayer(_playerID);
        myImage = GetComponent<Image>();

        playerID = _playerID;
        type = _type;

        if (p.GetPictureIndex() == -1)
            ShowText();
        else
            ShowImage();

        if (_selectable)
            Unselect();

        if (_rankingValue == float.MinValue)
        {
            imageValue.gameObject.SetActive(false);
        }
        else
        {
            textValue.transform.parent.gameObject.SetActive(true);
            imageValue.gameObject.SetActive(true);
            textValue.text = _rankingValue % 1 == 0 ? _rankingValue.ToString() : _rankingValue.ToString("F1");
        }
    }

    public void Clicked()
    {
        switch (type)
        {
            case ItemPlayerType.PLAYER_PLAYERS:
                ManagerPlayers.MP.OpenPlayerProfile(this);
                break;
            case ItemPlayerType.PLAYER_TEAMS:
                selected = !selected;

                if (selected)
                {
                    Select();
                    ManagerTournaments.MT.PickTeamPlayer(this);
                }
                else
                {
                    Unselect();
                    ManagerTournaments.MT.UnpickTeamPlayer(this);
                }
                break;
            case ItemPlayerType.PLAYER_GENERATOR:
                selected = !selected;

                if (selected)
                {
                    Select();
                    ManagerTeamGenerator.MTG.PickPlayer(GetPlayerID());
                }
                else
                {
                    Unselect();
                    ManagerTeamGenerator.MTG.UnpickPlayer(GetPlayerID());
                }
                break;
            case ItemPlayerType.PLAYER_TOURNAMENT:
                ManagerTournaments.MT.OpenPlayerProfile(this);
                break;
            case ItemPlayerType.PLAYER_TOURNAMENT_ADD:
                selected = !selected;

                if (selected)
                {
                    Select();
                    ManagerTournaments.MT.PickPlayer(this);
                }
                else
                {
                    Unselect();
                    ManagerTournaments.MT.UnpickPlayer(this);
                }
                break;
            case ItemPlayerType.PLAYER_PLAYING:
                ManagerPlaying.MP.ClickedPlayer(this);
                break;
        }
    }

    public void ShowImage()
    {
        if (SF.GetPlayer(playerID).GetPictureIndex() != -1)
        {
            picture.sprite = ManagerPictures.MP.pictures[SF.GetPlayer(playerID).GetPictureIndex()];
            picture.gameObject.SetActive(true);
            textName.gameObject.SetActive(false);
        }
    }

    public void ShowText()
    {
        picture.gameObject.SetActive(false);
        textName.gameObject.SetActive(true);
        textName.text = SF.GetPlayer(playerID).GetName();
    }

    public bool GetSelected()
    {
        return selected;
    }

    public void SetSelected(bool _value)
    {
        selected = _value;

        if (selected)
            Select();
        else
            Unselect();
    }

    private void Select()
    {
        myImage.color = Color.yellow;
        picture.color = Color.white;
    }

    private void Unselect()
    {
        myImage.color = Color.white;
        picture.color = new Color(0.5f, 0.5f, 0.5f);
    }

    public void SetAssist(bool _value)
    {
        imageAssist.SetActive(_value);
    }

    public void SetPoint(bool _value)
    {
        imagePoint.SetActive(_value);
    }

    public void SetDefense(bool _value)
    {
        imageDefense.SetActive(_value);
    }

    public void SetCallahan(bool _value)
    {
        imageCallahan.SetActive(_value);
    }

    public Player GetPlayer()
    {
        return SF.GetPlayer(playerID);
    }

    public int GetPlayerID()
    {
        return playerID;
    }
}
