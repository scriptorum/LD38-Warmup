using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class TaskView : MonoBehaviour 
{
	private List<SpriteRenderer> skills = new List<SpriteRenderer>();
	private List<SpriteRenderer> crew = new List<SpriteRenderer>();
	private List<SpriteRenderer> coins = new List<SpriteRenderer>();
	public GameView gameView;
	private bool[,] countToCoin = new bool[,] {
		{ false, false, false, false, false},
		{ false, false, false, true, false},
		{ true, true, false, false, false},
		{ false, false, true, true, true},
		{ true, true, true, false, true},
		{ true, true, true, true, true}
	};
	private bool[,] countToCrew = new bool[,] {
		{ false, false, false },
		{ false, true, false },
		{ true, false, true },
		{ true, true, true }
	};

	void Awake()
	{
		gameView = GameObject.Find(typeof(GameView).Name).GetComponent<GameView>();		
		gameView.ThrowIfNull();

		for(int i = 1; i <= 3; i++)
		{
			SpriteRenderer sr = transform.GetChild("skill" + i).GetComponent<SpriteRenderer>();
			sr.ThrowIfNull();
			skills.Add(sr);
		}

		for(int i = 1; i <= 3; i++)
			crew.Add(transform.GetChild("crew" + i).GetComponent<SpriteRenderer>());

		for(int i = 1; i <= 5; i++)
			coins.Add(transform.GetChild("coin" + i).GetComponent<SpriteRenderer>());
	}

	public void setSkill(int pos, SkillEnum skill)
	{
		Sprite spr = gameView.getSpriteForSkill(skill);
		skills[pos].sprite = spr;
	}

	public void setCrew(int count)
	{
		Debug.Assert(count >= 0 && count <= 3);
		for(int i = 0; i < 3; i++)
			crew[i].enabled = countToCrew[count, i];
	}

	public void setCoin(int count)
	{
		Debug.Assert(count >= 0 && count <= 5);
		for(int i = 0; i < 5; i++)
			coins[i].enabled = countToCoin[count, i];
	}
}
