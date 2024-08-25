# trello-to-markdown
 Small console application to convert a trello json exported board to markdown files

## What is this? 
This is a small script written in C#/.NET8 that I wrote for me so I can export my Trello boards and later import them into Obsidian.

I'm not updating this unless I need to do it for myself, I don't take requests, this software is not for commercial use and probably won't get anymore update. I just leave it here just in case someone finds it useful.

## How to run this?
I don't provide any standalone executable, just the Visual Studio solution, if you want to use this, then you have to install VS2022+ with the .NET SDK and run the code yourself.

## What does it do?

It reads an exported json Trello board and creates a structure of folders and markdown files based on it:

- Creates a folder for the Board itself.
- Creates a folder for each list.
- Creates a markdown file for each card.
- Downloads all the attachments for each card into an "_attachments" folder in the board folder. Puts all the attachments from all the lists and cards together in the same folder.

 The content of the markdown files go in this manner:
 
- **Attachments**. This is tested only with image attachments, I don't know how it would work with other types. It creates an Obsidian type link "![[attachment.jpg]]" so you can drop the attachment into the folder configured in your obsidian project and it will show up in the note.
- **Description**. The content/description on the Trello card.
- **Checlists**. Imports any checklists from the Trello card and creates them at the end of the markdown file.

## Setup

- Go to the Trello board you want to export, clic on the three dot option on the top right of the board. Go down to the "print, export, and share" option and choose "Export as JSON".
- You have to set your board as public if your cards contain attachments, so it can download them. If you don't want to download attachments, then set the "INCLUDE_ATTACHMENTS" flag at the top of the code to false.
- Set your input file and output path at the top section of the code.
- I would suggest reviewing the code just in case you want to make any changes. 
- Run the code!! 