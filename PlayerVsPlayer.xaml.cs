using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace FourLineUp
{
    /// <summary>
    /// Interaction logic for PlayerVsPlayer.xaml
    /// </summary>
    public partial class PlayerVsPlayer : UserControl
    {
        private int playerTurn;//int which is grow +1 every addTile process and i check %2 for which players turn

        private char[,] markBoard = new char[9, 10];

        //private bool mGameEnded;//True if the game has ended.



        public PlayerVsPlayer()
        {
            InitializeComponent();
        }

        private void NewGame()
        {
            // Clear out all the tiles.
            GameBoard.Children.OfType<TextBlock>().ToList().ForEach(block =>
            {
                GameBoard.Children.Remove(block);
            });
            //mGameEnded = false;
            cleanBoardArray(markBoard);// make sure the array clean (all items different(z) from X or O ) 
            playerTurn = 0;
        }

        
        private void cleanBoardArray(char[,] markBoard)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    markBoard[i, j] = 'z';
                }
            }
        }
        private void AddTile(int row, int column, char symbol)
        {
            // Lets Create TextBlock
            var block = new TextBlock
            {
                Text = symbol.ToString(),
                FontSize = 36,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
            };

            // Add Block to Board
            GameBoard.Children.Add(block);

            // Set the coordinate of the cell
            Grid.SetRow(block, row);
            Grid.SetColumn(block, column);

            markBoard[column, row] = symbol;

            playerTurn++;
            //MessageBox.Show(symbol.ToString());
            
           /* int checkWin = 0;
            checkWin=CheckFour(markBoard);
            if (checkWin==1)
            {
                MessageBox.Show("Player " + symbol.ToString() + " WIN !!!");
            }
            */

        }

        private void GameBoard_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // A Player clicked lets get the column we are on.
            // Lets scan where the mouse is.

            // Get Mouse position relative to GameBoard.
            var point = Mouse.GetPosition(GameBoard);

            var column = 0;
            var widthSum = 0.0;

            foreach (var columnDefinition in GameBoard.ColumnDefinitions)
            {
                widthSum += columnDefinition.ActualWidth;
                if (widthSum >= point.X)
                    break;
                column++;
            }

            var row = 0;

            // Lets find an empty row.
            for(var i = GameBoard.RowDefinitions.Count -1; i >= 0; i--)
            {
                var el = GameBoard.Children.Cast<UIElement>().FirstOrDefault(element => Grid.GetColumn(element) == column && Grid.GetRow(element) == i);
                if (el == null)
                {
                    // Its empty. Break loop and assign row number.
                    row = i;
                    break;
                }

                if (i == 0) // We are on the top and there are no empty cells left. Warn the Player.
                {
                    MessageBox.Show("Chosen column is full. Please chose another column");
                    return;
                }
            }

            //changing players by checking playerTurn
            char XO;
            if (playerTurn%2==0)
            {
                XO = 'X';
            }
            else
            {
                XO = 'O';
            }

            
            AddTile(row, column,XO);

            //?????????????????????????????????????????????????
            int checkWin = 0;
            checkWin = CheckFour(markBoard);
            if (checkWin == 1)
            {
                MessageBox.Show("Player " + XO.ToString() + " WIN !!!");
            }

        }

        // function for Congratulations player (this process replies too much so i write this func)??????????????????????
        private void Congratulations(char XO)
        {
            MessageBox.Show("Player " + XO + " WIN!!!");
            Environment.Exit(1);
        }

        // this func control if any 4 line condition for active player
        public int CheckFour(char[,] markBoard)
        {
            char XO;
            int win;

            if (playerTurn%2 == 0)
            {
                XO = 'X';
            }
            else
            {
                XO = 'O';
            }

            win = 0;

            for (int i = 8; i >= 1; --i)
            {

                for (int j = 9; j >= 1; --j)
                {

                    if (markBoard[i, j] == XO &&
                        markBoard[i - 1, j - 1] == XO &&
                        markBoard[i - 2, j - 2] == XO &&
                        markBoard[i - 3, j - 3] == XO)
                    {
                        win = 1;
                        //Congratulations(XO);
                        
                    }


                    if (markBoard[i, j] == XO &&
                        markBoard[i, j - 1] == XO &&
                        markBoard[i, j - 2] == XO &&
                        markBoard[i, j - 3] == XO)
                    {
                        win = 1;
                        //Congratulations(XO);
                    }

                    if (markBoard[i, j] == XO &&
                        markBoard[i - 1, j] == XO &&
                        markBoard[i - 2, j] == XO &&
                        markBoard[i - 3, j] == XO)
                    {
                        win = 1;
                        //Congratulations(XO);
                    }
                    
                    if (markBoard[i, j] == XO &&
                        markBoard[i - 1, j + 1] == XO &&
                        markBoard[i - 2, j + 2] == XO &&
                        markBoard[i - 3, j + 3] == XO)
                    {
                        win = 1;
                        //Congratulations(XO);
                    }
                    
                    if (markBoard[i, j] == XO &&
                        markBoard[i, j + 1] == XO &&
                        markBoard[i, j + 2] == XO &&
                        markBoard[i, j + 3] == XO)
                    {
                        win = 1;
                        //Congratulations(XO);
                    }
                    
                    
                }

            }

            return win;
        }
        

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            NewGame();
        }
    }
}
