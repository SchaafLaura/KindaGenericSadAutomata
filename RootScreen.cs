using System.Runtime.CompilerServices;

namespace GenericCellular.Scenes
{
    internal class RootScreen : ScreenObject
    {
        private ScreenSurface _mainSurface;
        CellularAutomaton<bool> _automaton;
        CAVisualizer<bool> _visualizer;

        public RootScreen()
        {
            _mainSurface = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);
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
                (vals) =>
                {
                    Color original = Color.Orange;

                    for(int i = 0; i < GameSettings.SIMULATION_LENGTH; i++)
                    {
                        if (vals[i])
                            return new ColoredGlyph(GetHSLShifted(original, -20 * i, -0.1f * i, 0), Color.Transparent, '#');
                    }
                    return new ColoredGlyph(Color.Transparent, Color.Transparent, ' ');
                });
        }

        public override void Update(TimeSpan delta)
        {
            for(int i = 0; i < _mainSurface.Width; i++)
                for(int j = 0; j < _mainSurface.Height; j++)
                    _mainSurface.SetCellAppearance(
                        i, j, 
                        _visualizer.Get(
                            _automaton.GetValues(i, j)));

            _automaton.UpdateSpaces();
            base.Update(delta);
        }

        public static Color GetHSLShifted(Color col, float hueShift, float satShift, float lightShift)
        {
            var hue = col.GetHSLHue();
            var sat = col.GetHSLSaturation();
            var lig = col.GetHSLLightness();

            var newHue = hue + hueShift;
            var newSat = sat + satShift;
            var newLig = lig + lightShift;

            while (newHue < 0)
                newHue += 360;

            while (newSat < 0)
                newSat += 1.0f;

            while (newLig < 0)
                newLig += 1.0f;

            while (newHue > 360)
                newHue -= 360;

            while(newSat > 1.0f)
                newSat -= 1.0f;

            while(newLig > 1.0f)
                newLig -= 1.0f;

            return Color.FromHSL(newHue, newSat, newLig);
        }

    }
}
