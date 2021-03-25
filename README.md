# FrogPrototype
Mechanics Prototype of the Frog Game for class

move around with the arrow keys or WASD
Camera moves with the mouse.
You can grapple with the left mouse button. You need to hold to continue grapple once started. When button is released the grapple stops. You need to aim with the mouse but problem is the mouse is centered and locked. Only real way to have camera work properly. At the moment you need to move the center of the screen to the point of where you want to grapple. The Grapple itself does work. Will try to implement an auto target at a later date.
Also character spins after a grapple. 
Wall walking does not work at the moment and is not implemented in this version. 

UPDATE
March 25, 2021
move using arrow keys or WASD.
move camera with mouse.
Grapple still funky when using, but targeting system in place for grapple. Press right mouse button to toggle the grapple on and off. Use press scroll wheel button to change targets. Program will crash when in targeting mode if change targets and move a target out of camera view. 
Hold left mouse button to use grapple. Relase to release grapple.
Character will fall infinitley if "falls" in a direction where there is nothing to land on. This can happen on moving to fast on a semi-flat or plane surface or when jumping and using the grapple. 
characer can still walk on walls.

to wall on walls, objects need to have layer "WallStick" and to make something the grapple will target to, add the script "Target In View" probably eaiser to add an empty object and place in the middle of branches. If add to art assets directly pivot point will be at the center of the asset and not sure how to move pivot point, which is where the grapple will target to. 
