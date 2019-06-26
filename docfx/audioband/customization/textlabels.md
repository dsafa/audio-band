# Text Labels

## Name
Name of the label. Used for indentification purposes

## Visibility
Whether to show the label or not

## Width
Width of the label. If the text exceeds the width, the text will scroll

## Height
Height of the text

## X Position
The x position of the text

## Y Position
The Y Position of the text

## Font family
The font family for the text

## Font size
The font size of the text

## Font color
The default color of the text

## Alignment
The alignment of the label

## Scroll speed
The scroll speed of the label when the text scrolls

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
- **remaining**: Remaining time of the current song

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