using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Match
{
    public int ID;
    private DateTime date;
    private Team teamA;
    private Team teamB;
    private int phase;
    private int matchWinnerA;   //Winner of this match is going to be TeamA
    private int matchLoserA;    //Loser of this match is going to be TeamA
    private int matchWinnerB;   //Winner of this match is going to be TeamB
    private int matchLoserB;    //Loser of this match is going to be TeamB
    private int groupA; //TeamA is coming from this group
    private int groupB; //TeamB is coming from this group
    private int placeA; //TeamA is going to be the team in this rank
    private int placeB; //TeamB is going to be the team in this rank
    private int scoreA;
    private int scoreB;
    private int[] spiritA;
    private int[] spiritB;
    private bool firstOffenseA;

    private MatchStatus status;

    public enum MatchStatus
    {
        NOTHING,
        READY,
        RUNNING,
        SPIRIT,
        FINISHED
    }


    private List<MatchEvent> events;

    public Match(int _ID, DateTime _date, Team _teamA, Team _teamB, int _phase)
    {
        ID = _ID;
        events = new List<MatchEvent> { new MatchEvent() }; //Needed to revert to 0-0
        date = _date;
        teamA = _teamA;
        teamB = _teamB;
        phase = _phase;
        spiritA = new int[5] { -1, -1, -1, -1, -1 };
        spiritB = new int[5] { -1, -1, -1, -1, -1 };
        status = MatchStatus.NOTHING;
    }

    public int GetPhase()
    {
        return phase;
    }

    public int GetPlaceA()
    {
        return placeA;
    }

    public int GetPlaceB()
    {
        return placeB;
    }

    public void SetTeamAOriginPlace(int _groupA, int _placeA)
    {
        groupA = _groupA;
        placeA = _placeA;
    }

    public void SetTeamBOriginPlace(int _groupB, int _placeB)
    {
        groupB = _groupB;
        placeB = _placeB;
    }

    public void SetTeamAOriginMatch(int _matchWinnerA, int _matchLoserA)
    {
        matchWinnerA = _matchWinnerA;
        matchLoserA = _matchLoserA;
    }

    public void SetTeamBOriginMatch(int _matchWinnerB, int _matchLoserB)
    {
        matchWinnerB = _matchWinnerB;
        matchLoserB = _matchLoserB;
    }

    public DateTime GetDate()
    {
        return date;
    }

    public void SetDate(DateTime _dateTime)
    {
        date = _dateTime;
    }

    public Team GetTeamA()
    {
        return teamA;
    }

    public Team GetTeamB()
    {
        return teamB;
    }

    public void SetTeamA(Team _teamA)
    {
        teamA = _teamA;
    }

    public void SetTeamB(Team _teamB)
    {
        teamB = _teamB;
    }

    public string GetNameA()
    {
        if (teamA != null)
            return teamA.GetName();

        return placeA + "º" + " Place | G" + groupA;
    }

    public string GetNameB()
    {
        if (teamB != null)
            return teamB.GetName();

        return placeB + "º" + " Place | G" + groupB;
    }

    public MatchStatus GetStatus()
    {
        return status;
    }

    public void SetStatus(MatchStatus _status)
    {
        status = _status;
    }

    public void AddEvent(MatchEvent _event)
    {
        events.Add(_event);
    }

    public int GetScoreA()
    {
        return scoreA;
    }

    public int GetScoreB()
    {
        return scoreB;
    }

    public void ScoredA()
    {
        scoreA++;
        teamA.ChangePointsScored(1);
        teamB.ChangePointsSuffered(1);
    }

    public void ScoredB()
    {
        scoreB++;
        teamA.ChangePointsSuffered(1);
        teamB.ChangePointsScored(1);
    }

    public void RevertScoreA()
    {
        scoreA--;
        teamA.ChangePointsScored(-1);
        teamB.ChangePointsSuffered(-1);
    }

    public void RevertScoreB()
    {
        scoreB--;
        teamA.ChangePointsSuffered(-1);
        teamB.ChangePointsScored(-1);
    }

    public void EndMatch()
    {
        List<Player> played = new List<Player>();

        for (int i = 1; i < events.Count; i++)
        {
            if (events[i].type != MatchEvent.MatchEventType.TURNOVER)
            {
                foreach (Player p in events[i].playingA)
                    if (!played.Contains(p)) played.Add(p);

                foreach (Player p in events[i].playingB)
                    if (!played.Contains(p)) played.Add(p);
            }
        }

        foreach (Player p in played)
            p.GetCurrentTournament().IncrementMatch();

        if (scoreA > scoreB)
        {
            teamA.IncrementVictories();
            teamB.IncrementDefeats();
        }
        else
        {
            teamA.IncrementDefeats();
            teamB.IncrementVictories();
        }

        SetStatus(MatchStatus.SPIRIT);
    }

    public int GetTotalSpiritA()
    {
        int count = 0;

        for (int i = 0; i < spiritA.Length; i++)
            count += Mathf.Max(0, spiritA[i]);

        return count;
    }

    public int GetSpiritA(int _index)
    {
        return spiritA[_index];
    }

    public int GetTotalSpiritB()
    {
        int count = 0;

        for (int i = 0; i < spiritB.Length; i++)
            count += Mathf.Max(0, spiritB[i]);

        return count;
    }

    public int GetSpiritB(int _index)
    {
        return spiritB[_index];
    }

    public void SetSpiritA(int _index, int _value)
    {
        teamA.ChangeSpirit(_index, Mathf.Min(0, -spiritA[_index]));
        spiritA[_index] = _value;
        teamA.ChangeSpirit(_index, _value);
    }

    public void SetSpiritB(int _index, int _value)
    {
        teamB.ChangeSpirit(_index, Mathf.Min(0, -spiritB[_index]));
        spiritB[_index] = _value;
        teamB.ChangeSpirit(_index, _value);
    }

    public bool GetFirstOffenseA()
    {
        return firstOffenseA;
    }

    public void SetFirstOffense(bool _offenseA)
    {
        firstOffenseA = _offenseA;
    }

    public TimeSpan GetDuration()
    {
        return events[events.Count - 1].gameTime;
    }

    public int GetNEvents()
    {
        return events.Count;
    }

    public MatchEvent GetEvent(int _index)
    {
        return events[_index];
    }

    public MatchEvent GetLastEvent()
    {
        return GetEvent(GetNEvents() - 1);
    }

    public void RemoveLastEvent()
    {
        events.RemoveAt(events.Count - 1);
    }

    public bool GetLastOffenseA()
    {
        if (scoreA == 0 && scoreB == 0)
            return firstOffenseA;
        else
            return GetLastScore().startedOffenceA;
    }

    private MatchEvent GetLastScore()
    {
        for (int i = 1; i < events.Count; i++)
        {
            if (GetEvent(GetNEvents() - i).type == MatchEvent.MatchEventType.SCORE || GetEvent(GetNEvents() - i).type == MatchEvent.MatchEventType.CALLAHAN)
                return GetEvent(GetNEvents() - i);
        }

        Debug.LogError("Event not found");
        return new MatchEvent();
    }

    public TimeSpan GetTimeOffenseA()
    {
        TimeSpan ts = TimeSpan.Zero;

        for (int i = firstOffenseA ? 1 : 2; i < events.Count; i += 2)
            ts += events[i].gameTime - (i > 0 ? events[i - 1].gameTime : TimeSpan.Zero);

        return ts;
    }

    public TimeSpan GetTimeOffenseB()
    {
        return GetDuration() - GetTimeOffenseA();
    }

    public float GetTimeOffenseAPercent()
    {
        return (float)(GetTimeOffenseA().TotalSeconds / GetDuration().TotalSeconds);
    }

    public float GetTimeOffenseBPercent()
    {
        return (float)(GetTimeOffenseB().TotalSeconds / GetDuration().TotalSeconds);
    }

    public int GetPointsOffenseA()
    {
        int count = 0;

        for (int i = 1; i < events.Count; i++)
            if (events[i].type == MatchEvent.MatchEventType.SCORE && events[i].startedOffenceA && events[i].eventA)
                count++;

        return count;
    }

    public int GetGoalsOffenseB()
    {
        int count = 0;

        for (int i = 1; i < events.Count; i++)
            if (events[i].type == MatchEvent.MatchEventType.SCORE && !events[i].startedOffenceA && !events[i].eventA)
                count++;

        return count;
    }

    public float GetGoalsOffenseAPercent()
    {
        if (scoreA == 0)
            return 0;

        return (float)GetPointsOffenseA() / scoreA;
    }

    public float GetGoalsOffenseBPercent()
    {
        if (scoreB == 0)
            return 0;

        return (float)GetGoalsOffenseB() / scoreB;
    }

}

