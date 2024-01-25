Welcome dear reader!

This project has been made as a representation of my skills, experience and capabilities regarding Unity and C#. 
The project features a Unity project with a small demo scene that showcases my implementation of an equipment system. 

In this README, I will dive a little deeper into the contents of this project. Explaining what's waiting for you, how to use it and how to implement and expand on it. 
First, a general overview: 

This project serves as a take on a broad equipment system. Although not completely perfected and refined, my goals have been met with creating a system that allows developers to create equipment objects with relative ease. 
Further down below, I will explain how to to do this and how you can change it, but first I need to tell you about the present equipment objects and their usage. 
Generally speaking, there are 5 types of equipments. These equipments can all be activated to do something. The player is able to equip an equipment in his left or right hand (non visible hand as of now). 
Pressing a certain button or key activates the equipment for that hand, therefore, there are two activation keys. Other actions revolving around a specific hand also have a specific key for their hand. 

Currently, the project features 5 different types of equipment, with each a different activation use.

The equipment types that can be made are, and their activation are:
- Weapons (ranged). Upon activation, shoots bullets towards the mouse. Can have different fire modes that get switched by FireModeSwap input (see down-down below)
- Ammunition. Upon activation, reloads the weapon in the other hand (if ammo type matches the weapon type)
- Throwables. Upon activation, throws the equipment from hand towards the mouse direction. Forces are set per object and can be tweaked. 
- Tools (I.e Flashlight). The Flashlight is currently the only tool, upon activation it toggles the light. 
- Apparel (I.e Hat). The hat is the currently the only apprel, upon activation, the hand gets put on the player's head. Activating again for an empty hand places the hat back in the hand. 

These equipments are all seperatly made as objects. A quick description of how the system works for developers: 
A Scriptable Object of each equipment type can be made to serve as the data of the equipment. A prefab object with a MonoBehaviour script handles the behaviour of the equipments. It has a reference to the scriptable object for specific values. 
Another object controlls the system of equipping and dropping. It has references to the Hand scripts and controls the input for activating the equipments. 
The activation logic is stored in a seperate script. The behaviour script has a reference to this to call the logic. A seperate script automatically adds the correct activation logic script to the equipment based on it's type, and initialzes it (ActivationLogicHandler.cs). 

This system allows developers to create new equipment objects without having to assign multiple component, references and other values. With this in mind, my general goal has been achieved. 


/* ---------- Making new equipments ---------- */

To make an equipment, follow these general steps: 

1) Select a 3D model you want to have as equipment
2) Add it to the scene, and add a fitting collider to it that matches the shape/bounding box of the model. 
3) Create a new ScriptableObject by pressing right click > Create > Equipment > {type you want to create}
4) In the inspector, set the relevant values such as name, equip distance, and scale(!). Depending on your type, you might need to select a prefab (I.e bullet)
5) In the prefab folder, search for the 'EquipmentObject' prefab.
6) Select the prefab, then drag into the Hierarchy.
7) Add the above prefab as child of your model from 1) (!).
8) On the new child prefab, look for the EquipmentBehaviour component (script)
9) Drag or select your ScriptableObject from 2) into the 'Equipment Data' slot. You might also need to assign the 'Main equipment object' to the parent of 1), this will happen automatically upon play but might get the wrong object. 
10) Done! Your equipment is ready to be used! One last thing however:
11) For equipment that has some sort of firepoint (weapons, flashlight) or other target transform, look in the child of 'EquipmentObject' for the 'EquipmentSpecificTransform' empty object.
12) Position this object at the firepoint you need for your equipment (I.e barrel of the gun, target position of hat, etc.) 


/* ---------- Expanding on equipments: Sub-types ---------- */

Now, for the developers who would like to expand on the system and create sub-types of existing equipment (I.e flashlight, bullet, ammo clip), I would recommend following the below general steps:

