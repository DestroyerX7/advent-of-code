namespace AdventOfCode.Year2022.Day02;

public class Strategy
{
    // Added in 2025 because I didn't save my code for part 1 originally
    public static int ScorePartOne(char opponentMove, char yourMove)
    {
        int moveValue = yourMove == 'X' ? 1 : yourMove == 'Y' ? 2 : 3;

        if ((yourMove == 'X' && opponentMove == 'C') || (yourMove == 'Y' && opponentMove == 'A') || (yourMove == 'Z' && opponentMove == 'B'))
        {
            return moveValue + 6;
        }
        else if ((yourMove == 'X' && opponentMove == 'A') || (yourMove == 'Y' && opponentMove == 'B') || (yourMove == 'Z' && opponentMove == 'C'))
        {
            return moveValue + 3;
        }
        else
        {
            return moveValue;
        }
    }

    public static int ScorePartTwo(char opponentMove, char endResult)
    {
        int score = 0;
        bool won = false;
        bool draw = false;

        switch (endResult)
        {
            case 'X':
                break;
            case 'Y':
                score += 3;
                draw = true;
                break;
            case 'Z':
                score += 6;
                won = true;
                break;
            default:
                break;
        }

        if (won)
        {
            switch (opponentMove)
            {
                case 'A':
                    score += 2;
                    break;
                case 'B':
                    score += 3;
                    break;
                case 'C':
                    score++;
                    break;
                default:
                    break;
            }
        }
        else if (draw)
        {
            switch (opponentMove)
            {
                case 'A':
                    score++;
                    break;
                case 'B':
                    score += 2;
                    break;
                case 'C':
                    score += 3;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (opponentMove)
            {
                case 'A':
                    score += 3;
                    break;
                case 'B':
                    score++;
                    break;
                case 'C':
                    score += 2;
                    break;
                default:
                    break;
            }
        }

        return score;
    }
}
