using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player
{
    public int ID;
    private int pictureIndex;
    private string name;
    private int number;
    private bool female;
    private List<PlayerInTournament> tournaments;
    private int currentTournamentIndex;

    public Player(int _pictureIndex, string _name, int _number, bool _female)
    {
        pictureIndex = _pictureIndex;
        name = _name;
        number = _number;
        female = _female;
        tournaments = new List<PlayerInTournament>();
    }

    public string GetName()
    {
        return name;
    }

    public void SetName(string _name)
    {
        name = _name;
    }

    public int GetPictureIndex()
    {
        return pictureIndex;
    }

    public void SetPictureIndex(int _index)
    {
        pictureIndex = _index;
    }

    public int GetNumber()
    {
        return number;
    }

    public void SetNumber(int _number)
    {
        number = _number;
    }

    public bool GetFemale()
    {
        return female;
    }

    public void SetFemale(bool _female)
    {
        female = _female;
    }

    public PlayerInTournament GetTournament(int _index)
    {
        return tournaments[_index];
    }

    public PlayerInTournament GetTournamentByID(int _ID)
    {
        return tournaments[FindTournamentIndex(_ID)];
    }

    public int GetNTournaments()
    {
        return tournaments.Count;
    }

    public void AddTournament(int _tournamentID)
    {
        PlayerInTournament pit = new PlayerInTournament(_tournamentID);
        tournaments.Add(pit);
        currentTournamentIndex = tournaments.Count - 1;
    }

    public void RemoveTournament(int _tournamentID)
    {
        tournaments.RemoveAt(FindTournamentIndex(_tournamentID));
    }

    public PlayerInTournament GetCurrentTournament()
    {
        return tournaments[currentTournamentIndex];
    }

    public void SetCurrentTournament(int _tournamentID)
    {
        currentTournamentIndex = FindTournamentIndex(_tournamentID);
    }

    public bool IsInTournament(int _tournamentID)
    {
        for (int i = 0; i < tournaments.Count; i++)
        {
            if (tournaments[i].GetTournamentID() == _tournamentID)
                return true;
        }

        return false;
    }

    private int FindTournamentIndex(int _tournamentID)
    {
        for (int i = 0; i < tournaments.Count; i++)
        {
            if (tournaments[i].GetTournamentID() == _tournamentID)
                return i;
        }

        Debug.LogError("Tournament not found");
        return -1;
    }

    public int GetNTotalMatches()
    {
        int c = 0;

        for (int i = 0; i < GetNTournaments(); i++)
            c += tournaments[i].GetMatches();

        return c;
    }

    public int GetNTotalPoints()
    {
        int c = 0;

        for (int i = 0; i < GetNTournaments(); i++)
            c += tournaments[i].GetPoints();

        return c;
    }

    public int GetNTotalAssists()
    {
        int c = 0;

        for (int i = 0; i < GetNTournaments(); i++)
            c += tournaments[i].GetAssists();

        return c;
    }

    public int GetNTotalDefenses()
    {
        int c = 0;

        for (int i = 0; i < GetNTournaments(); i++)
            c += tournaments[i].GetDefenses();

        return c;
    }
}

[Serializable]
public class PlayerInTournament
{
    private int tournamentID;

    private float startElo;
    private float elo;
    private int matches;
    private int points;
    private int assists;
    private int defenses;
    private int callahans;
    private int scoreOff;
    private int scoreDef;
    private int sufferOff;
    private int sufferDef;

    public PlayerInTournament(int _tournamentID)
    {
        tournamentID = _tournamentID;
    }

    public int GetTournamentID()
    {
        return tournamentID;
    }

    public void EloChange(int _value)
    {
        elo += _value;
    }

    public void ChangeAssist(int _value)
    {
        assists += _value;
    }

    public void ChangePoint(int _value)
    {
        points += _value;
    }

    public void ChangeDefense(int _value)
    {
        defenses += _value;
    }

    public void ChangeCallahan(int _value)
    {
        callahans += _value;
    }

    public void ScoreOffChange(int _value)
    {
        scoreOff += _value;
    }

    public void ScoreDefChange(int _value)
    {
        scoreDef += _value;
    }

    public void SufferOffChange(int _value)
    {
        sufferOff += _value;
    }

    public void SufferDefChange(int _value)
    {
        sufferDef += _value;
    }

    public float GetStartElo()
    {
        return startElo;
    }

    public void SetStartElo(float _startElo)
    {
        startElo = _startElo;
    }

    public float GetElo()
    {
        return elo;
    }

    public void SetElo(float _elo)
    {
        elo = _elo;
    }

    public int GetMatches()
    {
        return matches;
    }

    public void IncrementMatch()
    {
        matches++;
    }

    public int GetPoints()
    {
        return points;
    }

    public int GetAssists()
    {
        return assists;
    }

    public int GetDefenses()
    {
        return defenses;
    }

    public int GetCallahans()
    {
        return callahans;
    }

    public int GetScoreOff()
    {
        return scoreOff;
    }

    public int GetScoreDef()
    {
        return scoreDef;
    }

    public int GetSufferOff()
    {
        return sufferOff;
    }

    public int GetSufferDef()
    {
        return sufferDef;
    }
}
