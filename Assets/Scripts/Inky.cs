using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : Ghost
{
	private enum stateTypes { locked, unlocking, chasing, fleeing, wandering, locking };
	public int state = (int)stateTypes.locked;
	private float time = 0;
	private float fleeTime = 0;
	public float unlockingTime = 0;     //time at which ghost escapes confinement
	private Vector3 goal;
	private Vector3 startPosition;
	public GameObject ghostBlue;
	public GameObject ghostDead;
	private GameObject blinky = null;

	public override void Update()
	{
		prep();

		if (blinky == null)
		{
			for (int i = 0; i < ghosts.Length; i++)
			{
				if (ghosts[i].GetComponent<Blinky>() != null)
					blinky = ghosts[i];
			}
		}

		if (time == 0)
			startPosition = transform.position;

		if (state == (int)stateTypes.chasing)
		{
			chasingLogic();
			move();
		}
		else if (state == (int)stateTypes.unlocking)
		{
			unlockingLogic();
			move();
		}
		else if (state == (int)stateTypes.wandering)
		{
			wanderingLogic();
			move();
		}
		else if (state == (int)stateTypes.fleeing)
		{
			fleeingLogic();
			move();
		}
		else if (state == (int)stateTypes.locked)
		{
			ghostBlue.transform.position = transform.position + new Vector3(0, 0, 5);
			if ((time >= unlockingTime && fleeTime == 0) || (time - fleeTime) >= 7)
				state = (int)stateTypes.unlocking;
		}
		else if (state == (int)stateTypes.locking)
		{
			lockingLogic();
			move();
		}

		time += Time.deltaTime;
	}

	private void goToGoal()
	{
		float x = goal.x - transform.position.x;
		float y = goal.y - transform.position.y;

		if (Mathf.Abs(x) > Mathf.Abs(y))
		{
			if (x > 0)
			{
				if (!setGoalRight())
				{
					if (y > 0)
						setGoalUp();
					else
						setGoalDown();
				}
			}
			else
			{
				if (!setGoalLeft())
				{
					if (y > 0)
						setGoalUp();
					else
						setGoalDown();
				}
			}
		}
		else if (Mathf.Abs(x) < Mathf.Abs(y))
		{
			if (y > 0)
			{
				if (!setGoalUp())
				{
					if (x > 0)
						setGoalRight();
					else
						setGoalLeft();
				}
			}
			else
			{
				if (!setGoalDown())
				{
					if (x > 0)
						setGoalRight();
					else
						setGoalLeft();
				}
			}
		}
	}

	private void unlockingLogic()
	{
		if (transform.position.x <= 16.5f && transform.position.x > 14.5f)
			setGoalLeft();
		else if (transform.position.x < 13.5f && transform.position.x >= 11.5f)
			setGoalRight();
		else if (transform.position.y < 19f)
			setGoalUp();
		else
		{
			state = (int)stateTypes.wandering;
			if (transform.position.x < 14.5f)
				setGoalLeft();
			else
				setGoalRight();

			//set door as closed
		}
	}

	private void chasingLogic()
	{
		int pacmanDir = pacman.GetComponent<PacMaster>().dir;

		goal = pacman.transform.position;

		if (pacmanDir == 0)
			goal += new Vector3(0, 3);
		else if (pacmanDir == 1)
			goal += new Vector3(3, 0);
		else if (pacmanDir == 2)
			goal += new Vector3(0, -3);
		else if (pacmanDir == 3)
			goal += new Vector3(-3, 0);

		goal += goal - blinky.transform.position;

		if (atGoal() && !atCrossroad())
		{
			pathContinue();
		}
		else if (atGoal())
		{
			goToGoal();
		}

		if (time > 30 && time < 40)
		{
			state = (int)stateTypes.wandering;
		}
	}

	private void wanderingLogic()
	{
		if (atGoal() && !atCrossroad())
		{
			pathContinue();
		}
		else
		{
			if (dir == 0)
			{
				if (canMoveUp())
					pathContinue();
				else
				{
					float r = Random.value;
					if (r < .5f)
					{
						if (!setGoalRight())
							setGoalLeft();
					}
					else
					{
						if (!setGoalLeft())
							setGoalRight();
					}
				}
			}
			else if (dir == 1)
			{
				if (canMoveRight())
					pathContinue();
				else
				{
					float r = Random.value;
					if (r < .5f)
					{
						if (!setGoalUp())
							setGoalDown();
					}
					else
					{
						if (!setGoalDown())
							setGoalUp();
					}
				}
			}
			else if (dir == 2)
			{
				if (canMoveDown())
					pathContinue();
				else
				{
					float r = Random.value;
					if (r < .5f)
					{
						if (!setGoalRight())
							setGoalLeft();
					}
					else
					{
						if (!setGoalLeft())
							setGoalRight();
					}
				}
			}
			else
			{
				if (canMoveLeft())
					pathContinue();
				else
				{
					float r = Random.value;
					if (r < .5f)
					{
						if (!setGoalUp())
							setGoalDown();
					}
					else
					{
						if (!setGoalDown())
							setGoalUp();
					}
				}
			}
		}

		if ((time > 20 && time < 30) || time > 40)
		{
			state = (int)stateTypes.chasing;
		}
	}

	private void fleeingLogic()
	{
		goal = transform.position + (transform.position - pacman.transform.position);

		if (atGoal() && !atCrossroad())
		{
			pathContinue();
		}
		else if (atGoal())
		{
			goToGoal();
		}

		if (time - fleeTime > 5)
		{
			if (Mathf.FloorToInt(time * 5) % 2 == 0)
				ghostBlue.transform.position = transform.position + new Vector3(0, 0, 5);
			else
				ghostBlue.transform.position = transform.position + new Vector3(0, 0, -5);
		}

		if (time - fleeTime > 7)
		{
			state = (int)stateTypes.chasing;
			ghostBlue.transform.position = transform.position + new Vector3(0, 0, 5);
		}
	}

	void lockingLogic()
	{
		goal = startPosition;

		if (atGoal() && !atCrossroad())
		{
			pathContinue();
		}
		else if (atGoal())
		{
			goToGoal();
		}

		if (Vector3.Distance(transform.position, startPosition) < .05f)
			state = (int)stateTypes.locked;
	}

	public void setFleeing()
	{
		fleeTime = time;
		if (dir % 2 == 0)
		{
			if (Mathf.Abs(pacman.transform.position.x - transform.position.x) < 1)
			{
				if (dir == 0 && pacman.transform.position.y > transform.position.y)
				{
					if (!setGoalDown())
						if (!setGoalRight())
							setGoalLeft();
				}
				else if (dir == 2 && pacman.transform.position.y < transform.position.y)
					if (!setGoalDown())
						if (!setGoalRight())
							setGoalLeft();
			}
		}
		else
		{
			if (Mathf.Abs(pacman.transform.position.y - transform.position.y) < 1)
			{
				if (dir == 1 && pacman.transform.position.y > transform.position.y)
				{
					if (!setGoalLeft())
						if (!setGoalUp())
							setGoalDown();
				}
				else if (dir == 3 && pacman.transform.position.y < transform.position.y)
					if (!setGoalRight())
						if (!setGoalUp())
							setGoalDown();
			}
		}
		state = (int)stateTypes.fleeing;
		ghostBlue.transform.position = transform.position + new Vector3(0, 0, -5);
		//set killable on pacmaster or something
	}

	void setLocking()
	{
		state = (int)stateTypes.locking;
		ghostBlue.transform.position = transform.position + new Vector3(0, 0, 5);
		ghostDead.transform.position = transform.position + new Vector3(0, 0, -5);
	}
}
