using System;
using System.Collections.Generic;

namespace MatrixCalculator
{
    public static class MatrixOperations
    {
        // Метод для сложения матриц
        public static List<int> AddMatrices(int length, int width, List<int> matrixData)
        {
            if (matrixData.Count != length * width * 2)
            {
                throw new ArgumentException("Неверные размеры матриц для сложения.");
            }

            var matrix1 = new int[length, width];
            var matrix2 = new int[length, width];
            int index = 0;

            // Разделяем данные на две матрицы
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matrix1[i, j] = matrixData[index++];
                }
            }

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matrix2[i, j] = matrixData[index++];
                }
            }

            // Сложение матриц
            var result = new List<int>();
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result.Add(matrix1[i, j] + matrix2[i, j]);
                }
            }

            return result;
        }

        // Метод для умножения матриц
        public static List<int> MultiplyMatrices(int length, int width, List<int> matrixData)
        {
            if (matrixData.Count != length * width * 2)
            {
                throw new ArgumentException("Неверные размеры матриц для умножения.");
            }

            var matrix1 = new int[length, width];
            var matrix2 = new int[width, length];  // Для умножения матриц
            int index = 0;

            // Разделяем данные на две матрицы
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matrix1[i, j] = matrixData[index++];
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    matrix2[i, j] = matrixData[index++];
                }
            }

            // Умножение матриц
            var result = new List<int>();
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < width; k++)
                    {
                        sum += matrix1[i, k] * matrix2[k, j];
                    }
                    result.Add(sum);
                }
            }

            return result;
        }
    }
}
