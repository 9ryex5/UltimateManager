using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerTeamGenerator : MonoBehaviour
{
    public static ManagerTeamGenerator MTG;

    public GameObject manager;
    private ManagerUI MUI;
    private SaveFile SF;

    public Animator animTeams;
    public TextMeshProUGUI textNPlayers;
    public Slider sliderNTeams;
    public TextMeshProUGUI textSlider;

    public ItemPlayer prefabItemPlayer;
    public Transform parentPlayers;
    private List<int> playingPlayersIDs;
    private List<int> pickedPlayers;
    private int nTeams;
    private Team[] teams;

    private void Awake()
    {
        MTG = this;

        MUI = manager.GetComponent<ManagerUI>();
        SF = manager.GetComponent<SaveFile>();
    }

    private void Start()
    {
        parentPlayers.GetComponent<GridLayoutGroup>().cellSize = new Vector2(ManagerUI.MUI.sizePlayerGrid * Screen.height, ManagerUI.MUI.sizePlayerGrid * Screen.height);
    }

    public void OnEnable()
    {
        pickedPlayers = new List<int>();

        nTeams = 0;
        sliderNTeams.maxValue = 0;
        textNPlayers.text = "Players (0)";
        textSlider.text = "0";

        for (int i = 0; i < parentPlayers.childCount; i++)
            Destroy(parentPlayers.GetChild(i).gameObject);

        foreach (int p in ManagerTournaments.MT.GetAllPlayers())
        {
            ItemPlayer ip = Instantiate(prefabItemPlayer, parentPlayers);
            ip.StartThis(p, ItemPlayer.ItemPlayerType.PLAYER_GENERATOR, true, SF.GetPlayer(p).GetCurrentTournament().GetElo());
        }
    }

    public void PickPlayer(int _playerID)
    {
        pickedPlayers.Add(_playerID);
        textNPlayers.text = "Players (" + pickedPlayers.Count + ")";
        sliderNTeams.maxValue = pickedPlayers.Count / 3;
    }

    public void UnpickPlayer(int _playerID)
    {
        pickedPlayers.Remove(_playerID);
        textNPlayers.text = "Players (" + pickedPlayers.Count + ")";
        sliderNTeams.maxValue = pickedPlayers.Count / 3;
    }

    public void SliderChanged()
    {
        nTeams = (int)sliderNTeams.value;
        textSlider.text = nTeams.ToString();
    }

    public void ButtonGenerate()
    {
        if (pickedPlayers.Count < 6 || nTeams < 2)
            return;

        GenerateTeams();

        foreach (Team t in teams)
            ManagerTournaments.MT.AddGeneratedTeam(t);

        ManagerTournaments.MT.EndTeamGeneration(nTeams);
    }

    /*private void GenerateTeams()
    {
        teams = new Team[nTeams];

        for (int i = 0; i < nTeams; i++)
            teams[i] = new Team("Gen_" + i, ManagerTournaments.GenderFormat.MIXED);

        List<int> pickedMales = OnlyMales(pickedPlayers);
        List<int> pickedFemales = OnlyFemales(pickedPlayers);

        IEnumerable<int> ordered = null;
        ordered = pickedMales.OrderByDescending(p => SF.GetPlayer(p).GetCurrentTournament().GetElo());
        pickedMales = ordered.ToList();
        ordered = pickedFemales.OrderByDescending(p => SF.GetPlayer(p).GetCurrentTournament().GetElo());
        pickedFemales = ordered.ToList();

        int[] small = pickedMales.Count > pickedFemales.Count ? pickedFemales.ToArray() : pickedMales.ToArray();
        int[] big = pickedMales.Count > pickedFemales.Count ? pickedMales.ToArray() : pickedFemales.ToArray();

        //Small
        int sets = small.Length / nTeams;

        for (int i = 0; i < sets * nTeams; i++)
            AddPlayerLessEloTeam(TeamsLessPlayers(), small[i]);

        int lendFromBig = nTeams - small.Length % nTeams;
        if (lendFromBig == nTeams) lendFromBig = 0;

        //Uneven Small
        if (lendFromBig != 0)
        {
            for (int i = 0; i < lendFromBig; i++)
                AddPlayerLessEloTeam(TeamsLessPlayers(), big[i]);

            for (int i = 0; i < small.Length % nTeams; i++)
                AddPlayerLessEloTeam(TeamsLessPlayers(), small[sets * nTeams + i]);
        }

        //Big
        sets = (big.Length - lendFromBig) / nTeams;

        for (int i = lendFromBig; i < sets * nTeams + lendFromBig; i++)
            AddPlayerLessEloTeam(TeamsLessPlayers(), big[i]);

        //Uneven Big
        for (int i = 0; i < (big.Length - lendFromBig) % nTeams; i++)
            AddPlayerMoreEloTeam(TeamsLessPlayers(), big[big.Length - i - 1]);

        //DEBUG
        for (int i = 0; i < nTeams; i++)
            if (Application.isEditor) Debug.Log("M: " + teams[i].GetNMale() + "   F: " + teams[i].GetNFemale() + "   Elo: " + teams[i].GetAvgElo().ToString("F4"));

        if (Application.isEditor) Debug.Log("Std. Deviation: " + StandardDeviation(teams));
    }*/

    private void GenerateTeams()
    {
        teams = new Team[nTeams];

        for (int i = 0; i < nTeams; i++)
            teams[i] = new Team("Gen_" + i, ManagerTournaments.GenderFormat.MIXED);

        List<int> pickedMales = OnlyMales(pickedPlayers);
        List<int> pickedFemales = OnlyFemales(pickedPlayers);

        IEnumerable<int> ordered = null;
        ordered = pickedMales.OrderByDescending(p => SF.GetPlayer(p).GetCurrentTournament().GetElo());
        pickedMales = ordered.ToList();
        ordered = pickedFemales.OrderByDescending(p => SF.GetPlayer(p).GetCurrentTournament().GetElo());
        pickedFemales = ordered.ToList();

        while (pickedMales.Count > 0 || pickedFemales.Count > 0)
        {
            if (pickedMales.Count > 0 && (pickedFemales.Count == 0 || SF.GetPlayer(pickedMales[0]).GetCurrentTournament().GetElo() >= SF.GetPlayer(pickedFemales[0]).GetCurrentTournament().GetElo()))
            {
                //Place Males
                if (pickedMales.Count >= nTeams)
                {
                    for (int i = 0; i < nTeams; i++)
                        AddPlayerLessEloTeam(TeamsLessPlayers(), pickedMales[i]);

                    pickedMales.RemoveRange(0, nTeams);
                }
                else //Last Males
                {
                    for (int i = 0; i < pickedMales.Count; i++)
                        AddPlayerMoreEloTeam(TeamsLessPlayers(), pickedMales[pickedMales.Count - 1 - i]);

                    pickedMales.Clear();
                }
            }
            else
            {
                //Place Females
                if (pickedFemales.Count >= nTeams)
                {
                    for (int i = 0; i < nTeams; i++)
                        AddPlayerLessEloTeam(TeamsLessPlayers(), pickedFemales[i]);

                    pickedFemales.RemoveRange(0, nTeams);
                }
                else //Last Females
                {
                    for (int i = 0; i < pickedFemales.Count; i++)
                        AddPlayerMoreEloTeam(TeamsLessPlayers(), pickedFemales[pickedFemales.Count - 1 - i]);

                    pickedFemales.Clear();
                }
            }
        }

        //DEBUG
        for (int i = 0; i < nTeams; i++)
            if (Application.isEditor) Debug.Log("M: " + teams[i].GetNMale() + "   F: " + teams[i].GetNFemale() + "   Elo: " + teams[i].GetAvgElo().ToString("F4"));

        if (Application.isEditor) Debug.Log("Std. Deviation: " + StandardDeviation(teams));
    }

    private Team[] TeamsLessPlayers()
    {
        int nLessSizeTeams = 0;
        int lessSize = int.MaxValue;

        for (int i = 0; i < nTeams; i++)
        {
            if (teams[i].GetNPlayers() < lessSize)
            {
                nLessSizeTeams = 0;
                lessSize = teams[i].GetNPlayers();
            }

            if (teams[i].GetNPlayers() == lessSize)
                nLessSizeTeams++;
        }

        Team[] teamsLessPlayers = new Team[nLessSizeTeams];
        int index = 0;

        for (int i = 0; i < nTeams; i++)
        {
            if (teams[i].GetNPlayers() == lessSize)
            {
                teamsLessPlayers[index] = teams[i];
                index++;
            }
        }

        return teamsLessPlayers;
    }

    private void AddPlayerMoreEloTeam(Team[] _teams, int p)
    {
        Team bestTeam = new Team();
        int moreElo = int.MinValue;
        int elo;

        for (int i = 0; i < _teams.Length; i++)
        {
            elo = _teams[i].GetSumElo();

            if (elo > moreElo)
            {
                bestTeam = _teams[i];
                moreElo = elo;
            }
        }

        bestTeam.AddPlayer(p);
    }

    private void AddPlayerLessEloTeam(Team[] _teams, int p)
    {
        Team bestTeam = new Team();
        int lessElo = int.MaxValue;
        int elo;

        for (int i = 0; i < _teams.Length; i++)
        {
            elo = _teams[i].GetSumElo();

            if (elo < lessElo)
            {
                bestTeam = _teams[i];
                lessElo = elo;
            }
        }

        bestTeam.AddPlayer(p);
    }

    private List<int> OnlyMales(List<int> players)
    {
        List<int> males = new List<int>();

        foreach (int i in players)
        {
            if (!SaveFile.SF.GetPlayer(i).GetFemale())
                males.Add(i);
        }

        return males;
    }

    private List<int> OnlyFemales(List<int> players)
    {
        List<int> females = new List<int>();

        foreach (int i in players)
        {
            if (SaveFile.SF.GetPlayer(i).GetFemale())
                females.Add(i);
        }

        return females;
    }

    private int CountMale(List<int> players)
    {
        int sum = 0;

        foreach (int p in players)
        {
            if (!SF.GetPlayer(p).GetFemale())
                sum++;
        }

        return sum;
    }

    private int CountFemale(List<int> players)
    {
        return players.Count - CountMale(players);
    }

    private float StandardDeviation(Team[] teams)
    {
        int nTeams = teams.Length;
        float sum = 0;

        for (int i = 0; i < nTeams; i++)
            sum += teams[i].GetAvgElo();

        float mean = sum / nTeams;
        float somatory = 0;

        for (int i = 0; i < nTeams; i++)
            somatory += Mathf.Pow(teams[i].GetAvgElo() - mean, 2);

        return Mathf.Sqrt(somatory / (nTeams - 1));
    }
}
