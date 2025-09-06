using System;
using System.Collections.Generic;

class Program
{
    static Random rnd = new Random();

    static void Main()
    {
        // Griglia 9x9 da mostrare a schermo (riga, colonna)
        int[,] grid = new int[9, 9];
        /* Possibilità per ogni cella (riga, colonna, numero)
        
        true = possibile, false = non possibile

        per righe e colonne il valore del bool serve per indicare se un numero è possibile o no
        L'ho pensato come una quadrato 9X9 per le coordinate che poi diventa un cubo la terza dfimensione del cubo indica tutti i valori che puo assumere quel blocco
            Es: 
            0 = true            --> quella casella potrebbe essere 1
            1 = false           --> non può essere 2
            2 = true            --> potrebbe essere 3
            3 = false           --> non può essere 4
            ecc.

            Dopo aver inserito un numero in una cella, imposto il valore a false del numero inserito di tutte le celle della stessa riga, colonna e box 3x3.
        */
        bool[,,] disp = new bool[9, 9, 9];

        // Inizializza griglia e possibilità
        for (int r = 0; r < 9; r++)

            for (int c = 0; c < 9; c++)

                for (int n = 0; n < 9; n++)

                    disp[r, c, n] = true;

        // Genera Sudoku completo
        if (Sudoku(grid, disp, 0, 0) == true)
            StampaGriglia(grid);

        else
            Console.WriteLine("Errore nella generazione.");
    }

    static bool Sudoku(int[,] grid, bool[,,] disp, int r, int c)
    {


        // Se è alla fine della riga, va a capo, se è alla fine della griglia, ha finito
        if (c == 9)
        {
            c = 0;
            r++;
            if (r == 9)
                return true; 
        }

        // Trova numeri possibili per quella casella
        int[] possibili = TrovaNumeriPossibili(disp, r, c);
        if (possibili.Length == 0)
            return false; // nessun numero possibile --> backtrack    
        // Mischiamo i numeri per avere griglie diverse
        Mischia(possibili);

        foreach (int numero in possibili)
        {
            // Clono le possibilità per poterle ripristinare in caso di backtrack
            bool[,,] tempPoss = (bool[,,])disp.Clone();

            togliDisp(grid, disp, r, c, numero);

            if (Sudoku(grid, disp, r, c + 1)== true)
                return true;

            // Se fallisce, ripristiniamo le la vecchia griglia con le vecchie possibilità
            disp = tempPoss;
            grid[r, c] = 0;
        }

        return false; // nessun numero possibile --> backtrack
    }

    static int[] TrovaNumeriPossibili(bool[,,] disp, int r, int c)
    {
        // Uso una lista al posto degli array perché non so quanti numeri saranno disponibili
        // (Spera di averla usata nel modo giusto) ho chiesto a chatGPT come dichiarla
        List<int> list = new List<int>();
        for (int n = 0; n < 9; n++)

            if (disp[r, c, n])

                list.Add(n + 1);

        return list.ToArray();
    }

    static void StampaGriglia(int[,] grid)
    {
        
        for (int r = 0; r < 9; r++)
        {
            if (r % 3 == 0 && r != 0)
                Console.WriteLine("---------------------");
            for (int c = 0; c < 9; c++)
            {
                Console.Write(grid[r, c] + " ");
                if ((c + 1) % 3 == 0 && c != 9 - 1)
                    Console.Write("| ");
            }
            Console.WriteLine();
        }
    }

    static void togliDisp(int[,] grid, bool[,,] disp, int r, int c, int n)
    {
        //Per la casella trovata imposto il numero e tolgo tutte le possibilità per gli altri numeri
        grid[r, c] = n;
        for (int i = 0; i < 9; i++)
            disp[r, c, i] = false;

        //imposto false per tutte le celle colonna
        for (int col = 0; col < 9; col++)
            disp[r, col, n - 1] = false;
        //imposto false per tutte della stessa riga
        for (int row = 0; row < 9; row++)
            disp[row, c, n - 1] = false;
        //imposto false per tutte della stessa box 3x3
        //trovo l'inizio del box 3x3
        int inizioRig = ((int)(r / 3)) * 3;
        int inizioCol = ((int)(c / 3)) * 3;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                disp[inizioRig + i, inizioCol + j, n - 1] = false;
    }

    /* Per mescolare ho usato qualcosa simile allo scambio di un selection sort: 
        - scelgo a casa quante volte scambiare due numeri
        - scelgo due posizioni casuali nell'array e scambio i valori
        (Spero sia efficace, perché non mi vengono metodi migliori)
        */
    static void Mischia(int[] array)
    {

        int scambi = rnd.Next(10, 20);
        for (int i = 0; i < scambi; i++)
        {
            int x = rnd.Next(array.Length);
            int y = rnd.Next(array.Length);
            int tmp = array[x];
            array[x] = array[y];
            array[y] = tmp;
        }
        return;
    }
}