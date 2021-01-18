using UnityEngine;
using UnityEngine.UI;

public class Languages : MonoBehaviour
{
    private const int MAX_LANGUAGES = 2;

    private static string[,] words = {
        { "Players", "Jogadores" },
        { "Game", "Jogo"},
        { "Generator", "Gerador" },
        { "History", "Histórico" },
        { "Settings", "Definições" },
        { "Practice", "Treino" },
        { "Regional", "Regional" },
        { "Add", "Adicionar" },
        { "Mode", "Modo" },
        { "Size", "Tamanho" } };

    public Sprite[] flags;
    public Image imageFlag;

    private static int currentLangIndex;

    public void ButtonChangeLanguage()
    {
        currentLangIndex++;

        if (currentLangIndex == MAX_LANGUAGES)
            currentLangIndex = 0;

        imageFlag.sprite = flags[currentLangIndex];
    }

    public int GetLanguage()
    {
        return currentLangIndex;
    }

    public void SetLanguage(int _value)
    {
        currentLangIndex = _value;
        imageFlag.sprite = flags[currentLangIndex];
    }

    public static string Translate(string _word)
    {
        if (currentLangIndex == 0)
            return _word;

        for (int i = 0; i < words.GetLength(0); i++)
            if (_word == words[i, 0])
                return words[i, currentLangIndex];

        Debug.LogError("Word not found");
        return string.Empty;
    }
}
