Welcome dear reader!

This project has been made as a representation of my skills, experience and capabilities regarding Unity and C#. 
The project features a Unity project with a small demo scene that showcases my implementation of an equipment system. 
This system allows you to create multiple types of equipment with relevant ease. Atleast one specific equipment for each type has been made, but it is certainly possible to expand for the different types. 

The equipment types that can be made are: 
- Weapons (ranged)
- Ammunition
- Throwables 
- Tools (I.e Flashlight)
- Apparel (I.e Hat)

To make an equipment, follow these general steps: 

1) Select a 3D model you want to behave as an equipment
2) Create a new ScriptableObject by pressing right click > Create > Equipment > {type you want to create}
3) In the inspector, set the relevant values such as name, equip distance, and scale(!). Depending on your type, you might need to select a prefab (I.e bullet)
4) In the prefab folder, search for the 'EquipmentObject' prefab.
5) Select the prefab, then drag into the Hierarchy.
6) Add the above prefab as child of your model from 1) (!).
7) In the new childprefab, look for the EquipmentBehaviour component (script)
8) Drag or select your ScriptableObject from 2) into the 'Equipment Data' slot.
9) Done! Your equipment is ready to be used! One last thing however:
10) For equipment that has some sort of firepoint (weapons, flashlight), look in the child of 'EquipmentObject' for the 'EquipmentSpecificTransform' empty object.
11) Position this object at the firepoint you need for your equipment (I.e barrel of the gun) 

The standard controlls are as follows: 

- Movement: WASD/Arrow keys
- Aiming: Mouse
- Activate equipment: Mouse0 / Mouse1 for Left/Right hand
- Equip and Drop equipment: Q / E for Left/Right hand
- Swap fire modes for weapons: Z / X for Left/Right hand
- Interact with environment (buttons): Space

To make life easier, these values can be changed by pressing the 'Escape' key while playing. This toggles an setting panel where you can select different options.
To set the base values of these properties, head into the EquipmentSystemController and select the desired keys for each hand. For the interact key, head on over to the SettingManager class. 
To change values for movement, head into the PlayerMovement script. For mouse sensitivity values, look in the CameraController class. 

For users who would like to expand on the different equipments, this can be done following these general steps:

First, for new equipment of existing types: 
1) Create a ScriptableObject class for storing data of your new equipment. Make it derive from one of the existing EquipmentObject classes. Don't forget to create a new asset menu
2) Add your properties and data variables to the class as desired. 
3) Create a new Activation script for your equipment. Make this script derive from one of the existing Activation classes.
4) In parent Activation class, alter the code to handle a case of the EquipmentData property from the EquipmentBehaviour class being a {YourNewScriptableObjectClass). Cast the property to your new object type.
5) For an example of 4), look into the ToolActivation class. This casts an EquipmentData of EquipmentBehaviour to a FlashlightObject if that is the EquipmentData in the EquipmentBehaviour class (point 8. from earlier general steps).
6) In the parent Activation class, make a virtual void for Activate(), and Initialize of some sort.
7) In your new Activation script, set the ActivationLogic property of your EquipmentBehaviour to your new Activation script. This will make sure your derived Activation method in your new Activation script will be called upon activation.
8) Implement your logic for activating your equipment, make sure to call it in the Activation method.
9) Finally, follow the general steps for creating a new equipment. Don't forget to select your new ScriptableObject as Equipment Data in the EquipmentBehaviour inspector.
10) Test!
