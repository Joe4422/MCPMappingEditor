# MCPMappingEditor

This application allows the user to easily view and edit the mappings Minecraft's Mod Coder Pack uses to deobfuscate minecraft.jar. It is compatible with the .rgs format used in MCP version 1.6 (Minecraft Alpha 1.1.2_01), and theoretically beyond that for as long as the format is unchanged. It uses .NET Core 3.1 as the runtime, and Windows Presentation Foundation for the user interface.

## Searching
The application offers searching functionality. The format is "\<class name\>.\<member name\>", both of which can be partial. For example, "BlockPressurePlate.field467_a" will return the same result as "BlockPres.field46".

## Editing
To edit a mapping, either double click on it or press Enter. To save the edit, either click away from it or press Enter again.
If a method mapping exists in multiple functions and "Rename All Methods with Same Substitute" is checked, all methods with the same substitute will be renamed. This is so you don't have to rename methods that are overridden in child classes separately from those methods in their parents.

## Automatic decompilation
If "Decompile On Save" is checked and the application is being run from MCP's root folder where decompile.bat lives, it will automatically invoke decompile.bat when you save the updated mapping.

## Screenshots
![User Interface example](https://i.imgur.com/c35wWJ2.png)
