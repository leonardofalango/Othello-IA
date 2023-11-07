// using System.Data.Common;

// string file = args.Length < 1 ? "m1" : args[0];
// int dept = args.Length < 3 ? 6 : int.Parse(args[1]);

// ulong u = 1;
// Othello o = new Othello();

// o.whiteInfo = (u << 36) + (u << 27);
// o.blackInfo = (u << 35) + (u << 28);

// o.whiteCount = 2;
// o.blackCount = 2;

// Node tree = new Node
// {
//     State = o,
//     YouPlays = file == "m1"
// };

// tree.Expand(dept);

// if (tree.YouPlays)
// {
//     tree.MiniMax();
//     tree = tree.PlayBest();
//     tree.Expand(dept);
//     var last = tree.State.StringFile();
    
//     File.WriteAllText($"{file}.txt", last);
// }

// while (true)
// {
//     Thread.Sleep(800);

//     if (!File.Exists($"[OUTPUT]{file}.txt"))
//         continue;
    
//     var text = File.ReadAllText($"[OUTPUT]{file}.txt");
//     File.Delete($"[OUTPUT]{file}.txt");

//     var data = text.Split(" ");

    
//     ulong position = ulong.Parse(data[1]);

//     tree = tree.Play(position);
//     tree.Expand(dept);

//     tree.MiniMax();
//     tree = tree.PlayBest();
//     tree.Expand(dept);

//     var stringFile = tree.State.StringFile();

//     // TODO: Write File
//     File.WriteAllText($"{file}.txt", stringFile);

// }



ulong u = 1;
Othello o = new Othello();

o.whiteInfo = (u << 36) + (u << 27);
o.whiteCount = 2;

o.blackInfo = (u << 35) + (u << 28);
o.blackCount = 2;


System.Console.WriteLine("Actual State:");
o.Play(19);

// System.Console.WriteLine("\nNext: ");
// foreach (var item in o.Next())
// {
//     item.Print();
//     System.Console.WriteLine();
// }

o.Play(20);
o.Print();