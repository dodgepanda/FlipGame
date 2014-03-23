using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlipGame
{
    class FlipBoardSolver
    {
        Queue<FlipBoard> ToDo;
        List<FlipBoard> AlreadyDone;
        FlipBoard Winner = null;
        int solutionColor;
        public FlipBoardSolver(FlipBoard root, int color = 0)
        {
            solutionColor = color;
            ToDo = new Queue<FlipBoard>();
            AlreadyDone = new List<FlipBoard>();
            FlipBoard next = null;
            ToDo.Enqueue(root);
            while (ToDo.Count() != 0)
            {
                next = ToDo.Dequeue();
                if (next.isGameOver(solutionColor))
                {
                    Winner = next;
                    return;
                }
                if(!seenBefoore(next))
                    AlreadyDone.Add(next);
                //addSuccessors(next)
                //for each position
                //try each flip
                //check if winner
                //check for dupes
                List<FlipBoard> fbs = next.generateChildren();
                foreach (FlipBoard i in fbs)
                {
                    if (i.isGameOver(solutionColor))
                    {
                        Winner = i;
                        return;
                    }
                    //AlreadyDone.Add(i);
                    //foreach (FlipBoard j in AlreadyDone)
                    //{
                    //    if (!i.isEqual(j))
                    //    {
                    //        ToDo.Enqueue(i);
                    //    }
                    //}
                    if (!seenBefoore(i))
                    {
                        ToDo.Enqueue(i);
                        AlreadyDone.Add(i);
                    }
                }
                //Console.WriteLine("ToDo.Count(): " + ToDo.Count());
                //Console.WriteLine("AlreadyDone.Count(): " + AlreadyDone.Count());
                //addSuccessors(next);
            }
        }
        public bool seenBefoore(FlipBoard fb)
        {
            foreach(FlipBoard ad in AlreadyDone)
                if(fb.isEqual(ad))
                    return true;
            return false;
        }
        public void addSuccessors(FlipBoard next)
        {
            List<FlipBoard> children = next.generateChildren();
            foreach (FlipBoard child in children)
            {
                if (child.isGameOver(solutionColor))
                {
                    ToDo.Enqueue(child);
                    return;
                }
                foreach (FlipBoard previous in AlreadyDone)
                    if (!child.isEqual(previous))
                        ToDo.Enqueue(child);
            }

        }
        public FlipBoard getWinner() { return Winner; }
        public Stack<FlipBoard> getSolution()
        {
            Stack<FlipBoard> temp = new Stack<FlipBoard>();
            if(Winner==null)    return temp;
            temp.Push(Winner);
            while(temp.Peek().getParent()!=null)
                temp.Push(temp.Peek().getParent());
            return temp;
        }
    }
}
