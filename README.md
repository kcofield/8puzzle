# C# 8puzzle

## Description

This is an example of an A* algorithim used to solve a 3x3 tile puzzle with a single tile missing (8 actual values).  To solve the puzzle, tiles are rearranged by sliding tiles up, down, left, or right into the open position until the goal state is reached.

## Getting Started

This is a C# Console app targeting DotNet Core 

To run it just open the solution file in Visual Studio 2017 and start a debugging session, or else run the following commands from a CLI.

```
$> dotnet build 8puzzle.sln
$> dotnet run
```
*Note requires DotNet Core SDK installed (included with VS2017).*

## Usage

User can enter a disorganized state with with the numbers 0-8 in mixed up order.
The goal state for this puzzle is in text is 876543210.  Graphically:
||||
|-|-|-|
|8|7|6|
|5|4|3|
|2|1|0|
|||

The most moves required to solve an 8puzzle is 31. The two states that are 31 moves out are 132745608 and 352140678.

## Future enhancements

This could be expanded to have a user defined goal state, a user defined puzzle size, include a function for determining if the puzzle is solvable or not solvable (right now an unsolvable starting state will just cause it to run forever), a limit on the number moves so it doesn't run forever, the heuristic could be changed to a more traditional n-puzzle hueristic like manhattan distance or a more advanced heuristic with a learning component, adding  a windows interface to show a pretty UI.

### Have fun.

