# spine-viewer-monogame-wpf
Auto detect version and view gamesesotericsoftware spine file.

This is my rewrite of kiletw/SpineViewerWPF tool with following changes:
+ Use craftworkgames/MonoGame.WpfCore
+ Auto read spine file version
+ Fix memory leak by releasing texture when open a new file.
+ Remove global variables
- This doesn't have capture image or video like SpineViewerWPF.
