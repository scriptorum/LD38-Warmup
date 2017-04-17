using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class PirateView : MonoBehaviour 
{
	public GameView gameView;
	private List<SpriteRenderer> skills = new List<SpriteRenderer>();

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
	}

	public void setPirate(Pirate pirate)
	{
		for(int i = 0; i < 3; i++)
			if(i < pirate.skills.Count)
				setSkill(i, pirate.skills[i]);
			else setSkill(i, SkillEnum.None);
	}

	public void setSkill(int pos, SkillEnum skill)
	{

		if(skill == SkillEnum.None)
			skills[pos].enabled = false;
		else
		{
			skills[pos].enabled = true;
			Sprite spr = gameView.getSpriteForSkill(skill);
			skills[pos].sprite = spr;
		}
	}
}
