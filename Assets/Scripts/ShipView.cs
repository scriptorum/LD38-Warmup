using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class ShipView : MonoBehaviour 
{
	[HideInInspector]
	public List<PirateView> pirates;
	public Skill[] skills;
	
	void Awake() 
	{
		foreach(PirateView pv in transform.GetComponentsInChildren<PirateView>())
		{
			pirates.Add(pv);
			pv.setSkill(0, (SkillEnum) Random.Range(1,6));
			pv.setSkill(1, (SkillEnum) Random.Range(1,6));
			pv.setSkill(2, (SkillEnum) Random.Range(1,6));
		}
	}
	
	void Update () 
	{
		
	}
	public Sprite getSpriteForSkill(SkillEnum skillEnum)
	{
		foreach(Skill skill in skills)
			if(skill.type == skillEnum)
				return skill.sprite;
		return null;
	}
}


[System.Serializable]
public struct Skill
{
	public SkillEnum type;
	public Sprite sprite;
}