using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace Caro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int[,] _a;
        Button[,] _buttons;

        const int Rows = 6;
        const int Cols = 6;
        const int ButtonWidth = 70;
        const int ButtonHeigth = 70;
        const int WinCondition = 5;
        const int Padding = 1;
        const int TopOffset = 50;
        const int LeftOffset = 50;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _a = new int[Rows, Cols];
            _buttons = new Button[Rows, Cols];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    var button = new Button();
                    button.Width = ButtonWidth;
                    button.Height = ButtonHeigth;
                    button.Tag = new Tuple<int, int>(i, j);
                    button.Click += Button_Click;
                    button.BorderThickness =  new Thickness(2,2,2,2);

                    //Dua vao model quan li UI
                    _buttons[i, j] = button;
                    //add button to UI
                    UICanvas.Children.Add(button);
                    Canvas.SetLeft(button, LeftOffset + j * (ButtonWidth + Padding));
                    Canvas.SetTop(button, TopOffset + i * (ButtonHeigth +Padding) + Padding);
                }
                Debug.WriteLine("");
            }


        }
        bool isXTurn = true;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var (i, j) = button.Tag as Tuple<int, int>;

           // MessageBox.Show($"Click on button {i} - {j}");

            if (_a[i,j] == 0)
            {
                if (isXTurn)
                {
                    button.Content = "X";
                    _a[i, j] = 1;
                }
                else
                {
                    button.Content = "O";
                    _a[i, j] = 2;
                }
                isXTurn = !isXTurn;

                var (gameOver, xWin) = checkWin(_a, i, j);
                if (gameOver)
                {
                    if (xWin)
                    {
                        MessageBox.Show("X Win");
                    } else
                    {
                        MessageBox.Show("O Win");
                    }
                }
            }

        }

        private (bool, bool) checkWin(int[,] a, int i, int j)
        {
            var gameOver = false;
            var xWin = false;

            if (checkWinHorizontal(_a, i, j) || checkWinVertical(_a, i, j)
                || checkWinDiagonal(_a, i, j))
            {
                gameOver = true;
                xWin = _a[i, j] == 1;
            }


            return (gameOver, xWin);
        }

        /* Xet chieu ngang*/
        private bool checkWinHorizontal(int[,] a, int i, int j)
        {
            int di = 0;
            int dj = -1;
            int startI = i;
            int startJ = j;
            int count = 1;

            while (-1 != startJ + dj)
            {
                startJ += dj;
                if (_a[i, j] == a[i, startJ])
                {
                    count++;
                }
                else break;
            }

            //Xet theo ben phai
            startJ = j;
            dj = 1;
            while (startJ + dj != Cols)
            {
                startJ += dj;
                if (_a[i, j] == a[i, startJ])
                {
                    count++;
                }
                else break;
            }
            if (count >= WinCondition) return true;
            return false;
        }

        /* Xet chieu doc*/
        private bool checkWinVertical(int[,] a, int i, int j)
        {
            int di = -1;
            int dj = 0;
            int startI = i;
            int startJ = j;
            int count = 1;

            while (-1 != startI + di)
            {
                startI += di;
                if (_a[i, j] == a[startI, j])
                {
                    count++;
                }
                else break;
            }

            startI = i;
            di = 1;
            while (startI + di != Rows)
            {
                startI += di;
                if (_a[i, j] == a[startI, j])
                {
                    count++;
                }
                else break;
            }

            if (count >= WinCondition) return true;
            return false;
        }
        /* Xet duong cheo */
        private bool checkWinDiagonalToRight(int[,] a, int i, int j)
        {
            int di = -1;
            int dj = -1;
            int startI = i;
            int startJ = j;
            int count = 1;

            //xet cheo tren ben trai
            while (-1 != startI + di && -1 != startJ + dj)
            {
                startI += di;
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])
                {
                    count++;
                }
                else break;
            }


            //xet cheo duoi ben phai
            startI = i;
            startJ = j;
            di = 1;
            dj = 1;
            while (startI + di != Rows && startJ + dj != Cols)
            {
                startI += di;
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])
                {
                    count++;
                }
                else break;
            }

            if (count >= WinCondition) return true;
            return false;
        }
        private bool checkWinDiagonalToLeft(int[,] a, int i, int j)
        {
            int di = 1;
            int dj = -1;
            int startI = i;
            int startJ = j;
            int count = 1;

            //xet cheo duoi ben trai
            while (Rows != startI + di && -1 != startJ + dj)
            {
                startI += di;
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])
                {
                    count++;
                }
                else break;
            }


            //xet tren duoi ben phai
            startI = i;
            startJ = j;
            di = -1;
            dj = 1;
            while (startI + di != -1 && startJ + dj != Cols)
            {
                startI += di;
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])
                {
                    count++;
                }
                else break;
            }

            if (count >= WinCondition) return true;
            return false;
        }
        private bool checkWinDiagonal(int[,] a, int i, int j)
        {
            return checkWinDiagonalToLeft(a, i, j) || checkWinDiagonalToRight(a, i, j);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            const string filename = "save.txt";

            var writer = new StreamWriter(filename);
            //Dong dau tien la luot di hien tai
            writer.WriteLine(isXTurn ?"X" : "O");

            for (int i = 0; i < Cols; i++)
            {
                for (int j =0; j < Rows; j++)
                {
                    writer.Write($"{_a[i, j]}");
                    if (j != Cols)
                    {
                        writer.Write(" ");
                    }
                }
                writer.WriteLine("");
            }
            writer.Close();

            MessageBox.Show("Game saved");
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            if (screen.ShowDialog() == true)
            {
                var filename = screen.FileName;

                StreamReader reader = new StreamReader(filename);
                var firstLine = reader.ReadLine();
                isXTurn = firstLine == "X";

                for (int i = 0; i < Rows; i++)
                {
                    var tokens = reader.ReadLine().Split(
                        new string[] { " " }, StringSplitOptions.None);
                    //Model

                    for (int j = 0; j < Cols; j++)
                    {
                        _a[i, j] = int.Parse(tokens[j]);
                        //UI
                        if (_a[i, j] == 1)
                        {
                            _buttons[i, j].Content = "X";
                        }
                        if (_a[i, j] == 2)
                        {
                            _buttons[i, j].Content = "O";
                        }
                    }
                }
                MessageBox.Show("Game is Loaded");
            }
        }
    }
}