1) Create a ScriptableObject class for storing data of your new equipment. Make it derive from one of the existing EquipmentObject classes. Don't forget to create a new asset menu 
2) Add your properties and data variables to the class as desired. 
3) Create a new Activation script for your equipment. Make this script derive from one of the existing Activation classes.
4) In parent Activation class, alter the code to handle a case of the EquipmentData property from the EquipmentBehaviour class being a {YourNewScriptableObjectClass). Cast the property to your new object type.
5) For an example of 4), look into the ToolActivation class. This casts an EquipmentData of EquipmentBehaviour to a FlashlightObject if that is the EquipmentData in the EquipmentBehaviour class (point 8. from earlier general steps).
6) In the parent Activation class, make a virtual void for Activate(), and Initialize() of some sort. Call the Initialize method from the parent's Initialize method (!) <<< Note: Either call in parent, or alter the ActivationLogicHandler.cs if desired.
7) In your new Activation script, set the ActivationLogic property of your EquipmentBehaviour to your new Activation script. This will make sure your derived Activation method in your new Activation script will be called upon activation. (preferably done in your Init() )
8) Implement your logic for activating your equipment, make sure to add it in the Activation method.
9) Finally, follow the general steps for creating a new equipment. Don't forget to select your new ScriptableObject as Equipment Data in the EquipmentBehaviour inspector.
10) Your newly created equipment should now be calling the Activation method from it's own new class upon receiving general activation input! If not, make sure to override the Activate method from the parent class and test again. 


/* ---------- Expanding on equipments: New main=types ---------- */

For those who would like to create completely new types of equipment, not based on the current main types. I recommend taking these general steps: 

1) In the 'BaseEquipmentObject' class, add your new desired type in the 'EquipmentType' enum.
2) Create a new ScriptableObject class deriving from 'BaseEquipmentObject.cs'. Add your specific new desired variables, properties and other data related values. Override the 'OnEnable' function and automatically set it's 'EquipmentType' property to your new type.
3) Create a new ActivationLogic script for your equipment type. Make it derive from MonoBehaviour and implement the IEquipmentActivation interface and it's Activation() method.
4) Make a public method to startup your new activation script (I.e Init() ).Have it take a parameter of 'EquipmentBehaviour'. Also, implement your desired activation logic in your Activation method.
5) Inside the ActivationLogicHandler, create a new case inside the 'AddActivationLogic' function for your new equipment type. Take the other cases as examples and implement something similar for your activation logic. Make sure to pass the equipmentBehaviour variable.
6) Back inside your Init() function, or something similar, make sure to set the 'activationLogic' of the 'EquipmentBehaviour' parameter to your new activation script ('this'). This will call the Activation function from the class.
7) You should be good to go now! Follow the general steps to create a new equipment and try your new equipment out ;) 
      

Now that I've covered the main dish of the project and how to implement/use/expand on it, a few other things might be handy to know beforehand: 


/* ---------- Controls ---------- */

The standard controlls are as follows: 

- Movement: WASD/Arrow keys
- Aiming: Mouse
- Activate equipment: Mouse0 / Mouse1 for Left/Right hand
- Equip and Drop equipment: Q / E for Left/Right hand
- Swap fire modes for weapons: Z / X for Left/Right hand
- Interact with environment (buttons): Space

To make life easier, these values can be changed by pressing the 'Escape' key while playing. This toggles an setting panel where you can select different options. Other settings have been implemented to fit your desired needs as well.
To set the base values of these properties, head into the EquipmentSystemController and select the desired keys for each hand. For the interact key, head on over to the SettingManager class. 
To change values for movement, head into the PlayerMovement script. For mouse sensitivity values, look in the CameraController class. 
Quick side note: It is possible to have the same key for multiple actions. I heavily suggest not to do this, since it conflicts the actions and can result in errors. 

For those who are against automatically assigning and getting references for certain scripts, there are a few developer settings to use for you. In the SettingsManager script, set the values of the bools to your need. 
Note that this mainly affects the Equipment and relevant classes. It also means you will have to assign every reference in the inspector mannually for each equipment, and add needed components manually as well. 
The needed components on relevant objects are: 

- Main equipment object (your model): A collidder, a rigidbody, 'EquipmentPhysicsMAnager.cs'.
- As child of above object: the EquipmentObject prefab.
- 


