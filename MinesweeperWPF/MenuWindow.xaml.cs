//Martin PIAT group 2F

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
using System.Windows.Shapes;

namespace MinesweeperWPF
{
    //Window for game options
    public partial class MenuWindow : Window
    {
        private int gridSize;
        private int nbMines;

        public int GetGridSize()
        {
            return gridSize;
        }

        public int GetNbMines()
        {
            return nbMines;
        }

        public MenuWindow()
        {
            InitializeComponent();
        }

        private void BTNNewGame_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                int size = int.Parse(TXTGridSize.Text);
                int mines = int.Parse(TXTNbMines.Text);

                if (size > 1 && mines > 0 && mines < (size * size) - 8) //9 non-mine cells are necessary to eventually generate a grid with a cell of weight 0. Otherwise the grid will regenerate eternally on the first click. 
                {
                    //Save values entered as variables
                    gridSize = size;
                    nbMines = mines;
                    DialogResult = true; //Validate new values to be passed to the main window
                    Close();
                }
                else
                {
                    MessageBox.Show("Invalid numbers.\nGrid size must be > 1.\nMines must be > 0 and < total number of cells.");
                }
            }
            catch //if the user enters text that can't be parsed as int
            {
                MessageBox.Show("Please enter numbers only.");
            }
        }

        private void BTNCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
