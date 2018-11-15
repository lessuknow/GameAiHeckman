using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBrains : MonoBehaviour
{

    public enum phase { scatter, chase, run, leave, idle };
    private phase ps;
    private phase prev_phase;
    private int phase_num = 0;
    public Ghost red, orange, pink, purple;
    private Sprite rd, or, pi, p;
    public Sprite bl;
    public PacMaster pac;
    private int scatter_time = 7, chase_time = 20, count = 0;
    private int red_powerup = 0;
    public int dot_num;
    private int dot_threshold_one, dot_threshold_two;
    private bool am_run = false;

    private void Start()
    {
        ps = phase.leave;
        Invoke("SetScatter", 1.5f);
        dot_threshold_one = dot_num / 3 * 2;
        dot_threshold_two = dot_num / 3;
        rd = red.GetComponent<SpriteRenderer>().sprite;
        or = orange.GetComponent<SpriteRenderer>().sprite;
        pi = pink.GetComponent<SpriteRenderer>().sprite;
        p = purple.GetComponent<SpriteRenderer>().sprite;
    }


    private void SetChase()
    {
        ps = phase.chase;
        if(count < 4)
            Invoke("SetScatter", chase_time);
    }

    private void SetScatter()
    {
        count++;
        if (count > 3)
            scatter_time = 5;
        Invoke("SetChase", scatter_time);
        ps = phase.scatter;
    }

    private void SetRun()
    {
        red.GetComponent<SpriteRenderer>().sprite = bl;
        orange.GetComponent<SpriteRenderer>().sprite = bl;
        pink.GetComponent<SpriteRenderer>().sprite = bl;
        purple.GetComponent<SpriteRenderer>().sprite = bl;
        prev_phase = ps;
        ps = phase.run;
        CancelInvoke();

        DoScatter();
    }

    private void StopRun()
    {
        red.GetComponent<SpriteRenderer>().sprite = rd;
        orange.GetComponent<SpriteRenderer>().sprite = or;
        pink.GetComponent<SpriteRenderer>().sprite = pi;
        purple.GetComponent<SpriteRenderer>().sprite = p;
        ps = prev_phase;
        if(ps == phase.scatter)
            Invoke("SetChase", scatter_time);
        else
            Invoke("SetScatter", chase_time);

    }

    private void DoScatter()
    {
        if(red_powerup == 0)
        { 
            if(red.atCrossroad())
            { 
                if(!red.setGoalUp())
                {
                    if (!red.setGoalLeft())
                        if (!red.setGoalDown())
                            red.setGoalRight();
                }
            }
        }
        else
        {
            if (red.atCrossroad())
            {
                if ((int)red.transform.position.x > (int)pac.transform.position.x)
                {
                    red.setGoalLeft();
                }
                else if ((int)red.transform.position.x < (int)pac.transform.position.x)
                {
                    red.setGoalRight();
                }
                else if ((int)red.transform.position.y < (int)pac.transform.position.y)
                {
                    red.setGoalUp();
                }
                else
                {
                    red.setGoalDown();
                }
            }
        }
        if (orange.atCrossroad())
        { 
            if (!orange.setGoalUp())
            {
                if (!orange.setGoalRight())
                    if (!orange.setGoalLeft())
                        orange.setGoalDown();
            }
        }
        if(pink.atCrossroad())
        { 
            if (!pink.setGoalDown())
            {
                if (!pink.setGoalLeft())
                    if (!pink.setGoalRight())
                        pink.setGoalUp();
            }
        }

        if (purple.atCrossroad())
        {
            if (!purple.setGoalLeft())
            {
                if (!purple.setGoalDown())
                    if (!purple.setGoalRight())
                        purple.setGoalUp();
            }
        }
    }

    private void DoChase()
    {
        if (red.atCrossroad())
        {
            if ((int)red.transform.position.x > (int)pac.transform.position.x)
            {
                red.setGoalLeft();
            }
            else if ((int)red.transform.position.x < (int)pac.transform.position.x) 
            {
                red.setGoalRight();
            }
            else if ((int)red.transform.position.y < (int)pac.transform.position.y)
            {
                red.setGoalUp();
            }
            else
            {
                red.setGoalDown();
            }
        }
        if (pink.atCrossroad())
        {
            Vector3Int tmpPos = new Vector3Int((int)pac.transform.position.x,
                (int)pac.transform.position.y,
                (int)pac.transform.position.z);
            if (pac.dir == 0)
            {
                //BUFFER OVERFLOW
                tmpPos.x -= 3;
                tmpPos.y+=3;
            }
            if (pac.dir == 1)
                tmpPos.x -= 3;
            if (pac.dir == 2)
                tmpPos.y -= 3;
            else
                tmpPos.x += 3;

            if ((int)pink.transform.position.x > (int)tmpPos.x)
            {
                pink.setGoalLeft();
            }
            else if ((int)pink.transform.position.x < (int)tmpPos.x)
            {
                pink.setGoalRight();
            }
            else if ((int)pink.transform.position.y < (int)tmpPos.y)
            {
                pink.setGoalUp();
            }
            else
            {
                pink.setGoalDown();
            }
        }
        if (purple.atCrossroad())
        {
            Vector3Int tmpPos = new Vector3Int((int)pac.transform.position.x,
                (int)pac.transform.position.y,
                (int)pac.transform.position.z);
            if (pac.dir == 0)
            {
                tmpPos.y += 2;
            }
            if (pac.dir == 1)
                tmpPos.x -= 2;
            if (pac.dir == 2)
                tmpPos.y -= 2;
            else
                tmpPos.x += 2;

            Vector3Int tmpAdd = new Vector3Int();
            tmpAdd.x = (int)(tmpPos.x - red.transform.position.x);
            tmpAdd.y = (int)(tmpPos.y - red.transform.position.y);
            tmpPos += tmpAdd;

            if ((int)purple.transform.position.x > (int)tmpPos.x)
            {
                purple.setGoalLeft();
            }
            else if ((int)purple.transform.position.x < (int)tmpPos.x)
            {
                purple.setGoalRight();
            }
            else if ((int)purple.transform.position.y < (int)tmpPos.y)
            {
                purple.setGoalUp();
            }
            else
            {
                purple.setGoalDown();
            }
        }
        if (orange.atCrossroad())
        {
            if((int)Vector3.Distance(orange.transform.position, pac.transform.position) > 8)
            {
                if ((int)orange.transform.position.x > (int)pac.transform.position.x)
                {
                    orange.setGoalLeft();
                }
                else if ((int)orange.transform.position.x < (int)pac.transform.position.x)
                {
                    orange.setGoalRight();
                }
                else if ((int)orange.transform.position.y < (int)pac.transform.position.y)
                {
                    orange.setGoalUp();
                }
                else
                {
                    orange.setGoalDown();
                }
            }
            else
            {
                if (orange.atCrossroad())
                {
                    if (!orange.setGoalUp())
                    {
                        if (!orange.setGoalRight())
                            if (!orange.setGoalLeft())
                                orange.setGoalDown();
                    }
                }
            }

        }
    }



    private void DoLeaveTop()
    {
        if(!purple.passed_door)
            if (!purple.setGoalUp())
                purple.setGoalRight();
        if(!red.passed_door)
            if (!red.setGoalUp())
                red.setGoalLeft();
    }

  

    private void DoLeaveBottom()
    {
        if (!orange.passed_door)
            if (!orange.setGoalUp())
                orange.setGoalRight();

        if (!pink.passed_door)
            if (!pink.setGoalUp())
                pink.setGoalLeft();
    }

    private void DoRun()
    {
        if (red.atCrossroad())
        {
            if ((int)red.transform.position.x < (int)pac.transform.position.x)
            {
                red.setGoalLeft();
            }
            else if ((int)red.transform.position.x > (int)pac.transform.position.x)
            {
                red.setGoalRight();
            }
            else if ((int)red.transform.position.y > (int)pac.transform.position.y)
            {
                red.setGoalUp();
            }
            else
            {
                red.setGoalDown();
            }
        }
        if (orange.atCrossroad())
        {
            if ((int)orange.transform.position.x < (int)pac.transform.position.x)
            {
                orange.setGoalLeft();
            }
            else if ((int)orange.transform.position.x > (int)pac.transform.position.x)
            {
                orange.setGoalRight();
            }
            else if ((int)orange.transform.position.y > (int)pac.transform.position.y)
            {
                orange.setGoalUp();
            }
            else
            {
                orange.setGoalDown();
            }
        }
        if (pink.atCrossroad())
        {
            if ((int)pink.transform.position.x < (int)pac.transform.position.x)
            {
                pink.setGoalLeft();
            }
            else if ((int)pink.transform.position.x > (int)pac.transform.position.x)
            {
                pink.setGoalRight();
            }
            else if ((int)pink.transform.position.y > (int)pac.transform.position.y)
            {
                pink.setGoalUp();
            }
            else
            {
                pink.setGoalDown();
            }
        }
        if (purple.atCrossroad())
        {
            if ((int)purple.transform.position.x < (int)pac.transform.position.x)
            {
                purple.setGoalLeft();
            }
            else if ((int)purple.transform.position.x > (int)pac.transform.position.x)
            {
                purple.setGoalRight();
            }
            else if ((int)purple.transform.position.y > (int)pac.transform.position.y)
            {
                purple.setGoalUp();
            }
            else
            {
                purple.setGoalDown();
            }
        }
    }

    private void Update()
    {
        if(pac.is_super == true && am_run == false)
        {
            am_run = true;
            SetRun();
        }
        if(am_run == true && pac.is_super == false)
        {
            am_run = false;
            StopRun();
        }

        //For red to go SUPER aka Cruse Elroy (???)
        if (red_powerup == 0)
            if (pac.dots_eaten > dot_threshold_one)
            {
                red_powerup++;
                red.speed *= 1.05f;
            }
        if (red_powerup == 1)
            if (pac.dots_eaten > dot_threshold_two)
            {
                red_powerup++;
                red.speed *= 1.05f;
            }

        switch (ps)
        {
            case phase.scatter:
                DoScatter();
                break;
            case phase.chase:
                DoChase();
                break;
            case phase.run:
                DoRun();
                break;
            case phase.idle:
                return;
            case phase.leave:
                DoLeaveTop();
                DoLeaveBottom();
                break;
        }
 
            red.prep();
            if (red.atGoal())
            {
                red.pathContinue();
            }
            red.ghostCollisions();
            red.move();

            purple.prep();
            if (purple.atGoal())
            {
                purple.pathContinue();
            }
            purple.ghostCollisions();
            purple.move();
     
            orange.prep();
            if (orange.atGoal())
            {
                orange.pathContinue();
            }
            orange.ghostCollisions();
            orange.move();

            pink.prep();
            if (pink.atGoal())
            {
                pink.pathContinue();
            }
            pink.ghostCollisions();
            pink.move();
        

    }
    
}
