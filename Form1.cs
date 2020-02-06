using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XInARow
{
    public partial class Form1 : Form
    {
        public String currentPlayer = "X";
        public PictureBox[] allTiles;
        public PictureBox[][] possibleWins;
        public String winningPlayer;
        public bool hasWon = false;
        public bool draw = false;
        public int maxInARow = 4;

        public Form1()
        {
            InitializeComponent();
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

                if (checkForWin())
                {
                    hasWon = true;
                    MessageBox.Show("Player " + currentPlayer + " has won.");
                    currentTurnLabel.Text = "Winner: ";
                    return;
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

                if (checkForWin())
                {
                    hasWon = true;
                    MessageBox.Show("Player " + currentPlayer + " has won.");
                    currentTurnLabel.Text = "Winner: ";
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

        private bool checkForWin()
        {

        }

        private bool thereIsADraw()
        {

        }
    }
}
