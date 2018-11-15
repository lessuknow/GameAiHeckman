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
	
	