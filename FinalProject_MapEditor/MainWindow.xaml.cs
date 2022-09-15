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
using Microsoft.Win32;
using static FinalProject_MapEditor.LevelMap;

namespace FinalProject_MapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // The images of different tiles
        private BitmapImage playerImg;
        private BitmapImage enemyImg;
        private BitmapImage wallImg;

        /// <summary>
        /// The canvas representing the tiles
        /// </summary>
        private Canvas[,] canvasArray;

        /// <summary>
        /// Chosen tile
        /// </summary>
        private Tile curTile = Tile.None;

        /// <summary>
        /// Current Level map
        /// </summary>
        private LevelMap levelMap;
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set up this level map
        /// </summary>
        /// <param name="size">It equals the width and height of this level map</param>
        public void SetUp(int size)
        {
            canvasArray = new Canvas[size, size];
            levelMap = new LevelMap(size);
            ClearCanvasAndDrawBorder();
            // loads the images
            playerImg = new BitmapImage(new Uri("res\\player.png", UriKind.Relative));
            enemyImg = new BitmapImage(new Uri("res\\enemy.png", UriKind.Relative));
            wallImg = new BitmapImage(new Uri("res\\wall.png", UriKind.Relative));
        }

        /// <summary>
        /// Clear the canvas and then draw the border
        /// </summary>
        private void ClearCanvasAndDrawBorder()
        {
            // remove all chidlren of the canvas
            mapCanvas.Children.Clear();

            // draw the border of the tiles
            int size = levelMap.GetSize();
            int length = 580;
            double gap = length / size;
            for(int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // draw the borders
                    Rectangle rect = new Rectangle();
                    rect.Stroke = Brushes.Black;
                    rect.Width = gap;
                    rect.Height = gap;
                    mapCanvas.Children.Add(rect);
                    Canvas.SetLeft(rect, j * gap);
                    Canvas.SetTop(rect, i * gap);
                }
            }
        }

        /// <summary>
        /// Draw one tile
        /// </summary>
        /// <param name="sx">x component of the location</param>
        /// <param name="sy">y component of the location</param>
        /// <param name="tile">the type of the tile</param>
        private void DrawOneTile(int sx, int sy, Tile tile)
        {
            // Draw the tile on the canvas
            ImageBrush myImageBrush = null;
            if (tile == Tile.Player)
            {
                myImageBrush = new ImageBrush(playerImg);
            }
            else if (tile == Tile.Enemy)
            {
                myImageBrush = new ImageBrush(enemyImg);
            }
            else if (tile == Tile.Wall)
            {
                myImageBrush = new ImageBrush(wallImg);
            }

            int size = levelMap.GetSize();
            int length = 580;
            int gap = length / size;

            // First remove the thing in the tile
            int row = sx / gap;
            int col = sy / gap;
            if (!levelMap.CheckEmptyTileAt(row, col))
            {
                ClearOneTile(sx, sy);
            }

            // show this image
            Canvas myCanvas = new Canvas();
            myCanvas.Width = gap;
            myCanvas.Height = gap;
            myCanvas.Background = myImageBrush;
            canvasArray[row, col] = myCanvas;
            mapCanvas.Children.Add(myCanvas);
            Canvas.SetLeft(myCanvas, sx);
            Canvas.SetTop(myCanvas, sy);
            levelMap.PlaceThingAt(row, col, tile);
        }

        /// <summary>
        /// Clear one tile
        /// </summary>
        /// <param name="sx">x component of the location</param>
        /// <param name="sy">y component of the location</param>
        private void ClearOneTile(int sx, int sy)
        {
            int size = levelMap.GetSize();
            int length = 580;
            int gap = length / size;

            int row = sx / gap;
            int col = sy / gap;
            if (!levelMap.CheckEmptyTileAt(row, col))
            {
                levelMap.RemoveThingAt(row, col);
                mapCanvas.Children.Remove(canvasArray[row, col]);
            }
        }

        /// <summary>
        /// Get the tile location clicked by the user
        /// </summary>
        /// <param name="sx">x component of screen position</param>
        /// <param name="sy">y component of screen position</param>
        /// <param name="blockRow">the row of the tile</param>
        /// <param name="blockCol">the column of the til</param>
        private void GetClickedTile(int sx, int sy, out int tileRow, out int tileCol)
        {
            int size = levelMap.GetSize();
            int length = 580;
            int gap = length / size;
            if (sx < 0 || sx > gap * size
                || sy < 0 || sy > gap * size)
            {
                tileRow = -1;
                tileCol = -1;
            }
            else
            {
                tileRow = sx / gap;
                tileCol = sy / gap;
            }
        }

        /// <summary>
        /// Get the screen location by the tile location
        /// </summary>
        /// <param name="tileRow">the row of the tile</param>
        /// <param name="tileCol">the column of the tile</param>
        /// <param name="sx">x component of screen position</param>
        /// <param name="sy">y component of screen position</param>
        private void GetTileLocation(int tileRow, int tileCol, out int sx, out int sy)
        {
            int size = levelMap.GetSize();
            int length = 580;
            int gap = length / size;
            sx = tileRow * gap;
            sy = tileCol * gap;
        }


        /// <summary>
        /// Create a new level map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewLevel(object sender, RoutedEventArgs e)
        {
            AskForSaveLevelMap(sender, e);
            LevelConfig window = new LevelConfig();
            Close();
            window.Show();
        }

        /// <summary>
        /// Load one level map from one json file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenLevel(object sender, RoutedEventArgs e)
        {
            AskForSaveLevelMap(sender, e);
            // Prompt the user to choose input path
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Level map|*.json";
            ofd.Title = "Choose input path";
            if ((bool)ofd.ShowDialog())
            {
                LevelMap mLevelMap = LevelMap.LoadFromJsonFile(ofd.FileName);
                if (mLevelMap == null)
                {
                    // Prompt the user that the deserialization failed
                    MessageBox.Show("Failed to load this level map.");
                }
                else
                {
                    levelMap = mLevelMap;
                    ShowTilesByLevelMap();
                }
            }
        }

        /// <summary>
        /// Show tiles by the created level map
        /// </summary>
        private void ShowTilesByLevelMap()
        {
            ClearCanvasAndDrawBorder();
            // Show the level map
            int size = levelMap.GetSize();
            canvasArray = new Canvas[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // Get the location of the tiles in the screen coordinate system
                    int sx = -1, sy = -1;
                    GetTileLocation(i, j, out sx, out sy);
                    if (sx < 0 || sy < 0)
                    {
                        return;
                    }
                    Tile tile = levelMap.GetTile(i, j);
                    if (tile != Tile.None)
                    {
                        // Draw this tile
                        DrawOneTile(sx, sy, tile);
                    }
                }
            }
        }

        /// <summary>
        /// Save the level map into the json file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SavelLevel(object sender, RoutedEventArgs e)
        {
            if (levelMap.IsEmpty())
            {
                MessageBox.Show("The level map is empty.");
                return;
            }
            // Prompt the user to choose output path
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "level map|*.json";
            sfd.Title = "Choose output path";
            if ((bool)sfd.ShowDialog())
            {
                string path = sfd.FileName;
                // Call SaveToJsonFile to serialize the level map
                if (!levelMap.SaveToJsonFile(path))
                {
                    MessageBox.Show("Failed to save the level map.");
                }
            }
        }

        /// <summary>
        /// Exit the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitProgram(object sender, RoutedEventArgs e)
        {
            AskForSaveLevelMap(sender, e);
            // exit the program
            Environment.Exit(0);
        }

        /// <summary>
        /// Ask the user whether he/she would like to save the level map
        /// </summary>
        private void AskForSaveLevelMap(object sender, RoutedEventArgs e)
        {
            if (!levelMap.IsEmpty())
            {
                // Ask the user whether he/she would like to save the level map
                MessageBoxResult res = MessageBox.Show("Would you like to save the level map?",
                    "", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    // Save this level map
                    SavelLevel(sender, e);
                }
            }
        }

        /// <summary>
        /// Generate a random maze
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RandomMaze(object sender, RoutedEventArgs e)
        {
            levelMap = LevelMap.RandomLevelMap(levelMap.GetSize());
            ShowTilesByLevelMap();
        }

        /// <summary>
        /// Choose the Player title
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoosePlayer(object sender, RoutedEventArgs e)
        {
            curTile = Tile.Player;
        }
        /// <summary>
        /// Choose the Enemy title
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseEnemy(object sender, RoutedEventArgs e)
        {
            curTile = Tile.Enemy;
        }
        /// <summary>
        /// Choose the Wall title
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseWall(object sender, RoutedEventArgs e)
        {
            curTile = Tile.Wall;
        }
        /// <summary>
        /// Choose the Erase title
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseErase(object sender, RoutedEventArgs e)
        {
            curTile = Tile.None;
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (curTile == Tile.Player && levelMap.HasPlayer())
            {
                // The level map should only have one player
                MessageBox.Show("The level map has already one player.");
                return;
            }
            // Convert the screen position to the tile location
            Point pt = e.GetPosition(mapCanvas);
            int tileRow = -1, tileCol = -1;
            GetClickedTile((int)pt.X, (int)pt.Y, out tileRow, out tileCol);

            if(tileRow >= 0 && tileCol >= 0)
            {
                // Get the location of the tiles in the screen coordinate system
                int sx = -1, sy = -1;
                GetTileLocation(tileRow, tileCol, out sx, out sy);
                if(sx < 0 || sy < 0)
                {
                    return;
                }
                if(curTile != Tile.None)
                {
                    // Draw this tile
                    DrawOneTile(sx, sy, curTile);
                }
                else
                {
                    // Remove this tile
                    ClearOneTile(sx, sy);
                }
            }
        }
    }
}
