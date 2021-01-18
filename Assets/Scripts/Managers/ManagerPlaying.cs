using System;
using System.Collections.Generic;
using UnityEngine; //TODO Create temporary file to store match data and be able to continue after closing app
using UnityEngine.UI;
using TMPro;

public class ManagerPlaying : MonoBehaviour
{
    public static ManagerPlaying MP;

    public GameObject manager;
    private ManagerUI MUI;
    private SaveFile SF;

    public Animator animTournamentMatches;

    private Tournament cTournament;
    private ItemMatch itemMatch;
    private Match match;

    //MAIN
    public TextMeshProUGUI textTimer;
    public TextMeshProUGUI textTeamA;
    public TextMeshProUGUI textTeamB;
    public Transform parentTeamA;
    public Transform parentTeamB;
    public ItemPlayer prefabItemPlayer;
    private Team teamA;
    private Team teamB;
    private List<Player> playingTeamA;
    private List<Player> playingTeamB;
    public GameObject buttonMenu;
    public GameObject buttonHalftime;
    public GameObject buttonStart;
    public GameObject buttonOffenseA;
    public GameObject buttonOffenseB;
    public Image imageOffenseA;
    public Image imageOffenseB;
    public Sprite spriteChecked;
    public Sprite spriteUnchecked;
    public TextMeshProUGUI textScoreboard;
    public GameObject discA;
    public GameObject discB;
    private int scoreA;
    private int scoreB;
    private DateTime gameStartTime;
    private TimeSpan gameTime;
    private bool running;
    private bool showingImages;

    //WARNINGS
    public Animator animWarnings;
    public Text textWarning;
    private bool showedHalfTime;
    private bool showedFullTime;

    //EVENT
    public Button buttonScoreA;
    public Button buttonScoreB;
    public Button buttonAssist;
    public Button buttonPoint;
    public Button buttonDefense;
    public Button buttonCallahan;
    public GameObject buttonConfirmEvent;
    private EventType eventType;
    private ItemPlayer playerAssist;
    private ItemPlayer playerPoint;
    private ItemPlayer playerDefense;
    private ItemPlayer playerCallahan;

    //Undo
    public Animator animPanelUndo;
    public TextMeshProUGUI textUndoTitle;
    public Image imageUndoAssist;
    public Image imageUndoPoint;
    public GameObject imageUndoArrow;
    public Image imageUndoDefense;
    public Sprite spriteNothing;

    private bool firstOffenseA;
    private bool currentOffenseA;
    private bool secondHalf;

    private enum EventType
    {
        NOTHING,
        ASSIST,
        POINT,
        DEFENSE,
        CALLAHAN
    }

    private void Awake()
    {
        MP = this;

        MUI = manager.GetComponent<ManagerUI>();
        SF = manager.GetComponent<SaveFile>();
    }

    private void Start()
    {
        parentTeamA.GetComponent<GridLayoutGroup>().cellSize = new Vector2(MUI.sizePlayerGrid * Screen.height, MUI.sizePlayerGrid * Screen.height);
        parentTeamB.GetComponent<GridLayoutGroup>().cellSize = new Vector2(MUI.sizePlayerGrid * Screen.height, MUI.sizePlayerGrid * Screen.height);
    }

