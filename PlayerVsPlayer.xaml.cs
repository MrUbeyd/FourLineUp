using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;




namespace FourLineUp
{
    /// <summary>
    /// Interaction logic for PlayerVsPlayer.xaml
    /// </summary>
    public partial class PlayerVsPlayer : UserControl
    {
        private int playerTurn;//int which is grow +1 every addTile process and i check %2 for which players turn

        //this array keep {X,O} for control condition of FourLineUpGame
        private char[,] markBoard = new char[6, 6];

        private int Height = 6;
        private int Width = 6;

        



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
            
            cleanBoardArray(markBoard);// make sure the markBoard array clean (all items different(z) from X or O ) 
            playerTurn = 0;
        }

        
        //clean markBoard array for new game
        private void cleanBoardArray(char[,] markBoard)
        {
            for (int i = 0; i < Height-1; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    markBoard[i, j] = 'z';//z just different char from {X,O}
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

            //adding tile to board every mouse left click 
            AddTile(row, column,XO);

            //if we have winner message box pops (player {X or O} WIN !!!) and exit
            int checkWin ;
            checkWin = Check4New(markBoard);
            if (checkWin == 1)
            {
                MessageBox.Show("Player " + XO.ToString() + " WIN !!!");
                Environment.Exit(1);
            }

            playerTurn++;

        }

        // this func control if any 4 line condition for active player
        public int Check4New(char[,] markBoard)
        {
            char XO;
            int win;

            //who's turn  player X or O 
            if (playerTurn % 2 == 0)
            {
                XO = 'X';
            }
            else
            {
                XO = 'O';
            }

            win = 0;

            // horizontalCheck 
            for (int j = 0; j < Height - 3; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    if (markBoard[i,j] == XO && markBoard[i,j + 1] == XO && markBoard[i,j + 2] == XO && markBoard[i,j + 3] == XO)
                    {
                        win=1;
                        return win;
                    }
                }
            }
            // verticalCheck
            for (int i = 0; i < Width - 3; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (markBoard[i,j] == XO && markBoard[i + 1,j] == XO && markBoard[i + 2,j] == XO && markBoard[i + 3,j] == XO)
                    {
                        win=1;
                        return win;
                    }
                }
            }
            // ascendingDiagonalCheck 
            for (int i = 3; i < Width; i++)
            {
                for (int j = 0; j < Height - 3; j++)
                {
                    if (markBoard[i,j] == XO && markBoard[i - 1,j + 1] == XO && markBoard[i - 2,j + 2] == XO && markBoard[i - 3,j + 3] == XO)
                    {
                        win = 1;
                        return win;
                    }
                }
            }
            // descendingDiagonalCheck
            for (int i = 3; i < Width; i++)
            {
                for (int j = 3; j < Height; j++)
                {
                    if (markBoard[i,j] == XO && markBoard[i - 1,j - 1] == XO && markBoard[i - 2,j - 2] == XO && markBoard[i - 3,j - 3] == XO)
                    {
                        win=1;
                        return win;
                    }
                        
                }
            }
            win=0;
            return win;
        }

        

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            NewGame();
        }
    }
}
