using System.Globalization;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;

public class Node
{
    public Othello State { get; set; }
    public float Evaluation { get; set; } = 0;
    public List<Node> Children { get; set; } = new();
    public bool Expanded { get; set; } = false;
    public bool YouPlays { get; set; } = true;

    public Node Play(ulong position)
    {
        // Seu código aqui...
        foreach(Node child in this.Children)
        {
            var last = child.State.GetLast(); 
            if (last == position)
                return child;
        }
        
        return this;
    }

    public void Expand(int deep)
    {
        if (deep == 0)
            return;

        if (!Expanded)
        {
            foreach (var item in State.Next())
            {
                Node node = new Node(){
                    State = item,
                    YouPlays = !YouPlays
                };
                
                Children.Add(node);
            }
        }

        Expanded = true;

        foreach (var child in Children)
        {
            child.Expand(deep - 1);
        }   
    }

    public Node PlayBest()
    {
        // Seu código aqui...
        float max = float.NegativeInfinity;
        Node bestNode = this.Children[0];

        foreach (Node child in this.Children)
            if (max < child.Evaluation)
                bestNode = child;
        
        bestNode.State.Print();
        return bestNode;
    }

    public float MiniMax()
    {
        if (this.isTerminalNode())
        {
            this.Evaluation = eval();
            return this.Evaluation;
        }
        
        bool maximize = this.YouPlays;

        float value;
        if (maximize)
        {
            value = float.NegativeInfinity;
            
            foreach (Node child in this.Children)
                value = Math.Max(value, child.MiniMax());

            this.Evaluation = value;
            return value;
        }
        else
        {
            value = float.PositiveInfinity;
            
            foreach (Node child in this.Children)
                value = Math.Min(value, child.MiniMax());

            this.Evaluation = value;
            return value;
        }
    }

    private float eval()
    {

        float heu = 0;
        heu += this.State.myCount * 0.5f;
        heu -= this.State.enemyCount * 0.5f;

        heu += this.State.enemyCount * 0.7f * this.Children.Count;

        return heu;
    }

    private bool isTerminalNode() 
        => Children.Count == 0;
}