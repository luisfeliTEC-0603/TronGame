# TRON GAME | WindowsForms 

## Project Overview
The following is a game inspired by the classic movie _Tron_. The goal is to avoid crashing into trails or obstacles while collecting items and using powers strategically. The primary objective of this project is to implement a solution to a problem using linear data structures. Specifically, the game incorporates singly linked lists and  stacks to handle the powers and in-game items.

## Prerequisites
- _**Operating System**_.: Windows, macOS, or Linux.
- _**.NET 8 SDK**_.: Ensure .NET 8 SDK is installed on your system.

## Features And Functionalities
The game features light spaceships, each represented as a singly linked list that leaves behind a destructive trail as it moves. The ships will move throughout a grid-based map. 

Items collected in the game are applied automatically in the order they are gathered, with a priority given to energy cells. Bots are also included in the game, simulating other players.

## Project Structure
```sh
TronGame
├── Game Logic            
│   ├── InGameObj.cs
│   ├── TheGrid.cs
│   └── Player.cs           
├── Data Structures        
│   ├── itemStack.cs        
│   └── SimpleLinkedList.cs 
├── WinForms Program 
│   ├── Program.cs
│   └── Form1.cs            
└── TronGame.generated.sln             
```

## License 
This project is intended for educational purposes.