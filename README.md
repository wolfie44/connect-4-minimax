# Connect 4 - Made with Unity

#### Video Demo: [YouTube](https://www.youtube.com/watch?v=tnpCFKdiOQo)

#### Description:

Welcome to Connect 4 Made with Unity C#! This project recreates the classic Connect Four game using Unity and C#. Connect Four is a two-player game where the objective is to be the first to align four tokens in a row, either horizontally, vertically, or diagonally, on a grid.

#### File Overview:

#### Datas

1. **BoardManager.cs**: This script manages the game board mechanics, including setting up the grid, placing tokens, and checking for win conditions. I'm using a two-dimensional array to organize the data. The script has events any script can subscribe to, in order to trigger behavior regarding the state of the game.

2. **BoardHelper.cs**: This script contains static utility functions to simplify working with a Token[,] array. Functions can be used by both the board manager and the AI.

3. **TestBoard.cs**: This script performs some Unit Tests to test the Board Manager Script, making sure functions like checking 4 alignments are correct.

4. **Token.cs**: This script is a simple representation of a game token, saving the GameObject visual and its ownerId.

#### Player

1. **Player.cs**: The Player script references their score, Id, and name, handling basic visual-related mechanics.

2. **PlayerLocalMovement.cs**: The PlayerLocalMovement script controls player input. By updating the velocity of their rigidbody, the player can move left or right. It also allows players to drop tokens into columns if it's their turn.

#### Gameplay

1. **GameManager.cs**: The GameManager script oversees the game's flow, handling tasks such as setting up the game, managing turns, and determining win/loss conditions. It ensures a smooth gameplay experience by coordinating interactions between the game board and players.

2. **DetectionPlacementToken.cs**: The DetectionPlacementToken script is a simple script placed on tokens, detecting a collision with a column to add this token to the data board.

#### AI

1. **ComputerAI.cs**: The AI script provides an AI opponent for single-player mode. It utilizes the minimax algorithm to challenge players by predicting future moves and selecting the optimal strategy. Additionally, the AI incorporates a belief system and evaluation function to make informed decisions, creating a challenging and engaging experience for players.

2. **SpawnPoint.cs**: Handles a list of all the ordered positions the AI needs to drop a token in a column.

#### Network

3. **GameManagerNetwork.cs**: The GameManagerNetwork script oversees the game's flow, handling tasks such as setting up the game when 2 players joined, managing turns, and determining win/loss conditions. It ensures a smooth gameplay experience for both the client and the server, sending RPCs to clients to handle the flow, and making sure clients are not cheating by using server-side validation only.

5. **PlayerEntity.cs**: This script handles all the player logic in Network with several networked variables like their name and score.

6. **PlayerMovement.cs**: This script handles all the player movement in Network with Client RPCs to send their input to the server and instantiate a dummy token waiting for the server response, ensuring direct feedback and no visual lag.

7. **ApplicationController.cs, NetworkServer.cs, ClientGameManager.cs, HostGameManager**: These scripts manage network functions for online multiplayer. Using Unity Netcode and Unity Relay, players can host or join games over the network. These scripts handle hosting and joining sessions.

8. **PlayerDisplay.cs**: Several UI scripts subscribe to networked variables, updating screen feedback.

#### UI

1. **HUDManager.cs**: This script manages the HUD by updating player information like score or name.

2. **MenuManager.cs**: This script manages all the UI events and display for the menu.

3. **MenuNetworked.cs**: This script manages all the UI events and display for the networked menu, as well as allowing the player to host or join a session.

#### Unity

1. **Assets/**: The Assets folder contains all assets used in the game, such as sprites, sounds, and visual effects. These assets enhance the game's presentation and create a visually appealing and immersive environment for players.

2. **Prefab/**: The Prefab folder contains all prefab used in the game, such as UI, player, and token. These prefabs are instantiated when needed or used in several scenes, ensuring they are modified in only one place, enhancing coherence.

3. **Scripts/**: The Scripts folder contains all scripts used in the game.

4. **Sounds/**: The Sounds folder contains all sounds used in the game, including main music and some SFX.

5. **Scenes/**: The Scenes folder contains all scenes used in the game, starting with the MainMenu and GameLocal for local games, and NetBootstarp, NetMenu, and NetworkGame for online mode.

#### Design Choices:

- **Physics**: I chose to let the player drop a token wherever they want to. The physics system handles the token drop to the column, adding a slight dose of skill and ability to not fail dropping your token. If the player drops their token in an already full column, the token will disappear and a new chance will be given to the player.

- **Unity Netcode and Unity Relay**: The decision to use Unity Netcode and Unity Relay for online multiplayer was based on their reliability and ease of implementation. These tools provide robust networking solutions, enabling smooth and stable gameplay experiences for players across different network conditions. They are also easier to set up than tools like Photon because they are directly linked with other Unity backend systems like Authentication.

- **Minimax Algorithm for AI**: The implementation of the minimax algorithm was chosen to create a challenging AI opponent for single-player mode. By analyzing possible future moves and selecting the best course of action, the AI provides players with a formidable opponent capable of strategic gameplay. It's a common algorithm studied in AI courses for game development.

- **Belief System and Evaluation Function**: The addition of a belief system is made on purpose to simulate usual AI bots, which could have access only to "what they can see" and not the full game state. The eval function was quite hard to build. I tried several algorithms and decided to go with a basic scoring based on the number of tokens aligned, balanced with the number of tokens aligned by the opponent. Scores can be tweaked to make the AI more offensive or defensive. Heuristics could also be added to take some specific alignments into account.

- **User Interface Enhancements**: The inclusion of UI elements, assets, visual effects, and sounds improves the game's aesthetics and user experience. These enhancements contribute to a more engaging and enjoyable gameplay experience, captivating players and immersing them. I chose a retro look because it looked cool.

#### Resources Used:
- **Hit Impact Effects FREE** (Asset Store) [Link](https://assetstore.unity.com/packages/vfx/particles/hit-impact-effects-free-218385)
- Scripts from "Make Online Games Using Unity's NEW Multiplayer Framework" - GameDev.tv
- **ChatGPT, Gemini** for English text correction and overall technical questions on networking mechanics
