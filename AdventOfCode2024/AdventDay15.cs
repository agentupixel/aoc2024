using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class AdventDay15 : IAdventDay
{
    public async Task<object> ExecuteTask1()
    {
        string[] input = await File.ReadAllLinesAsync("C:\\repos\\days\\day15.txt");

        bool processMovements = false;
        List<char> movements = [];
        List<List<char>> matrix = [];

        foreach(var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                processMovements = true;
                continue;
            }

            if(processMovements)
            {
                movements.AddRange(line);
            }
            else
            {
                matrix.Add([.. line]);
            }
        }

        Point position = GetInitialPosition(matrix);

        foreach(var movement in movements)
        {
            switch(movement)
            {
                case '<':
                    Point? nearestFreeSpot = GetNearestFreeSpot(matrix, -1, 0, position);
                    if(nearestFreeSpot.HasValue)
                    {
                        Move(position, nearestFreeSpot.Value, matrix, -1, 0);
                        matrix[position.Y][position.X] = '.';
                        position = new(position.X - 1, position.Y);
                    }
                    break;
                case '>':
                    nearestFreeSpot = GetNearestFreeSpot(matrix, 1, 0, position);
                    if (nearestFreeSpot.HasValue)
                    {
                        Move(position, nearestFreeSpot.Value, matrix, 1, 0);
                        matrix[position.Y][position.X] = '.';
                        position = new(position.X + 1, position.Y);
                    }
                    break;
                case '^':
                    nearestFreeSpot = GetNearestFreeSpot(matrix, 0, -1, position);
                    if (nearestFreeSpot.HasValue)
                    {
                        Move(position, nearestFreeSpot.Value, matrix, 0, -1);
                        matrix[position.Y][position.X] = '.';
                        position = new(position.X, position.Y - 1);
                    }
                    break;
                case 'v':
                    nearestFreeSpot = GetNearestFreeSpot(matrix, 0, 1, position);
                    if (nearestFreeSpot.HasValue)
                    {
                        Move(position, nearestFreeSpot.Value, matrix, 0, 1);
                        matrix[position.Y][position.X] = '.';
                        position = new(position.X, position.Y + 1);
                    }
                    break;
            }
        }

        long result = 0;

        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[i].Count; j++)
            {
                if (matrix[i][j] == 'O')
                {
                    result += i * 100 + j;
                }
            }
        }

        return result;
    }

    private static void Move(
        Point position,
        Point nearestFreeSpot,
        List<List<char>> matrix,
        int xMovement,
        int yMovement)
    {
        if (xMovement == 0)
        {
            for (int i = nearestFreeSpot.Y; i != position.Y; i += yMovement * (-1))
            {
                matrix[i][position.X] = matrix[i + yMovement * (-1)][position.X];
            }
        }
        if (yMovement == 0)
        {
            for (int j = nearestFreeSpot.X; j != position.X; j += xMovement * (-1))
            {
                matrix[position.Y][j] = matrix[position.Y][j + xMovement * (-1)];
            }
        }
    }

    private static Point? GetNearestFreeSpot(
        List<List<char>> matrix,
        int xMovement,
        int yMovement,
        Point position)
    {
        if(xMovement == 0)
        {
            for (int i =position.Y;i>=0 && i <matrix.Count; i+= yMovement)
            {
                if (matrix[i][position.X] == '#')
                    return null;
                if (matrix[i][position.X] == '.')
                    return new Point(position.X, i);
            }
        }
        if (yMovement == 0)
        {
            for (int i = position.X; i >= 0 && i < matrix[0].Count; i += xMovement)
            {
                if (matrix[position.Y][i] == '#')
                    return null;
                if (matrix[position.Y][i] == '.')
                    return new Point(i, position.Y);
            }
        }
        return null;
    }

    private static Point GetInitialPosition(List<List<char>> matrix)
    {
        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[i].Count; j++)
            {
                if (matrix[i][j] == '@')
                {
                    return new Point(j, i);
                }
            }
        }

        throw new KeyNotFoundException();
    }

    public async Task<object> ExecuteTask2()
    {
        throw new NotImplementedException();
    }
}
