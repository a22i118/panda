using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static reversi.Reversi;

namespace server
{
    internal class GameManager
    {
        const int players = 4;
        int turn_ = 0;
        Yama yama = new Yama();
        Tehai[] tehais = new Tehai[players];
        Kawa[] kawas = new Kawa[players];
        List<Hai> list_ = new List<Hai>();
        
        public List<Hai> List { get { return list_; } }
        public enum eMode
        {
            Tsumo,
            wait,
        }
        eMode mode_;

        public GameManager()
        { 
            Init();
           
        }

        public void Init()
        {
            for (int i = 0; i < players; i++)
            {
                tehais[i] = new Tehai();

            }

            //四人に配る
            for (int i = 0; i < players; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    tehais[i].Add(yama.List[0]);
                    yama.List.RemoveAt(0);
                }
            }

            //手牌のソート
            for (int i = 0; i < players; i++)
            {
                tehais[i].Sort();
            }
        }
        public void Exec()
        {
            if(mode_ == eMode.Tsumo)
            {
                tehais[turn_].Tsumo(yama);
                mode_ = eMode.wait;
            }
            else
            {
                if(tehais[turn_].List.Count < 14){
                    turn_=(turn_+1) % 4;
                    mode_ = eMode.Tsumo;
                }
                //tehais[turn_].Click(x, y);
            }




            //foreach (var tehai in tehais)
            //{
            //    tehai.Tsumo(yama);
            //}


        }
        public void ClickCheck(int x,int y)
        {
#if false
            for (int i = 0; i < players; i++)
            {
                //tehais[i].click(x, y, kawas[i], i);
                tehais[i].Click(x, y);
                tehais[i].Sort();
            }

#else
            if (tehais[turn_].List.Count >= 14)
            {
                tehais[turn_].Click(x, y);
                tehais[turn_].Sort();
            }

#endif
        }


        
        public void Draw(Graphics g)
        {
            for (int i = 0;i < players;i++)
            {
                tehais[i].Draw(g, i);
                //if (kawas[i] != null)
                //{
                //    kawas[i].Draw(g, i);
                //}
            }
        }
    }
}
