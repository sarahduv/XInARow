﻿using System;
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

            currentStateOfBoard = new String[10, 10];
        }


        private void playerChoosesTile(object sender, EventArgs e)
        {
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

                /*if (checkForWin(tileToPlay))
                {

                }*/

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

                /*   if (checkForWin(tileToPlayer))
                   {

                   }*/
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

        private void checkForWin(PictureBox tileLastPlayed)
        {
            var currentState = getCurrentState();
            var tileRow = getTileRow(tileLastPlayed);
            var tileCol = getTileCol(tileLastPlayed);


        }

        private int getTileRow(PictureBox tile)
        {
            PictureBox[] arrayOfTiles = allTiles.ToArray();
            int tileIndex = Array.IndexOf(arrayOfTiles, tile);
            return tileIndex / 10;            
        }

        private int getTileCol(PictureBox tile)
        {
            PictureBox[] arrayOfTiles = allTiles.ToArray();
            int tileIndex = Array.IndexOf(arrayOfTiles, tile);
            return tileIndex % 10;
        }

        private Array getCurrentState()
        {
            int row = 0;
            int col = 0;

            for(var tile = 0; tile < allTiles.Count; tile++)
            {
                if(col >= gridSize) // gridSize = 10
                {
                    row++;
                    col = 0;
                    currentStateOfBoard[row, col] = (String)allTiles[tile].Tag;
                }
                else
                {
                    currentStateOfBoard[row, col] = (String)allTiles[tile].Tag;
                    col++;
                }
            }
            return currentStateOfBoard;
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
