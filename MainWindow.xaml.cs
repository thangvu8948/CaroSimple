using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        const int Rows = 6;
        const int Cols = 6;
        const int ButtonWidth = 70;
        const int ButtonHeigth = 70;
        const int WinCondition = 5;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _a = new int[Rows, Cols];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    var button = new Button();
                    button.Width = ButtonWidth;
                    button.Height = ButtonHeigth;
                    button.Tag = new Tuple<int, int>(i, j);
                    button.Click += Button_Click;

                    //add button to UI
                    UICanvas.Children.Add(button);
                    Canvas.SetLeft(button, j * ButtonWidth);
                    Canvas.SetTop(button, i * ButtonHeigth);

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
    }
}
