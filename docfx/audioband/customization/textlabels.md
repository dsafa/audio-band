# Text Labels
## Text Format
The text format for a label determines what text is shown
It can be any combination of text or placeholders. For example the format `This is some text` will make the label display `This is some text` or `Song name: {song}` will display `Song name: ` followed by whatever the current song's name is.

The format for a placeholder is {`style` `tag`:`color`} where `tag` and `color` are any of the following values:

**Tag** (*case sensitive!*)
- **artist**: Artist for the current song
- **song**: The title of the current song
- **album**: Album name for the current song
- **time**: Current playback time for the current song
- **length**: Total length of the current song

**Color**

Use an html color code in the format #RRGGBB

**Text Style**
- **\*** (asterisk): Bolds the placeholder
- **&** (ampersand): Italicizes the placeholder
- **_** (underline) - Underlines the placeholder

### Example formats
- Normal label showing the song and artist : `{song} by {artist}`
- Showing the album name in bold : `{*album}`
- Showing the song progress in gray : `{time:#A9A9A9} : {length:#A9A9A9}`
- Using style and color : `{*artist:#a9a9a9}`