namespace GenericCellular
{
    internal class CellularAutomaton<T>
    {
        IEnumerable<(int, int)> neighborhood;
        int N, M;
        Func<T[], T> transitionFunction;
        T[,,] spaces;
        int spaceIndex = 0;

        public CellularAutomaton(IEnumerable<(int, int)> neighborhood, Func<T[], T> transitionFunction, int N, int M){
            this.neighborhood = neighborhood;
            this.transitionFunction = transitionFunction;
            this.N = N;
            this.M = M;
            spaces = new T[N, M, GameSettings.SIMULATION_LENGTH];
        }

        public T[] GetValues(int i, int j)
        {
            T[] ret = new T[GameSettings.SIMULATION_LENGTH];
            for(int k = 0; k < GameSettings.SIMULATION_LENGTH; k++)
                ret[k] = spaces[i, j, (spaceIndex + k) % GameSettings.SIMULATION_LENGTH];
            return ret;
        }

        public void UpdateSpaces()
        {
            T[] n = new T[neighborhood.Count()];
            int k;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    k = 0;
                    foreach (var xy in neighborhood)
                        n[k++] = spaces[(i + xy.Item1 + N) % N, (j + xy.Item2 + M) % M, spaceIndex];
                    spaces[i, j, (spaceIndex + 1) % GameSettings.SIMULATION_LENGTH] = transitionFunction.Invoke(n);
                }
            }
            spaceIndex = (spaceIndex + 1) % GameSettings.SIMULATION_LENGTH;
        }

        public void Init(Func<int, int, T> initFn)
        {
            for(int i = 0; i < N; i++)
                for(int j = 0; j < M; j++)
                    spaces[i, j, 0] = initFn(i, j);
        }
    }
}
