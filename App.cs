using System.Collections.Specialized;
using Timer = System.Windows.Forms.Timer;
namespace CellularAutomata
{
  public partial class App : Form
  {
    private Timer Timer;
    private bool[,] grid;
    private VScrollBar scrollBar;
    private const int gridSize = 100;
    private const int cellSize = 20;
   
    public App()
    {
      this.DoubleBuffered = true;
      this.BackColor = Color.Red;
      InitializeComponent();
      InitializeGrid();
      this.Paint += Draw;
      InitializeTimer();
      InitializeControls();
    }

    private void InitializeTimer()
    {
      this.Timer = new Timer { Interval = 500 };
      this.Timer.Tick += (s, e) => UpdateGrid();
    }

    private void InitializeGrid()
    {
      this.grid = new bool[100, 100];
      Random random = new Random();
      for (int i = 0; i < gridSize; i++)
      {
        for (int j = 0; j < gridSize; j++)
        {
          this.grid[i, j] = random.Next(2) == 1;
        }
      }
    }
    private void InitializeControls()
    {
      Button startButton = new Button
      {
        Text = "Start",
        Location = new Point(10, 10),
        BackColor = Color.DarkGray,
        ForeColor = Color.Black,
        FlatStyle = FlatStyle.Flat,
      };
      startButton.Click += StartButtonClick;
      this.Controls.Add(startButton);

      Button resetButton = new Button
      {
        Text = "Reset",
        Location = new Point(100, 10),
        BackColor = Color.DarkGray,
        ForeColor = Color.Black,
        FlatStyle = FlatStyle.Flat,
      };
      resetButton.Click += ResetButtonClick;
      this.Controls.Add(resetButton);

      this.scrollBar = new VScrollBar
      {
        AccessibleName = "speed",
        Minimum = 100,
        Maximum = 1000,
        Value = this.Timer.Interval,
        LargeChange = 100,
        SmallChange = 50,
        Location = new Point(10, 50),
        Width = 25,
        Height = 500,
        BackColor = Color.Black,
        ForeColor = Color.DarkGray,
      };
      this.scrollBar.Scroll += ScrollBarScroll;
      this.Controls.Add(this.scrollBar);
    }
    private void UpdateGrid()
    {
      bool[,] newGrid = new bool[gridSize, gridSize];
      for (int x = 0; x < gridSize; x++)
      {
        for (int y = 0; y < gridSize; y++)
        {
          int neighbours = countNeighbours(x, y);
          if (grid[x, y])
          {
            newGrid[x, y] = neighbours == 2 || neighbours == 3;
          }
          else
          {
            newGrid[x, y] = neighbours == 3;
          }
        }
      }
      grid = newGrid;
      this.Invalidate();
    }
    private int countNeighbours(int x, int y)
    {
      int count = 0;
      for (int dx = -1; dx <= 1; dx++)
      {
        for (int dy = -1; dy <= 1; dy++)
        {
          if (dx == 0 && dy == 0) continue;
          int newX = x + dx;
          int newY = y + dy;
          if (newX >= 0 && newY >= 0 && newX < gridSize && newY < gridSize)
          {
            if (grid[newX, newY]) count++;
          }
        }
      }
      return count;
    }

    private void Draw(object sender, PaintEventArgs e)
    {
      Graphics g = e.Graphics;
      for (int x = 0; x < gridSize; x++)
      {
        for (int y = 0; y < gridSize; y++)
        {
          Rectangle cell = new Rectangle(x * cellSize, y * cellSize, cellSize, cellSize);
          g.FillRectangle(grid[x, y] ? Brushes.Black : Brushes.White, cell);
          g.DrawRectangle(Pens.Gray, cell);

        }
      }
    }

    private void StartButtonClick(object sender, EventArgs e)
    {
      this.Timer.Start();
    }
    private void ResetButtonClick(object sender, EventArgs e)
    {
      this.Timer.Stop();
      InitializeGrid();
      this.Invalidate();
      this.scrollBar.Value = this.Timer.Interval;
      this.Timer.Start();
    }

    private void ScrollBarScroll(object sender, EventArgs e)
    {
      this.Timer.Interval = Math.Max(100, this.scrollBar.Value);
    }
  }
}
