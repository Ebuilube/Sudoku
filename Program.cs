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

        Console.WriteLine("Selezione il livello di difficoltà (1-5):  ");
        string input = Console.ReadLine();
        if (!int.TryParse(input, out int livello) || livello < 1 || livello > 5)
        {
            Console.WriteLine("Input non valido. Impostato livello di difficoltà a 1.");
            livello = 1;
        }
        Console.WriteLine("");

        // Genera Sudoku completo
        if (Sudoku(grid, disp, 0, 0) == true)
        {
            // Rimuovi numeri in base al livello di difficoltà
            int rimuovi = 25 + livello * 5;
            for (int i = 0; i < rimuovi; i++)
            {
                int r = rnd.Next(9);
                int c = rnd.Next(9);
                if (grid[r, c] != 0)
                {
                    int temp = grid[r, c];
                    grid[r, c] = 0; // Rimuovi il numero

                    // Controlla se la griglia ha ancora una soluzione unica
                    if (ContaSoluzioni(grid) != 1)
                    {
                        grid[r, c] = temp; // Ripristina il numero se non è unica
                    }

                }
                else
                {
                    i--; // Se la cella è già vuota, riprova
                }

            }

            StampaGriglia(grid);
            Console.WriteLine("Vuoi esportare la griglia in un file di testo? (s/n)");
            string risposta = Console.ReadLine();
            if (risposta.ToLower() == "s")
            {
                Esporta(grid);
                Console.WriteLine("Griglia esportata in sudoku.txt");
            }   
            
        }

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

            if (Sudoku(grid, disp, r, c + 1) == true)
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
                if(grid[r,c] == 0)
                    Console.Write("  ");
                else
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
    static bool Valido(int[,] grid, int r, int c, int n)
    {
        // Controlla se n è già presente nella riga, colonna o box 3x3
        // Riga
        for (int i = 0; i < 9; i++)
            if (grid[r, i] == n) return false;

        // Colonna
        for (int i = 0; i < 9; i++)
            if (grid[i, c] == n) return false;

        // Box 3x3
        int inizioRig = ((int)(r / 3)) * 3;
        int inizioCol = ((int)(c / 3)) * 3;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (grid[inizioRig + i, inizioCol + j] == n) return false;

        return true;
    }

    static int ContaSoluzioni(int[,] grid, int r = 0, int c = 0, int maxSol = 2)
{
    
    if (c == 9)
    {
        c = 0;
        r++;
        if (r == 9)
            return 1;
    }

    // Se la cella è già piena, vai avanti
    if (grid[r, c] != 0)
        return ContaSoluzioni(grid, r, c + 1, maxSol);

    int count = 0;

    // Prova i numeri da 1 a 9
    for (int n = 1; n <= 9; n++)
    {
        if (Valido(grid, r, c, n))
        {
            grid[r, c] = n;
            count = count + ContaSoluzioni(grid, r, c + 1, maxSol);

            if (count >= maxSol) // non serve contare oltre
            {
                grid[r, c] = 0;  
                return count;
            }
            grid[r, c] = 0;
        }
    }

    return count;
}
    static void Esporta(int[,] grid)
    {
        string path = "sudoku.txt";
        string contenuto = "";
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                contenuto += grid[r, c];
            }
            
        }
        File.WriteAllText(path, contenuto);
    }

}
