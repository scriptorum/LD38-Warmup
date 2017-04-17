using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class GameView : MonoBehaviour 
{
	public SkillView[] skills;
	public GameObject piratePrefab;
	public GameObject taskPrefab;
	private MutinyModel model;
	private Transform stage;
	private int numPirates;
	private int numTasks;

	void Awake()
	{
		model = GameObject.Find(typeof(MutinyModel).Name).GetComponent<MutinyModel>();		
		model.ThrowIfNull();

		stage = transform.Find("Stage");
		stage.ThrowIfNull();
	}
	
	void Start() 
	{
		model.reset();
	}

	public void onReset()
	{
		numTasks = numPirates = 0;		
	}

	public void onPirateCreated(Pirate pirate)
	{
		GameObject go = Instantiate(piratePrefab);
		go.name = "pirate" + numPirates;
		go.transform.parent = stage;
		go.transform.localPosition = new Vector3(-3.4f + 0.85f * numPirates, 2.15f, 0);
		PirateView view = go.GetComponent<PirateView>();
		view.setPirate(pirate);
		numPirates++;
	}
	public void onTaskCreated(Task task)
	{
		GameObject go = Instantiate(taskPrefab);
		go.name = "task" + numTasks;
		go.transform.parent = stage;
		go.transform.localPosition = new Vector3(-2.7f, .6f - 1.35f * numTasks, 0);
		TaskView view = go.GetComponent<TaskView>();
		view.setTask(task);
		numTasks++;
	}
	public void onStartMission()
	{
		Debug.Log("Start of mission #" + model.mission);
	}

	public Sprite getSpriteForSkill(SkillEnum skillEnum)
	{
		foreach(SkillView skill in skills)
		{
			if(skill.type == skillEnum)
				return skill.sprite;
		}
		Debug.Log("Skill not found:" + skillEnum);
		return null;
	}
}


[System.Serializable]
public struct SkillView
{
	public SkillEnum type;
	public Sprite sprite;
}