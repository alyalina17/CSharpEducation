using System;

class Program
{
    // Глобальная переменная для игрового поля
    private static char[,] board = new char[3, 3];

    // Инициализация игровой доски
    static void InitializeBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board[i, j] = ' ';
            }
        }
    }

    // Основной метод программы
    static void Main()
    {
        InitializeBoard(); // Инициализируем доску перед началом игры
        bool gameOver = false;
        bool currentPlayerIsX = true;

        while (!gameOver)
        {
            char currentPlayerSymbol = currentPlayerIsX ? 'X' : 'O'; // Определяем символ текущего игрока
            Console.WriteLine($"Ходит {currentPlayerSymbol}, введите координату (например A1): ");
            string input = Console.ReadLine().ToUpper();

            if (TryMakeMove(input))
            {
                int row = input[1] - '1';
                int col = input[0] - 'A';
                board[row, col] = currentPlayerSymbol; // Ставим знак игрока на поле

                PrintBoard(); // Выводим обновленную доску

                if (CheckWinner(currentPlayerSymbol)) // Проверяем победу
                {
                    Console.WriteLine($"{currentPlayerSymbol} победил!");
                    gameOver = true;
                }
                else if (IsDraw()) // Проверяем ничью
                {
                    Console.WriteLine("Ничья!");
                    gameOver = true;
                }
                else
                {
                    currentPlayerIsX = !currentPlayerIsX; // Меняем игрока
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод! Попробуйте снова.");
            }
        }
    }

    // Метод отрисовки доски
    static void PrintBoard()
    {
        Console.Clear(); // Очищаем экран перед каждым ходом
        Console.WriteLine("     A   B   C"); // Шапка буквенных координат колонок
        Console.WriteLine("   -----------");

        for (int i = 0; i < 3; i++)
        {
            Console.Write(i + 1); // Номер ряда
            Console.Write(" | ");

            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] != ' ')
                    Console.ForegroundColor = board[i, j] == 'X' ? ConsoleColor.Red : ConsoleColor.Blue;

                Console.Write(board[i, j]);

                Console.ResetColor();

                if (j < 2) // Разделительные символы между колонками
                    Console.Write(" | ");
            }

            Console.WriteLine(); // Перенос строки
            if (i < 2) // Линии между рядами
                Console.WriteLine("   -----------");
        }
    }

    // Метод проверки правильного хода
    static bool TryMakeMove(string move)
    {
        if (move.Length != 2 || move[0] < 'A' || move[0] > 'C' || move[1] < '1' || move[1] > '3')
            return false;

        int row = move[1] - '1';
        int col = move[0] - 'A';

        if (board[row, col] == ' ')
            return true;
        else
            return false;
    }

    // Метод проверки победителя
    static bool CheckWinner(char player)
    {
        for (int i = 0; i < 3; i++)
        {
            if ((board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) ||
               (board[0, i] == player && board[1, i] == player && board[2, i] == player))
                return true;
        }
        if ((board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) ||
           (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player))
            return true;

        return false;
    }

    // Метод проверки ничьи
    static bool IsDraw()
    {
        foreach (var cell in board)
        {
            if (cell == ' ')
                return false;
        }
        return true;
    }
}