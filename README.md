# My mods to Project Zomboid / Meus mods para Project Zomboid

### Technologies
- Lua
- C#

## Mods
### PacocaMod
It was created to add a delicious Brazilian sweet treat, called PAÇOCA.

![Paçoca Mod](PacocaMod/poster.png)

![alt text](img\pacoca.png)

To use, download it and put in C:\Users\<YOUR_USER>\Zomboid\mods (steam)

### AutoSave
- I created this mode to autosave every 10 min.
- You can change this time here:
- Path: mods\AutoSave\media\lua\shared
```
local SAVE_INTERVAL = 600
```

- IMPORTANT: I couldn't craate a native autosave using Lua.
- It just save the world, but it isn't create a save backup.
- To fix it, a create a simple program in C#, called Gambiarra AutoSave.

### GambiarraAutoSave
- To fix the problem to create a backup autosave, a create this code.
- It get the world saved and compress in a zip file in a BKP directory.
- There is a file called configs.json. In this file you can change the directory
necessary to config your backup.
```
{
    "LogPath": "C:\\Users\\%USERNAME%\\Zomboid\\Logs",
    "SavePath": "C:\\Users\\%USERNAME%\\Zomboid\\Saves",
    "BackupPath": "C:\\Users\\%USERNAME%\\Zomboid\\BKP",
    "ThreadSleepTime": 10000
}
```

#### How it works?
- I will need to execute the program (GambiarraAutoSaveMod) then execute Project Zomboid.
- The program reads the log written by autosave mod in C:\Users\<YOUR_USER>\Zomboid\Logs\*DebugLog.txt
![alt text](img\image.png)

- If has a new entry, than GambiarraAutoSaveMod get the world saved, copy to BKP folder and compress to zip file.
- The saves are renamed by Date Time.

![alt text](img\bkps-img.png)