using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    //public GameObject[] target = new GameObject[7];
    public SpriteRenderer[] target;// = new SpriteRenderer[7];
    public SpriteRenderer[] getpoint;
    public Globals globe;
    public ScoreBoard score;
    // Start is called before the first frame update
    void Start()
    {
        for (int i=0;i<7;i++) {
            //target[i] = GameObject.Find(i.ToString());
            //target[i].SetActive(false);
            //target[i] = GetComponent<SpriteRenderer>();
            target[i].enabled = false;
            getpoint[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (globe.now == Globals.OP.WAIT_4_LIGHT)
        {
            Debug.Log("on");
            for (int i = 0;i < 7; i++) {
                target[i].enabled = false;
                getpoint[i].enabled = false;
            }
            for (int i = 0; i < Random.Range(1, 5); i++)
            {
                target[Random.Range(0, 7)].enabled = true;
            }
            globe.now = Globals.OP.WAIT_4_VELOCITY;
            int count = 0;
            for (int i =0;i<7;i++) {
                if (target[i].enabled) {
                    count++;
                }
            }
            score.scale = 2*(5-count);
        }
        if (globe.now == Globals.OP.WAIT_4_END) {
            if (GameObject.FindGameObjectWithTag("ball").transform.position.y < -4.5) {
                if (GameObject.FindGameObjectWithTag("ball").transform.position.x < -3.7)
                {
                    if (target[0].enabled == true)
                    {
                        //get point
                        getpoint[0].enabled = true;
                        score.score += score.scale * score.bet;
                    }
                    globe.now = Globals.OP.WAIT_4_BUTTON;
                }
                else if (GameObject.FindGameObjectWithTag("ball").transform.position.x < -2.4)
                {
                    if (target[1].enabled == true)
                    {
                        //get point
                        getpoint[1].enabled = true;
                        score.score += score.scale * score.bet;
                    }
                    globe.now = Globals.OP.WAIT_4_BUTTON;
                }
                else if (GameObject.FindGameObjectWithTag("ball").transform.position.x < -1.2)
                {
                    if (target[2].enabled == true)
                    {
                        //get point
                        getpoint[2].enabled = true;
                        score.score += score.scale * score.bet;
                    }
                    globe.now = Globals.OP.WAIT_4_BUTTON;
                }
                else if (GameObject.FindGameObjectWithTag("ball").transform.position.x < 0)
                {
                    if (target[3].enabled == true)
                    {
                        //get point
                        getpoint[3].enabled = true;
                        score.score += score.scale * score.bet;
                    }
                    globe.now = Globals.OP.WAIT_4_BUTTON;
                }
                else if (GameObject.FindGameObjectWithTag("ball").transform.position.x < 1.2)
                {
                    if (target[4].enabled == true)
                    {
                        //get point
                        getpoint[4].enabled = true;
                        score.score += score.scale * score.bet;
                    }
                    globe.now = Globals.OP.WAIT_4_BUTTON;
                }
                else if (GameObject.FindGameObjectWithTag("ball").transform.position.x < 2.4)
                {
                    if (target[5].enabled == true)
                    {
                        //get point
                        getpoint[5].enabled = true;
                        score.score += score.scale * score.bet;
                    }
                    globe.now = Globals.OP.WAIT_4_BUTTON;
                }
                else if (GameObject.FindGameObjectWithTag("ball").transform.position.x < 3.4)
                {
                    if (target[6].enabled == true)
                    {
                        //get point
                        getpoint[6].enabled = true;
                        score.score += score.scale * score.bet;
                    }
                    globe.now = Globals.OP.WAIT_4_BUTTON;
                }
                else {
                    globe.now = Globals.OP.WAIT_4_VELOCITY;
                }
            }
        }
    }
}
