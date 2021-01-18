using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerTournaments : MonoBehaviour
{
    public static ManagerTournaments MT;

    public GameObject manager;
    private ManagerUI MUI;
    private SaveFile SF;

    public Animator animPanelMainMenu;
    public GameObject panelPlaying;
    public Animator animPanelSpirit;
    public GameObject panelHistory;

    //TOURNAMENTS
    public Transform parentTournaments;
    public ItemTournament prefabItemTournament;
    private ItemTournament currentItemTournament;
    private Tournament cTournament;

    //NEW TOURNAMENT
    public InputField fieldNewTournamentName;
    public Image imageNameConfirmation;
    public Sprite spriteInvalid;
    public Sprite spriteValid;
    public InputField fieldNewYear;
    public InputField fieldNewMonth;
    public InputField fieldNewDay;
    public InputField fieldNewHour;
    public InputField fieldNewMinute;
    public Image imageCheckEvents;
    public Image imageCheckPlaying;
    public TextMeshProUGUI textGenderFormat;
    private bool checkWhoEvents;
    private bool checkWhosPlaying;
    private int genderFormatIndex;

    //TOURNAMENT PROFILE
    public Animator animTournamentProfile;
    public TextMeshProUGUI textProfileName;
    public TextMeshProUGUI textProfileStatus;

    //TOURNAMENT SETTINGS
    public InputField fieldSettingsName;
    public Image imageSettingsNameConfirmation;
    public InputField fieldSettingsYear;
    public InputField fieldSettingsMonth;
    public InputField fieldSettingsDay;
    public InputField fieldSettingsHour;
    public InputField fieldSettingsMinute;
    public TextMeshProUGUI textTournamentTeamFormat;

    //PLAYERS
    public Animator animTournamentPlayers;
    public Transform parentPlayers;
    public ItemPlayer prefabItemPlayer;
    public GameObject buttonSelectPlayers;
    private List<int> participantPlayers;
    public Transform parentPlayersSelect;

    //PLAYERS - PROFILE
    public Animator animPlayerProfile;
    public InputField fieldStartElo;
    public TextMeshProUGUI textPlayerProfileName;
    public TextMeshProUGUI textPlayerProfileMatches;
    public TextMeshProUGUI textPlayerProfileElo;
    public TextMeshProUGUI textPlayerProfileAssists;
    public TextMeshProUGUI textPlayerProfilePoints;
    public TextMeshProUGUI textPlayerProfileDefenses;
    public TextMeshProUGUI textPlayerProfileCallahans;
    public TextMeshProUGUI textPlayerProfileScoreOff;
    public TextMeshProUGUI textPlayerProfileScoreDef;
    public TextMeshProUGUI textPlayerProfileSufferOff;
    public TextMeshProUGUI textPlayerProfileSufferDef;
    private Player cPlayer;

    //TEAMS
    public Animator animTournamentTeams;
    public Transform parentTeams;
    public ItemTeam prefabItemTeam;
    public GameObject buttonNewTeam;
    public GameObject buttonGenerateTeam;
    public Animator animNewTeam;
    private ItemTeam currentItemTeam;
    public InputField fieldNewTeamName;
    public Image imageTeamNameConfirmation;
    public Image imageTeamColor;
    public Text textTeamFormat;
    public Animator animTeamProfile;
    public TextMeshProUGUI textTeamProfileTitle;
    public GameObject buttonDeleteTeam;
    public Image imageTeamProfileNameConfirmation;
    public InputField fieldTeamProfileName;
    public TextMeshProUGUI textTeamNPickedPlayers;
    private List<int> pickedTeamPlayersIDs;
    public Transform parentTeamPlayers;
    public Animator animGenerator;

    //MATCHES
    public Animator animTournamentMatches;
    public Transform parentMatches;
    public ItemMatch prefabItemMatch;
    private List<ItemMatch> itemsMatches;
    public GameObject buttonBackMatches;
    public GameObject buttonSettings;
    public GameObject buttonStartTournament;
    public GameObject buttonEndTournament;
    public Text textBracket1;
    private int bracketIndex1;
    public Text textBracket2;
    private int bracketIndex2;
    public InputField fieldGroupSize;
    public InputField fieldAdvanceEachGroup;
    public InputField fieldFullTime;
    public InputField fieldHalfTime;
    public InputField fieldTimeBetweenMatches;
    public InputField fieldNFields;

    //RANKING - TEAMS
    public Transform parentRankingTeams;
    public TextMeshProUGUI textSortTeams;
    public Image imageButtonTeamsRank;
    public Image imageButtonTeamsAlpha;
    public Image imageButtonTeamsSpirit;
    public Image imageButtonTeamsVictory;
    public Image imageButtonTeamsPointDif;

    //RANKING - PLAYERS
    public Transform parentRankingPlayers;
    public TextMeshProUGUI textSortPlayers;
    public InputField fieldRankingPlayersPoints;
    public InputField fieldRankingPlayersAssists;
    public InputField fieldRankingPlayersDefenses;
    public InputField fieldRankingPlayersCallahans;
    public GameObject textRankingPlayersScoreOff;
    public GameObject textRankingPlayersScoreDef;
    public GameObject textRankingPlayersSufferOff;
    public GameObject textRankingPlayersSufferDef;
    public InputField fieldRankingPlayersScoreOff;
    public InputField fieldRankingPlayersScoreDef;
    public InputField fieldRankingPlayersSufferOff;
    public InputField fieldRankingPlayersSufferDef;
    public Image imageButtonPlayersRank;
    public Image imageButtonPlayersAlpha;
    public Image imageButtonPlayersPoint;
    public Image imageButtonPlayersAssist;
    public Image imageButtonPlayersDefense;

    public Sprite spriteChecked;
    public Sprite spriteUnchecked;

    public enum SortTeamCriteria
    {
        RANK,
        ALPHA,
        SPIRIT,
        VICTORY,
        POINT_DIF
    }

    public enum SortPlayerCriteria
    {
        RANK,
        ALPHA,
        POINT,
        ASSIST,
        DEFENSE
    }

    public enum BracketFormatFirst
    {
        ROUND_ROBIN,
        SINGLE_ELIMINATION,
        DOUBLE_ELIMINATION
    }

    public enum BracketFormatSecond
    {
        NOTHING,
        ROUND_ROBIN,
        SINGLE_ELIMINATION,
        DOUBLE_ELIMINATION,
        MANY_FINALS
    }

    public enum GenderFormat
    {
        MIXED,
        MEN,
        WOMEN
    }

    private void Awake()
    {
        MT = this;

        MUI = manager.GetComponent<ManagerUI>();
        SF = manager.GetComponent<SaveFile>();

        participantPlayers = new List<int>();
        itemsMatches = new List<ItemMatch>();
    }

    private void Start()
    {
        parentPlayers.GetComponent<GridLayoutGroup>().cellSize = new Vector2(ManagerUI.MUI.sizePlayerGrid * Screen.height, ManagerUI.MUI.sizePlayerGrid * Screen.height);
        parentPlayersSelect.GetComponent<GridLayoutGroup>().cellSize = new Vector2(ManagerUI.MUI.sizePlayerGrid * Screen.height, ManagerUI.MUI.sizePlayerGrid * Screen.height);
        parentTeamPlayers.GetComponent<GridLayoutGroup>().cellSize = new Vector2(ManagerUI.MUI.sizePlayerGrid * Screen.height, ManagerUI.MUI.sizePlayerGrid * Screen.height);
        parentRankingPlayers.GetComponent<GridLayoutGroup>().cellSize = new Vector2(ManagerUI.MUI.sizePlayerGrid * Screen.height, ManagerUI.MUI.sizePlayerGrid * Screen.height);
    }

    private void OnEnable()
    {
        prefabItemTournament.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, ManagerUI.MUI.sizeItem);

        for (int i = 0; i < parentTournaments.childCount; i++)
            Destroy(parentTournaments.GetChild(i).gameObject);

        foreach (Tournament t in SF.GetAllTournaments())
        {
            ItemTournament it = Instantiate(prefabItemTournament, parentTournaments);
            it.StartThis(t, ItemTournament.ItemTournamentType.TOURNAMENT);
        }
    }

    #region TOURNAMENT
    public void ButtonNewTournament()
    {
        currentItemTournament = null;
        cTournament = null;
        fieldNewTournamentName.text = string.Empty;
        imageNameConfirmation.sprite = spriteInvalid;
        checkWhoEvents = false;
        checkWhosPlaying = false;
        genderFormatIndex = 0;
        imageCheckEvents.sprite = spriteUnchecked;
        imageCheckPlaying.sprite = spriteUnchecked;
        textGenderFormat.text = GenderToString(GenderFormat.MIXED);
    }

    public void EndEditNewTournamentName()
    {
        imageNameConfirmation.sprite = IsNameValid(fieldNewTournamentName.text) ? spriteValid : spriteInvalid;
    }

    private bool IsNameValid(string _name)
    {
        if (_name == string.Empty)
            return false;

        foreach (Tournament t in SF.GetAllTournaments())
        {
            if (currentItemTournament != null && cTournament == t)
                continue;

            if (t.GetName() == _name)
                return false;
        }

        return true;
    }

    public void ButtonCheckWhoEvents()
    {
        checkWhoEvents = !checkWhoEvents;

        imageCheckEvents.sprite = checkWhoEvents ? spriteChecked : spriteUnchecked;
    }

    public void ButtonCheckWhosPlaying()
    {
        checkWhosPlaying = !checkWhosPlaying;

        imageCheckPlaying.sprite = checkWhosPlaying ? spriteChecked : spriteUnchecked;
    }

    public void ButtonGenderFormatRight()
    {
        genderFormatIndex++;

        if (genderFormatIndex >= Enum.GetValues(typeof(GenderFormat)).Length)
            genderFormatIndex = 0;

        textGenderFormat.text = GenderToString((GenderFormat)genderFormatIndex);
    }

    public void ButtonGenderFormatLeft()
    {
        genderFormatIndex--;

        if (genderFormatIndex < 0)
            genderFormatIndex = Enum.GetValues(typeof(GenderFormat)).Length - 1;

        textGenderFormat.text = GenderToString((GenderFormat)genderFormatIndex);
    }

    public void ButtonConfirmNewTournament()
    {
        if (!IsNameValid(fieldNewTournamentName.text))
            return;

        int year = Mathf.Clamp(fieldNewYear.text == string.Empty ? DateTime.Now.Year : int.Parse(fieldNewYear.text), DateTime.Now.Year, DateTime.Now.Year + 5);
        int month = Mathf.Clamp(fieldNewMonth.text == string.Empty ? DateTime.Now.Month : int.Parse(fieldNewMonth.text), 1, 12);
        int day = Mathf.Clamp(fieldNewDay.text == string.Empty ? DateTime.Now.Day : int.Parse(fieldNewDay.text), 1, 31);
        int hour = Mathf.Clamp(fieldNewHour.text == string.Empty ? DateTime.Now.Hour : int.Parse(fieldNewHour.text), 0, 23);
        int minute = Mathf.Clamp(fieldNewMinute.text == string.Empty ? DateTime.Now.Minute : int.Parse(fieldNewMinute.text), 0, 59);
        Tournament t = new Tournament(fieldNewTournamentName.text, new DateTime(year, month, day, hour, minute, 0), checkWhoEvents, checkWhosPlaying, (GenderFormat)genderFormatIndex);
        ItemTournament it = Instantiate(prefabItemTournament, parentTournaments);
        it.StartThis(t, ItemTournament.ItemTournamentType.TOURNAMENT);

        SF.AddTournament(t);

        OpenTournamentProfile(it);
    }

    public void OpenTournamentProfile(ItemTournament _itemTournament)
    {
        currentItemTournament = _itemTournament;
        cTournament = _itemTournament.GetTournament();

        for (int i = 0; i < cTournament.GetNPlayers(); i++)
            SF.GetPlayer(cTournament.GetPlayerID(i)).SetCurrentTournament(cTournament.ID);

        textProfileName.text = _itemTournament.GetTournament().GetName();

        switch (cTournament.GetStatus())
        {
            case Tournament.TournamentStatus.REGISTRATIONS:
            case Tournament.TournamentStatus.READY:
                textProfileStatus.text = "(Register)";
                break;
            case Tournament.TournamentStatus.RUNNING:
                textProfileStatus.text = "(Running - Phase " + (cTournament.GetPhase() + 1) + ")";
                break;
            case Tournament.TournamentStatus.FINISHED:
                textProfileStatus.text = "(Finished)";
                break;
        }

        if (!animTournamentProfile.gameObject.activeSelf) MUI.GoRight(animTournamentProfile);
    }

    public void ButtonOpenTournamentSettings()
    {
        fieldSettingsName.text = cTournament.GetName();
        imageSettingsNameConfirmation.sprite = spriteValid;
        fieldSettingsYear.text = cTournament.GetDate().Year.ToString();
        fieldSettingsMonth.text = cTournament.GetDate().Month.ToString("00");
        fieldSettingsDay.text = cTournament.GetDate().Day.ToString("00");
        fieldSettingsHour.text = cTournament.GetDate().Hour.ToString("00");
        fieldSettingsMinute.text = cTournament.GetDate().Minute.ToString("00");
    }

    public void EndEditSettingsTournamentName()
    {
        imageSettingsNameConfirmation.sprite = IsNameValid(fieldSettingsName.text) ? spriteValid : spriteInvalid;
    }

    public void ButtonSaveTournamentSettings()
    {
        if (imageSettingsNameConfirmation.sprite == spriteInvalid)
            return;

        int year = Mathf.Clamp(fieldSettingsYear.text == string.Empty ? 0 : int.Parse(fieldSettingsYear.text), DateTime.Now.Year, DateTime.Now.Year + 5);
        int month = Mathf.Clamp(fieldSettingsMonth.text == string.Empty ? 0 : int.Parse(fieldSettingsMonth.text), 1, 12);
        int day = Mathf.Clamp(fieldSettingsDay.text == string.Empty ? 0 : int.Parse(fieldSettingsDay.text), 1, 31);
        int hour = Mathf.Clamp(fieldSettingsHour.text == string.Empty ? 0 : int.Parse(fieldSettingsHour.text), 0, 23);
        int minute = Mathf.Clamp(fieldSettingsMinute.text == string.Empty ? 0 : int.Parse(fieldSettingsMinute.text), 0, 59);
        Tournament t = cTournament;
        t.SetName(fieldSettingsName.text);
        t.SetDate(new DateTime(year, month, day, hour, minute, 0));
        textProfileName.text = t.GetName();

        SF.SaveTournamentsData();
    }

    public void ButtonConfirmDeleteTournament()
    {
        SF.RemoveTournament(cTournament);
    }
    #endregion

    #region PLAYERS
    public void ButtonOpenPlayers()
    {
        for (int i = 0; i < parentPlayers.childCount; i++)
            Destroy(parentPlayers.GetChild(i).gameObject);

        foreach (Player p in SF.GetAllPlayers())
        {
            if (cTournament.HasPlayer(p.ID))
            {
                ItemPlayer ip = Instantiate(prefabItemPlayer, parentPlayers);
                ip.StartThis(p.ID, ItemPlayer.ItemPlayerType.PLAYER_TOURNAMENT, false, float.MinValue);
            }
        }

        buttonSelectPlayers.SetActive(cTournament.GetStatus() != Tournament.TournamentStatus.FINISHED);

        MUI.GoRight(animTournamentPlayers);
    }

    public void OpenPlayerProfile(ItemPlayer _itemPlayer)
    {
        cPlayer = _itemPlayer.GetPlayer();

        Player p = _itemPlayer.GetPlayer();
        PlayerInTournament pit = p.GetCurrentTournament();

        textPlayerProfileName.text = p.GetName();
        textPlayerProfileMatches.text = p.GetCurrentTournament().GetMatches().ToString();
        fieldStartElo.text = p.GetCurrentTournament().GetStartElo().ToString();
        textPlayerProfileElo.text = p.GetCurrentTournament().GetElo().ToString();
        textPlayerProfileAssists.text = pit.GetAssists().ToString();
        textPlayerProfilePoints.text = pit.GetPoints().ToString();
        textPlayerProfileDefenses.text = pit.GetDefenses().ToString();
        textPlayerProfileCallahans.text = pit.GetCallahans().ToString();
        textPlayerProfileScoreOff.text = pit.GetScoreOff().ToString();
        textPlayerProfileScoreDef.text = pit.GetScoreDef().ToString();
        textPlayerProfileSufferOff.text = pit.GetSufferOff().ToString();
        textPlayerProfileSufferDef.text = pit.GetSufferDef().ToString();

        MUI.GoRight(animPlayerProfile);
    }

    public void EndEditPlayerProfile()
    {
        cPlayer.GetCurrentTournament().SetStartElo(fieldStartElo.text == string.Empty ? 0 : int.Parse(fieldStartElo.text));
        cTournament.UpdatePlayerElo(cPlayer);
        textPlayerProfileElo.text = cPlayer.GetCurrentTournament().GetElo().ToString();

        SF.SavePlayersData();
    }

    public void ButtonOpenSelectPlayers()
    {
        participantPlayers.Clear();

        for (int i = 0; i < parentPlayersSelect.childCount; i++)
            Destroy(parentPlayersSelect.GetChild(i).gameObject);

        foreach (Player p in SF.GetAllPlayers())
        {
            if (cTournament.GetGenderFormat() == GenderFormat.MEN && p.GetFemale())
                continue;

            if (cTournament.GetGenderFormat() == GenderFormat.WOMEN && !p.GetFemale())
                continue;

            ItemPlayer ip = Instantiate(prefabItemPlayer, parentPlayersSelect);
            ip.StartThis(p.ID, ItemPlayer.ItemPlayerType.PLAYER_TOURNAMENT_ADD, true, float.MinValue);

            if (cTournament.HasPlayer(p.ID))
                ip.Clicked();
        }
    }

    public void PickPlayer(ItemPlayer _itemPlayer)
    {
        participantPlayers.Add(_itemPlayer.GetPlayerID());
    }

    public void UnpickPlayer(ItemPlayer _itemPlayer)
    {
        participantPlayers.Remove(_itemPlayer.GetPlayerID());
    }

    public void ButtonConfirmSelectPlayers()
    {
        for (int i = 0; i < participantPlayers.Count; i++)
        {
            if (!SF.GetPlayer(participantPlayers[i]).IsInTournament(cTournament.ID))
                SF.GetPlayer(participantPlayers[i]).AddTournament(cTournament.ID);
        }

        for (int i = 0; i < cTournament.GetNPlayers(); i++)
        {
            if (!IsInParticipantPlayers(cTournament.GetPlayerID(i)))
            {
                SF.GetPlayer(cTournament.GetPlayerID(i)).RemoveTournament(cTournament.ID);

                //Remove player from teams
                for (int t = 0; t < cTournament.GetNTeams(); t++)
                {
                    if (cTournament.GetTeam(t).HasPlayer(cTournament.GetPlayerID(i)))
                        cTournament.GetTeam(t).RemovePlayer(cTournament.GetPlayerID(i));
                }
            }
        }

        participantPlayers.Sort((p1, p2) => SF.GetPlayer(p1).GetName().CompareTo(SF.GetPlayer(p2).GetName()));
        cTournament.SetPlayers(participantPlayers);

        SF.SavePlayersData();
        SF.SaveTournamentsData();

        ButtonOpenPlayers();
    }

    private bool IsInParticipantPlayers(int _playerID)
    {
        for (int i = 0; i < participantPlayers.Count; i++)
        {
            if (participantPlayers[i] == _playerID)
                return true;
        }

        return false;
    }

    public List<int> GetAllPlayers()
    {
        return cTournament.GetPlayersIDs();
    }
    #endregion

    #region TEAMS
    public void ButtonOpenTeams()
    {
        for (int i = 0; i < parentTeams.childCount; i++)
            Destroy(parentTeams.GetChild(i).gameObject);

        for (int i = 0; i < cTournament.GetNTeams(); i++)
        {
            ItemTeam it = Instantiate(prefabItemTeam, parentTeams);
            it.StartThis(cTournament.GetTeam(i), ItemTeam.ItemTeamType.TEAM_TOURNAMENT);
        }

        MUI.GoRight(animTournamentTeams);
    }

    public void ButtonNewTeam()
    {
        currentItemTeam = null;
        fieldNewTeamName.text = string.Empty;
        imageTeamNameConfirmation.sprite = spriteInvalid;

        MUI.GoRight(animNewTeam);
    }

    public void ButtonGenerateTeams()
    {
        MUI.GoRight(animGenerator);
    }

    public void EndEditTeamName()
    {
        imageTeamNameConfirmation.sprite = IsTeamNameValid(fieldNewTeamName.text) ? spriteValid : spriteInvalid;
    }

    private bool IsTeamNameValid(string _name)
    {
        if (_name == string.Empty)
            return false;

        for (int i = 0; i < cTournament.GetNTeams(); i++)
        {
            if (currentItemTeam != null && currentItemTeam.GetTeam() == cTournament.GetTeam(i))
                continue;

            if (cTournament.GetTeam(i).GetName() == _name)
                return false;
        }

        return true;
    }

    public void ButtonGenderLeft()
    {
        genderFormatIndex--;

        if (genderFormatIndex < 0)
            genderFormatIndex = Enum.GetValues(typeof(GenderFormat)).Length - 1;

        textTeamFormat.text = GenderToString((GenderFormat)genderFormatIndex);
    }

    public void ButtonGenderRight()
    {
        genderFormatIndex++;

        if (genderFormatIndex >= Enum.GetValues(typeof(GenderFormat)).Length)
            genderFormatIndex = 0;

        textTeamFormat.text = GenderToString((GenderFormat)genderFormatIndex);
    }

    public void ButtonConfirmNewTeam()
    {
        if (!IsTeamNameValid(fieldNewTeamName.text))
            return;

        Team t = new Team(fieldNewTeamName.text, (GenderFormat)genderFormatIndex);
        ItemTeam it = Instantiate(prefabItemTeam, parentTeams);
        it.StartThis(t, ItemTeam.ItemTeamType.TEAM_TOURNAMENT);

        cTournament.AddTeam(t);

        SF.SaveTournamentsData();

        OpenTeamProfile(it);
    }

    public void AddGeneratedTeam(Team _t)
    {
        ItemTeam it = Instantiate(prefabItemTeam, parentTeams);
        it.StartThis(_t, ItemTeam.ItemTeamType.TEAM_TOURNAMENT);

        cTournament.AddTeam(_t);
    }

    public void EndTeamGeneration(int _nGeneratedTeams)
    {
        cTournament.GetTeams().RemoveRange(0, cTournament.GetNTeams() - _nGeneratedTeams);
        ButtonOpenTeams();
    }

    public void OpenTeamProfile(ItemTeam _itemTeam)
    {
        Team te = _itemTeam.GetTeam();

        currentItemTeam = _itemTeam;
        textTeamProfileTitle.text = te.GetName();
        fieldTeamProfileName.text = te.GetName();
        imageTeamProfileNameConfirmation.sprite = spriteValid;
        imageTeamColor.color = ManagerColors.MC.teamColors[te.GetColorIndex()];

        pickedTeamPlayersIDs = new List<int>();
        textTeamNPickedPlayers.text = te.GetNPlayers().ToString();

        for (int i = 0; i < parentTeamPlayers.childCount; i++)
            Destroy(parentTeamPlayers.GetChild(i).gameObject);

        bool playerInOtherTeam;

        for (int i = 0; i < cTournament.GetNPlayers(); i++)
        {
            playerInOtherTeam = false;

            for (int j = 0; j < cTournament.GetNTeams(); j++)
            {
                if (te == cTournament.GetTeam(j))
                    continue;

                if (cTournament.GetTeam(j).HasPlayer(cTournament.GetPlayerID(i)))
                    playerInOtherTeam = true;
            }

            if (!playerInOtherTeam)
            {
                ItemPlayer ip = Instantiate(prefabItemPlayer, parentTeamPlayers);
                ip.StartThis(cTournament.GetPlayerID(i), cTournament.GetStatus() == Tournament.TournamentStatus.FINISHED ? ItemPlayer.ItemPlayerType.NOTHING : ItemPlayer.ItemPlayerType.PLAYER_TEAMS, true, float.MinValue);

                if (te.HasPlayer(cTournament.GetPlayerID(i)))
                    ip.Clicked();
            }
        }

        if (cTournament.GetStatus() == Tournament.TournamentStatus.RUNNING || cTournament.GetStatus() == Tournament.TournamentStatus.FINISHED)
            buttonDeleteTeam.SetActive(false);

        if (!animTeamProfile.gameObject.activeSelf) MUI.GoRight(animTeamProfile);
    }

    public void EndEditTeamProfileName()
    {
        imageTeamProfileNameConfirmation.sprite = IsTeamNameValid(fieldTeamProfileName.text) ? spriteValid : spriteInvalid;
    }

    public void ButtonTeamColor()
    {
        Team t = currentItemTeam.GetTeam();

        t.IncrementColorIndex();
        imageTeamColor.color = ManagerColors.MC.teamColors[t.GetColorIndex()];
    }

    public void PickTeamPlayer(ItemPlayer _itemPlayer)
    {
        pickedTeamPlayersIDs.Add(_itemPlayer.GetPlayerID());
        textTeamNPickedPlayers.text = pickedTeamPlayersIDs.Count.ToString();
    }

    public void UnpickTeamPlayer(ItemPlayer _itemPlayer)
    {
        if (cTournament.GetStatus() == Tournament.TournamentStatus.FINISHED)
            return;

        pickedTeamPlayersIDs.Remove(_itemPlayer.GetPlayerID());
        textTeamNPickedPlayers.text = pickedTeamPlayersIDs.Count.ToString();
    }

    public void ButtonSaveTeamProfile()
    {
        Team t = currentItemTeam.GetTeam();

        if (!IsTeamNameValid(fieldTeamProfileName.text))
            return;

        t.SetName(fieldTeamProfileName.text);
        if (cTournament.GetStatus() != Tournament.TournamentStatus.FINISHED)
            t.SetPlayers(pickedTeamPlayersIDs);

        SF.SaveTournamentsData();

        ButtonOpenTeams();
    }

    public void ButtonConfirmDeleteTeam()
    {
        cTournament.RemoveTeam(currentItemTeam.GetTeam());

        SF.SaveTournamentsData();

        ButtonOpenTeams();
    }
    #endregion

    #region MATCHES
    public void ButtonOpenMatches()
    {
        switch (cTournament.GetStatus())
        {
            case Tournament.TournamentStatus.REGISTRATIONS:
                buttonSettings.SetActive(cTournament.GetNTeams() >= 2);
                buttonStartTournament.SetActive(false);
                break;
            case Tournament.TournamentStatus.READY:
                buttonSettings.SetActive(true);
                buttonStartTournament.SetActive(true);
                break;
            case Tournament.TournamentStatus.RUNNING:
            case Tournament.TournamentStatus.FINISHED:
                buttonSettings.SetActive(false);
                buttonStartTournament.SetActive(false);
                break;
        }

        FlushMatches();

        MUI.GoRight(animTournamentMatches);
    }

    private void FlushMatches()
    {
        Tournament t = cTournament;

        for (int i = 0; i < parentMatches.childCount; i++)
            Destroy(parentMatches.GetChild(i).gameObject);

        itemsMatches.Clear();

        switch (t.GetStatus())
        {
            case Tournament.TournamentStatus.READY:
            case Tournament.TournamentStatus.RUNNING:
            case Tournament.TournamentStatus.FINISHED:
                for (int i = 0; i < t.GetNMatches(); i++)
                {
                    ItemMatch im = Instantiate(prefabItemMatch, parentMatches);
                    im.StartThis(t.GetMatch(i));
                    itemsMatches.Add(im);
                }
                break;
        }
    }

    public void ButtonBackFromMatch()
    {
        for (int i = 0; i < cTournament.GetNMatches(); i++)
        {
            if (cTournament.GetMatch(i).GetStatus() == Match.MatchStatus.RUNNING)
            {
                buttonBackMatches.SetActive(false);
                return;
            }
        }

        buttonBackMatches.SetActive(true);
    }

    public void ButtonOpenGenerationSettings()
    {
        Tournament t = cTournament;

        textBracket1.text = BracketFormat1ToString(t.GetBracketFormatFirst());
        textBracket2.text = BracketFormat2ToString(t.GetBracketFormatSecond());
        bracketIndex1 = (int)t.GetBracketFormatFirst();
        bracketIndex2 = (int)t.GetBracketFormatSecond();

        if (bracketIndex2 != 0)
        {
            fieldGroupSize.gameObject.SetActive(true);
            fieldAdvanceEachGroup.gameObject.SetActive(true);
            fieldGroupSize.text = t.GetGroupSize().ToString();
            fieldAdvanceEachGroup.text = t.GetNAdvance().ToString();
        }

        fieldFullTime.text = t.GetFullTime() <= 0 ? string.Empty : t.GetFullTime().ToString();
        fieldHalfTime.text = t.GetHalfTime() <= 0 ? string.Empty : t.GetHalfTime().ToString();
        fieldTimeBetweenMatches.text = t.GetTimeBetweenMatches() == 0 ? string.Empty : t.GetTimeBetweenMatches().ToString();
        fieldNFields.text = t.GetNFields() <= 1 ? string.Empty : t.GetNFields().ToString();
    }

    public void ButtonBracket1FormatLeft()
    {
        bracketIndex1--;

        if (bracketIndex1 < 0)
            bracketIndex1 = Enum.GetValues(typeof(BracketFormatFirst)).Length - 1;

        textBracket1.text = BracketFormat1ToString((BracketFormatFirst)bracketIndex1);
    }

    public void ButtonBracket1FormatRight()
    {
        bracketIndex1++;

        if (bracketIndex1 >= Enum.GetValues(typeof(BracketFormatFirst)).Length)
            bracketIndex1 = 0;

        textBracket1.text = BracketFormat1ToString((BracketFormatFirst)bracketIndex1);
    }

    public void ButtonBracket2FormatLeft()
    {
        bracketIndex2--;

        if (bracketIndex2 < 0)
            bracketIndex2 = Enum.GetValues(typeof(BracketFormatSecond)).Length - 1;

        textBracket2.text = BracketFormat2ToString((BracketFormatSecond)bracketIndex2);

        fieldGroupSize.gameObject.SetActive(bracketIndex2 != 0);
        fieldAdvanceEachGroup.gameObject.SetActive(bracketIndex2 != 0);
    }

    public void ButtonBracket2FormatRight()
    {
        bracketIndex2++;

        if (bracketIndex2 >= Enum.GetValues(typeof(BracketFormatSecond)).Length)
            bracketIndex2 = 0;

        textBracket2.text = BracketFormat2ToString((BracketFormatSecond)bracketIndex2);

        fieldGroupSize.gameObject.SetActive(bracketIndex2 != 0);
        fieldAdvanceEachGroup.gameObject.SetActive(bracketIndex2 != 0);
    }

    public void ButtonConfirmGenerate()
    {
        Tournament t = cTournament;

        t.ClearMatches();
        t.SetBracketFormatFirst((BracketFormatFirst)bracketIndex1);
        t.SetBracketFormatSecond((BracketFormatSecond)bracketIndex2);

        if (bracketIndex2 != 0)
        {
            if (fieldGroupSize.text == string.Empty)
                fieldGroupSize.text = t.GetNTeams().ToString();

            if (fieldAdvanceEachGroup.text == string.Empty)
                fieldAdvanceEachGroup.text = fieldGroupSize.text;

            t.SetGroupSize(int.Parse(fieldGroupSize.text));
            t.SetNAdvance(int.Parse(fieldAdvanceEachGroup.text));
        }

        t.SetFullTime(fieldFullTime.text == string.Empty ? -1 : int.Parse(fieldFullTime.text));
        t.SetHalfTime(fieldHalfTime.text == string.Empty ? -1 : int.Parse(fieldHalfTime.text));
        t.SetTimeBetweenMatches(fieldTimeBetweenMatches.text == string.Empty ? 0 : int.Parse(fieldTimeBetweenMatches.text));
        t.SetNFields(fieldNFields.text == string.Empty ? 1 : int.Parse(fieldNFields.text));

        for (int i = 0; i < parentMatches.childCount; i++)
            Destroy(parentMatches.GetChild(i).gameObject);

        t.ShuffleTeams();

        DateTime dateTime = t.GetDate();

        switch (t.GetBracketFormatFirst())
        {
            case BracketFormatFirst.ROUND_ROBIN:
                int roundSize = Mathf.CeilToInt(t.GetNTeams() / 2f);
                int nTeams = t.GetNTeams() % 2 == 0 ? t.GetNTeams() : t.GetNTeams() + 1; //Number of teams, can include 1 dummy if odd
                int[] top = new int[roundSize];
                int[] bottom = new int[roundSize];

                for (int i = 0; i < roundSize; i++)
                    top[i] = i;

                for (int i = 0; i < roundSize; i++)
                    bottom[i] = nTeams - 1 - i;

                for (int i = 0; i < nTeams - 1; i++)
                {
                    for (int j = 0; j < roundSize; j++)
                    {
                        if (top[j] != t.GetNTeams() && bottom[j] != t.GetNTeams()) //Check if dummy
                        {
                            t.AddMatch(new Match(t.GetNMatches(), dateTime, t.GetTeam(top[j]), t.GetTeam(bottom[j]), 0));
                            dateTime = dateTime.AddMinutes(t.GetFullTime() + t.GetTimeBetweenMatches());
                        }
                    }

                    for (int j = 1; j < roundSize; j++)
                    {
                        top[j]--;

                        if (top[j] == 0)
                            top[j] = nTeams - 1;
                    }

                    for (int j = 0; j < roundSize; j++)
                    {
                        bottom[j]--;

                        if (bottom[j] == 0)
                            bottom[j] = nTeams - 1;
                    }
                }
                break;
            case BracketFormatFirst.SINGLE_ELIMINATION:
                break;
            case BracketFormatFirst.DOUBLE_ELIMINATION:
                break;
        }

        switch (t.GetBracketFormatSecond())
        {
            case BracketFormatSecond.NOTHING:
                break;
            case BracketFormatSecond.ROUND_ROBIN:
                break;
            case BracketFormatSecond.SINGLE_ELIMINATION:
                break;
            case BracketFormatSecond.DOUBLE_ELIMINATION:
                break;
            case BracketFormatSecond.MANY_FINALS:
                for (int place = t.GetNAdvance() * t.GetNGroups(); place > 1; place -= 2)
                {
                    Match m = new Match(t.GetNMatches(), dateTime, null, null, 1);
                    m.SetTeamAOriginPlace(1, place - 1);
                    m.SetTeamBOriginPlace(1, place);
                    t.AddMatch(m);
                    dateTime = dateTime.AddMinutes(t.GetFullTime() + t.GetTimeBetweenMatches());
                }
                break;
        }

        t.SetStatus(Tournament.TournamentStatus.READY);
        buttonStartTournament.SetActive(true);

        FlushMatches();

        SF.SaveTournamentsData();
    }

    public void ButtonStartTournament()
    {
        Tournament t = cTournament;

        t.SetStatus(Tournament.TournamentStatus.RUNNING);
        textProfileStatus.text = "(Running - Phase 1)";
        buttonSettings.SetActive(false);
        buttonStartTournament.SetActive(false);

        for (int i = 0; i < t.GetNMatches(); i++)
        {
            if (t.GetMatch(i).GetTeamA() != null && t.GetMatch(i).GetTeamB() != null)
                t.GetMatch(i).SetStatus(Match.MatchStatus.READY);
        }

        FlushMatches();

        SF.SaveTournamentsData();
    }

    public void ButtonOpenItemMatch(ItemMatch _itemMatch)
    {
        switch (_itemMatch.GetMatch().GetStatus())
        {
            case Match.MatchStatus.READY:
                if (!panelPlaying.activeSelf)
                {
                    MUI.GoRight(panelPlaying.GetComponent<Animator>());
                    panelPlaying.GetComponent<ManagerPlaying>().StartThis(cTournament, _itemMatch);
                }
                break;
            case Match.MatchStatus.RUNNING:
                if (!panelPlaying.activeSelf)
                {
                    MUI.GoRight(panelPlaying.GetComponent<Animator>());
                    panelPlaying.GetComponent<ManagerPlaying>().ContinueThis(_itemMatch);
                }
                break;
            case Match.MatchStatus.SPIRIT:
                if (!animPanelSpirit.gameObject.activeSelf)
                {
                    MUI.GoRight(animPanelSpirit);
                    animPanelSpirit.GetComponent<ManagerSpirit>().StartThis(_itemMatch);
                }
                break;
            case Match.MatchStatus.FINISHED:
                if (!panelHistory.activeSelf)
                {
                    MUI.GoRight(panelHistory.GetComponent<Animator>());
                    panelHistory.GetComponent<ManagerHistory>().StartThis(_itemMatch);
                }
                break;
        }
    }

    public void ButtonEndMatch(ItemMatch _itemMatch)
    {
        _itemMatch.Refresh();

        UpdateBracket(_itemMatch);
        ButtonBackFromMatch();

        SF.SavePlayersData();
        SF.SaveTournamentsData();
    }

    public void UpdateBracket(ItemMatch _itemMatch)
    {
        Tournament t = cTournament;

        switch (t.GetBracketFormatSecond())
        {
            case BracketFormatSecond.NOTHING:
                t.UpdateTeamRanks();
                break;
            case BracketFormatSecond.ROUND_ROBIN:
                break;
            case BracketFormatSecond.SINGLE_ELIMINATION:
                break;
            case BracketFormatSecond.DOUBLE_ELIMINATION:
                break;
            case BracketFormatSecond.MANY_FINALS:
                switch (t.GetPhase())
                {
                    case 0:
                        for (int i = 0; i < t.GetNMatches(); i++)
                        {
                            if (t.GetMatch(i).GetPhase() == 0 && (t.GetMatch(i).GetStatus() == Match.MatchStatus.READY || t.GetMatch(i).GetStatus() == Match.MatchStatus.RUNNING))
                                return;
                        }

                        t.UpdateTeamRanks();
                        t.IncrementPhase();

                        ItemMatch im;

                        for (int i = 0; i < itemsMatches.Count; i++)
                        {
                            im = itemsMatches[i];

                            if (im.GetMatch().GetPhase() == 1 && im.GetMatch().GetStatus() == Match.MatchStatus.NOTHING)
                            {
                                im.GetMatch().SetTeamA(t.GetTeamAtRank(t.GetMatch(i).GetPlaceA()));
                                im.GetMatch().SetTeamB(t.GetTeamAtRank(t.GetMatch(i).GetPlaceB()));
                                im.SetStatus(Match.MatchStatus.READY);
                            }
                        }
                        break;
                    case 1:
                        Match m = _itemMatch.GetMatch();

                        if (m.GetScoreA() > m.GetScoreB())
                        {
                            if (m.GetTeamA().GetRank() > m.GetTeamB().GetRank())
                            {
                                m.GetTeamA().ChangeRank(-1);
                                m.GetTeamB().ChangeRank(1);
                            }
                        }
                        else
                        {
                            if (m.GetTeamB().GetRank() > m.GetTeamA().GetRank())
                            {
                                m.GetTeamA().ChangeRank(1);
                                m.GetTeamB().ChangeRank(-1);
                            }
                        }
                        break;
                }
                break;
        }
    }

    public void VerifyEndTournament()
    {
        for (int i = 0; i < cTournament.GetNMatches(); i++)
        {
            if (cTournament.GetMatch(i).GetStatus() != Match.MatchStatus.FINISHED)
                return;
        }

        buttonEndTournament.SetActive(true);
    }

    public void ButtonEndTournament()
    {
        cTournament.SetStatus(Tournament.TournamentStatus.FINISHED);
        textProfileStatus.text = "(Finished)";
        buttonEndTournament.SetActive(false);

        SF.SaveTournamentsData();

        ManagerUI.MUI.GoRight(animTournamentProfile);
    }
    #endregion

    #region RANKING - TEAMS
    public void ButtonSortTeams(int _criteria)
    {
        IEnumerable<Team> ordered = null;

        imageButtonTeamsRank.color = new Color(1, 1, 1, 0.9f);
        imageButtonTeamsAlpha.color = new Color(1, 1, 1, 0.9f);
        imageButtonTeamsSpirit.color = new Color(1, 1, 1, 0.9f);
        imageButtonTeamsVictory.color = new Color(1, 1, 1, 0.9f);
        imageButtonTeamsPointDif.color = new Color(1, 1, 1, 0.9f);

        switch ((SortTeamCriteria)_criteria)
        {
            case SortTeamCriteria.RANK:
                ordered = cTournament.GetTeams().OrderBy(t => t.GetRank());
                imageButtonTeamsRank.color = new Color(1, 1, 0, 0.9f);
                break;
            case SortTeamCriteria.ALPHA:
                ordered = cTournament.GetTeams().OrderBy(t => t.GetName());
                imageButtonTeamsAlpha.color = new Color(1, 1, 0, 0.9f);
                break;
            case SortTeamCriteria.SPIRIT:
                ordered = cTournament.GetTeams().OrderByDescending(t => t.GetTotalSpirit());
                imageButtonTeamsSpirit.color = new Color(1, 1, 0, 0.9f);
                break;
            case SortTeamCriteria.VICTORY:
                ordered = cTournament.GetTeams().OrderByDescending(t => t.GetVictoriesDefeats());
                imageButtonTeamsVictory.color = new Color(1, 1, 0, 0.9f);
                break;
            case SortTeamCriteria.POINT_DIF:
                ordered = cTournament.GetTeams().OrderByDescending(t => t.GetPointDif());
                imageButtonTeamsPointDif.color = new Color(1, 1, 0, 0.9f);
                break;
        }

        textSortTeams.text = SortTeamToString((SortTeamCriteria)_criteria);

        FlushRankingTeams(ordered.ToList());
    }

    private void FlushRankingTeams(List<Team> _teams)
    {
        for (int i = 0; i < parentRankingTeams.childCount; i++)
            Destroy(parentRankingTeams.GetChild(i).gameObject);

        for (int i = 0; i < _teams.Count; i++)
        {
            ItemTeam it = Instantiate(prefabItemTeam, parentRankingTeams);
            it.StartThis(_teams[i], ItemTeam.ItemTeamType.TEAM_RANKING);
            it.SetRanking(i + 1);
        }
    }
    #endregion

    #region RANKING - PLAYERS
    public void ButtonOpenSettingsRankingPlayers()
    {
        Tournament t = cTournament;

        fieldRankingPlayersPoints.text = t.GetScorePlayerPoint().ToString();
        fieldRankingPlayersAssists.text = t.GetScorePlayerAssist().ToString();
        fieldRankingPlayersDefenses.text = t.GetScorePlayerDefense().ToString();
        fieldRankingPlayersCallahans.text = t.GetScorePlayerCallahan().ToString();

        if (t.GetCheckWhosPlaying())
        {
            textRankingPlayersScoreOff.SetActive(true);
            textRankingPlayersScoreDef.SetActive(true);
            textRankingPlayersSufferOff.SetActive(true);
            textRankingPlayersSufferDef.SetActive(true);
            fieldRankingPlayersScoreOff.gameObject.SetActive(true);
            fieldRankingPlayersScoreDef.gameObject.SetActive(true);
            fieldRankingPlayersSufferOff.gameObject.SetActive(true);
            fieldRankingPlayersSufferDef.gameObject.SetActive(true);
            fieldRankingPlayersScoreOff.text = t.GetScorePlayerScoreOff().ToString();
            fieldRankingPlayersScoreDef.text = t.GetScorePlayerScoreDef().ToString();
            fieldRankingPlayersSufferOff.text = t.GetScorePlayerSufferOff().ToString();
            fieldRankingPlayersSufferDef.text = t.GetScorePlayerSufferDef().ToString();
        }
        else
        {
            textRankingPlayersScoreOff.SetActive(false);
            textRankingPlayersScoreDef.SetActive(false);
            textRankingPlayersSufferOff.SetActive(false);
            textRankingPlayersSufferDef.SetActive(false);
            fieldRankingPlayersScoreOff.gameObject.SetActive(false);
            fieldRankingPlayersScoreDef.gameObject.SetActive(false);
            fieldRankingPlayersSufferOff.gameObject.SetActive(false);
            fieldRankingPlayersSufferDef.gameObject.SetActive(false);
        }
    }

    public void ButtonSaveSettingsRankingPlayers()
    {
        cTournament.SetTournamentPlayerScoreRules(fieldRankingPlayersPoints.text == string.Empty ? 0 : int.Parse(fieldRankingPlayersPoints.text),
           fieldRankingPlayersAssists.text == string.Empty ? 0 : int.Parse(fieldRankingPlayersAssists.text),
           fieldRankingPlayersDefenses.text == string.Empty ? 0 : int.Parse(fieldRankingPlayersDefenses.text),
           fieldRankingPlayersCallahans.text == string.Empty ? 0 : int.Parse(fieldRankingPlayersCallahans.text),
           fieldRankingPlayersScoreOff.text == string.Empty ? 0 : int.Parse(fieldRankingPlayersScoreOff.text),
           fieldRankingPlayersScoreDef.text == string.Empty ? 0 : int.Parse(fieldRankingPlayersScoreDef.text),
           fieldRankingPlayersSufferOff.text == string.Empty ? 0 : int.Parse(fieldRankingPlayersSufferOff.text),
           fieldRankingPlayersSufferDef.text == string.Empty ? 0 : int.Parse(fieldRankingPlayersSufferDef.text));

        SF.SaveTournamentsData();

        ButtonSortPlayers(0);
    }

    public void ButtonSortPlayers(int _criteria)
    {
        IEnumerable<int> ordered = null;

        imageButtonPlayersRank.color = new Color(1, 1, 1, 0.9f);
        imageButtonPlayersAlpha.color = new Color(1, 1, 1, 0.9f);
        imageButtonPlayersPoint.color = new Color(1, 1, 1, 0.9f);
        imageButtonPlayersAssist.color = new Color(1, 1, 1, 0.9f);
        imageButtonPlayersDefense.color = new Color(1, 1, 1, 0.9f);

        switch ((SortPlayerCriteria)_criteria)
        {
            case SortPlayerCriteria.RANK:
                ordered = cTournament.GetPlayersIDs().OrderByDescending(p => SF.GetPlayer(p).GetCurrentTournament().GetElo());
                imageButtonPlayersRank.color = new Color(1, 1, 0, 0.9f);
                break;
            case SortPlayerCriteria.ALPHA:
                ordered = cTournament.GetPlayersIDs().OrderBy(p => SF.GetPlayer(p).GetName());
                imageButtonPlayersAlpha.color = new Color(1, 1, 0, 0.9f);
                break;
            case SortPlayerCriteria.POINT:
                ordered = cTournament.GetPlayersIDs().OrderByDescending(p => SF.GetPlayer(p).GetCurrentTournament().GetPoints());
                imageButtonPlayersPoint.color = new Color(1, 1, 0, 0.9f);
                break;
            case SortPlayerCriteria.ASSIST:
                ordered = cTournament.GetPlayersIDs().OrderByDescending(p => SF.GetPlayer(p).GetCurrentTournament().GetAssists());
                imageButtonPlayersAssist.color = new Color(1, 1, 0, 0.9f);
                break;
            case SortPlayerCriteria.DEFENSE:
                ordered = cTournament.GetPlayersIDs().OrderByDescending(p => SF.GetPlayer(p).GetCurrentTournament().GetDefenses());
                imageButtonPlayersDefense.color = new Color(1, 1, 0, 0.9f);
                break;
        }

        textSortPlayers.text = SortPlayerToString((SortPlayerCriteria)_criteria);

        FlushRankingPlayers(ordered.ToList(), ((SortPlayerCriteria)_criteria));
    }

    private void FlushRankingPlayers(List<int> _playersIDs, SortPlayerCriteria _criteria)
    {
        for (int i = 0; i < parentRankingPlayers.childCount; i++)
            Destroy(parentRankingPlayers.GetChild(i).gameObject);

        for (int i = 0; i < _playersIDs.Count; i++)
        {
            ItemPlayer ip = Instantiate(prefabItemPlayer, parentRankingPlayers);
            switch (_criteria)
            {
                case SortPlayerCriteria.RANK:
                    cTournament.UpdatePlayerElo(SF.GetPlayer(_playersIDs[i]));
                    ip.StartThis(_playersIDs[i], ItemPlayer.ItemPlayerType.PLAYER_RANKING, false, SF.GetPlayer(_playersIDs[i]).GetCurrentTournament().GetElo());
                    break;
                case SortPlayerCriteria.ALPHA:
                    ip.StartThis(_playersIDs[i], ItemPlayer.ItemPlayerType.PLAYER_RANKING, false, float.MinValue);
                    break;
                case SortPlayerCriteria.ASSIST:
                    ip.StartThis(_playersIDs[i], ItemPlayer.ItemPlayerType.PLAYER_RANKING, false, SF.GetPlayer(_playersIDs[i]).GetCurrentTournament().GetAssists());
                    break;
                case SortPlayerCriteria.POINT:
                    ip.StartThis(_playersIDs[i], ItemPlayer.ItemPlayerType.PLAYER_RANKING, false, SF.GetPlayer(_playersIDs[i]).GetCurrentTournament().GetPoints());
                    break;
                case SortPlayerCriteria.DEFENSE:
                    ip.StartThis(_playersIDs[i], ItemPlayer.ItemPlayerType.PLAYER_RANKING, false, SF.GetPlayer(_playersIDs[i]).GetCurrentTournament().GetDefenses());
                    break;
            }
        }
    }
    #endregion

    #region OTHER
    public string GenderToString(GenderFormat _format)
    {
        switch (_format)
        {
            case GenderFormat.MIXED:
                return "Mixed";
            case GenderFormat.MEN:
                return "Men";
            case GenderFormat.WOMEN:
                return "Women";
            default:
                return null;
        }
    }

    public string BracketFormat1ToString(BracketFormatFirst _format)
    {
        switch (_format)
        {
            case BracketFormatFirst.ROUND_ROBIN:
                return "Round Robin";
            case BracketFormatFirst.SINGLE_ELIMINATION:
                return "Single Elimination";
            case BracketFormatFirst.DOUBLE_ELIMINATION:
                return "Double Elimination";
            default:
                Debug.LogError("Format not found");
                return null;
        }
    }

    public string BracketFormat2ToString(BracketFormatSecond _format)
    {
        switch (_format)
        {
            case BracketFormatSecond.NOTHING:
                return "-";
            case BracketFormatSecond.ROUND_ROBIN:
                return "Round Robin";
            case BracketFormatSecond.SINGLE_ELIMINATION:
                return "Single Elimination";
            case BracketFormatSecond.DOUBLE_ELIMINATION:
                return "Double Elimination";
            case BracketFormatSecond.MANY_FINALS:
                return "Many Finals";
            default:
                Debug.LogError("Format not found");
                return null;
        }
    }

    private string SortTeamToString(SortTeamCriteria _criteria)
    {
        switch (_criteria)
        {
            case SortTeamCriteria.RANK:
                return "Rank";
            case SortTeamCriteria.ALPHA:
                return "A-Z";
            case SortTeamCriteria.SPIRIT:
                return "Spirit";
            case SortTeamCriteria.VICTORY:
                return "Victories";
            case SortTeamCriteria.POINT_DIF:
                return "Point Difference";
            default:
                return null;
        }
    }

    private string SortPlayerToString(SortPlayerCriteria _criteria)
    {
        switch (_criteria)
        {
            case SortPlayerCriteria.RANK:
                return "Rank";
            case SortPlayerCriteria.ALPHA:
                return "A-Z";
            case SortPlayerCriteria.POINT:
                return "Points";
            case SortPlayerCriteria.ASSIST:
                return "Assists";
            case SortPlayerCriteria.DEFENSE:
                return "Defenses";
            default:
                return null;
        }
    }
    #endregion
}
