﻿using System.IO;
using OCR.Engine.Contracts;
using OCR.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using OCR.Engine.Constants;

namespace OCR.Engine.Code
{
    public class NeuronFactory : INeuronFactory
    {
        public Neuron Create(char symbol, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var weightsList = new List<int[]>();

            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var lineWeights = reader.ReadLine().Split(' ');
                    weightsList.Add(new int[lineWeights.Length]);
                    for (int i = 0; i < lineWeights.Length; i++)
                    {
                        weightsList[weightsList.Count - 1][i] = Convert.ToInt32(lineWeights[i]);
                    }
                }
            }

            if (!weightsList.Any())
            {
                return new Neuron(symbol, RecognitionSettings.DefaultWeightWidth, RecognitionSettings.DefaultWeightHeight);
            }

            var neuron = new Neuron(symbol, weightsList.Count, weightsList[0].Length);

            for (int i = 0; i < weightsList.Count; i++)
            {
                for (int j = 0; j < weightsList[i].Length; j++)
                {
                    neuron.Weight[i, j] = weightsList[i][j];
                }
            }

            return neuron;
        }
    }
}