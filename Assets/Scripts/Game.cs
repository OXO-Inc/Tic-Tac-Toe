using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    char player = 'x', opponent = 'o';
    
    bool gameOver = false;

    int turn = 0;

    public Button[] grids; 

    public Sprite cross;
    public Sprite zero;

    public Text winnerText;
    public Text playerText;
    public Text aiText;
    public Text turnText;

    public GameObject yourTurn;
    public GameObject panel;



    char[,] board = new char[,]
   {
        { '-', '-', '-' },
        { '-', '-', '-' },
        { '-', '-', '-' }
   };

    void Start()
    {
        int isAI = PlayerPrefs.GetInt("isAI");

        if (isAI == 1)
        {
            int option = PlayerPrefs.GetInt("option");

            if (option == 1)
            {
                setComputerAsX();
            }
            else if (option == 2)
            {
                int lastWinner = PlayerPrefs.GetInt("lastWinner", 1);
                if (lastWinner == 0)
                    setComputerAsX();
            }
            else if (option == 3)
            {
                int lastX = PlayerPrefs.GetInt("lastX", 1);
                if (lastX == 1)
                    setComputerAsX();
            }
        }
        else
        {
            playerText.text = "Player 1: X";
            aiText.text = "Player 2: O";
            turnText.text = "Player 1's turn";
        }
    }

    void setComputerAsX()
    {
        playerText.text = "Player: O";
        aiText.text = "AI: X";
        yourTurn.SetActive(false);
        player = 'o';
        opponent = 'x';
        moveByComputer();
    }

    bool isMovesLeft()
    {

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (board[i, j] == '-')
                    return true;
        return false;
    }


    int evaluate(char[,] board)
    {
        // Checking for Rows for X or O victory. 
        for (int row = 0; row < 3; row++)
        {
            if (board[row, 0] == board[row, 1] &&
                board[row, 1] == board[row, 2])
            {
                if (board[row, 0] == 'x')
                    return +10;
                else if (board[row, 0] == 'o')
                    return -10;
            }
        }

        // Checking for Columns for X or O victory. 
        for (int col = 0; col < 3; col++)
        {
            if (board[0, col] == board[1, col] &&
                board[1, col] == board[2, col])
            {
                if (board[0, col] == 'x')
                    return +10;

                else if (board[0, col] == 'o')
                    return -10;
            }
        }

        // Checking for Diagonals for X or O victory. 
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            if (board[0, 0] == 'x')
                return +10;
            else if (board[0, 0] == 'o')
                return -10;
        }

        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            if (board[0, 2] == 'x')
                return +10;
            else if (board[0, 2] == 'o')
                return -10;
        }

        if (isMovesLeft() == false)
            return 0;
        else
            return -1; 
    }

    /*
    void printBoard(char[,] board)
    {
        string temp;
        for (int i = 0; i < 3; i++)
        {
            temp = "";
            for (int j = 0; j < 3; j++)
            {
                temp = temp + " " + board[i, j] + " ";
            }
            Debug.Log(temp);
        }
        Debug.Log(" ");
    }
    */

    int minimax(char[,] board, int depth, bool isMaximizing)
    {
        int eval = evaluate(board);
        if (eval != -1)
        {
            return eval;
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == '-')
                    {
                        board[i, j] = 'x';
                        int score = minimax(board, depth + 1,false);
                        board[i, j] = '-';
                        bestScore = Math.Max(score, bestScore);
                    }
                }
            }

            return bestScore;

        }

        else
        {
            int bestScore = int.MaxValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == '-')
                    {
                        board[i, j] = 'o';
                        int score = minimax(board, depth + 1, true);
                        board[i, j] = '-';
                        bestScore = Math.Min(score, bestScore);
                    }
                }
            }

            return bestScore;
        }
    }


    public void moveByPlayer(string i)
    {
        turn += 1;

        Handheld.Vibrate();
        string[] indices = i.Split(' ');
        int x = int.Parse(indices[0]);
        int y = int.Parse(indices[1]);
        int index = int.Parse(indices[2]);

        board[x, y] = player;

        if (player == 'x' && PlayerPrefs.GetInt("isAI") == 1)
            grids[index].GetComponent<Image>().sprite = cross;
        else if (player == 'o' && PlayerPrefs.GetInt("isAI") == 1)
            grids[index].GetComponent<Image>().sprite = zero;
        else if (turn % 2 == 1 && PlayerPrefs.GetInt("isAI") != 1)
            grids[index].GetComponent<Image>().sprite = cross;
        else
        {
            grids[index].GetComponent<Image>().sprite = zero;
            board[x, y] = opponent;
        }

       
       
        grids[index].interactable = false;

        if (PlayerPrefs.GetInt("isAI") == 1)
            moveByComputer();
        else
        {
            if(turn%2 == 1)
                turnText.text = "Player 2's turn";
            else
                turnText.text = "Player 1's turn";
        }
    }


    void moveByComputer()
    {
        if (PlayerPrefs.GetInt("isAI") == 1)
            yourTurn.SetActive(false);

        int bestScore = int.MaxValue;
        int bestX = -1, bestY = -1;
        int c = 0;
        int index = -1;
        bool isMax;

        if (opponent == 'x')
        {
            isMax = false;
            bestScore = int.MinValue;
        }
        else
            isMax = true;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == '-')
                {
                    board[i, j] = opponent;
                    int score = minimax(board, 0, isMax);
                    board[i, j] = '-';
                    if (isMax && score < bestScore)
                    {
                        index = c;
                        bestScore = score;
                        bestX = i;
                        bestY = j;
                    }
                    else if (isMax == false && score > bestScore)
                    {
                        index = c;
                        bestScore = score;
                        bestX = i;
                        bestY = j;
                    }
                }

                c += 1;
            }
        }

        board[bestX, bestY] = opponent;

        if (opponent == 'x')
            grids[index].GetComponent<Image>().sprite = cross;
        else
            grids[index].GetComponent<Image>().sprite = zero;

        grids[index].interactable = false;

    }

    //TODO: Make button anim: small to big, or fade in

    IEnumerator setPanelActive()
    {
        yield return new WaitForSeconds(.85f);
        panel.SetActive(true);
    }


    void Update()
    {

        if (gameOver)
        {
            foreach(Button elem in grids)
            {
                elem.interactable = false;
            }

            if (player == 'x')
                PlayerPrefs.SetInt("lastX", 1);
            else
                PlayerPrefs.SetInt("lastX", 0);

            StartCoroutine(setPanelActive());
            
            return;
        }

        int eval = evaluate(board);

        if(eval == 10)
        {
            int isAI = PlayerPrefs.GetInt("isAI");

            if (isAI == 1)
            {
                if (player == 'x')
                {
                    winnerText.text = "You Win";
                    PlayerPrefs.SetInt("lastWinner", 1);
                }
                else
                {
                    winnerText.text = "You Lose";
                    PlayerPrefs.SetInt("lastWinner", 0);

                }
            }
            else
            {
                winnerText.text = "Winner: Player 1";
            }

            gameOver = true;
            return;
        }
        else if(eval == -10)
        {
            int isAI = PlayerPrefs.GetInt("isAI");

            if (isAI == 1)
            {
                if (player == 'o')
                {
                    winnerText.text = "You Win";
                    PlayerPrefs.SetInt("lastWinner", 1);
                }
                else
                {
                    winnerText.text = "You Lose";
                    PlayerPrefs.SetInt("lastWinner", 0);

                }
            }
            else
            {
                winnerText.text = "Winner: Player 2";
            }

            gameOver = true;
            return;
        }
        else if(eval == 0)
        {
            winnerText.text = "TIE";
            PlayerPrefs.SetInt("lastWinner", 1);
            gameOver = true;
            return;
        }

        if (isMovesLeft() == false)
        {
            gameOver = true;
            return;
        }

    }

    public void retry()
    {
        SceneManager.LoadScene("Game");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
