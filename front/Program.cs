using System;
using System.Drawing;
using System.Collections.Generic;

using Pamella;

App.Open<OthelloView>();

public struct OthelloGame
{
    private const ulong u = 1;

    private ulong whiteInfo;
    private ulong blackInfo;
    private byte whiteCount;
    private byte blackCount;
    private byte whitePlays;

    public static OthelloGame New()
    {
        return new OthelloGame
        {
            whiteInfo = (u << 27) + (u << 36),
            blackInfo = (u << 28) + (u << 35),
            whiteCount = 2,
            blackCount = 2,
            whitePlays = 1
        };
    }

    public static OthelloGame New(
        ulong white, ulong black,
        byte wCount, byte bCount
    )
    {
        return new OthelloGame
        {
            whiteInfo = white,
            blackInfo = black,
            whiteCount = wCount,
            blackCount = bCount
        };
    }

    public int this[int i, int j]
    {
        get
        {
            int index = i + j * 8;
            return ((whiteInfo & (u << index)) > 0) ? 1 : 
                   ((blackInfo & (u << index)) > 0) ? 2 : 0;
        }
    }

    public override string ToString()
        => $"{whitePlays} {whiteInfo} {whiteCount} {blackInfo} {blackCount}";
}

public class OthelloView : View
{
    OthelloGame game = OthelloGame.New();

    protected override void OnStart(IGraphics g)
    {
        g.SubscribeKeyDownEvent(key => {
            if (key == Input.Escape)
                App.Close();
        });
    }

    protected override void OnFrame(IGraphics g)
    {
        
    }

    protected override void OnRender(IGraphics g)
    {
        const int boardMargin = 20;
        const int boardPadding = 10;
        const int squareMargin = 5;
        const int squarePadding = 5;

        int min = int.Min(g.Width, g.Height);
        int boardSize = min - 2 * boardMargin;
        int squareSize = (boardSize - 7 * squareMargin - 2 * boardPadding) / 8;
        int radius = (squareSize - 2 * squarePadding) / 2;

        float x0 = (g.Width - boardSize) / 2 + boardPadding;
        float x = x0;
        float y = (g.Height - boardSize) / 2 + boardPadding;

        g.Clear(Color.DarkRed);
        g.DrawText(
            new RectangleF(0, 0, 150, 150), 
            StringAlignment.Center, StringAlignment.Center,
            Brushes.White, "debugInfo:\n" + game.ToString());
        g.FillRectangle(
            x - boardPadding, y - boardPadding, 
            boardSize, boardSize,
            Brushes.DarkGreen
        );

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var piece = game[i, j];
                g.FillRectangle(x, y, 
                    squareSize, squareSize,
                    Brushes.Green
                );

                if (piece != 0)
                {
                    List<PointF> pts = new List<PointF>();

                    for (float theta = 0; theta < MathF.Tau; theta += 0.2f)
                    {
                        pts.Add(new PointF(
                            x + squareSize / 2 + radius * MathF.Cos(theta),
                            y + squareSize / 2 + radius * MathF.Sin(theta) 
                        ));
                    }

                    g.FillPolygon(pts.ToArray(),
                        piece == 1 ? Brushes.White : Brushes.Black
                    );
                }
                
                x += squareSize + squareMargin;
            }
            x = x0;
            y += squareSize + squareMargin;
        }
    }
}