using System;

namespace OCR.Engine.Models
{
    public class Neuron
    {
        public Neuron(char symbol, int n, int m) : this(symbol, n, m, new int[n,m]) { }
        public Neuron(char symbol, int n, int m, int[,] weight)
        {
            N = n;
            M = m;
            Symbol = symbol;
            Weight = weight ?? throw new ArgumentNullException(nameof(weight));
        }

        public int N { get; }

        public int M { get; }

        public char Symbol { get; }

        public int[,] Weight { get; }

        public int GetSum(int[,] input)
        {
            var sum = 0;
            var scaledSignals = GetScaledSignals(input);

            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < M; y++)
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

        public override string ToString() => $"{Symbol}";
    }
}