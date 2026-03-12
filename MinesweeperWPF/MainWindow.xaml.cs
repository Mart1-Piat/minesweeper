//Martin PIAT group 2F
using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;


namespace MinesweeperWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //----------------------------
        //  VARIABLES
        //----------------------------

        private int gridSize = 10;      // grid size
        private int nbMines = 10;       // number of mines
        private int nbCellsChecked = 0; // number of cells that have been checked (opened)
        private int[,] matrix;          // matrix preserving grid values (see below)



        //----------------------------
        //  INITIALIZING FUNCTIONS
        //----------------------------

        public MainWindow()
        {
            InitializeComponent();
        }

        private UIElement GetUIElementFromPosition(Grid g, int col, int row)
        {
            return g.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);
        }

        private void WNDMainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            InitializeGrid();
        }

        private void InitializeGrid()
        {
            matrix = new int[gridSize, gridSize];
            nbCellsChecked = 0;
            GRDGame.Children.Clear();
            GRDGame.ColumnDefinitions.Clear();
            GRDGame.RowDefinitions.Clear();
            for (int i = 0; i < gridSize; i++)
            {
                GRDGame.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                GRDGame.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            }
            AddMinesToMatrix();
            FillGrid();
        }

        private void FillGrid()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    //Build a border
                    Border b = new Border();
                    b.BorderThickness = new Thickness(1);
                    b.BorderBrush = new SolidColorBrush(Colors.LightBlue);
                    b.SetValue(Grid.RowProperty, j);
                    b.SetValue(Grid.ColumnProperty, i);
                    GRDGame.Children.Add(b);

                    //Add a grid to a border to contain two children 
                    Grid g = new Grid();
                    b.Child = g;

                    //Add a button and Label
                    Label LBLCell = new Label();
                    //Add game logic to the Label
                    AddLogic(LBLCell, i, j);
                    g.Children.Add(LBLCell);

                    Button BTNCell = new Button();
                    //Add game logic to the button
                    AddLogic(BTNCell);
                    g.Children.Add(BTNCell);
                }
            }
        }

        private void AddMinesToMatrix()
        {
            Random rnd = new Random();
            int planted = 0;
            while (planted < nbMines)
            {
                int i = rnd.Next(0, gridSize);
                int j = rnd.Next(0, gridSize);

                if (matrix[i, j] != -1) // Only place if there isn't a mine there
                {
                    matrix[i, j] = -1;
                    IncrementAdjacent(i, j);
                    planted++;
                }
            }
        }

        private void IncrementAdjacent(int i, int j) //Handles the proximity score of the cells bordering a mine
        {
            for (int m = i - 1; m <= i + 1; m++)
            {
                for (int n = j - 1; n <= j + 1; n++)
                {
                    if (m >= 0 && m < gridSize && n >= 0 && n < gridSize && matrix[m, n] != -1) //For each non-mine cell within the grid, increment the "weight" (proximity score)
                    {
                        matrix[m, n] = matrix[m, n] + 1;
                    }
                }
            }
        }

        private void AddLogic(Button BTNCell) //
        {
            BTNCell.Click += ClickCell;
            BTNCell.MouseRightButtonUp += MarkCell;
        }

        private void AddLogic(Label LBLCell, int i, int j)
        {
            LBLCell.Content = matrix[i, j]; //Set the label to display the cell weight (proximity score)

            if (LBLCell.Content.Equals(0)) //If cell is not near a mine, hide the proximity score of 0
            {
                LBLCell.Visibility = Visibility.Collapsed;
            }
            if (LBLCell.Content.Equals(-1))
            {
                LBLCell.Content = "💣";
            }

            LBLCell.HorizontalAlignment = HorizontalAlignment.Center;
            LBLCell.VerticalAlignment = VerticalAlignment.Center;
            LBLCell.FontWeight = FontWeights.Bold;
        }

        private void ResetGame()
        {
            InitializeGrid();
        }



        //-----------------
        // EVENT PROCEDURES
        //-----------------

        private void ClickCell(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            //Get button coordinates
            Border bor = (Border)VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(b));
            int col = Grid.GetColumn(bor);
            int row = Grid.GetRow(bor);


            if (!(b.Content == "🚩")) //Prevent accidental clicking on marked mines
            {
                CheckCell(col, row);
            }
        }

        private void MarkCell(object sender, RoutedEventArgs e) //Visually Mark the cell as a mine with Right Click
        {
            Button b = (Button)sender;
            if (b.Content == "🚩") { b.Content = null; }
            else { b.Content = "🚩"; }
        }



        private bool CheckCell(int col, int row)
        {
            //Cell components
            Border bor = (Border)GetUIElementFromPosition(GRDGame, col, row);
            Grid cellGrid = (Grid)bor.Child;
            Label lbl = (Label)cellGrid.Children[0];
            Button b = (Button)cellGrid.Children[1];

            //IF the cell has not already been checked (the button is still visible / active)
            if (b.Visibility == Visibility.Visible)
            {
                // Hide / deactivate the button and display the value of this cell
                b.Visibility = Visibility.Collapsed;

                //Increment CellsChecked count
                nbCellsChecked++;

                //If it is the first click, generate grid until cell value is 0
                if (nbCellsChecked == 1)
                {
                    if (!lbl.Content.Equals(0))
                    {
                        InitializeGrid();
                        return (CheckCell(col, row));
                    }
                }

                //IF the cell is a bomb
                if (lbl.Content.Equals("💣"))
                {
                    //THEN{ game lost, reset the game; return TRUE }
                    MessageBox.Show("You hit a mine!", "Game over");
                    ResetGame();
                    return true;
                }
                else
                {
                    //IF it was the last cell to be checked
                    if (nbCellsChecked == (gridSize * gridSize) - nbMines)
                    {
                        //THEN{ game won, reset the game; return TRUE}
                        MessageBox.Show("You've successfully cleared the mine field!", "Victory");
                        ResetGame();
                        return true;
                    }
                    else
                    {
                        //Check the value of this cell
                        if (lbl.Content.Equals(0))
                        {
                            // the procedure calls itself on neighboring cells
                            for (int i = Math.Max(0, col - 1); i <= Math.Min(gridSize - 1, col + 1); i++)
                            {
                                for (int j = Math.Max(0, row - 1); j <= Math.Min(gridSize - 1, row + 1); j++)
                                {
                                    bool resultat = CheckCell(i, j);
                                    if (resultat) return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        //------------------
        //  MENU PROCEDURES
        //------------------
        private void BTNGameOptions_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow();

            if (menu.ShowDialog() == true) //When the menu is exited with the New Game button, update the game parameters and reset the game
            {
                gridSize = menu.GetGridSize();
                nbMines = menu.GetNbMines();
                ResetGame();
            }
        }
    }
}
