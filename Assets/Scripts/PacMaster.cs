using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMaster : MonoBehaviour
{
	public GameObject pacSide;
	public GameObject pacTop;
	public ParseLevel pl;
	private bool levelSet = false;
	private int[,] levelRef;
	private Vector3 goal;
	private Vector3 invGoal;
	private int dir = 1;
	private float t = 0;
	public float speed = .2f;

	void Start ()
	{
		pacSide.transform.localPosition = new Vector3(0, 0, -5);
	}

	void Update()
	{
		if (!levelSet)
		{
			levelSet = true;
			levelRef = pl.levelArray;
			invGoal = transform.position;
			goal = new Vector3(transform.position.x + 1, transform.position.y, 0);
		}
		
		t += Time.deltaTime;
		transform.position = Vector3.Lerp(invGoal, goal, t / speed);

		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			if (((transform.position.y - .5f) % 1f == 0 && (transform.position.x - .5f) % 1f == 0 &&
				levelRef[(int)(transform.position.x - .5f), (int)(transform.position.y - .5f) + 1] == 1))
			{
				dir = 0;
				t = 0;
			}
		}
		else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			if (((transform.position.y - .5f) % 1f == 0 && (transform.position.x - .5f) % 1f == 0 &&
				levelRef[(int)(transform.position.x - .5f) + 1, (int)(transform.position.y - .5f)] == 1))
			{
				dir = 1;
				t = 0;
			}
		}
		else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			if (((transform.position.y - .5f) % 1f == 0 && (transform.position.x - .5f) % 1f == 0 &&
				levelRef[(int)(transform.position.x - .5f), (int)(transform.position.y - .5f) - 1] == 1))
			{
				dir = 2;
				t = 0;
			}
		}
		else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			if (((transform.position.y - .5f) % 1f == 0 && (transform.position.x - .5f) % 1f == 0 &&
				levelRef[(int)(transform.position.x - .5f) - 1, (int)(transform.position.y - .5f)] == 1))
			{
				dir = 3;
				t = 0;
			}
		}

		if (transform.position == goal || t == 0)
		{
			t = 0;

			if (dir == 0)
			{
				if ((int)(transform.position.y - .5f) + 1 > 31 && (transform.position.y - .5f) % 1f == 0)
					transform.position = new Vector3(transform.position.x, 0, 0);
				if (levelRef[(int)(transform.position.x - .5f), (int)(transform.position.y - .5f) + 1] == 1)
				{
					pacTop.transform.localPosition = new Vector3(0, 0, -5);
					pacSide.transform.localPosition = new Vector3(0, 0, 5);
					pacTop.transform.localScale = new Vector3(1, 1, 1);

					goal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + 1.5f, 0);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, 0);
				}
				else
				{
					goal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) - .5f, 0);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, 0);
					dir = 2;
					pacTop.transform.localPosition = new Vector3(0, 0, -5);
					pacSide.transform.localPosition = new Vector3(0, 0, 5);
					pacTop.transform.localScale = new Vector3(-1, 1, 1);
				}
			}
			else if (dir == 1)
			{
				if ((int)(transform.position.x - .5f) + 1 > 27 && (transform.position.x - .5f) % 1f == 0)
					transform.position = new Vector3(0, transform.position.y, 0);

				if (levelRef[(int)(transform.position.x - .5f) + 1, (int)(transform.position.y - .5f)] == 1)
				{
					pacSide.transform.localPosition = new Vector3(0, 0, -5);
					pacTop.transform.localPosition = new Vector3(0, 0, 5);
					pacSide.transform.localScale = new Vector3(1, 1, 1);

					goal = new Vector3((int)(transform.position.x) + 1.5f, (int)(transform.position.y) + .5f, 0);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, 0);
				}
				else
				{
					goal = new Vector3((int)(transform.position.x) - .5f, (int)(transform.position.y) + .5f, 0);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, 0);
					dir = 3;
					pacSide.transform.localPosition = new Vector3(0, 0, -5);
					pacTop.transform.localPosition = new Vector3(0, 0, 5);
					pacSide.transform.localScale = new Vector3(-1, 1, 1);
				}
			}
			else if (dir == 2)
			{
				if ((int)(transform.position.y - .5f) - 1 < 0 && (transform.position.y - .5f) % 1f == 0)
					transform.position = new Vector3(transform.position.x, 31, 0);

				if (levelRef[(int)(transform.position.x - .5f), (int)(transform.position.y - .5f) - 1] == 1)
				{
					pacTop.transform.localPosition = new Vector3(0, 0, -5);
					pacSide.transform.localPosition = new Vector3(0, 0, 5);
					pacTop.transform.localScale = new Vector3(-1, 1, 1);

					goal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) - .5f, 0);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, 0);
				}
				else
				{
					goal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + 1.5f, 0);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, 0);
					dir = 0;
					pacTop.transform.localPosition = new Vector3(0, 0, -5);
					pacSide.transform.localPosition = new Vector3(0, 0, 5);
					pacTop.transform.localScale = new Vector3(1, 1, 1);
				}
			}
			else
			{
				if ((int)(transform.position.x - .5f) - 1 < 0 && (transform.position.x - .5f) % 1f == 0)
					transform.position = new Vector3(27, transform.position.y, 0);

				if (levelRef[(int)(transform.position.x - .5f) - 1, (int)(transform.position.y - .5f)] == 1)
				{
					pacSide.transform.localPosition = new Vector3(0, 0, -5);
					pacTop.transform.localPosition = new Vector3(0, 0, 5);
					pacSide.transform.localScale = new Vector3(-1, 1, 1);

					goal = new Vector3((int)(transform.position.x) - .5f, (int)(transform.position.y) + .5f, 0);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, 0);
				}
				else
				{
					goal = new Vector3((int)(transform.position.x) + 1.5f, (int)(transform.position.y) + .5f, 0);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, 0);
					dir = 1;
					pacSide.transform.localPosition = new Vector3(0, 0, -5);
					pacTop.transform.localPosition = new Vector3(0, 0, 5);
					pacSide.transform.localScale = new Vector3(1, 1, 1);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Pellet")
		{
			Destroy(collision.gameObject);
		}
		if (collision.tag == "Death")
		{
			Destroy(gameObject);
		}
	}
}