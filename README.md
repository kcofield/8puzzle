# 8puzzle
8puzzle

C# 8 puzzle. 

This is an example of an A* algorithim used to solve a 3x3 n-puzzle. To run it just open Visual Studio start a new console app 
and run, or copy and paste the code from the, .cs file. 

The goal state for this puzzle is 876543210. User can enter a disorganized state with with the numbers 0-8 in mixed up order.

The most moves required to solve an 8puzzle is 31. The two states that are 31 moves out are 132745608 and 352140621.

this could be expanded to - have a user defined goal state, a user defined puzzle size, include a function for determining if the 
puzzle is solvable or not solvable (right now an unsolvable starting state will just cause it to run forever), a limit on the number 
moves so it doesn't run forever, the heuristic could be changed to a more traditional n-puzzle hueristic like manhattan distance
or a more advanced heuristic with a learning component, adding  a windows interface to show a pretty UI.

Have fun.

