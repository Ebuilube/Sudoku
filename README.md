# Generatore di Sudoku con Algoritmo X

Questo programma genera una griglia Sudoku completa utilizzando **backtracking** basato sull’**Algoritmo X** di Donald Knuth.  
Non ho ancora implementato i **Dancing Links (DLX)**, ma il principio di ricerca è lo stesso.
Ho cercato di capire al meglio l'algoritmo, ma non ho ancora capito bene come implementarlo :(
Ho caricato il fle da cui ho studiato i dancing links + algoritmo X si chiama: `Dancin links.pdf`

---

## Come funziona
- La griglia è rappresentata da un array `int[9,9]`.  
- Le possibilità sono gestite con un array `bool[9,9,9]`:
  - `disp[r, c, n] = true` significa che nella cella `(r,c)` si può ancora inserire il numero `n+1`.
- La procedura ricorsiva:
  1. Trova i numeri possibili per una cella.
  2. Li mescola con `Random` per avere griglie diverse.
  3. Prova a inserirne uno aggiornando i vincoli di riga, colonna e box 3×3.
  4. Se non porta a una soluzione → backtracking (annulla e prova un altro numero).

Per gestire il ritorno allo stato precedente, viene usata una copia (`Clone()`) della matrice `disp`.

---

## Differenza con Dancing Links
- In questo progetto uso **array + clonazione** dello stato → soluzione semplice ma meno efficiente.  
- Nei Dancing Links invece si lavora con **liste doppiamente collegate**, dove “rimuovere” e “ripristinare” vincoli è molto più veloce.  