    public void StartThis(Tournament _cTournament, ItemMatch _itemMatch)
    {
        cTournament = _cTournament;
        itemMatch = _itemMatch;
        match = _itemMatch.GetMatch();

        textTeamA.text = match.GetTeamA().GetName();
        textTeamB.text = match.GetTeamB().GetName();
        textTeamA.color = ManagerColors.MC.teamColors[match.GetTeamA().GetColorIndex()];
        textTeamB.color = ManagerColors.MC.teamColors[match.GetTeamB().GetColorIndex()];
        buttonMenu.SetActive(false);
        buttonStart.SetActive(true);
        buttonOffenseA.SetActive(true);
        buttonOffenseB.SetActive(true);
        ButtonOffenseA();
        discA.SetActive(false);
        discB.SetActive(false);
        running = false;
        showingImages = true;
        scoreA = 0;
        scoreB = 0;
        textTimer.text = "00:00:00";
        showedHalfTime = cTournament.GetHalfTime() < 0;
        showedFullTime = cTournament.GetFullTime() < 0;
        buttonHalftime.SetActive(false);
        secondHalf = false;
        textScoreboard.text = "0 - 0";
        buttonScoreA.gameObject.SetActive(false);
        buttonScoreB.gameObject.SetActive(false);
        buttonAssist.gameObject.SetActive(false);
        buttonPoint.gameObject.SetActive(false);
        buttonDefense.gameObject.SetActive(false);
        buttonCallahan.gameObject.SetActive(false);

        for (int i = 0; i < parentTeamA.childCount; i++)
            Destroy(parentTeamA.GetChild(i).gameObject);

        for (int i = 0; i < parentTeamB.childCount; i++)
            Destroy(parentTeamB.GetChild(i).gameObject);

        teamA = match.GetTeamA();
        teamB = match.GetTeamB();

        playingTeamA = new List<Player>();
        playingTeamB = new List<Player>();

        for (int i = 0; i < teamA.GetNPlayers(); i++)
        {
            ItemPlayer ip = Instantiate(prefabItemPlayer, parentTeamA);
            ip.StartThis(teamA.GetPlayerID(i), ItemPlayer.ItemPlayerType.PLAYER_PLAYING, cTournament.GetCheckWhosPlaying(), float.MinValue);
        }

        for (int i = 0; i < teamB.GetNPlayers(); i++)
        {
            ItemPlayer ip = Instantiate(prefabItemPlayer, parentTeamB);
            ip.StartThis(teamB.GetPlayerID(i), ItemPlayer.ItemPlayerType.PLAYER_PLAYING, cTournament.GetCheckWhosPlaying(), float.MinValue);
        }
    }

    public void ContinueThis(ItemMatch _itemMatch)
    {
        itemMatch = _itemMatch;
        match = _itemMatch.GetMatch();

        textTeamA.text = match.GetTeamA().GetName();
        textTeamB.text = match.GetTeamB().GetName();
        textTeamA.color = ManagerColors.MC.teamColors[match.GetTeamA().GetColorIndex()];
        textTeamB.color = ManagerColors.MC.teamColors[match.GetTeamB().GetColorIndex()];
        buttonMenu.SetActive(match.GetNEvents() > 1);
        buttonStart.SetActive(false);
        discA.SetActive(currentOffenseA);
        discB.SetActive(!currentOffenseA);
        running = true;
        showingImages = true;
        scoreA = match.GetScoreA();
        scoreB = match.GetScoreB();
        gameStartTime = match.GetDate();
        gameTime = DateTime.Now - gameStartTime;
        textTimer.text = gameTime.Hours.ToString("00") + ":" + gameTime.Minutes.ToString("00") + ":" + gameTime.Seconds.ToString("00");
        showedHalfTime = itemMatch.GetShowedHalfTime();
        showedFullTime = itemMatch.GetShowedFullTime();
        secondHalf = itemMatch.GetSecondHalf();
        buttonHalftime.SetActive(!secondHalf && showedHalfTime);
        textScoreboard.text = scoreA + " - " + scoreB;
        buttonScoreA.gameObject.SetActive(!cTournament.GetCheckWhoEvents());
        buttonScoreB.gameObject.SetActive(!cTournament.GetCheckWhoEvents());
        buttonAssist.gameObject.SetActive(cTournament.GetCheckWhoEvents());
        buttonPoint.gameObject.SetActive(cTournament.GetCheckWhoEvents());
        buttonDefense.gameObject.SetActive(cTournament.GetCheckWhoEvents());
        buttonCallahan.gameObject.SetActive(cTournament.GetCheckWhoEvents());

        for (int i = 0; i < parentTeamA.childCount; i++)
            Destroy(parentTeamA.GetChild(i).gameObject);

        for (int i = 0; i < parentTeamB.childCount; i++)
            Destroy(parentTeamB.GetChild(i).gameObject);

        teamA = match.GetTeamA();
        teamB = match.GetTeamB();

        playingTeamA.Clear();
        playingTeamB.Clear();

        for (int i = 0; i < teamA.GetNPlayers(); i++)
        {
            ItemPlayer ip = Instantiate(prefabItemPlayer, parentTeamA);
            ip.StartThis(teamA.GetPlayerID(i), ItemPlayer.ItemPlayerType.PLAYER_PLAYING, cTournament.GetCheckWhosPlaying(), float.MinValue);
        }

        for (int i = 0; i < teamB.GetNPlayers(); i++)
        {
            ItemPlayer ip = Instantiate(prefabItemPlayer, parentTeamB);
            ip.StartThis(teamB.GetPlayerID(i), ItemPlayer.ItemPlayerType.PLAYER_PLAYING, cTournament.GetCheckWhosPlaying(), float.MinValue);
        }
    }

