using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {

        static void Main(string[] args)
        {
            //good spot to call a graphic interface

            //declare int array to handle to the move that is being tested
            int[] move = new int[10];

            //dummy is an array handed when there is no need to back test to make sure a bad 
            //positionis reused.
            int[] dummy = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            //call methode to get unorganized state from the user
            move = UserInput(move);

            //new moves are compared and the highes scoreing move that hasn't already
            //been used is added to the solutions list. if all possible moves are previously used
            //or on the avoid list, highest scoring move is passed to the CompareMove method to evaluated.
            //this method searches back to find a higher, or next highest score, and modifies the 
            //solutions list to remove future moves
            //eg we have moves 1,2,3,4,5,6,7,8,9 and we have to go back to move 4 we want to remove
            //5,6,7,8,9 from the solutions list. 


            List<int[]> solution = new List<int[]>();
            solution.Add(move);



            //if puzzle is unsolved keep generating solutions.
            //check if the move has been generated before an if so moves back one position from the first
            //time it was called and starts again asking the generateMove method not to return that result
            //again. 
            // then it removes any moves on the solutions list past the one that let to a solutions loop.

            while (move[9] < 10000)
            {
                bool moveIsDuplicate = false;
                int duplicateIndex = 0;
                for (int i = 1; i < solution.Count; i++)
                {
                    int count = 0;
                    for (int t = 0; t < 9; t++)
                    {
                        if (move[t] == solution[i][t])
                        {
                            count++;
                        }
                        if (count == 9)
                        {
                            moveIsDuplicate = true;
                            duplicateIndex = i;

                            break;
                        }
                    }
                    if (moveIsDuplicate == true)
                    {
                        break;
                    }
                }
                if (moveIsDuplicate == true)
                {
                    move = GenerateMove(solution[duplicateIndex - 1], move);
                    Console.WriteLine("calling for a non repeating solution");
                }
                if (moveIsDuplicate == false)
                {
                    solution.Add(move);
                    move = GenerateMove(move, dummy);
                    for (int u = 0; u < 10; u++)
                    {
                        Console.Write(move[u]);
                    }
                    Console.WriteLine();
                    Console.WriteLine("calling for next move");
                    Console.ReadKey();
                }
            }
            //if score indicates puzzle is solved print solution.
            if (move[9] >= 10000)
            {
                Console.WriteLine(solution.Count);
                //this is a logic only excersise if you wanted to add an animation or finished image
                //this would be the place call put.

                //for each move array in the solution list print the array
                for (int i = 0; i < solution.Count; i++)
                {
                    for (int t = 0; t < 9; t++)
                    {
                        Console.Write(solution[i][t]);
                    }
                    Console.WriteLine();
                    Console.ReadKey();
                }
                Console.ReadKey();
            }

        }

        public static int[] UserInput(int[] Start)
        {
            //here we get the initial starting point from the user
            bool validEntry = false;

            //the while loop keeps the user at the input stage until a valid entry is submitted
            while (validEntry == false)
            {
                Console.WriteLine("Please enter the numbers");
                Console.WriteLine("0 through 8 and hit Enter");
                Console.WriteLine("be sure to use each number");
                Console.WriteLine("only once.");

                String getStart = Console.ReadLine();
                int[] testList = new int[9] { 8, 7, 6, 5, 4, 3, 2, 1, 0 };

                int testCount = 0;




                //starting position is converted from a character string '876543210' to an int32(number) 
                //array [8,7,6,5,4,3,2,1,0]. First position is scored and added to the solutions list.
                //First position is returned to the main method to be handed off to the GenerateMove method
                //Start[] has 10 positions so that the score can be kept with the move on the used
                //moves list. 

                //check to see if the entered string contains 0 through 8 then convert to int array

                if (getStart.Length == 9)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        Start[i] = Convert.ToInt32(getStart[i] - '0');
                    }
                    for (int z = 0; z < 9; z++)
                    {

                        for (int t = 0; t < 9; t++)
                        {
                            if (Start[z] == testList[t])
                            {
                                testCount++;

                            }
                        }
                    }
                    if (testCount == 9)
                    {
                        validEntry = true;
                        Heuristic(Start);

                    }

                    //incorrect starting position is rejected
                    else
                    {
                        Console.WriteLine("Must contain the numbers 0-8");
                        Console.ReadKey();
                    }

                }

                //incorrect starting position is rejected
                else
                {
                    Console.WriteLine("Must be nine didgets.");
                    Console.ReadKey();
                }

            }
            return Start;
        }
        public static int[] Heuristic(int[] unscoredMove)
        {

            //This is the scoring hueristic. Writen specifically for an 8-puzzle (as opposed to an n-puzzle).

            //Points are awarded for getting closer the the goal position 
            // corner, edge, center (number moves from goal position)
            // 012     101   212
            // 123     212   101
            // 234     323   212  
            //larger n-puzzles have a higher risk of getting stuck in score loops and it can be more practical
            //to solve for positions further away from 0/0 (the goal position of our emptly tile on the puzzle)
            // xxxxx
            // xxxxx
            // xxxxx
            // xxxxx
            // xxxx24
            //24 should be the top left tile so if we start with 24 then go down the arraylooking for the 
            //next highest tile out of place the hueristic can provide more direct feedback. 
            //this approach might not be necessary unless you're doing a large puzzle or a user defined puzzle 
            //size. it's fun to experiment with and see at what point you start savings moves with 
            //different approaches. 

            //points are also awarded for achieving certain 
            //subset positions effectively locking in that subset and reduce the options going forward.
            //An N-puzzle only needs to be a 2x3 grid to be solvable so anytime a row (or column) is solved
            //we can insentivise the maintenence of those subsets, no matter how large the puzzle, and
            //eliminate the need to touch those pieces again as long as the remaining unorganized set is
            //at least 2x3.
            //In this Hueristic higher scores are awarded for a position where the top row is solved. 
            //you could write it with either the top row or the outside column scoring higher.
            // xxx      x76
            // 543  or  x43
            // 210      x10
            //on an n-puzzle that is larger you can insetivise both rows and colums leaving the last
            //2x3 scored on distance.
            // xxxxx
            // xxxxx
            // xxxxx
            // xx765
            // xx210
            //an outsized award is offered for a complete solution so the computer can easily check
            //for a final solution.

            //this hueristic only checks for a score adds it to the move and returns.
            int[] scoredMove = new int[10];
            int score = 0;

            //once the eight tile is in the right spot it doesn't need to move again
            //once 876 are in place they don't need to move. near intermediat moves
            //betweeen 8?? and 876 are incentivised. 
            if (unscoredMove[0] == 8)
            {
                score = score + 1000;
                if (unscoredMove[1] == 7 && unscoredMove[2] == 6)
                {
                    score = score + 1000;
                }
                if (unscoredMove[4] == 7 && unscoredMove[2] == 6)
                {
                    score = score + 700;
                }
                if (unscoredMove[4] == 7 && unscoredMove[1] == 6)
                {
                    score = score + 500;
                }
                if (unscoredMove[3] == 7 && unscoredMove[1] == 6)
                {
                    score = score + 300;
                }
            }
            //points for distance away from 8's goal position
            if (unscoredMove[1] == 8 || unscoredMove[3] == 8)
            {
                score = score + 4;
            }
            if (unscoredMove[6] == 8 || unscoredMove[4] == 8 || unscoredMove[2] == 8)
            {
                score = score + 3;
            }
            if (unscoredMove[7] == 8 || unscoredMove[5] == 8)
            {
                score = score + 2;
            }

            //points away from 6's goal position
            if (unscoredMove[2] == 6)
            {
                score = score + 4;
            }
            if (unscoredMove[1] == 6 || unscoredMove[5] == 6)
            {
                score = score + 3;
            }
            if (unscoredMove[0] == 6 || unscoredMove[4] == 6 || unscoredMove[8] == 6)
            {
                score = score + 2;
            }
            if (unscoredMove[7] == 6 || unscoredMove[3] == 6)
            {
                score = score + 1;
            }

            //points away from 2's goal position
            if (unscoredMove[6] == 2)
            {
                score = score + 4;
            }
            if (unscoredMove[3] == 2 || unscoredMove[7] == 2)
            {
                score = score + 3;
            }
            if (unscoredMove[0] == 2 || unscoredMove[4] == 2 || unscoredMove[8] == 2)
            {
                score = score + 2;
            }
            if (unscoredMove[1] == 2 || unscoredMove[5] == 2)
            {
                score = score + 1;
            }

            //points away from 7's goal position
            if (unscoredMove[1] == 7)
            {
                score = score + 4;
            }
            if (unscoredMove[0] == 7 || unscoredMove[2] == 7 || unscoredMove[4] == 7)
            {
                score = score + 3;
            }
            if (unscoredMove[3] == 7 || unscoredMove[5] == 7 || unscoredMove[7] == 7)
            {
                score = score + 2;
            }

            //points away from 3's goal position
            if (unscoredMove[5] == 3)
            {
                score = score + 4;
            }
            if (unscoredMove[2] == 3 || unscoredMove[4] == 3 || unscoredMove[8] == 3)
            {
                score = score + 3;
            }
            if (unscoredMove[1] == 3 || unscoredMove[3] == 3 || unscoredMove[7] == 3)
            {
                score = score + 2;
            }

            //points away from 1's goal position
            if (unscoredMove[7] == 1)
            {
                score = score + 4;
            }
            if (unscoredMove[6] == 1 || unscoredMove[4] == 1 || unscoredMove[8] == 1)
            {
                score = score + 3;
            }
            if (unscoredMove[1] == 1 || unscoredMove[3] == 1 || unscoredMove[5] == 1)
            {
                score = score + 2;
            }

            //points away from 5's goal position
            if (unscoredMove[7] == 5)
            {
                score = score + 4;
            }
            if (unscoredMove[6] == 5 || unscoredMove[4] == 5 || unscoredMove[0] == 5)
            {
                score = score + 3;
            }
            if (unscoredMove[1] == 5 || unscoredMove[7] == 5 || unscoredMove[5] == 5)
            {
                score = score + 2;
            }

            //points away from 4's goal position

            if (unscoredMove[4] == 4)
            {
                score = score + 4;
            }
            if (unscoredMove[1] == 4 || unscoredMove[3] == 4 || unscoredMove[5] == 4 || unscoredMove[7] == 4)
            {
                score = score + 3;
            }
            if (unscoredMove[0] == 4 || unscoredMove[2] == 4 || unscoredMove[6] == 4 || unscoredMove[8] == 4)
            {
                score = score + 2;
            }
            unscoredMove[9] = score;
            return unscoredMove;
        }
        public static int[] GenerateMove(int[] lastMove, int[] dontRepeat)
        {
            //Pass in the most recent move

            //any board arangement can create between two and four new position
            //below a new array is formed for each new possible position
            int[] newMoveOne = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] newMoveTwo = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] newMoveThree = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] newMoveFour = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


            //Moves are determined by which possition in the array 0 occupies
            //0 can change places with positions above or below and to the left or right.
            //Think of the board as having array positions-
            //012           876
            //345   filled  543
            //678           210    

            //Below we check, for each array position, if that position could
            //move either up, down, left or right and whether or not that array
            //position contains a zero. If it does we switch the 0 with nubmer the 
            //corresponding array position, either up a row(+3) down a row(-3) 
            //left (+1) or right (-1).
            //Moves are scored and highest score is returned
            for (int i = 0; i < 9; i++)
            {
                //positions 0-5 can move down +3
                if (i < 6 && lastMove[i] == 0)
                {
                    //fill new move array with last move
                    for (int x = 0; x < 9; x++)
                    {
                        newMoveOne[x] = lastMove[x];
                    }
                    //switch 0 position with the number at the index of 0 plus 3
                    for (int y = 0; y < 6; y++)
                    {
                        if (lastMove[y] == 0)
                        {
                            newMoveOne[y + 3] = lastMove[y];
                            newMoveOne[y] = lastMove[y + 3];
                        }
                    }
                    //score position
                    Heuristic(newMoveOne);
                    int count = 0;
                    for (int t = 0; t < 9; t++)
                    {
                        if (newMoveOne[t] == dontRepeat[t])
                        {
                            count++;
                        }
                    }
                    if (count == 9)
                    {
                        newMoveOne[9] = 0;
                    }


                }
                //positions 3-8 can move up -3
                if (i > 2 && lastMove[i] == 0)
                {
                    //fill new move array with last move
                    for (int x = 0; x < 9; x++)
                    {
                        newMoveTwo[x] = lastMove[x];
                    }
                    //switch 0 position with the number at the index of 0 minus 3
                    for (int y = 0; y < 9; y++)
                    {
                        if (lastMove[y] == 0)
                        {
                            newMoveTwo[y - 3] = lastMove[y];
                            newMoveTwo[y] = lastMove[y - 3];
                        }
                    }
                    //score position
                    Heuristic(newMoveTwo);
                    int count = 0;
                    for (int t = 0; t < 9; t++)
                    {
                        if (newMoveTwo[t] == dontRepeat[t])
                        {
                            count++;
                        }
                    }
                    if (count == 9)
                    {
                        newMoveTwo[9] = 0;
                    }

                }
                //all positions can move right exept those that have a remainder of zero when you
                // % by 3 (0,3,6)
                if (i % 3 != 0 && lastMove[i] == 0)
                {
                    //fill new move array with last move
                    for (int x = 0; x < 9; x++)
                    {
                        newMoveThree[x] = lastMove[x];
                    }
                    //switch 0 position with the number at the index of 0 minus 1
                    for (int y = 0; y < 9; y++)
                    {
                        if (lastMove[y] == 0)
                        {
                            newMoveThree[y - 1] = lastMove[y];
                            newMoveThree[y] = lastMove[y - 1];
                        }
                    }
                    //score position
                    Heuristic(newMoveThree);
                    int count = 0;
                    for (int t = 0; t < 9; t++)
                    {
                        if (newMoveThree[t] == dontRepeat[t])
                        {
                            count++;
                        }
                    }
                    if (count == 9)
                    {
                        newMoveThree[9] = 0;
                    }

                }
                //all positions can move left except those that have a remainder of zero when you 
                //add one to the index and % 3 (2,5,8)
                if ((i + 1) % 3 != 0 && lastMove[i] == 0)
                {
                    //fill new move array with last move
                    for (int x = 0; x < 9; x++)
                    {
                        newMoveFour[x] = lastMove[x];
                    }
                    //switch 0 position with the number at the index of 0 plus 1
                    for (int y = 0; y < 9; y++)
                    {
                        if (lastMove[y] == 0)
                        {
                            newMoveFour[y + 1] = lastMove[y];
                            newMoveFour[y] = lastMove[y + 1];
                        }
                    }
                    //score position
                    Heuristic(newMoveFour);
                    int count = 0;
                    for (int t = 0; t < 9; t++)
                    {
                        if (newMoveFour[t] == dontRepeat[t])
                        {
                            count++;
                        }
                    }
                    if (count == 9)
                    {
                        newMoveFour[9] = 0;
                    }

                }
            }
            //after new moves have been calculated and scores returned the highest scoring move
            //is returned to the method that called it. 
            //some are marked as > and some as >= in case of a tying score
            if (newMoveOne[9] >= newMoveTwo[9] && newMoveOne[9] >= newMoveThree[9] && newMoveOne[9] >= newMoveFour[9])
            {
                Console.WriteLine("return move one");
                Console.ReadKey();
                return newMoveOne;
            }
            else if (newMoveTwo[9] > newMoveOne[9] && newMoveTwo[9] >= newMoveThree[9] && newMoveTwo[9] >= newMoveFour[9])
            {
                Console.WriteLine("return move two");
                Console.ReadKey();
                return newMoveTwo;
            }
            else if (newMoveThree[9] > newMoveTwo[9] && newMoveThree[9] > newMoveOne[9] && newMoveThree[9] >= newMoveFour[9])
            {
                Console.WriteLine("return move three");
                Console.ReadKey();
                return newMoveThree;
            }
            else if (newMoveFour[9] > newMoveTwo[9] && newMoveFour[9] > newMoveThree[9] && newMoveFour[9] > newMoveOne[9])
            {
                Console.WriteLine("return move four");
                Console.ReadKey();
                return newMoveFour;
            }
            else
            {
                Console.WriteLine("return error");
                Console.ReadKey();
                return dontRepeat;
            }
        }
    }
}
