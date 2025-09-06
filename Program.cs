using System;

class Program
{
    static void Main()
    {
        
        int[,] grid = new int[9, 9];

        // Inizializza la griglia a 0
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                grid[r, c] = 0;
            }
        }

        // Stampa la griglia
        int col=0;
        int rig=0;
        for (int r = 0; r < 9; r++)
        {


            if (rig == 3)
            {
                Console.WriteLine("---------------------");
                rig = 0;
            }
            for (int c = 0; c < 9; c++)
            {
                Console.Write(grid[r, c] + " ");
                col++;
                if (col % 3 == 0 && col != 0 && col != 9)
                {
                    Console.Write("| ");
                    
                }
                else if (col == 9)
                {
                    col = 0;
                }
            }
            Console.WriteLine();
            rig++;
        }
    }
}

