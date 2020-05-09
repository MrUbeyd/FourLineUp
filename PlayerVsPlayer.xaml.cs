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
        private int playerTurn;//Int which is grow +1 every addTile process and I check (%2) for which players turn

        //This array keep {X,O} for control condition of FourLineUpGame
        private char[,] markBoard = new char[6, 6];

        private int boardHeight = 6;
        private int boardWidth = 6;

        



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
            
            CleanBoardArray(markBoard);// Make sure the markBoard array clean (all items different(z) from X or O ) 
            playerTurn = 0;
            //TestCleanBoardArray(markBoard);
        }

        
        //Clean markBoard array for new game
        private void CleanBoardArray(char[,] markBoard)
        {
            for (int i = 0; i < boardHeight-1; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    markBoard[i, j] = 'z';//z just different char from {X,O}
                }
            }
        }


        //I ran this func at video 
        private void TestCleanBoardArray(char[,] markBoard)
        {
            //I show every 3 item of  each row just for testing CleanBoardArray func run correctly
            for (int i = 0; i < boardHeight ; i++)
            {
                MessageBox.Show( (i+1).ToString(". line")+" "+markBoard[i,0].ToString()+ " " + markBoard[i, 1].ToString()+ " " + markBoard[i, 2].ToString());
            }
        }


        private void AddTile(int row, int column, char symbol)
        {
            // Lets Create TextBlock
            var block = new TextBlock
            {
                //Set the text fontSize and location
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

            //Changing players by checking playerTurn
            char XO;
            if (playerTurn%2==0)
            {
                XO = 'X';
            }
            else
            {
                XO = 'O';
            }

            //Adding tile to board every mouse left click 
            AddTile(row, column,XO);

            //If we have winner message box pops (player {X or O} WIN !!!) and exit
            int checkWin ;//store the CheckFourLine func return value
            checkWin = CheckFourLine(markBoard);
            if (checkWin == 1)
            {
                MessageBox.Show("Player " + XO.ToString() + " WIN !!!");
                Environment.Exit(1);
            }

            playerTurn++;

        }

        // This func control if any 4 line condition for active player
        //If any player wins the function return 1 otherwise return 0
        public int CheckFourLine(char[,] markBoard)
        {
            char XO;
            int win;

            //Who's turn  player X or O 
            if (playerTurn % 2 == 0)
            {
                XO = 'X';
            }
            else
            {
                XO = 'O';
            }

            win = 0;

            // HorizontalCheck 
            for (int j = 0; j < boardHeight - 3; j++)
            {
                for (int i = 0; i < boardWidth; i++)
                {
                    if (markBoard[i,j] == XO && markBoard[i,j + 1] == XO && markBoard[i,j + 2] == XO && markBoard[i,j + 3] == XO)
                    {
                        win=1;
                        return win;
                    }
                }
            }
            // VerticalCheck
            for (int i = 0; i < boardWidth - 3; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (markBoard[i,j] == XO && markBoard[i + 1,j] == XO && markBoard[i + 2,j] == XO && markBoard[i + 3,j] == XO)
                    {
                        win=1;
                        return win;
                    }
                }
            }
            // AscendingDiagonalCheck 
            for (int i = 3; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight - 3; j++)
                {
                    if (markBoard[i,j] == XO && markBoard[i - 1,j + 1] == XO && markBoard[i - 2,j + 2] == XO && markBoard[i - 3,j + 3] == XO)
                    {
                        win = 1;
                        return win;
                    }
                }
            }
            // DescendingDiagonalCheck
            for (int i = 3; i < boardWidth; i++)
            {
                for (int j = 3; j < boardHeight; j++)
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
