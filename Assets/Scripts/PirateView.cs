using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class PirateView : MonoBehaviour 
{
	[HideInInspector]
	public List<SpriteRenderer> skills;
	ShipView shipView;

	void Awake() 
	{
		skills.Add(transform.GetChild("skill1").GetComponent<SpriteRenderer>());
		skills.Add(transform.GetChild("skill2").GetComponent<SpriteRenderer>());
		skills.Add(transform.GetChild("skill3").GetComponent<SpriteRenderer>());
		shipView = GameObject.Find("ShipView").GetComponent<ShipView>();
	}

	public void setSkill(int pos, SkillEnum skill)
	{
		Sprite spr = shipView.getSpriteForSkill(skill);
		skills[pos].sprite = spr;
	}
	
	
	// Update is called once per frame
	void Update () {
		
	}
}
