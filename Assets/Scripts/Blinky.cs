using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
	private enum stateTypes { locked, unlocking, chasing, fleeing, wandering };
	private int state = (int)stateTypes.unlocking;
	private float time = 0;

	public override void Update()
	{
		prep();

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

	private void unlockingLogic()
	{

	}

	private void chasingLogic()
	{

	}

	private void wanderingLogic()
	{

	}

	private void fleeingLogic()
	{

	}
}
