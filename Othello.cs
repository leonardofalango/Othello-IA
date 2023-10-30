using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

public struct Othello
{
    private bool ended = false;
    private const ulong u = 1;
    public bool WhitePlays => whitePlays == 1;
    public byte WhitePoints => whiteCount;
    public byte BlackPoints => blackCount;

    public void Pass()
        => whitePlays = (byte)(1 - whitePlays); 

    public byte whiteCount;
    public byte blackCount;
    public ulong whiteInfo;
    public ulong blackInfo;
    public byte whitePlays;
    public ulong myBoard
    {
        get {
            if (WhitePlays) 
                return whiteInfo;
            return blackInfo;
        }
        set {
            if (WhitePlays) 
                whiteInfo = value;
            else blackInfo = value;
        }
    }
    public ulong enemyBoard
    {
        get {
            if (WhitePlays) 
                return blackInfo;
            return whiteInfo;
        }
        set {
            if (WhitePlays) 
                blackInfo = value;
            else whiteInfo = value;
        }
    }
    
    public byte myCount
    {
        get {
            if (WhitePlays) 
                return whiteCount;
            return blackCount;
        }
        set {
            if (WhitePlays) 
                whiteCount = value;
            else blackCount = value;
        }
    }
    public byte enemyCount
    {
        get {
            if (WhitePlays) 
                return blackCount;
            return whiteCount;
        }
        set {
            if (WhitePlays) 
                blackCount = value;
            else whiteCount = value;
        }
    }
    
    private ulong lastPlay;

    public Othello(bool isWhite)
    {
        this.whitePlays = isWhite ? (byte)1 : (byte)0;
    }

    public ulong GetLast() => lastPlay;

    /// <summary>
    /// Joga em uma posição em um tabuleiro
    /// </summary>
    public void Play(int i)
    {
        myBoard += u << i;
        // Play
        turnSides(i, 1);
        turnSides(i, -1);
        turnSides(i, 7);
        turnSides(i, -7);
        turnSides(i, 9);
        turnSides(i, -9);
        turnSides(i, 8);
        turnSides(i, -8);


        whitePlays = (byte)(1 - whitePlays);
        lastPlay = myBoard;
    }
    
    /// <summary>
    /// Testa e retorna verdadeiro se você pode jogar
    /// </summary>
    public bool CanPlay(int i)
    {
        int rowInGame = i % 8;
        int colInGame = i / 8;
        bool flag = false;


        // Verifying if surrounded by enemy pieces
        for (int shiftI = colInGame - 1; shiftI <= colInGame + 1; shiftI++)
            for (int shiftJ = rowInGame - 1; shiftJ <= rowInGame + 1; shiftJ++)
            {
                if (shiftI < 0 || shiftJ < 0)
                    continue;

                int col = shiftI;
                int row = shiftJ * 8;
                ulong posArr = u << (row + col);

                if ((enemyBoard & posArr) > 0)
                {
                    flag = true;
                    break;
                }
            }
        
        if(!flag)
            return false;
        
        // System.Console.WriteLine($"Line in Game: ({rowInGame + 1}, {colInGame + 1})");
        bool enemyPieces = verifyHorizontalVerticalDiagonal(rowInGame, colInGame);

        if (enemyPieces)
            return true;
        
        return false;
    }
    
    /// <summary>
    /// Retorna verdadeiro se o jogo acabou
    /// </summary>
    public bool GameEnded() => ended;

    /// <summary>
    /// Cria uma cópia indentica do estado.
    /// </summary>
    public Othello Clone()
    {
        // new Instance of Othello

        // ? Copy the arrays if exists
        Othello o = new Othello(WhitePlays)
        {
            blackCount = this.blackCount,
            whiteCount = this.whiteCount,
            blackInfo = this.blackInfo,
            whiteInfo = this.whiteInfo,
        };

        return o;
    }

    /// <summary>
    /// Obtém próximas jogadas válidas
    /// </summary>
    public IEnumerable<Othello> Next()
    {

        for (int i = 0; i < 63; i++)
        {
            if (CanPlay(i))
            {
                Othello newState = this.Clone();
                // ! change states
                newState.Play(i);

                yield return newState;
            }
        }

        ended = true;
    }

