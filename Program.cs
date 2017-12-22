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
            //declare int array to handle moves that are going to be used and added to the 
            //solutions list
            //index 0-8 are tile positions, 9 is score, 10 is weather or not the move has 
            //been parent already so the tree doesn't get stuck in a loop and 11 is the index  
            //of the moves parent  
            int[] move = new int[12];

            // an array to store the first move until the puzzle is solved
            int[] firstMove = new int[12];

            //call method to get unorganized state from the user
            move = UserInput(move);

            //set first move to be the actual first move
            firstMove = move;
            //solution tree list
            List<int[]> solution = new List<int[]>();

            //Final Move List list without unused moves
            List<int[]> finalMoveList = new List<int[]>();

            //2d array used to evaluate the possible moves
            int[,] possibleMoves = new int[4, 12];


            // first move will be a part of parent solution list
            move[10] = 1;

            //add the starting state to the solutions lisit
            solution.Add(move);

            //each time a move is chosen the index of that move in the solution tree is stored
            //here and added to the next chosen move to establish a link backwards from the 
            //solution to the initial state
            int parentIndex = new int();

            //variable to contain score to determine if organized solution is complete
            int score = new int();

            //variable to indicate if score has passed 2000 indicating that the top row is complete
            //and that the Choosemove method should not move those tiles.
            bool fixTopRow = false;

            //establishing an initial score for the while loop to evaluate based on the initial 
            //state entered by the user
            score = move[9];

            //keep searching for solution as long as target position isn't achieved
            while (score < 2300)
            {
                //method called to get array of possible moves
                possibleMoves = GenerateMove(move, parentIndex);

                //of the array of possible moves any that are eligible to add
                // to the solutions list are added
                solution = AmendSolutionsList(solution, possibleMoves);

                //next move is chosen based on highest scoring not yet used move
                //fixTopRow is passed to elimiate moving the top row
                parentIndex = ChooseMove(solution, fixTopRow);

                //once the move is chosen the sub array move 10 is changed to 1 to
                //indicate that it is being used as a parent
                solution[parentIndex][10] = 1;

                //if the score of the chosen move is over 2000 change fixTopRow to
                //true to keep the selection of future moves close to an organized solution
                if (solution[parentIndex][9] > 2000)
                {
                    fixTopRow = true;
                }

                //score value is updated to most recent move score to inform while loop usage
                score = solution[parentIndex][9];

                //move is changed to the most recent chosen move and fed back into the while
                //loop for the next round of possible moves
                for (int y = 0; y < 12; y++)
                {
                    move[y] = solution[parentIndex][y];
                }
            }

            //if score indicates puzzle is solved print solution.
            if (score > 2299)
            {

                //call the method to remove unused moves
                finalMoveList = RemoveUnusedMoves(solution, finalMoveList);

                //writing the first move
                // having the three loops allows for the display of the consol app
                // to look like the puzzle. 
                for (int w = 0; w < 3; w++)
                    {
                        Console.Write(firstMove[w]);
                    }
                    Console.WriteLine();
                    for (int n = 3; n < 6; n++)
                    {
                        Console.Write(firstMove[n]);
                    }
                    Console.WriteLine();
                    for (int m = 6; m < 9; m++)
                    {
                        Console.Write(firstMove[m]);
                    }
                    Console.WriteLine();
                    Console.WriteLine();

                //for each move array in the final moves list print the array

                for (int i = 0; i < finalMoveList.Count; i++)
                {
                    for (int t = 0; t < 3; t++)
                    {
                        Console.Write(finalMoveList[i][t]);
                    }
                    Console.WriteLine();
                    for (int t = 3; t < 6; t++)
                    {
                        Console.Write(finalMoveList[i][t]);
                    }
                    Console.WriteLine();
                    for (int t = 6; t < 9; t++)
                    {
                        Console.Write(finalMoveList[i][t]);
                    }
                    Console.WriteLine();
                    Console.WriteLine();

                }

                // uncomment this and comment the section above if you want a single line display
                //for (int i = 0; i < finalMoveList.Count; i++)
                //{
                //    for (int t = 0; t < 9; t++)
                //    {
                //        Console.Write(finalMoveList[i][t]);
                //    }
                //    Console.WriteLine();
                //}

                    Console.ReadKey();

            }
        }

        // method for returing a list of moves that are used and removing the 
        // ones that aren't used. solution list and finalmoveslist are both 
        // passed in and finalmovelist / finallist is returned
        public static List<int[]> RemoveUnusedMoves(List<int[]> solutiontree, List<int[]> finalList)
        {
            // Each move array is stored in the solution list - 
            // In the move array the index of the parent move (on the solutioin list) 
            // is stored at index 11-
            //previousParent is created to tell the function which is the next move to add
            //to the finalList
            int previousParent = new int();

            // this loop finds the organized state and sets the move array index 11 as
            // previousParent so the method knows where to look for the next move
            // the organized state is added to the final list since the loop to count 
            // back starts with the organized moves parent
            for(int a = 0; a < solutiontree.Count; a++)
            {
                if(solutiontree[a][9]==2300)
                {
                    previousParent = solutiontree[a][11];
                    finalList.Add(solutiontree[a]);
                    break;
                }
            }

            // moving backwards from the final move's parent to the first move and adding
            // each of the moves to the finalList
            while(previousParent > 0)
            {
                finalList.Add(solutiontree[previousParent]);
                previousParent = solutiontree[previousParent][11];
            }

            // the final List is started at the organized state then formed by moving 
            // backwards to the original state. here the final list is reversed so it
            // shows moves from first to last
            finalList.Reverse();

            return finalList;
        }

        //method for choosing the next moove and returning the index of that move
        public static int ChooseMove(List<int[]> solutions, bool fixTopRow)
        {
            //index of the chosen high score
            int parentIndex = new int();

            //variable established to evaluate high score
            int highScore = new int();

            //if the top row is not organized find highest score
            if (fixTopRow == false)
            {
                for (int i = 0; i < solutions.Count; i++)
                {
                    if (solutions[i][9] > highScore && solutions[i][10] != 1)
                    {
                        highScore = solutions[i][9];
                        parentIndex = i;
                    }
                }
            }

            //if top row is organized use the highest score with an organized top row
            if (fixTopRow == true)
            {
                for (int i = 0; i < solutions.Count; i++)
                {
                    if (solutions[i][9] > highScore && solutions[i][10] != 1 && solutions[i][9] >= 2000)
                    {
                        highScore = solutions[i][9];
                        parentIndex = i;
                    }
                }
            }
            return parentIndex;
        }

        //Method to determin if possible moves are valid moves and add them to the solutions tree
        //if they haven't been used already
        public static List<int[]> AmendSolutionsList(List<int[]> solutionTree, int[,] scoredMoves)
        {
            //check to make sure the move hasn't been used already
            for (int i = 0; i < solutionTree.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int count = 0;
                    for (int k = 0; k < 10; k++)
                    {
                        if (solutionTree[i][k] == scoredMoves[j, k])
                        {
                            count++;
                        }
                        if (count == 10)
                        {
                            scoredMoves[j, 9] = 9;
                        }
                    }
                }
            }

            //if move hasn't been used already and it's a valid move add it to the solution list
            for (int v = 0; v < 4; v++)
            {
                //default score of move array is 9 so if it is still 9 the move is void and not 
                //eligible to add to the solution list
                if (scoredMoves[v, 10] != 1 && scoredMoves[v, 9] != 9)
                {
                    int[] addalbeMove = new int[12];
                    for (int l = 0; l < 12; l++)
                    {
                        addalbeMove[l] = scoredMoves[v, l];
                    }
                    solutionTree.Add(addalbeMove);
                }
            }

            return solutionTree;
        }

        //method to get disorganized state from the user
        public static int[] UserInput(int[] Start)
        {
            //variable declared to for testing if state is a valid state
            bool validEntry = false;

            //the while loop keeps the user at the input stage until a valid entry is submitted
            while (validEntry == false)
            {
                //instructions for the user
                Console.WriteLine("Please enter the numbers");
                Console.WriteLine("0 through 8 and hit Enter");
                Console.WriteLine("be sure to use each number");
                Console.WriteLine("only once.");

                String getStart = Console.ReadLine();
                int[] testList = new int[9] { 8, 7, 6, 5, 4, 3, 2, 1, 0 };

                int testCount = 0;

                //First position is scored and added to the solutions list.

                if (getStart.Length == 9)
                {
                    //starting position is converted from a character string '876543210' to an int32(number) 
                    //array [8,7,6,5,4,3,2,1,0]. 
                    for (int i = 0; i < 9; i++)
                    {
                        Start[i] = Convert.ToInt32(getStart[i] - '0');
                    }

                    //check to see if the entered string contains 0 through 8 and that it contains 9 character
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

                    //if the move passes the length and complexity test its scored and returned
                    if (testCount == 9)
                    {
                        validEntry = true;
                        Heuristic(Start);
                        Start[11] = 0;
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

            //First position is returned to the main method to be handed off to the GenerateMove method
            return Start;
        }

        //Method for returning a score based on board positions
        public static int[] Heuristic(int[] unscoredMove)
        {
            //This is the scoring hueristic. Writen specifically for an 8-puzzle (as opposed to an n-puzzle).
            //it's not exactly a manhattan distance it prioritizes positions that close to a solution
            // it is not efficient the manhattan distace is easy and boring this is fun to play with

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
            //A solvable N-puzzle only needs to be a 2x3 grid to be solvable so anytime a outside row (or column) is solved
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
                score = score + 200;
            }
            if (unscoredMove[6] == 8 || unscoredMove[4] == 8 || unscoredMove[2] == 8)
            {
                score = score + 150;
            }
            if (unscoredMove[7] == 8 || unscoredMove[5] == 8)
            {
                score = score + 100;
            }

            //points away from 7's goal position
            if (unscoredMove[1] == 7)
            {
                score = score + 50;
            }
            if (unscoredMove[4] == 7 || unscoredMove[2] == 7 || unscoredMove[0] == 7)
            {
                score = score + 40;
            }
            if (unscoredMove[7] == 7 || unscoredMove[5] == 7 || unscoredMove[3] == 7)
            {
                score = score + 30;
            }

            //points away from 6's goal position
            if (unscoredMove[2] == 6)
            {
                score = score + 50;
            }
            if (unscoredMove[1] == 6 || unscoredMove[5] == 6)
            {
                score = score + 40;
            }
            if (unscoredMove[8] == 6 || unscoredMove[4] == 6 || unscoredMove[0] == 6)
            {
                score = score + 30;
            }
            if (unscoredMove[7] == 6 || unscoredMove[3] == 6)
            {
                score = score + 10;
            }

            //points away from 5's goal position
            if (unscoredMove[3] == 5)
            {
                score = score + 40;
            }
            if (unscoredMove[6] == 5 || unscoredMove[4] == 5 || unscoredMove[0] == 5)
            {
                score = score + 30;
            }
            if (unscoredMove[7] == 5 || unscoredMove[5] == 5 || unscoredMove[1] == 5)
            {
                score = score + 20;
            }

            //points away from 4's goal position
            if (unscoredMove[4] == 4)
            {
                score = score + 40;
            }
            if (unscoredMove[1] == 4 || unscoredMove[3] == 4 || unscoredMove[5] == 4 || unscoredMove[7] == 4)
            {
                score = score + 30;
            }

            //points away from 3's goal position
            if (unscoredMove[5] == 3)
            {
                score = score + 40;
            }
            if (unscoredMove[2] == 3 || unscoredMove[4] == 3 || unscoredMove[8] == 3)
            {
                score = score + 30;
            }
            if (unscoredMove[1] == 3 || unscoredMove[3] == 3 || unscoredMove[7] == 3)
            {
                score = score + 20;
            }

            //points away from 2's goal position
            if (unscoredMove[6] == 2)
            {
                score = score + 40;
            }
            if (unscoredMove[7] == 2 || unscoredMove[3] == 2)
            {
                score = score + 30;
            }
            if (unscoredMove[8] == 2 || unscoredMove[4] == 2 || unscoredMove[0] == 2)
            {
                score = score + 20;
            }
            if (unscoredMove[1] == 2 || unscoredMove[5] == 2)
            {
                score = score + 10;
            }

            //points away from 1's goal position
            if (unscoredMove[7] == 1)
            {
                score = score + 40;
            }
            if (unscoredMove[6] == 1 || unscoredMove[4] == 1 || unscoredMove[8] == 1)
            {
                score = score + 30;
            }
            if (unscoredMove[1] == 1 || unscoredMove[3] == 1 || unscoredMove[5] == 1)
            {
                score = score + 20;
            }

            //update the move being scored to include final score
            unscoredMove[9] = score;

            //returns move with score
            return unscoredMove;
        }

        //Method for generating possible moves passes in the last chosen move
        //as well as the index for that move. new moves are scored and parentindex
        //is added so the solution can be printed.
        public static int[,] GenerateMove(int[] lastMove, int parentIndex)
        {
            //below a new array is formed for each new possible position
            int[] newMoveOne = new int[12] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 0, parentIndex };
            int[] newMoveTwo = new int[12] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 0, parentIndex };
            int[] newMoveThree = new int[12] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 0, parentIndex };
            int[] newMoveFour = new int[12] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 0, parentIndex };

            //array is declared to hold possible moves
            int[,] returnMoves = new int[4, 12];

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
                    for (int y = 3; y < 9; y++)
                    {
                        if (lastMove[y] == 0)
                        {
                            newMoveTwo[y - 3] = lastMove[y];
                            newMoveTwo[y] = lastMove[y - 3];
                        }
                    }

                    //score position
                    Heuristic(newMoveTwo);
                }

                //all positions can move left exept those that have a remainder of zero when you
                // % by 3 (0,3,6)
                if ((i % 3) != 0 && lastMove[i] == 0)
                {
                    //fill new move array with last move
                    for (int x = 0; x < 9; x++)
                    {
                        newMoveThree[x] = lastMove[x];
                    }

                    //switch 0 position with the number at the index of 0 minus 1
                    for (int y = 1; y < 9; y++)
                    {
                        if (lastMove[y] == 0)
                        {
                            newMoveThree[y - 1] = lastMove[y];
                            newMoveThree[y] = lastMove[y - 1];
                        }
                    }

                    //score position
                    Heuristic(newMoveThree);
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
                }
            }

            //add each of the new moves to an array to return to the main method
            for (int k = 0; k < 12; k++)
            {
                returnMoves[0, k] = newMoveOne[k];
            }
            for (int k = 0; k < 12; k++)
            {
                returnMoves[1, k] = newMoveTwo[k];
            }
            for (int k = 0; k < 12; k++)
            {
                returnMoves[2, k] = newMoveThree[k];
            }
            for (int k = 0; k < 12; k++)
            {
                returnMoves[3, k] = newMoveFour[k];
            }

            //return new moves options
            return returnMoves;
        }
    }
}