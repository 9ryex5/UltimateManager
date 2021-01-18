using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveFile : MonoBehaviour
{
    public static SaveFile SF;

    private List<Player> allPlayers;
    private int allPlayersID;
    private List<Tournament> allTournaments;
    private int allTournamentsID;

    public void Start()
    {
        SF = this;
        LoadPlayersData();
        LoadTournamentsData();
    }

    public void AddPlayer(Player _p)
    {
        _p.ID = allPlayersID;
        allPlayersID++;
        allPlayers.Add(_p);
        allPlayers.Sort((p1, p2) => p1.GetName().CompareTo(p2.GetName()));
        SavePlayersData();
    }

    public void RemovePlayer(Player _p)
    {
        foreach (Tournament t in allTournaments)
        {
            if (t.HasPlayer(_p.ID))
            {
                for (int i = 0; i < t.GetNTeams(); i++)
                    if (t.GetTeam(i).HasPlayer(t.GetPlayerID(i)))
                        t.GetTeam(i).RemovePlayer(t.GetPlayerID(i));
            }
        }

        allPlayers.Remove(_p);
        SavePlayersData();
    }

    public List<Player> GetAllPlayers()
    {
        return allPlayers;
    }

    public Player GetPlayer(int _ID)
    {
        for (int i = 0; i < allPlayers.Count; i++)
        {
            if (_ID == allPlayers[i].ID)
                return allPlayers[i];
        }

        Debug.LogWarning("Player not found");

        return null;
    }

    public Tournament GetTournament(int _ID)
    {
        for (int i = 0; i < allTournaments.Count; i++)
        {
            if (_ID == allTournaments[i].ID)
                return allTournaments[i];
        }

        Debug.LogWarning("Tournament not found");

        return null;
    }

    public List<Tournament> GetAllTournaments()
    {
        return allTournaments;
    }

    public void AddTournament(Tournament _t)
    {
        _t.ID = allTournamentsID;
        allTournamentsID++;
        allTournaments.Add(_t);
        SaveTournamentsData();
    }

    public void RemoveTournament(Tournament _t)
    {
        foreach (Player p in allPlayers)
            if (p.IsInTournament(_t.ID))
                p.RemoveTournament(_t.ID);

        allTournaments.Remove(_t);
        SaveTournamentsData();
    }

    public void SavePlayersData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerData.pd");

        PlayerData data = new PlayerData();

        data.allPlayers = allPlayers;
        data.lastID = allPlayersID;

        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadPlayersData()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.pd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerData.pd", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            allPlayers = data.allPlayers;
            allPlayersID = data.lastID;
        }
        else
        {
            allPlayers = new List<Player>();
            allPlayersID = 0;
            SavePlayersData();
        }
    }

    public void SaveTournamentsData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/TournamentData.td");

        TournamentData data = new TournamentData();

        data.allTournaments = allTournaments;
        data.lastID = allTournamentsID;

        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadTournamentsData()
    {
        if (File.Exists(Application.persistentDataPath + "/TournamentData.td"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/TournamentData.td", FileMode.Open);
            TournamentData data = (TournamentData)bf.Deserialize(file);
            file.Close();

            allTournaments = data.allTournaments;
            allTournamentsID = data.lastID;
        }
        else
        {
            allTournaments = new List<Tournament>();
            allTournamentsID = 0;
            SaveTournamentsData();
        }
    }
}

[Serializable]
class PlayerData
{
    public List<Player> allPlayers;
    public int lastID;
}

[Serializable]
class TournamentData
{
    public List<Tournament> allTournaments;
    public int lastID;
}