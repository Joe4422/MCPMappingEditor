# MCPMappingEditor

This application allows the user to easily view and edit the mappings Minecraft's Mod Coder Pack uses to deobfuscate minecraft.jar. It is compatible with the .rgs format used in MCP version 1.6 (Minecraft Alpha 1.1.2_01), and theoretically beyond that for as long as the format is unchanged. It uses .NET Core 3.1 as the runtime, and Windows Presentation Foundation for the user interface.

## Searching
The application offers searching functionality. The format is "\<class name\>.\<member name\>", both of which can be partial. For example, "BlockPressurePlate.field467_a" will return the same result as "BlockPres.field46".

## Editing
To edit a mapping, either double click on it or press Enter. To save the edit, either click away from it or press Enter again.

![User Interface example](https://i.imgur.com/c35wWJ2.png)