[Serializable]
public struct MatchEvent
{
    public TimeSpan gameTime;
    public bool startedOffenceA;
    public bool eventA;
    public int eloChange; //Needed to revert changes
    public int scoreboardA;
    public int scoreboardB;
    public List<Player> playingA;
    public List<Player> playingB;
    public Player whoPoint;
    public Player whoAssist;
    public Player whoDefend;
    public MatchEventType type;
    public int colorIndex;

    public enum MatchEventType
    {
        SCORE,
        TURNOVER,
        CALLAHAN
    }

    public MatchEvent(MatchEventType _type, TimeSpan _gameTime, bool _startedOffenceA, bool _eventA, int _eloChange, int _scoreboardA, int _scoreboardB, List<Player> _playingA, List<Player> _playingB, Player _point, Player _assist, Player _defense, int _colorIndex)
    {
        type = _type;
        gameTime = _gameTime;
        startedOffenceA = _startedOffenceA;
        eventA = _eventA;
        eloChange = _eloChange;
        scoreboardA = _scoreboardA;
        scoreboardB = _scoreboardB;
        playingA = new List<Player>(_playingA);
        playingB = new List<Player>(_playingB);
        whoPoint = _point;
        whoAssist = _assist;
        whoDefend = _defense;
        colorIndex = _colorIndex;
    }

    public bool IsBreak()
    {
        return startedOffenceA != eventA;
    }
}