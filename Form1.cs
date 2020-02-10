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
        private Game game;
        

        public Form1()
        {
            InitializeComponent();
            game = new Game();
            game.allTiles = new PictureBox[game.gridSize * game.gridSize];

            foreach (var pb in tableLayoutPanel1.Controls.OfType<PictureBox>())
            {
                if (pb.Name.StartsWith("pictureBox"))
                {
                    var pbIndex = int.Parse(pb.Name.Replace("pictureBox", "")) - 1;
                    game.allTiles[pbIndex] = pb;
                }
            }

            game.currentStateOfBoard = new String[game.gridSize, game.gridSize];
        }


        private void playerChoosesTile(object sender, EventArgs e)
        {
            if (game.hasWon || game.draw)
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

            var nextPlayer = game.currentPlayer == "X" ? "O" : "X";
            var nextPlayerImage = game.currentPlayer == "X" ? Properties.Resources.o : Properties.Resources.x;
            var currentPlayerImage = game.currentPlayer == "X" ? Properties.Resources.x : Properties.Resources.o;
            
            // Put the current player's image in the tile
            tileToPlay.Image = currentPlayerImage;
            tileToPlay.Tag = game.currentPlayer;

            // Check for win or draw
            if (game.checkForWin(tileToPlay))
            {
                MessageBox.Show("Win: " + tileToPlay.Tag);
                return;
            } 
            else if (game.thereIsADraw())
            {
                MessageBox.Show("It's a draw");
                currentTurnLabel.Text = "Draw!";
                currentTurnImage.Image = null;
                return;
            }

            // Round finished, update to show it's next player's turn
            game.currentPlayer = nextPlayer;
            currentTurnImage.Image = nextPlayerImage;
        }

               
        private void resetButton_Click(object sender, EventArgs e)
        {

            for (var tile = 0; tile < game.allTiles.Length; tile++)
            {
                game.allTiles[tile].Tag = null;
                game.allTiles[tile].Image = null;
                game.currentPlayer = "X";
                game.hasWon = false;
                game.winningPlayer = null;
                currentTurnLabel.Text = "Current Turn: ";
                currentTurnImage.Image = Properties.Resources.x;
            }
        }
    }
}
