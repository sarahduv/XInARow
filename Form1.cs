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
        public String winningPlayer;
        public bool hasWon = false;
        public bool draw = false;
        public int maxInARow = 4;

        public Form1()
        {
            InitializeComponent();
            allTiles = new List<PictureBox>();
            createList();
            
            /*foreach(var pb in Controls.OfType<PictureBox>())
            {
                if (pb.Name.StartsWith("pictureBox"))
                {
                    allTiles.Add(pb);
                }
            }*/
        }

        private void createList()
        {
            Control[] matches;
            for(var i = 1; i <= 100; i++)
            {
                matches = this.Controls.Find("pictureBox" + i.ToString(), true);
                if (matches.Length > 0 && matches[0] is PictureBox)
                {
                    allTiles.Add((PictureBox)matches[0]);
                }
            }
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

                /*if (checkForWin())
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

             /*   if (checkForWin())
                {

                }*/

                currentPlayer = "X";
                currentTurnImage.Image = Properties.Resources.x;
            }
            else
            {
                MessageBox.Show("This tile is taken, choose another tile.");
            }
        }

        private bool checkForWin()
        {
            return true;
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