    //*    ↖  ⬆  ↗
    //*   ⬅ 🀄 ➡
    //*   ↙  ⬇  ↘
    // TODO: fix this function
    private bool verifyHorizontalVerticalDiagonal(int rowInGame, int colInGame)
    {
        // ⬅ 🀄
        for (int shiftI = colInGame - 1, quant = 0; shiftI >= 0; shiftI--)
        {
            int col = shiftI * 8;
            ulong posArr = u << (rowInGame + col);
            
            if ((enemyBoard & posArr) > 0)
                quant += 1;
            
            if (quant >= 1 && ((myBoard & posArr) > 0))
                return true;
            
            if ((myBoard & posArr) > 0)
                break;
        }
        
        //   🀄 ➡
        for (int shiftI = colInGame + 1, quant = 0; shiftI < 8; shiftI++)
        {
            int col = shiftI * 8;
            ulong posArr = u << (rowInGame + col);
            
            if ((enemyBoard & posArr) > 0)
                quant += 1;
            
            if (quant >= 1 && ((myBoard & posArr) > 0))
                return true;
            
            if ((myBoard & posArr) > 0)
                break;
        }
        
        //  🀄
        //  ⬇
        for (int shiftI = rowInGame + 1, quant = 0; shiftI < 8; shiftI++)
        {
            int row = shiftI;
            ulong posArr = u << (row + (colInGame * 8));
            
            if ((enemyBoard & posArr) > 0)
                quant += 1;
            
            if (quant >= 1 && ((myBoard & posArr) > 0))
                return true;
            
            if ((myBoard & posArr) > 0)
                break;
            
        }

        //    ⬆
        //   🀄
        for (int shiftI = colInGame - 1, quant = 0; shiftI >= 0; shiftI--)
        {
            int row = shiftI;
            ulong posArr = u << (row + (colInGame * 8));
            
            if ((enemyBoard & posArr) > 0)
                quant += 1;
            
            if (quant >= 1 && ((myBoard & posArr) > 0))
                return true;
            
            if ((myBoard & posArr) > 0)
                break;
        }

        //     ↗
        //  🀄
        for (int shiftI = 0, quant = 0; shiftI < 8; shiftI++)
        {
            int col = colInGame + shiftI;
            int row = rowInGame + shiftI * 8;

            if (row < 0 || col < 0)
                break;

            ulong posArr = u << (col + row);
            
            if ((enemyBoard & posArr) > 0)
                quant += 1;
            
            if (quant >= 1 && ((myBoard & posArr) > 0))
                return true;
        }

        //  🀄
        //↙
        for (int shiftI = 0, quant = 0; shiftI >= 0; shiftI--)
        {
            int col = colInGame + shiftI;
            int row = rowInGame + shiftI * 8;

            if (row < 0 || col < 0)
                break;

            ulong posArr = u << (col + row);
            
            if ((enemyBoard & posArr) > 0)
                quant += 1;
            
            if (quant >= 1 && ((myBoard & posArr) > 0))
                return true;
        }

        // ↖
        //  🀄
        for (int shiftI = 0, quant = 0; shiftI >= 0; shiftI--)
        {
            int col = colInGame + shiftI;
            int row = rowInGame + shiftI * 8;

            if (row < 0 || col < 0)
                break;
            ulong posArr = u << (col + row);
            
            if ((enemyBoard & posArr) > 0)
                quant += 1;
            
            if (quant >= 1 && ((myBoard & posArr) > 0))
                return true;
        }
    
        //  🀄
        //   ↘
        for (int shiftI = 0, quant = 0; shiftI < 8; shiftI++)
        {
            int col = colInGame + shiftI;
            int row = rowInGame + shiftI * 8;

            if (row > 7 || col > 7)
                break;

            ulong posArr = u << (col + row);
            
            if ((enemyBoard & posArr) > 0)
                quant += 1;
            
            if (quant >= 1 && ((myBoard & posArr) > 0))
                return true;
        }

        return false;
    }

    public void turnSides(int played, int dir)
    {
        var next = played + dir;
        
        ulong play = u << next;

        if ((play & myBoard) > 0)
            return;
        
        if ((play & myBoard) == 0 && (play & enemyBoard) == 0)
            return;
        
        myBoard += play;
        myCount += 1;
        
        enemyBoard -= play;
        enemyCount -= 1;

        turnSides(next, dir);
    }

    public void Print()
    {
        for (int i = 0; i < 64; i++)
        {
            if (i % 8 == 0)
                Console.WriteLine();

            bool isWhite = ((whiteInfo >> i) & 1) > 0;
            bool isBlack = ((blackInfo >> i) & 1) > 0;
            
            // Console.Write("| ");

            if (isWhite)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" 0 ");
            }
            else if (isBlack)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" 0 ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("   ");
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(" | ");

            
        }

    }

    public string StringFile()
    {
        int play = WhitePlays ? 1 : 0;
        return $"{play} {myBoard} {myCount} {enemyBoard} {enemyCount}";
    }
}
