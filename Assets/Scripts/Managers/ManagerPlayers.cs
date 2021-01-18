using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerPlayers : MonoBehaviour
{
    public static ManagerPlayers MP;

    public GameObject manager;
    private ManagerUI MUI;
    private ManagerPictures MPI;
    private SaveFile SF;

    public Animator animPanelMainMenu;

    //COMPONENTS
    private Animator myAnimator;

    //PLAYERS
    public Transform parentPlayers;
    public ItemPlayer prefabItemPlayer;
    private List<ItemPlayer> itemsPlayers;
    private ItemPlayer currentItemPlayer;

    //NEW PLAYER
    public Animator animNewPlayer;
    public Image imagePicture;
    public Sprite spriteEmptyPicture;
    public Animator animGallery;
    public Transform parentGallery;
    public ItemPicture prefabItemPicture;
    private int currentPictureIndex;
    public InputField fieldNewPlayerName;
    public InputField fieldNewPlayerNumber;
    public Image imageNameConfirmation;
    public Sprite spriteValid;
    public Sprite spriteInvalid;
    public Image imageMale;
    public Image imageFemale;
    public Sprite spriteChecked;
    public Sprite spriteUnchecked;
    private bool female;

    //PLAYER PROFILE
    public Animator animProfile;
    public TextMeshProUGUI textProfileTitle;
    public Image imageProfilePicture;
    public InputField fieldProfileName;
    public InputField fieldProfileNumber;
    public Image imageProfileNameConfirmation;
    public Image imageProfileMale;
    public Image imageProfileFemale;
    public GameObject[] profilePages;
    private int profileCurrentPage;
    public Button buttonLeft;
    public Button buttonRight;
    public TextMeshProUGUI textProfileMatches;
    public TextMeshProUGUI textProfilePoints;
    public TextMeshProUGUI textProfileAssists;
    public TextMeshProUGUI textProfileDefenses;
    public Transform parentTournaments;
    public ItemTournament prefabItemTournament;

    //PLAYER_IN_TOURNAMENT PROFILE
    public Animator panelPlayerInTournament;
    public TextMeshProUGUI textPITTitle;
    public TextMeshProUGUI textPITMatches;
    public TextMeshProUGUI textPITElo;
    public TextMeshProUGUI textPITPoints;
    public TextMeshProUGUI textPITAssists;
    public TextMeshProUGUI textPITDefenses;
    public TextMeshProUGUI textPITCallahans;
    public TextMeshProUGUI textPITScoreOff;
    public TextMeshProUGUI textPITScoreDef;
    public TextMeshProUGUI textPITSufferOff;
    public TextMeshProUGUI textPITSufferDef;

    private void Awake()
    {
        MP = this;

        MUI = manager.GetComponent<ManagerUI>();
        MPI = manager.GetComponent<ManagerPictures>();
        SF = manager.GetComponent<SaveFile>();

    }

    public void Start()
    {
        myAnimator = GetComponent<Animator>();
        parentPlayers.GetComponent<GridLayoutGroup>().cellSize = new Vector2(ManagerUI.MUI.sizePlayerGrid * Screen.height, ManagerUI.MUI.sizePlayerGrid * Screen.height);
        parentGallery.GetComponent<GridLayoutGroup>().cellSize = new Vector2(ManagerUI.MUI.sizePlayerGrid * Screen.height, ManagerUI.MUI.sizePlayerGrid * Screen.height);

        for (int i = 0; i < ManagerPictures.MP.pictures.Length; i++)
        {
            ItemPicture ip = Instantiate(prefabItemPicture, parentGallery);
            ip.StartThis(i, ManagerPictures.MP.pictures[i]);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < parentPlayers.childCount; i++)
            Destroy(parentPlayers.GetChild(i).gameObject);

        foreach (Player p in SF.GetAllPlayers())
        {
            ItemPlayer ip = Instantiate(prefabItemPlayer, parentPlayers);
            ip.StartThis(p.ID, ItemPlayer.ItemPlayerType.PLAYER_PLAYERS, false, float.MinValue);
        }
    }

    #region NEW_PLAYER
    public void ButtonNewPlayer()
    {
        currentItemPlayer = null;
        currentPictureIndex = -1;

        imagePicture.sprite = spriteEmptyPicture;
        fieldNewPlayerName.text = string.Empty;
        imageNameConfirmation.sprite = spriteInvalid;
        fieldNewPlayerNumber.text = string.Empty;
        ButtonMale();
    }

    public void PickPicture(int _pictureIndex)
    {
        currentPictureIndex = _pictureIndex;

        if (animNewPlayer.gameObject.activeSelf)
            imagePicture.sprite = ManagerPictures.MP.pictures[_pictureIndex];
        else if (animProfile.gameObject.activeSelf)
            imageProfilePicture.sprite = ManagerPictures.MP.pictures[_pictureIndex];

        MUI.Disappear(animGallery);
    }

    public void ButtonMale()
    {
        imageMale.sprite = spriteChecked;
        imageFemale.sprite = spriteUnchecked;
        female = false;
    }

    public void ButtonFemale()
    {
        imageMale.sprite = spriteUnchecked;
        imageFemale.sprite = spriteChecked;
        female = true;
    }

    public void EndEditNewPlayerName()
    {
        imageNameConfirmation.sprite = IsNameValid(fieldNewPlayerName.text) ? spriteValid : spriteInvalid;
    }

    public void ButtonConfirmNewPlayer()
    {
        if (!IsNameValid(fieldNewPlayerName.text))
            return;

        Player p = new Player(currentPictureIndex, fieldNewPlayerName.text, fieldNewPlayerNumber.text == string.Empty ? -1 : int.Parse(fieldNewPlayerNumber.text), female);
        SF.AddPlayer(p);

        ItemPlayer ip = Instantiate(prefabItemPlayer, parentPlayers);
        ip.StartThis(p.ID, ItemPlayer.ItemPlayerType.PLAYER_PLAYERS, false, float.MinValue);

        MUI.GoRight(myAnimator);
    }
    #endregion

    #region PLAYER_PROFILE
    public void OpenPlayerProfile(ItemPlayer _itemPlayer)
    {
        currentItemPlayer = _itemPlayer;
        profilePages[profileCurrentPage].SetActive(false);
        profileCurrentPage = 0;
        buttonLeft.interactable = false;
        buttonRight.interactable = true;
        profilePages[profileCurrentPage].SetActive(true);

        Player p = _itemPlayer.GetPlayer();
        textProfileTitle.text = "Profile";
        currentPictureIndex = p.GetPictureIndex();
        imageProfilePicture.sprite = p.GetPictureIndex() == -1 ? spriteEmptyPicture : MPI.GetPicture(p.GetPictureIndex());
        fieldProfileName.text = p.GetName();
        imageProfileNameConfirmation.sprite = spriteValid;
        fieldProfileNumber.text = p.GetNumber() == -1 ? string.Empty : p.GetNumber().ToString();
        if (p.GetFemale())
            ButtonProfileFemale();
        else
            ButtonProfileMale();

        textProfileMatches.text = p.GetNTotalMatches().ToString();
        textProfilePoints.text = p.GetNTotalPoints().ToString();
        textProfileAssists.text = p.GetNTotalAssists().ToString();
        textProfileDefenses.text = p.GetNTotalDefenses().ToString();

        for (int i = 0; i < parentTournaments.childCount; i++)
            Destroy(parentTournaments.GetChild(i).gameObject);

        for (int i = 0; i < p.GetNTournaments(); i++)
        {
            ItemTournament it = Instantiate(prefabItemTournament, parentTournaments);
            it.StartThis(SF.GetTournament(p.GetTournament(i).GetTournamentID()), ItemTournament.ItemTournamentType.PLAYER_IN_TOURNAMENT);
        }

        MUI.GoRight(animProfile);
    }

    public void OpenPlayerInTournamentProfile(ItemTournament _itemTournament)
    {
        PlayerInTournament pit = currentItemPlayer.GetPlayer().GetTournamentByID(_itemTournament.GetTournament().ID);

        textPITTitle.text = _itemTournament.GetTournament().GetName();
        textPITMatches.text = pit.GetMatches().ToString();
        textPITElo.text = pit.GetElo().ToString();
        textPITPoints.text = pit.GetPoints().ToString();
        textPITAssists.text = pit.GetAssists().ToString();
        textPITDefenses.text = pit.GetDefenses().ToString();
        textPITCallahans.text = pit.GetCallahans().ToString();
        textPITScoreOff.text = pit.GetScoreOff().ToString();
        textPITScoreDef.text = pit.GetScoreDef().ToString();
        textPITSufferOff.text = pit.GetSufferOff().ToString();
        textPITSufferDef.text = pit.GetSufferDef().ToString();

        ManagerUI.MUI.GoRight(panelPlayerInTournament);
    }

    public void ButtonProfileRight()
    {
        profilePages[profileCurrentPage].SetActive(false);

        if (profileCurrentPage == 0)
            buttonLeft.interactable = true;

        if (profileCurrentPage < profilePages.Length - 1)
            profileCurrentPage++;

        profilePages[profileCurrentPage].SetActive(true);

        switch (profileCurrentPage)
        {
            case 1:
                textProfileTitle.text = "Stats";
                break;
            case 2:
                textProfileTitle.text = "Tournaments";
                buttonRight.interactable = false;
                break;
        }
    }

    public void ButtonProfileLeft()
    {
        profilePages[profileCurrentPage].SetActive(false);

        if (profileCurrentPage == profilePages.Length - 1)
            buttonRight.interactable = true;

        if (profileCurrentPage > 0)
            profileCurrentPage--;

        profilePages[profileCurrentPage].SetActive(true);

        switch (profileCurrentPage)
        {
            case 0:
                textProfileTitle.text = "Profile";
                buttonLeft.interactable = false;
                break;
            case 1:
                textProfileTitle.text = "Stats";
                break;
        }
    }

    public void ButtonProfileMale()
    {
        imageProfileMale.sprite = spriteChecked;
        imageProfileFemale.sprite = spriteUnchecked;
        female = false;
    }

    public void ButtonProfileFemale()
    {
        imageProfileMale.sprite = spriteUnchecked;
        imageProfileFemale.sprite = spriteChecked;
        female = true;
    }

    public void EndEditProfilePlayerName()
    {
        imageProfileNameConfirmation.sprite = IsNameValid(fieldProfileName.text) ? spriteValid : spriteInvalid;
    }

    public void ButtonSavePlayerProfile()
    {
        if (!IsNameValid(fieldProfileName.text))
            return;

        Player p = currentItemPlayer.GetPlayer();
        p.SetName(fieldProfileName.text);
        p.SetPictureIndex(currentPictureIndex);
        p.SetNumber(fieldProfileNumber.text == string.Empty ? -1 : int.Parse(fieldProfileNumber.text));
        p.SetFemale(female);

        SF.SavePlayersData();
    }

    public void ButtonConfirmDeletePlayer()
    {
        SF.RemovePlayer(currentItemPlayer.GetPlayer());
    }
    #endregion

    private bool IsNameValid(string _name)
    {
        if (_name == string.Empty)
            return false;

        foreach (Player p in SF.GetAllPlayers())
        {
            if (currentItemPlayer != null && currentItemPlayer.GetPlayer() == p)
                continue;

            if (p.GetName() == _name)
                return false;
        }

        return true;
    }
}
