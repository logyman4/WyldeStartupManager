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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WyldeStartupManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Storyboard closeStoryBoard;
        //private Storyboard closeStoryBoardLeave;
        private Storyboard maximizeStoryBoard;
        private Storyboard minimizeStortBoard;
        private DoubleAnimation systemButtons;

        public MainWindow()
        {
            InitializeComponent();
            systemButtons = new DoubleAnimation();
            systemButtons.From = 0;
            systemButtons.To = 1;
            systemButtons.Duration = new Duration(TimeSpan.FromSeconds(.5));
            systemButtons.FillBehavior = FillBehavior.HoldEnd;
            closeStoryBoard = new Storyboard();
            Storyboard.SetTarget(systemButtons, closeColorBox);
            maximizeStoryBoard = new Storyboard();
            Storyboard.SetTarget(systemButtons, maximizeButton);
            minimizeStortBoard = new Storyboard();
            Storyboard.SetTarget(systemButtons, minimizeColorBox);
            Storyboard.SetTargetProperty(systemButtons, new PropertyPath(TextBlock.OpacityProperty));
            maximizeStoryBoard.Children.Add(systemButtons);
            minimizeStortBoard.Children.Add(systemButtons);
            closeStoryBoard.Children.Add(systemButtons);
        }

        private void dragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButtons_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void maximizeButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Maximized)
            {
                WindowState = System.Windows.WindowState.Normal;
                maximizeButton.Text = "1";
            }
            else
            {
                WindowState = System.Windows.WindowState.Maximized;
                maximizeButton.Text = "2";
            }
        }

        private void minimizeButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private void closeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender == closeButton)
            {
                systemButtons.From = 0;
                systemButtons.To = 1;
                closeStoryBoard.Begin();
            }
            else if (sender == maximizeButton)
            {
                systemButtons.From = 0;
                systemButtons.To = 1;
                maximizeStoryBoard.Begin();
            }
            else if (sender == minimizeButton)
            {
                systemButtons.From = 0;
                systemButtons.To = 1;
                minimizeStortBoard.Begin();
            }
        }

        private void closeColorBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender == closeButton)
            {
                systemButtons.From = 1;
                systemButtons.To = 0;
                closeStoryBoard.Begin();
            }
            else if (sender == maximizeButton)
            {
                systemButtons.From = 1;
                systemButtons.To = 0;
                maximizeStoryBoard.Begin();
            }
            else if (sender == minimizeButton)
            {
                systemButtons.From = 1;
                systemButtons.To = 0;
                minimizeStortBoard.Begin();
            }
        }
    }
}
