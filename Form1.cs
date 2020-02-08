using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace XInARow
{
    public partial class Form1 : Form
    {
        public String currentPlayer = "X";
        public PictureBox[] allTiles;
        public PictureBox[][] possibleWins;
        public String[,] currentStateOfBoard;
        public String winningPlayer;
        public bool hasWon = false;
        public bool draw = false;
        public int maxInARow = 4;
        public int gridSize = 10;

        public Form1()
        {
            InitializeComponent();
            allTiles = new PictureBox[gridSize * gridSize];

            foreach (var pb in tableLayoutPanel1.Controls.OfType<PictureBox>())
            {
                if (pb.Name.StartsWith("pictureBox"))
                {
                    var pbIndex = int.Parse(pb.Name.Replace("pictureBox", "")) - 1;
                    allTiles[pbIndex] = pb;
                }
            }

            currentStateOfBoard = new String[gridSize, gridSize];
        }


        private void playerChoosesTile(object sender, EventArgs e)
        {
            if (hasWon || draw)
            {
                MessageBox.Show("Reset the game");
                return;
            }

            var tileToPlay = (PictureBox)sender;
            if (tileToPlay.Tag != null)
            {
                MessageBox.Show("This tile is taken, choose another tile.");
                return;
            }

            var nextPlayer = currentPlayer == "X" ? "O" : "X";
            var nextPlayerImage = currentPlayer == "X" ? Properties.Resources.o : Properties.Resources.x;
            var currentPlayerImage = currentPlayer == "X" ? Properties.Resources.x : Properties.Resources.o;
            
            // Put the current player's image in the tile
            tileToPlay.Image = currentPlayerImage;
            tileToPlay.Tag = currentPlayer;

            // Check for win or draw
            if (checkForWin(tileToPlay))
            {
                MessageBox.Show("Win: " + tileToPlay.Tag);
                return;
            } 
            else if (thereIsADraw())
            {
                MessageBox.Show("It's a draw");
                currentTurnLabel.Text = "Draw!";
                currentTurnImage.Image = null;
                return;
            }

            // Round finished, update to show it's next player's turn
            currentPlayer = nextPlayer;
            currentTurnImage.Image = nextPlayerImage;
        }

        private bool checkForWin(PictureBox tileLastPlayed)
        {
            getCurrentState();
            var tileRow = getTileRow(tileLastPlayed);
            var tileCol = getTileCol(tileLastPlayed);

            return (
                getResult(getRowArray(tileRow, tileLastPlayed)) ||
                getResult(getColArray(tileCol, tileLastPlayed)) ||
                getResult(getDiagArray2(tileRow, tileCol, tileLastPlayed, true)) ||
                getResult(getDiagArray2(tileRow, tileCol, tileLastPlayed, false))
            );
        }

        private bool[] getRowArray(int row, PictureBox tile)
        {
            bool[] rowArr = new bool[gridSize]; // true's will represent current player tag type

            for(var i = 0; i < gridSize; i++) // gridSize = 10
            {
                rowArr[i] = currentStateOfBoard[row, i] == (String)tile.Tag;
            }

            return rowArr;
        }

        private bool[] getColArray(int col, PictureBox tile)
        {
            bool[] colArr = new bool[gridSize]; // true's will represent current player tag type

            for (var i = 0; i < gridSize; i++) // gridSize = 10
            {
                colArr[i] = currentStateOfBoard[i, col] == (String)tile.Tag;
            }

            return colArr;
        }

        private bool[] getDiagArray2(int row, int col, PictureBox tile, bool isDownRight)
        {
            bool[] diag = new bool[10];

            var colDirection = isDownRight ? 1 : -1;

            diag[col] = true;
            for (int distance = 1; distance < gridSize; distance++)
            {
                var ptLeft = isDownRight
                    ? new RowCol(row - distance, col - distance)
                    : new RowCol(row + distance, col - distance);
                var ptRight = !isDownRight
                    ? new RowCol(row + distance, col + distance)
                    : new RowCol(row - distance, col + distance);

                if (ptLeft.isValid(gridSize))
                    diag[ptLeft.Col] = currentStateOfBoard[ptLeft.Row, ptLeft.Col] == (String)tile.Tag; // diag[index to check] --> is the tag here the same as the tile tag?
                
                if (ptRight.isValid(gridSize))
                    diag[ptRight.Col] = currentStateOfBoard[ptRight.Row, ptRight.Col] == (String)tile.Tag;
            }

            return diag;
        }

        private bool[] getDiagArray(int row, int col, PictureBox tile, bool isDownRight)
        {
            List<bool> diagListLower = new List<bool>();
            List<bool> diagListUpper = new List<bool>();

            var colDirection = isDownRight ? 1 : -1;

            int lowerRow = row + 1; // adjust to be offset 1 col right and 1 row below
            int lowerCol = col + colDirection;

            int upperRow = row - 1; // adjust to be offset 1 col left and 1 row higher
            int upperCol = col - colDirection;


            while (lowerCol >= 0 && lowerRow >= 0 && lowerCol < gridSize && lowerRow < gridSize)
            {
                diagListLower.Add(currentStateOfBoard[lowerRow, lowerCol] == (String)tile.Tag);
                lowerRow += 1;
                lowerCol += colDirection;
            }


            while(upperCol >= 0 && upperRow >= 0 && upperCol < gridSize && upperRow < gridSize)
            {
                diagListUpper.Add(currentStateOfBoard[upperRow, upperCol] == (String)tile.Tag);
                upperRow -= 1;
                upperCol -= colDirection;
            }

            diagListUpper.Reverse();

            bool[] diagArr = new bool[diagListUpper.Count + diagListLower.Count + 1];
            
            for (var i = 0; i < diagListUpper.Count; i++)
            {
                diagArr[i] = diagListUpper[i];

            }

            diagArr[diagListUpper.Count] = true; // putting the current tile played as true
            var offset = diagListUpper.Count + 1;
            for (var i = 0; i < diagListLower.Count; i++)
            {
                diagArr[i + offset] = diagListLower[i];
            }
            return diagArr;
        }

        private bool getResult(bool[] arr)
        {
            int count = 0;
            for (var i = 0; i < arr.Length; i++)
            {
                count = arr[i] ? count + 1 : 0;
                if (count == maxInARow)
                {
                    return true;
                }
            }
            return false;
        } 

        private int getTileRow(PictureBox tile)
        {
            return Array.IndexOf(allTiles, tile) / gridSize;            
        }

        private int getTileCol(PictureBox tile)
        {
            return Array.IndexOf(allTiles, tile) % gridSize;
        }

        private void getCurrentState()
        {
            for(var tile = 0; tile < allTiles.Length; tile++)
            {
                currentStateOfBoard[tile / gridSize, tile % gridSize] = (String)allTiles[tile].Tag;
            }
        }

        private bool thereIsADraw()
        {
            bool allTilesTakenWithNoWin = true;

            for (var tile = 0; tile < allTiles.Length; tile++)
            {
                if (allTiles[tile].Tag == null)
                {
                    allTilesTakenWithNoWin = false;
                    return allTilesTakenWithNoWin;
                }
            }
            draw = true;
            return allTilesTakenWithNoWin;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {

            for (var tile = 0; tile < allTiles.Length; tile++)
            {
                allTiles[tile].Tag = null;
                allTiles[tile].Image = null;
                currentPlayer = "X";
                hasWon = false;
                winningPlayer = null;
                currentTurnLabel.Text = "Current Turn: ";
                currentTurnImage.Image = Properties.Resources.x;
            }
        }
    }

    class RowCol
    {
        public int Row;
        public int Col;

        public RowCol(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public bool isValid(int max)
        {
            return Row >= 0 && Col >= 0 && Row < max && Col < max;
        }
    }
}