    private void Update()
    {
        if (!running)
            return;

        gameTime = DateTime.Now - gameStartTime;
        textTimer.text = gameTime.Hours.ToString("00") + ":" + gameTime.Minutes.ToString("00") + ":" + gameTime.Seconds.ToString("00");

        if (!showedHalfTime && gameTime.Minutes == cTournament.GetHalfTime())
        {
            showedHalfTime = true;
            textWarning.text = "Half Time!";
            animWarnings.gameObject.SetActive(true);
            animWarnings.Play("Appear");
            buttonHalftime.SetActive(true);
        }

        if (!showedFullTime && gameTime.Minutes == cTournament.GetFullTime())
        {
            showedFullTime = true;
            textWarning.text = "Time is Over!";
            animWarnings.gameObject.SetActive(true);
            animWarnings.Play("Appear");
        }
    }

    public void ButtonPlayerInfo()
    {
        showingImages = !showingImages;

        if (showingImages)
        {
            for (int i = 0; i < parentTeamA.childCount; i++)
                parentTeamA.GetChild(i).GetComponent<ItemPlayer>().ShowImage();

            for (int i = 0; i < parentTeamB.childCount; i++)
                parentTeamB.GetChild(i).GetComponent<ItemPlayer>().ShowImage();
        }
        else
        {
            for (int i = 0; i < parentTeamA.childCount; i++)
                parentTeamA.GetChild(i).GetComponent<ItemPlayer>().ShowText();

            for (int i = 0; i < parentTeamB.childCount; i++)
                parentTeamB.GetChild(i).GetComponent<ItemPlayer>().ShowText();
        }
    }

    public void ButtonOffenseA()
    {
        imageOffenseA.sprite = spriteChecked;
        imageOffenseB.sprite = spriteUnchecked;
        firstOffenseA = true;
        currentOffenseA = true;
    }

    public void ButtonOffenseB()
    {
        imageOffenseA.sprite = spriteUnchecked;
        imageOffenseB.sprite = spriteChecked;
        firstOffenseA = false;
        currentOffenseA = false;
    }

    public void ButtonStartMatch()
    {
        gameStartTime = DateTime.Now;
        buttonStart.SetActive(false);
        buttonOffenseA.SetActive(false);
        buttonOffenseB.SetActive(false);
        discA.SetActive(currentOffenseA);
        discB.SetActive(!currentOffenseA);
        match.SetFirstOffense(firstOffenseA);
        match.SetDate(gameStartTime);
        itemMatch.SetStatus(Match.MatchStatus.RUNNING);
        running = true;
        buttonScoreA.gameObject.SetActive(!cTournament.GetCheckWhoEvents());
        buttonScoreB.gameObject.SetActive(!cTournament.GetCheckWhoEvents());
        buttonAssist.gameObject.SetActive(cTournament.GetCheckWhoEvents());
        buttonPoint.gameObject.SetActive(cTournament.GetCheckWhoEvents());
        buttonDefense.gameObject.SetActive(cTournament.GetCheckWhoEvents());
        buttonCallahan.gameObject.SetActive(cTournament.GetCheckWhoEvents());
    }

