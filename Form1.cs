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
        public List<PictureBox> allTiles;
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
            allTiles = new List<PictureBox>();
            
            foreach(var pb in tableLayoutPanel1.Controls.OfType<PictureBox>())
            {
                if (pb.Name.StartsWith("pictureBox"))
                {
                    allTiles.Add(pb);
                }
            }

            currentStateOfBoard = new String[gridSize, gridSize];
        }


        private void playerChoosesTile(object sender, EventArgs e)
        {
            Debug.WriteLine("current:" + currentStateOfBoard[1, 1]);
            if (hasWon || draw)
            {
                MessageBox.Show("Reset the game");
                return;
            }

            var tileToPlay = (PictureBox)sender;

            if (currentPlayer == "X" && tileToPlay.Tag == null)
            {
                tileToPlay.Image = Properties.Resources.x;
                tileToPlay.Tag = "X";

                if (checkForWin(tileToPlay))
                {
                    MessageBox.Show("Win: " + tileToPlay.Tag);
                }

                if (thereIsADraw())
                {
                    MessageBox.Show("It's a draw");
                    currentTurnLabel.Text = "Draw!";
                    currentTurnImage.Image = null;
                    return;
                }

                currentPlayer = "O";
                currentTurnImage.Image = Properties.Resources.o;
            }
            else if (currentPlayer == "O" && tileToPlay.Tag == null)
            {
                tileToPlay.Image = Properties.Resources.o;
                tileToPlay.Tag = "O";

                if (checkForWin(tileToPlay))
                {
                    MessageBox.Show("Win: " + tileToPlay.Tag);
                }

                if (thereIsADraw())
                {
                    MessageBox.Show("It's a draw");
                    currentTurnLabel.Text = "Draw!";
                    currentTurnImage.Image = null;
                    return;
                }

                currentPlayer = "X";
                currentTurnImage.Image = Properties.Resources.x;
            }
            else
            {
                MessageBox.Show("This tile is taken, choose another tile.");
            }
        }

        private bool checkForWin(PictureBox tileLastPlayed)
        {
            getCurrentState();
            var tileRow = getTileRow(tileLastPlayed);
            var tileCol = getTileCol(tileLastPlayed);

            bool rowWin = getResult(getRowArray(tileRow, tileLastPlayed));
            bool colWin = getResult(getColArray(tileCol, tileLastPlayed));
            bool diagWin = getResult(getDiagArray(tileRow, tileCol, tileLastPlayed));

            if(rowWin || colWin || diagWin)
            {
                return true;
            }

            return false;
        }

        private bool[] getRowArray(int row, PictureBox tile)
        {
            bool[] rowArr = new bool[gridSize]; // true's will represent current player tag type

            for(var i = 0; i < gridSize; i++) // gridSize = 10
            {
                if (currentStateOfBoard[row, i] != (String)tile.Tag)
                {
                    rowArr[i] = false;
                }
                else { rowArr[i] = true; };
            }

            return rowArr;
        }

        private bool[] getColArray(int col, PictureBox tile)
        {
            bool[] colArr = new bool[gridSize]; // true's will represent current player tag type

            for (var i = 0; i < gridSize; i++) // gridSize = 10
            {
                if (currentStateOfBoard[i, col] != (String)tile.Tag)
                {
                    colArr[i] = false;
                }
                else { colArr[i] = true; };
            }

            return colArr;
        }

        private bool[] getDiagArray(int row, int col, PictureBox tile)
        {
            List<bool> diagListLower = new List<bool>();
            int lowerRow = row + 1; // adjust to be offset lower than the current tile
            int lowerCol = col + 1;

            while(lowerCol < gridSize && lowerRow < gridSize)
            {
                if (currentStateOfBoard[lowerRow, lowerCol] != (String)tile.Tag)
                {
                    diagListLower.Add(false);
                }
                else { diagListLower.Add(true); };
            }

            diagListLower.Reverse();

            List<bool> diagListUpper = new List<bool>();
            int upperRow = row - 1;
            int upperCol = col - 1;

            while(upperCol > 0 && upperRow > 0)
            {
                if (currentStateOfBoard[upperRow, upperCol] != (String)tile.Tag)
                {
                    diagListUpper.Add(false);
                }
                else { diagListUpper.Add(true); };
            }

            bool[] diagArr = new bool[diagListUpper.Count + diagListLower.Count + 1];

            for(var i = 0; i < diagListLower.Count; i++)
            {
                diagArr[i] = diagListLower[i];
            }

            diagArr[diagListLower.Count] = true;

            int index = diagListUpper.Count + 1;
            for (var i = 0; i < diagListUpper.Count; i++)
            {
                diagArr[index] = diagListUpper[i];
            }

            return diagArr;
        }

        private bool getResult(bool[] arr)
        {
            int count = 0;

            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] == true)
                {
                    count++;
                }
                else if (arr[i] == false)
                {
                    count = 0;
                }

                if (count == maxInARow)
                {
                    return true;
                }
            }
            return false;
        }

  

        private int getTileRow(PictureBox tile)
        {
            PictureBox[] arrayOfTiles = allTiles.ToArray();
            int tileIndex = Array.IndexOf(arrayOfTiles, tile);
            return tileIndex / gridSize;            
        }

        private int getTileCol(PictureBox tile)
        {
            PictureBox[] arrayOfTiles = allTiles.ToArray();
            int tileIndex = Array.IndexOf(arrayOfTiles, tile);
            return tileIndex % gridSize;
        }

        private void getCurrentState()
        {
           // int row = 0;
           // int col = 0;

            for(var tile = 0; tile < allTiles.Count; tile++)
            {
                currentStateOfBoard[tile / gridSize, tile % gridSize] = (String)allTiles[tile].Tag;

               /* if(col >= gridSize) // gridSize = 10
                {
                    row++;
                    col = 0;
                    currentStateOfBoard[row, col] = (String)allTiles[tile].Tag;
                    col++;
                }
                else
                {
                    currentStateOfBoard[row, col] = (String)allTiles[tile].Tag;
                    col++;
                }*/
            }
        }

        private bool thereIsADraw()
        {
            bool allTilesTakenWithNoWin = true;

            for (var tile = 0; tile < allTiles.Count; tile++)
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

            for (var tile = 0; tile < allTiles.Count; tile++)
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
}
