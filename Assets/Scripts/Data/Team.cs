using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class Team
{
    private string name;
    private int colorIndex;
    private ManagerTournaments.GenderFormat format;

    private List<int> playersIDs;

    private int rank;
    private int victories;
    private int defeats;
    private int[] spirit;
    private int pointsScored;
    private int pointsSuffered;

    public Team()
    {
        name = "Dummy";
        format = ManagerTournaments.GenderFormat.MIXED;
        victories = 0;
        defeats = 0;
        spirit = new int[5];
        pointsScored = 0;
        pointsSuffered = 0;

        playersIDs = new List<int>();
    }

    public Team(string _name, ManagerTournaments.GenderFormat _format)
    {
        name = _name;
        format = _format;
        victories = 0;
        defeats = 0;
        spirit = new int[5];
        pointsScored = 0;
        pointsSuffered = 0;

        playersIDs = new List<int>();
    }

    public string GetName()
    {
        return name;
    }

    public void SetName(string _name)
    {
        name = _name;
    }

    public int GetColorIndex()
    {
        return colorIndex;
    }

    public void IncrementColorIndex()
    {
        colorIndex++;

        if (colorIndex == ManagerColors.MC.teamColors.Length)
            colorIndex = 0;
    }

    public int GetNPlayers()
    {
        return playersIDs.Count;
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

    public void AddPlayer(int _playerID)
    {
        playersIDs.Add(_playerID);
    }

    public void RemovePlayer(int _playerID)
    {
        playersIDs.Remove(_playerID);
    }

    public void SetPlayers(List<int> _playersIDs)
    {
        playersIDs = new List<int>(_playersIDs);
    }

    public int GetRank()
    {
        return rank;
    }

    public void ChangeRank(int _value)
    {
        rank += _value;
    }

    public void SetRank(int _rank)
    {
        rank = _rank;
    }

    public int GetVictories()
    {
        return victories;
    }

    public int GetDefeats()
    {
        return defeats;
    }

    public int GetVictoriesDefeats()
    {
        return victories - defeats;
    }

    public void IncrementVictories()
    {
        victories++;
    }

    public void IncrementDefeats()
    {
        defeats++;
    }

    public void ChangePointsScored(int _value)
    {
        pointsScored += _value;
    }

    public void ChangePointsSuffered(int _value)
    {
        pointsSuffered += _value;
    }

    public int GetPointsScored()
    {
        return pointsScored;
    }

    public int GetPointsSuffered()
    {
        return pointsSuffered;
    }

    public int GetPointDif()
    {
        return pointsScored - pointsSuffered;
    }

    public int GetTotalSpirit()
    {
        int count = 0;

        for (int i = 0; i < spirit.Length; i++)
            count += spirit[i];

        return count;
    }

    public void ChangeSpirit(int _index, int _value)
    {
        spirit[_index] += _value;
    }

    public int GetNMale()
    {
        int count = 0;

        for (int i = 0; i < GetNPlayers(); i++)
        {
            if (!SaveFile.SF.GetPlayer(playersIDs[i]).GetFemale())
                count++;
        }

        return count;
    }

    public int GetNFemale()
    {
        return GetNPlayers() - GetNMale();
    }

    public int GetSumElo()
    {
        float sum = 0;

        foreach (int p in playersIDs)
            sum += SaveFile.SF.GetPlayer(p).GetCurrentTournament().GetElo();

        return (int)sum;
    }

    public float GetAvgElo()
    {
        if (GetNPlayers() == 0)
            return 0;

        return (float)GetSumElo() / GetNPlayers();
    }

    public void EloChange(int _value)
    {
        foreach (int p in playersIDs)
            SaveFile.SF.GetPlayer(p).GetCurrentTournament().EloChange(_value);
    }

    public int GetPlayerID(int _index)
    {
        return playersIDs[_index];
    }

    public List<int> GetPlayersIDs()
    {
        return playersIDs;
    }

    public void ShufflePlayers()
    {
        playersIDs = playersIDs.OrderBy(x => UnityEngine.Random.value).ToList();
    }
}