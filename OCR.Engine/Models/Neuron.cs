using System;

namespace OCR.Engine.Models
{
    public class Neuron
    {
        public Neuron(string name, int n, int m) : this(name, n, m, new int[n,m]) { }
        public Neuron(string name, int n, int m, int[,] weight)
        {
            N = n;
            M = m;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Weight = weight ?? throw new ArgumentNullException(nameof(weight));
        }

        public int N { get; }

        public int M { get; }

        public string Name { get; }

        public int[,] Weight { get; }

        public int GetSum(int[,] input)
        {
            var sum = 0;
            var scaledSignals = GetScaledSignals(input);

            for (int x = 0; x <= N; x++)
            {
                for (int y = 0; y <= M; y++)
                {
                    sum += scaledSignals[x, y];
                }
            }

            return sum;
        }

        private int[,] GetScaledSignals(int[,] input)
        {
            var result = new int[N, M];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    result[i, j] = input[i, j] * Weight[i, j];
                }
            }

            return result;
        }
    }
}