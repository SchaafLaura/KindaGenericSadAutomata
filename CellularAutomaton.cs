using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCellular
{
    internal class CellularAutomaton<T>
    {
        IEnumerable<(int, int)> neighborhood;
        T[,] space;
        T[,] buffer;
        int N, M;
        Func<T[], T> transitionFunction;

        bool pingPong = false;

        public CellularAutomaton(IEnumerable<(int, int)> neighborhood, Func<T[], T> transitionFunction, int N, int M){
            this.neighborhood = neighborhood;
            this.transitionFunction = transitionFunction;
            this.N = N;
            this.M = M;
            space =  new T[N, M];
            buffer = new T[N, M];
        }

        public T Get(int i, int j)
        {
            if (!pingPong)
                return space[i, j];
            else
                return buffer[i, j];
        }

        public void Init(Func<int, int, T> initFn)
        {
            for(int i = 0; i < N; i++)
                for(int j = 0; j < M; j++)
                    space[i, j] = initFn(i, j);
        }

        private void UpdateSpace(T[,] arr, T[,] buff)
        {
            T[] n = new T[neighborhood.Count()];
            int k;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    k = 0;
                    foreach (var xy in neighborhood)
                        n[k++] = arr[(i + xy.Item1 + N) % N, (j + xy.Item2 + M) % M];

                    buff[i, j] = transitionFunction.Invoke(n);
                }
            }
        }

        public void Update()
        {
            if (!pingPong)
                UpdateSpace(space, buffer);
            else
                UpdateSpace(buffer, space);
            pingPong = !pingPong;
        }
    }
}
