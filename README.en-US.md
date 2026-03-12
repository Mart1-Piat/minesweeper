# Minesweeper

### A minesweeper?

Based on the classic Windows game :

<img src="./img/minesweeper1.png" height="200"/>
<img src="./img/minesweeper2.png" height="200"/>
<img src="./img/minesweeper3.png" height="200"/>

The player plays on a grid containing cells that are all hidden when the game starts. Some of these cells contain bombs. The player must reveal all the cells that do not contain bombs.

To do this, they can click on a cell to reveal its contents. If the clicked cell contains a bomb, the game is immediately lost.

If the clicked cell did not contain a bomb, then the game continues and part of the grid is uncovered on the basis of the the following verifications:
- If the clicked cell (which didn't contain a bomb) has at least one bomb in its immediate vicinity (one cell around in all directions, even diagonally), then this cell reveals the number of bombs in this immediate vicinity and we stop there.
- On the other hand, if the clicked cell has no bombs in its immediate vicinity, then it reveals an empty square, and the game checks its immediate neighbors as well. From neighbor to neighbor, if many of the cells visited are empty and not surrounded by bombs, then a large part of the grid can be revealed with a single click. The first click on the field always lands on such a cell, ensuring that the player can't lose or be stuck on the first click.

From deduction to deduction, we'll try to find all the cells (empty and numbered) that don't contain bombs.
 
The user can choose the size of the grid and the number of bombs. They can also mark a deduced "bomb cell" with a flag, by right-clicking the tile.
