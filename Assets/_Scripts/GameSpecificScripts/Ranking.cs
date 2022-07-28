using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;
using TMPro;

public class Ranking : SingletonMonoBehaviour<Ranking>
{
    public List<CharacterProperty> characters;
    public List<int> characterBallValues;
    public List<CharacterProperty> rankedCharacters;
    public List<Transform> targetPositions;
    public Transform cameraPosition;
    public GameObject rankingPanel;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        characters = FindObjectsOfType<CharacterProperty>().ToList();
        rankedCharacters = new List<CharacterProperty>();
    }

    public void StartRanking(CharacterProperty character)
    {
        character.isRanked = true;
        RankCharacters(character);
    }
    private void RankCharacters(CharacterProperty character)
    {
        rankedCharacters.Add(character);
        character.finalRank = rankedCharacters.Count;
        ShowOrder(character, character.finalRank);

        if (character.playerController.characterType == PlayerController.CharacterType.Player)
        {
            RankRemainingCharacters();
        }
        else
        {
            if (rankedCharacters.Count == 3)
            {
                RankRemainingCharacters();
            }
        }
    }

    private void RankRemainingCharacters()
    {
        for (int j = 0; j < characters.Count; j++)
        {
            if (!rankedCharacters.Contains(characters[j]))
            {
                rankedCharacters.Add(characters[j]);
                characters[j].finalRank = rankedCharacters.Count;
                characters[j].isRanked = true;
                ShowOrder(characters[j], characters[j].finalRank);
                
            }

        }
    }

    private void ShowOrder(CharacterProperty characterProperty, int order)
    {
        rankingPanel.transform.GetChild(order - 1).gameObject.SetActive(true);
        rankingPanel.transform.GetChild(order - 1).GetComponent<TextMeshProUGUI>().text = order + "." + characterProperty.playerController.name;
    }


}