    public void ButtonScoreA()
    {
        int colorIndex = -1;

        if (cTournament.GetCheckWhosPlaying())
        {
            if (currentOffenseA)
            {
                foreach (Player p in playingTeamA)
                    p.GetCurrentTournament().ScoreOffChange(1);

                foreach (Player p in playingTeamB)
                    p.GetCurrentTournament().SufferDefChange(1);
            }
            else
            {
                foreach (Player p in playingTeamA)
                    p.GetCurrentTournament().ScoreDefChange(1);

                foreach (Player p in playingTeamB)
                    p.GetCurrentTournament().SufferOffChange(1);
            }
        }

        scoreA++;
        match.ScoredA();
        colorIndex = teamA.GetColorIndex();

        textScoreboard.text = scoreA + " - " + scoreB;
        match.AddEvent(new MatchEvent(MatchEvent.MatchEventType.SCORE, gameTime, currentOffenseA, true, 0, scoreA, scoreB, playingTeamA, playingTeamB, null, null, null, colorIndex));

        currentOffenseA = false;
        discA.SetActive(false);
        discB.SetActive(true);

        itemMatch.Refresh();
        SF.SavePlayersData();
    }

    public void ButtonScoreB()
    {
        int colorIndex = -1;

        if (cTournament.GetCheckWhosPlaying())
        {
            if (currentOffenseA)
            {
                foreach (Player p in playingTeamA)
                    p.GetCurrentTournament().SufferOffChange(1);

                foreach (Player p in playingTeamB)
                    p.GetCurrentTournament().ScoreDefChange(1);
            }
            else
            {
                foreach (Player p in playingTeamA)
                    p.GetCurrentTournament().SufferDefChange(1);

                foreach (Player p in playingTeamB)
                    p.GetCurrentTournament().ScoreOffChange(1);
            }
        }

        scoreB++;
        match.ScoredB();
        colorIndex = teamB.GetColorIndex();

        textScoreboard.text = scoreA + " - " + scoreB;
        match.AddEvent(new MatchEvent(MatchEvent.MatchEventType.SCORE, gameTime, currentOffenseA, false, 0, scoreA, scoreB, playingTeamA, playingTeamB, null, null, null, colorIndex));

        currentOffenseA = true;
        discA.SetActive(true);
        discB.SetActive(false);

        itemMatch.Refresh();
        SF.SavePlayersData();
    }

    public void ButtonAssist()
    {
        buttonPoint.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
        buttonDefense.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
        buttonCallahan.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);

