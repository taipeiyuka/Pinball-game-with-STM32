using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public int score;
    public int bet;
    public int scale;
    public Text scoretext;
    public Text bettext;
    public Globals globe;
    // Start is called before the first frame update
    void Start()
    {
        score = 10;
        bet = 0;
        scale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (score <= 0 && globe.now == Globals.OP.WAIT_4_BUTTON)
        {
            scoretext.text = "GAME OVER\n";
            bettext.text = "Press\n" + "the\n" + "button\n" + "to\n" + "restart\n";
            globe.now = Globals.OP.GAME_OVER;
        }
        else if (globe.now == Globals.OP.GAME_OVER) {
            scoretext.text = "GAME OVER\n";
            bettext.text = "Press\n" + "the\n" + "button\n" + "to\n" + "restart\n";
        }
        else
        {
            scoretext.text = "Score:\n" + score;
            bettext.text = "Bet:\n" + bet + " * " + scale;
        }
        
    }
}
