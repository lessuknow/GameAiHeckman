Group: Chris Chen, Owen Elliff

This unity project runs a game of Pacman.

To move Pacman, you can use WASD or arrow keys. Running into a ghost removes a life and resets Pacman to the center.
When you run out of lives, the game ends and your high score is set.
Each pellet increases your score by one point (including power pellets), and
eating a power pellet makes the ghosts become scared of pacman and change color for 10 seconds. 
When the ghosts are scared of Pacman, you can eat them for points.
Eating more ghosts during the same power pellet increases your score multiplicatively. 
Every time you reach 10,000 points you gain a life.

When Pacman eats enough pellets to pass a threshold, the red ghost increases his speed by 5%.
and changes his scatter behavior; this is referred to as "Cruise Elroy" mode. This happens twice in the level,
when Pacman has eaten 1/3 of the total pellets and 2/3 of the total pellets. During the second time it happens,
only the speed is increased; the AI does not change again. The speed boosts stack multiplicatively.


How the single FSM works is as follows:
	There are five states: Scatter, Chase, Run, Leave.
	Scatter:
		The ghosts each attempt to move towards a corner. Specifically they tend to move towards:
			Red : Upper-Left
			Orange : Upper-Right
			Pink : Bottom-Left
			Purple : Bottom-Right.
		However, when Red is in "Cruise Elroy" mode his Scatter mode changes. Instead, he uses his "Chase" AI during the
			Scatter state as well as during the Chase state.
		
	Chase:
		Each ghost moves differently when the FSM is in this state. Specifically:
			Red : Attempts to move direcltly towards where the player is currently.
			Pink : Attempts to move towards where Pacman is going to go; specifically his currently
				facing direction + 3 tiles. Notably, when Pacman is facing North Pink will try to move 
				to his North + 3 tiles, AND to the Pacman's West + 3 tiles. This is due to a buffer overflow
				error, which I felt should be simulated.
			Purple : Attempts to move in a similar manner to Pink, but using 2 tiles instead of 3. He also
				uses Red's distance from Pacman and adds it to the previously found end position. This means that
				when Pacman is far away from Purple he acts somewhat unpredictably, but when Purple and Red are close
				to Pacman they "work together".
			Orange : When he is more then 8 tiles away from Pacman, he trys to move directly towards the Player.
				When he is within 8 tiles, he then uses his Scatter code.
	
	Run:
		Each ghost tries to move the opposite of where Pacman currently is.
	Leave:
		This AI is specifically for having the ghosts leave the box in an orderly fashion.
		
	The FSM starts in the Leave state. After 1.5 seconds, it changes to Scatter for 7 seconds. 
		After that it switches to Chase for 20 seconds. It repeats in a similar fashion, but the 3rd and 4th
		switch to Scatter last for 5 seconds and after the 4th switch it stays in Chase.
		To be more clear, the states are (in State (duration in seconds)):
		
		Leave (1.5) -> Scatter(7) -> Chase(20) -> Scatter(7) -> Chase(20) -> Scatter(5) -> Chase(20) -> Scatter(5) -> Chase (Infinite)
	
	Whenever a power pellet is consumed, the state changes to Run for 10 seconds. After this state is over, the state is set to what state
		it was previously and the previous pattern continues. An example is:
		
		Leave (1.5) -> Scatter(7) -> Chase(20) -> [Power Pellet] -> Run (10) -> Chase (20) -> Scatter (7) -> ...
	
	The diagram included should hopefully provide more clarify, if that is needed.
		
	For the single FSM, all the logic is from //http://gameinternals.com/post/2072558330/understanding-pac-man-ghost-behavior
	
	
How the FSM's on each ghost work are as follows:
	Each FSM is the same, but has minor tweaks to some of the behaviors within it
	Each FSM has 6 states: Locked, Unlocking, Wandering, Chasing, Fleeing, and Locking
	Locked:
		This is the state each ghost starts in where they are locked in their pen. When the exit this state, they always enter the Unlocking state
		Blinky: Blinky exits this state at zero seconds
		Pinky: Pinky exits this state at 1 seconds
		Inky: Inky exits at 2 seconds
		Clyde: Exits at 3 seconds
	Unlocking:
		This state is the same for all ghosts
		They try to go up until they are above the door of the pen and then exit this state into the Wandering state
	Wandering:
		In this state, the ghosts will try to continue in the same direction until they are blocked. It is the same for all ghosts
		When a ghost is blocked, it will randomly decide which direction to go from there
		After a certain amount of time in this state, the ghosts will enter the chasing state
	Chasing:
		In this state, the ghosts will try to chase Pacman. The code for chasing is slightly different in each ghost
		Blinky: Blinky tries to head directly for Pacman. Blinky will not exit this state to Wandering if 100 pellets have been collected
			This not exitting to Wandering is a simplified version of the speed-up according to pellet count
		Pinky: Pinky heads for 3 spaces in front of Pacman
		Inky: Inky heads for the location gotten by this formula:
			goal = (Blinky's location) + ((Blinky's location) - (location 3 spaces in front of Pacman))
		Clyde: Clyde heads for Pacman until he is 5 spaces in radius away from Pacman, at which point he will change direction at the first chance he gets
			Clyde does this in repetition almost as though it were switching between two states within the Chasing state
	Fleeing:
		The Fleeing state is the same for all ghosts
		In the Fleeing state, the ghosts try to turn away from Pacman at any chance they get
		In the Fleeing state the ghosts can be killed, if they are, they enter the Locking state
		After 10 seconds in this state, the ghosts return to the Chasing state, or Fleeing state if it is the time for that
	Locking:
		The Locking state is the same for all ghosts
		In the Locking state, the ghosts try to get back to their starting position, usually moving around in the center of the board as they try to
		After the Super Pellet is done, they return to Chasing or Wandering
	A diagram named "Multi_FSM.png" has been included for this FSM
	All logic for this FSM gotten from this video: https://www.youtube.com/watch?v=xMEjovSlyqs
	