        if (eventType == EventType.ASSIST)
        {
            if (playerAssist != null)
            {
                playerAssist.SetAssist(false);
                playerAssist = null;
            }

            if (playerPoint != null)
            {
                playerPoint.SetPoint(false);
                playerPoint = null;
            }

            buttonAssist.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
            eventType = EventType.NOTHING;
            buttonDefense.interactable = true;
            buttonCallahan.interactable = true;
            buttonConfirmEvent.SetActive(false);
        }
        else
        {
            buttonAssist.GetComponent<Image>().color = new Color(1, 1, 0, 0.9f);
            eventType = EventType.ASSIST;
        }
    }

    public void ButtonPoint()
    {
        buttonAssist.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
        buttonDefense.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
        buttonCallahan.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);

        if (eventType == EventType.POINT)
        {
            if (playerAssist != null)
            {
                playerAssist.SetAssist(false);
                playerAssist = null;
            }

            if (playerPoint != null)
            {
                playerPoint.SetPoint(false);
                playerPoint = null;
            }

            buttonPoint.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
            eventType = EventType.NOTHING;
            buttonDefense.interactable = true;
            buttonCallahan.interactable = true;
            buttonConfirmEvent.SetActive(false);
        }
        else
        {
            buttonPoint.GetComponent<Image>().color = new Color(1, 1, 0, 0.9f);
            eventType = EventType.POINT;
        }
    }

    public void ButtonDefense()
    {
        buttonAssist.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
        buttonPoint.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
        buttonCallahan.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);

        if (eventType == EventType.DEFENSE)
        {
            if (playerDefense != null)
            {
                playerDefense.SetDefense(false);
                playerDefense = null;
            }

            buttonDefense.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
            eventType = EventType.NOTHING;
            buttonAssist.interactable = true;
            buttonPoint.interactable = true;
            buttonCallahan.interactable = true;
            buttonConfirmEvent.SetActive(false);
        }
        else
        {
            buttonDefense.GetComponent<Image>().color = new Color(1, 1, 0, 0.9f);
            eventType = EventType.DEFENSE;
        }
    }

    public void ButtonCallahan()
    {
        buttonAssist.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
        buttonPoint.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
        buttonDefense.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);

        if (eventType == EventType.CALLAHAN)
        {
            if (playerCallahan != null)
            {
                playerCallahan.SetCallahan(false);
                playerCallahan = null;
            }

            buttonCallahan.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
            eventType = EventType.NOTHING;
            buttonAssist.interactable = true;
            buttonPoint.interactable = true;
            buttonDefense.interactable = true;
            buttonConfirmEvent.SetActive(false);
        }
        else
        {
            buttonCallahan.GetComponent<Image>().color = new Color(1, 1, 0, 0.9f);
            eventType = EventType.CALLAHAN;
        }
    }

    public void ClickedPlayer(ItemPlayer _itemPlayer)
    {
        switch (eventType)
        {
            case EventType.NOTHING:
                Player p = _itemPlayer.GetPlayer();
                if (cTournament.GetCheckWhosPlaying())
                {
                    if (teamA.HasPlayer(_itemPlayer.GetPlayerID()))
                    {
                        if (playingTeamA.Contains(p))
                        {
                            _itemPlayer.SetSelected(false);
                            playingTeamA.Remove(p);
                        }
                        else
                        {
                            _itemPlayer.SetSelected(true);
                            playingTeamA.Add(p);
                        }
                    }
                    else
                    {
                        if (playingTeamB.Contains(p))
                        {
                            _itemPlayer.SetSelected(false);
                            playingTeamB.Remove(p);
                        }
                        else
                        {
                            _itemPlayer.SetSelected(true);
                            playingTeamB.Add(p);
                        }
                    }
                }
                return;
            case EventType.ASSIST:
                if (cTournament.GetCheckWhosPlaying() && !_itemPlayer.GetSelected())
                    return;

                //Check if assist and point are on same team
                if (playerPoint != null && (teamA.HasPlayer(playerPoint.GetPlayerID()) && !teamA.HasPlayer(_itemPlayer.GetPlayerID()) || (teamB.HasPlayer(playerPoint.GetPlayerID()) && !teamB.HasPlayer(_itemPlayer.GetPlayerID()))))
                    break;

                if (playerAssist == _itemPlayer)
                {
                    playerAssist.SetAssist(false);
                    playerAssist = null;
                }
                else
                {
                    if (playerAssist != null) playerAssist.SetAssist(false);
                    playerAssist = _itemPlayer;
                    playerAssist.SetAssist(true);
                }
                break;
            case EventType.POINT:

                if (cTournament.GetCheckWhosPlaying() && !_itemPlayer.GetSelected())
                    return;

                //Check if point and assist are on same team
                if (playerAssist != null && (teamA.HasPlayer(playerAssist.GetPlayerID()) && !teamA.HasPlayer(_itemPlayer.GetPlayerID()) || (teamB.HasPlayer(playerAssist.GetPlayerID()) && !teamB.HasPlayer(_itemPlayer.GetPlayerID()))))
                    break;

                if (playerPoint == _itemPlayer)
                {
                    playerPoint.SetPoint(false);
                    playerPoint = null;
                }
                else
                {
                    if (playerPoint != null) playerPoint.SetPoint(false);
                    playerPoint = _itemPlayer;
                    playerPoint.SetPoint(true);
                }
                break;
            case EventType.DEFENSE:

                if (cTournament.GetCheckWhosPlaying() && !_itemPlayer.GetSelected())
                    return;

                if (playerDefense == _itemPlayer)
                {
                    playerDefense.SetDefense(false);
                    playerDefense = null;
                }
                else
                {
                    if (playerDefense != null) playerDefense.SetDefense(false);
                    playerDefense = _itemPlayer;
                    playerDefense.SetDefense(true);
                }
                break;
            case EventType.CALLAHAN:

                if (cTournament.GetCheckWhosPlaying() && !_itemPlayer.GetSelected())
                    return;

                if (playerCallahan == _itemPlayer)
                {
                    playerCallahan.SetCallahan(false);
                    playerCallahan = null;
                }
                else
                {
                    if (playerCallahan != null) playerCallahan.SetCallahan(false);
                    playerCallahan = _itemPlayer;
                    playerCallahan.SetCallahan(true);
                }
                break;
        }

        buttonAssist.interactable = eventType == EventType.POINT || eventType == EventType.ASSIST;
        buttonPoint.interactable = eventType == EventType.POINT || eventType == EventType.ASSIST;
        buttonDefense.interactable = eventType == EventType.DEFENSE;
        buttonCallahan.interactable = eventType == EventType.CALLAHAN;

        buttonConfirmEvent.SetActive(playerPoint != null && playerAssist != null || playerDefense != null || playerCallahan != null);
    }

    public void ButtonConfirmEvent()
    {
        bool eventTeamA = true;
        int colorIndex = -1;

        switch (eventType)
        {
            case EventType.ASSIST:
            case EventType.POINT:

                playerPoint.GetPlayer().GetCurrentTournament().ChangePoint(1);
                playerAssist.GetPlayer().GetCurrentTournament().ChangeAssist(1);

                if (teamA.HasPlayer(playerPoint.GetPlayerID()))
                {
                    if (cTournament.GetCheckWhosPlaying())
                    {
                        if (currentOffenseA)
                        {
                            foreach (Player p in playingTeamA)
                                p.GetCurrentTournament().ScoreOffChange(1);

                            foreach (Player p in playingTeamB)
                                p.GetCurrentTournament().SufferDefChange(1);
                        }
                        else
                        {
                            foreach (Player p in playingTeamA)
                                p.GetCurrentTournament().ScoreDefChange(1);

                            foreach (Player p in playingTeamB)
                                p.GetCurrentTournament().SufferOffChange(1);
                        }
                    }

                    eventTeamA = true;
                    scoreA++;
                    match.ScoredA();
                    colorIndex = teamA.GetColorIndex();
                }
                else
                {
                    if (cTournament.GetCheckWhosPlaying())
                    {
                        if (currentOffenseA)
                        {
                            foreach (Player p in playingTeamA)
                                p.GetCurrentTournament().SufferOffChange(1);

                            foreach (Player p in playingTeamB)
                                p.GetCurrentTournament().ScoreDefChange(1);
                        }
                        else
                        {
                            foreach (Player p in playingTeamA)
                                p.GetCurrentTournament().SufferDefChange(1);

                            foreach (Player p in playingTeamB)
                                p.GetCurrentTournament().ScoreOffChange(1);
                        }
                    }

                    eventTeamA = false;
                    scoreB++;
                    match.ScoredB();
                    colorIndex = teamB.GetColorIndex();
                }

                textScoreboard.text = scoreA + " - " + scoreB;
                match.AddEvent(new MatchEvent(MatchEvent.MatchEventType.SCORE, gameTime, currentOffenseA, eventTeamA, 0, scoreA, scoreB, playingTeamA, playingTeamB, playerPoint.GetPlayer(), playerAssist.GetPlayer(), null, colorIndex));

                currentOffenseA = !eventTeamA;
                discA.SetActive(currentOffenseA);
                discB.SetActive(!currentOffenseA);

                itemMatch.Refresh();
                SF.SavePlayersData();

                playerAssist.SetAssist(false);
                playerPoint.SetPoint(false);
                break;
            case EventType.DEFENSE:

                if (teamA.HasPlayer(playerDefense.GetPlayerID()))
                {
                    eventTeamA = true;
                    colorIndex = teamA.GetColorIndex();
                }
                else
                {
                    eventTeamA = false;
                    colorIndex = teamB.GetColorIndex();
                }

                playerDefense.GetPlayer().GetCurrentTournament().ChangeDefense(1);

                match.AddEvent(new MatchEvent(MatchEvent.MatchEventType.TURNOVER, gameTime, currentOffenseA, eventTeamA, 0, scoreA, scoreB, playingTeamA, playingTeamB, null, null, playerDefense.GetPlayer(), colorIndex));

                SF.SavePlayersData();

                playerDefense.SetDefense(false);
                break;
            case EventType.CALLAHAN:

                playerCallahan.GetPlayer().GetCurrentTournament().ChangeCallahan(1);

                if (teamA.HasPlayer(playerCallahan.GetPlayerID()))
                {
                    if (cTournament.GetCheckWhosPlaying())
                    {
                        if (currentOffenseA)
                        {
                            foreach (Player p in playingTeamA)
                                p.GetCurrentTournament().ScoreOffChange(1);

                            foreach (Player p in playingTeamB)
                                p.GetCurrentTournament().SufferDefChange(1);
                        }
                        else
                        {
                            foreach (Player p in playingTeamA)
                                p.GetCurrentTournament().ScoreDefChange(1);

                            foreach (Player p in playingTeamB)
                                p.GetCurrentTournament().SufferOffChange(1);
                        }
                    }

                    scoreA++;
                    match.ScoredA();
                    colorIndex = teamA.GetColorIndex();
                }
                else
                {
                    if (cTournament.GetCheckWhosPlaying())
                    {
                        if (currentOffenseA)
                        {
                            foreach (Player p in playingTeamA)
                                p.GetCurrentTournament().SufferOffChange(1);

                            foreach (Player p in playingTeamB)
                                p.GetCurrentTournament().ScoreDefChange(1);
                        }
                        else
                        {
                            foreach (Player p in playingTeamA)
                                p.GetCurrentTournament().SufferDefChange(1);

                            foreach (Player p in playingTeamB)
                                p.GetCurrentTournament().ScoreOffChange(1);
                        }
                    }

                    eventTeamA = false;
                    scoreB++;
                    match.ScoredB();
                    colorIndex = teamB.GetColorIndex();
                }

                textScoreboard.text = scoreA + " - " + scoreB;
                match.AddEvent(new MatchEvent(MatchEvent.MatchEventType.CALLAHAN, gameTime, currentOffenseA, eventTeamA, 0, scoreA, scoreB, playingTeamA, playingTeamB, playerCallahan.GetPlayer(), null, playerCallahan.GetPlayer(), colorIndex));

                currentOffenseA = !eventTeamA;
                discA.SetActive(currentOffenseA);
                discB.SetActive(!currentOffenseA);

                itemMatch.Refresh();
                SF.SavePlayersData();

                playerCallahan.SetCallahan(false);
                break;
        }

        //DEBUG
        string textTeam = eventTeamA ? teamA.GetName() : teamB.GetName();
        string textEvent = string.Empty;
        if (eventType == EventType.POINT || eventType == EventType.ASSIST) textEvent = "scored";
        if (eventType == EventType.DEFENSE) textEvent = "defended";
        if (eventType == EventType.CALLAHAN) textEvent = "callahan";
        string startingOn = string.Empty;
        if (eventTeamA && match.GetLastOffenseA()) startingOn = "offense";
        if (eventTeamA && !match.GetLastOffenseA()) startingOn = "defense";
        if (!eventTeamA && match.GetLastOffenseA()) startingOn = "defense";
        if (!eventTeamA && !match.GetLastOffenseA()) startingOn = "offense";

        Debug.Log(textTeam + " " + textEvent + " starting on " + startingOn);
        //- - -

        eventType = EventType.NOTHING;
        buttonConfirmEvent.SetActive(false);
        playerAssist = null;
        playerPoint = null;
        playerDefense = null;
        playerCallahan = null;
        buttonAssist.interactable = true;
        buttonPoint.interactable = true;
        buttonDefense.interactable = true;
        buttonCallahan.interactable = true;
        buttonAssist.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
        buttonPoint.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
        buttonDefense.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
        buttonCallahan.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);

        buttonMenu.SetActive(true);
        textTimer.gameObject.SetActive(true);
        textScoreboard.gameObject.SetActive(true);
    }

    public void ButtonUndo()
    {
        MatchEvent me = match.GetLastEvent();

        switch (me.type)
        {
            case MatchEvent.MatchEventType.SCORE:
                textUndoTitle.text = "Undo this point?";
                imageUndoAssist.sprite = me.whoAssist == null ? spriteNothing : ManagerPictures.MP.GetPicture(me.whoAssist.GetPictureIndex());
                imageUndoPoint.sprite = me.whoPoint == null ? spriteNothing : ManagerPictures.MP.GetPicture(me.whoPoint.GetPictureIndex());
                imageUndoDefense.sprite = spriteNothing;
                imageUndoArrow.SetActive(true);
                break;
            case MatchEvent.MatchEventType.TURNOVER:
                textUndoTitle.text = "Undo this defense?";
                imageUndoAssist.sprite = spriteNothing;
                imageUndoPoint.sprite = spriteNothing;
                imageUndoDefense.sprite = me.whoDefend == null ? spriteNothing : ManagerPictures.MP.GetPicture(me.whoDefend.GetPictureIndex());
                imageUndoArrow.SetActive(false);
                break;
            case MatchEvent.MatchEventType.CALLAHAN:
                textUndoTitle.text = "Undo this callahan?";
                imageUndoAssist.sprite = spriteNothing;
                imageUndoPoint.sprite = spriteNothing;
                imageUndoDefense.sprite = me.whoDefend == null ? spriteNothing : ManagerPictures.MP.GetPicture(me.whoDefend.GetPictureIndex());
                imageUndoArrow.SetActive(false);
                break;
        }

        MUI.Appear(animPanelUndo);
    }

    public void ButtonConfirmUndo()
    {
        MatchEvent me = match.GetLastEvent();

        if (me.whoPoint != null) me.whoPoint.GetCurrentTournament().ChangePoint(-1);
        if (me.whoAssist != null) me.whoAssist.GetCurrentTournament().ChangeAssist(-1);
        if (me.whoDefend != null) me.whoDefend.GetCurrentTournament().ChangeDefense(-1);

        if (me.type == MatchEvent.MatchEventType.SCORE || me.type == MatchEvent.MatchEventType.CALLAHAN)
        {
            if (me.eventA)
            {
                scoreA--;
                match.RevertScoreA();

                if (cTournament.GetCheckWhosPlaying())
                {
                    if (me.startedOffenceA)
                    {
                        foreach (Player p in playingTeamA)
                            p.GetCurrentTournament().SufferOffChange(-1);

                        foreach (Player p in playingTeamB)
                            p.GetCurrentTournament().ScoreDefChange(-1);
                    }
                    else
                    {
                        foreach (Player p in playingTeamA)
                            p.GetCurrentTournament().SufferDefChange(-1);

                        foreach (Player p in playingTeamB)
                            p.GetCurrentTournament().ScoreOffChange(-1);
                    }
                }
            }
            else
            {
                scoreB--;
                match.RevertScoreB();

                if (cTournament.GetCheckWhosPlaying())
                {
                    if (me.startedOffenceA)
                    {
                        foreach (Player p in playingTeamA)
                            p.GetCurrentTournament().ScoreOffChange(-1);

                        foreach (Player p in playingTeamB)
                            p.GetCurrentTournament().SufferDefChange(-1);
                    }
                    else
                    {
                        foreach (Player p in playingTeamA)
                            p.GetCurrentTournament().ScoreDefChange(-1);

                        foreach (Player p in playingTeamB)
                            p.GetCurrentTournament().SufferOffChange(-1);
                    }
                }
            }

            textScoreboard.text = scoreA + " - " + scoreB;
            itemMatch.Refresh();
        }

        match.RemoveLastEvent();
        currentOffenseA = match.GetLastOffenseA();
        discA.SetActive(currentOffenseA);
        discB.SetActive(!currentOffenseA);

        if (match.GetNEvents() == 1)
            buttonMenu.SetActive(false);

        MUI.Disappear(animPanelUndo);
    }

    public void ButtonBackFromMatch()
    {
        itemMatch.SetShowedHalfTime(showedHalfTime);
        itemMatch.SetShowedFullTime(showedFullTime);

        ManagerTournaments.MT.ButtonBackFromMatch();
    }

    public void ButtonHalfTime()
    {
        if (firstOffenseA)
        {
            textWarning.text = "Offense to " + teamB.GetName();
            animWarnings.gameObject.SetActive(true);
            animWarnings.Play("Appear");
            currentOffenseA = false;
        }
        else
        {
            textWarning.text = "Offense to " + teamA.GetName();
            animWarnings.gameObject.SetActive(true);
            animWarnings.Play("Appear");
            currentOffenseA = true;
        }

        secondHalf = true;
        buttonHalftime.SetActive(false);
    }

    public void ButtonEndMatch()
    {
        if (scoreA == 0 && scoreB == 0)
            return;

        match.EndMatch();

        ManagerTournaments.MT.ButtonEndMatch(itemMatch);

        MUI.GoRight(animTournamentMatches);
    }
}
