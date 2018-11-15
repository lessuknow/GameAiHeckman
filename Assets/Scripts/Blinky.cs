using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
	private enum stateTypes { locked, unlocking, chasing, fleeing, wandering };
	private int state = (int)stateTypes.unlocking;
	private float time = 0;
	public float unlockingTime = 0;		//time at which ghost escapes confinement

	public override void Update()
	{
		prep();

		if (state == (int)stateTypes.chasing)
		{
			chasingLogic();
		}
		else if (state == (int)stateTypes.unlocking)
		{
			unlockingLogic();
		}
		else if (state == (int)stateTypes.wandering)
		{
			wanderingLogic();
		}
		else if (state == (int)stateTypes.fleeing)
		{
			fleeingLogic();
		}
		else
		{
			if (time >= unlockingTime)
				state = (int)stateTypes.unlocking;
		}

		time += Time.deltaTime;
	}

	private void unlockingLogic()
	{
		if (transform.position.x <= 16.5f && transform.position.x > 14.5f)
			print(setGoalLeft());
		else if (transform.position.x < 13.5f && transform.position.x >= 11.5f)
			setGoalRight();
		else if (transform.position.y < 19.5f)
			setGoalUp();
		else
		{
			state = (int)stateTypes.wandering;
			int r = Mathf.FloorToInt(2 * Random.value);
			if (r == 0)
				setGoalRight();
			else
				setGoalLeft();
		}

		move();
	}

	private void chasingLogic()
	{
		if (atGoal() && !atCrossroad())
		{
			pathContinue();
		}
		else if (atGoal())
		{
			float x = pacman.transform.position.x - transform.position.x;
			float y = pacman.transform.position.y - transform.position.y;

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

		move();
	}

	private void wanderingLogic()
	{

	}

	private void fleeingLogic()
	{

	}
}
