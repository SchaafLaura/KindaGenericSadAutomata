namespace GenericCellular.Scenes
{
    internal class RootScreen : ScreenObject
    {
        private ScreenSurface _mainSurface;
        CellularAutomaton<bool> _automaton;
        CAVisualizer<bool> _visualizer;

        public RootScreen()
        {
            _mainSurface = new ScreenSurface(200, 50);
            _mainSurface.FillWithRandomGarbage(_mainSurface.Font);
            _mainSurface.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, Mirror.None);
            _mainSurface.Print(4, 4, "Hello from SadConsole");
            Children.Add(_mainSurface);

            _automaton = new CellularAutomaton<bool>(
                new List<(int, int)> 
                { 
                    (0,   0),
                    (-1, -1), (-1, 0), (-1, 1),
                    ( 0, -1),          ( 0, 1),
                    ( 1, -1), ( 1, 0), ( 1, 1)
                },
                (list) =>
                {
                    bool self = list[0];
                    int n = 0;
                    for(int i = 1; i < list.Length; i++)
                        n += list[i] ? 1 : 0;

                    if (self)
                        return n == 2 || n == 3;
                    else
                        return n == 3;
                },
                _mainSurface.Width,
                _mainSurface.Height
            );
             
            var rng = new Random();
            _automaton.Init((_, _) => rng.Next(100) > 50);

            _visualizer = new CAVisualizer<bool>(
                (b) => b ? 
                new ColoredGlyph(Color.Orange, Color.DarkBlue, '#') : 
                new ColoredGlyph(Color.Transparent, Color.DarkBlue, ' '));
        }

        public override void Update(TimeSpan delta)
        {
            for(int i = 0; i < _mainSurface.Width; i++)
                for(int j = 0; j < _mainSurface.Height; j++)
                    _mainSurface.SetCellAppearance(i, j, _visualizer.Get(_automaton.Get(i, j)));

            _automaton.Update();
            base.Update(delta);
        }
    }
}
