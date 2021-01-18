using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tournament
{
    public int ID;
    private string name;
    private DateTime date;
    private ManagerTournaments.GenderFormat genderFormat;
    private bool checkWhoEvents;
    private bool checkWhosPlaying;
    private ManagerTournaments.BracketFormatFirst bracketFormatFirst;
    private ManagerTournaments.BracketFormatSecond bracketFormatSecond;
    private int groupSize;
    private int nQualified;
    private int fullTime;
    private int halfTime;
    private int timeBetweenMatches;
    private int nFields;
    private TournamentStatus status;
    private int phase;

    private List<int> playersIDs;
    private List<Team> teams;
    private List<Match> matches;

    private int scorePlayerPoint;
    private int scorePlayerAssist;
    private int scorePlayerDefense;
    private int scorePlayerCallahan;
    private int scorePlayerScoreOff;
    private int scorePlayerScoreDef;
    private int scorePlayerSufferOff;
    private int scorePlayerSufferDef;

    public enum TournamentStatus
    {
        REGISTRATIONS,
        READY,
        RUNNING,
        FINISHED
    }

    public Tournament(string _name, DateTime _date, bool _checkWhoEvents, bool _checkWhosPlaying, ManagerTournaments.GenderFormat _genderFormat)
    {
        name = _name;
        date = _date;
        checkWhoEvents = _checkWhoEvents;
        checkWhosPlaying = _checkWhosPlaying;
        genderFormat = _genderFormat;
        status = TournamentStatus.REGISTRATIONS;

        playersIDs = new List<int>();
        teams = new List<Team>();
        matches = new List<Match>();
    }

    public string GetName()
    {
        return name;
    }

    public void SetName(string _name)
    {
        name = _name;
    }

    public DateTime GetDate()
    {
        return date;
    }

    public void SetDate(DateTime _date)
    {
        date = _date;
    }

    public bool GetCheckWhoEvents()
    {
        return checkWhoEvents;
    }

    public bool GetCheckWhosPlaying()
    {
        return checkWhosPlaying;
    }

    public ManagerTournaments.GenderFormat GetGenderFormat()
    {
        return genderFormat;
    }

    public ManagerTournaments.BracketFormatFirst GetBracketFormatFirst()
    {
        return bracketFormatFirst;
    }

    public void SetBracketFormatFirst(ManagerTournaments.BracketFormatFirst _format)
    {
        bracketFormatFirst = _format;
    }

    public ManagerTournaments.BracketFormatSecond GetBracketFormatSecond()
    {
        return bracketFormatSecond;
    }

    public void SetBracketFormatSecond(ManagerTournaments.BracketFormatSecond _format)
    {
        bracketFormatSecond = _format;
    }

    public int GetGroupSize()
    {
        return groupSize;
    }

    public void SetGroupSize(int _size)
    {
        groupSize = _size;
    }

    public int GetNGroups()
    {
        return Mathf.CeilToInt(GetNTeams() / (float)groupSize);
    }

    public int GetNAdvance()
    {
        return nQualified;
    }

    public void SetNAdvance(int _nAdvance)
    {
        nQualified = _nAdvance;
    }

    public int GetNFields()
    {
        return nFields;
    }

    public void SetNFields(int _fields)
    {
        nFields = _fields;
    }

    public TournamentStatus GetStatus()
    {
        return status;
    }

    public void SetStatus(TournamentStatus _status)
    {
        status = _status;
    }

    public int GetPhase()
    {
        return phase;
    }

    public void IncrementPhase()
    {
        phase++;
    }

    public int GetNPlayers()
    {
        return playersIDs.Count;
    }

    public int GetPlayerID(int _index)
    {
        return playersIDs[_index];
    }

    public List<int> GetPlayersIDs()
    {
        return playersIDs;
    }

    public bool HasPlayer(int _playerID)
    {
        for (int i = 0; i < GetNPlayers(); i++)
        {
            if (playersIDs[i] == _playerID)
                return true;
        }

        return false;
    }

    public void SetPlayers(List<int> _players)
    {
        playersIDs = new List<int>(_players);
    }

    public void AddTeam(Team _t)
    {
        teams.Add(_t);
    }

    public void RemoveTeam(Team _t)
    {
        teams.Remove(_t);
    }

    public int GetNTeams()
    {
        return teams.Count;
    }

    public Team GetTeam(int _index)
    {
        return teams[_index];
    }

    public List<Team> GetTeams()
    {
        return teams;
    }

    public void SetTeams(List<Team> _teams)
    {
        teams = new List<Team>(_teams);
    }

    public void ShuffleTeams()
    {
        teams = teams.OrderBy(x => UnityEngine.Random.value).ToList();
    }

    public void AddMatch(Match _match)
    {
        matches.Add(_match);
    }

    public int GetNMatches()
    {
        return matches.Count;
    }

    public Match GetMatch(int _index)
    {
        return matches[_index];
    }

    public void ClearMatches()
    {
        matches.Clear();
    }

    public int GetFullTime()
    {
        return fullTime;
    }

    public void SetFullTime(int _time)
    {
        fullTime = _time;
    }

    public int GetHalfTime()
    {
        return halfTime;
    }

    public void SetHalfTime(int _time)
    {
        halfTime = _time;
    }

    public int GetTimeBetweenMatches()
    {
        return timeBetweenMatches;
    }

    public void SetTimeBetweenMatches(int _time)
    {
        timeBetweenMatches = _time;
    }

    public int GetTeamRank(Team _team)
    {
        return _team.GetRank();
    }

    public Team GetTeamAtRank(int _rank)
    {
        for (int i = 0; i < GetNTeams(); i++)
        {
            if (teams[i].GetRank() == _rank)
                return teams[i];
        }

        Debug.LogError("Team rank not found");
        return null;
    }

    public void UpdateTeamRanks()
    {
        IEnumerable<Team> ordered = teams.OrderByDescending(t => t.GetVictoriesDefeats()).ThenByDescending(t => t.GetPointDif());

        for (int i = 0; i < teams.Count; i++)
            ordered.ElementAt(i).SetRank(i + 1);
    }

    public int GetScorePlayerPoint()
    {
        return scorePlayerPoint;
    }

    public int GetScorePlayerAssist()
    {
        return scorePlayerAssist;
    }

    public int GetScorePlayerDefense()
    {
        return scorePlayerDefense;
    }

    public int GetScorePlayerCallahan()
    {
        return scorePlayerCallahan;
    }

    public int GetScorePlayerScoreOff()
    {
        return scorePlayerScoreOff;
    }

    public int GetScorePlayerScoreDef()
    {
        return scorePlayerScoreDef;
    }

    public int GetScorePlayerSufferOff()
    {
        return scorePlayerSufferOff;
    }

    public int GetScorePlayerSufferDef()
    {
        return scorePlayerSufferDef;
    }

    public void SetTournamentPlayerScoreRules(int _point, int _assist, int _defense, int _callahan, int _scoreOff, int _scoreDef, int _sufferOff, int _sufferDef)
    {
        scorePlayerPoint = _point;
        scorePlayerAssist = _assist;
        scorePlayerDefense = _defense;
        scorePlayerCallahan = _callahan;
        scorePlayerScoreOff = _scoreOff;
        scorePlayerScoreDef = _scoreDef;
        scorePlayerSufferOff = _sufferOff;
        scorePlayerSufferDef = _sufferDef;
    }

    public void UpdatePlayerElo(Player _player)
    {
        PlayerInTournament pit = _player.GetCurrentTournament();

        pit.SetElo(pit.GetStartElo() +
            pit.GetPoints() * scorePlayerPoint +
            pit.GetAssists() * scorePlayerAssist +
            pit.GetDefenses() * scorePlayerDefense +
            pit.GetCallahans() * scorePlayerCallahan +
            pit.GetScoreOff() * scorePlayerScoreOff +
            pit.GetScoreDef() * scorePlayerScoreDef +
            pit.GetSufferOff() * scorePlayerSufferOff +
            pit.GetSufferDef() * scorePlayerSufferDef);
    }
}
