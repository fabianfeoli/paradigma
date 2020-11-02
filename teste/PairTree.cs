using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace teste
{
    public class PairTree
    {
        #region singleParentChildren
        /// <summary>
        /// Classe protegida para manipular a árvore de componentes
        /// </summary>
        protected class singleParentChildren
        {

            private char Parent { get; set; }
            private bool IsMaster { get; set; } = false;
            private List<singleParentChildren> Children { get; set; }

            #region Construtores
            public singleParentChildren(KeyValuePair<char, char> novo, bool isMaster = false)
            {
                Parent = novo.Key;
                IsMaster = isMaster;
                Children = new List<singleParentChildren>();
                Children.Add(new singleParentChildren(novo.Value));
            }
            public singleParentChildren(Char novo)
            {
                Parent = novo;
                Children = new List<singleParentChildren>();
            }
            #endregion

            #region SetChild
            /// <summary>
            /// Método para definir um child (recursivo)
            /// </summary>
            /// <param name="parent"></param>
            /// <param name="child"></param>
            /// <returns></returns>
            public bool SetChild(char parent, char child)
            {
                if (parent == Parent)
                {
                    if (Children.Count == 2)
                    {
                        throw new Exception("E1 Mais de 2 filhos");
                    }
                    Children.Add(new singleParentChildren(child));
                    return true;
                }
                else
                {
                    bool found = false;
                    foreach (var c in Children)
                    {
                        found = c.SetChild(parent, child);
                        if (found) break;
                    }
                    if (IsMaster && !found)
                    {
                        throw new Exception("E4 Pai não encontrado");
                    }
                    return found;
                }
            }
            #endregion

            #region Print
            /// <summary>
            /// Método para imprimir a árvore
            /// </summary>
            /// <returns></returns>
            public string Print()
            {
                string ret = Parent.ToString() + "[";
                if (IsMaster) ret += "\n";
                foreach (var c in Children)
                {
                    string n = "";
                    ret += "\t" + c.printRec(n);
                    if (IsMaster) ret += "\n";

                }
                ret = ret + "]";

                return ret;
            }
            #endregion

            #region printRec
            /// <summary>
            /// Método auxiliar para imprimir a árvore (recursivo)
            /// </summary>
            /// <param name="ret"></param>
            /// <returns></returns>
            private string printRec(string ret)
            {
                ret += "[" + Parent.ToString();
                foreach (var c in Children)
                {
                    ret = c.printRec(ret);

                }
                return ret + "]";
            }
            #endregion
        }
        #endregion

        singleParentChildren _tree;

        public PairTree(string pares)
        {
            List<KeyValuePair<char, char>> list = new List<KeyValuePair<char, char>>();
            list = toListOfKeyValuePair(pares);
            load(list);

        }

        #region load
        /// <summary>
        /// Carga da árvore
        /// </summary>
        /// <param name="list"></param>
        private void load(List<KeyValuePair<char, char>> list)
        {
            KeyValuePair<char, char> p = findRoot(list);
            _tree = new singleParentChildren(p, true);
            list.Remove(p);
            while (true)
            {
                var sub = list.Where(x => x.Key == p.Key).ToList();
                foreach (var x in sub)
                {
                    _tree.SetChild(x.Key, x.Value);
                    list.Remove(x);
                }
                if (list.Count == 0) break;
                p = list[0];
            }
        }
        #endregion

        #region findRoot
        /// <summary>
        /// Localiza a raiz da árvore
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        KeyValuePair<char, char> findRoot(List<KeyValuePair<char, char>> list)
        {
            KeyValuePair<char, char> chave = new KeyValuePair<char, char>((char)0, (char)0);
            
            foreach (var c in list)
            {
                if (chave.Key == c.Key) continue;
                int counter = list.Where(x => x.Key == c.Key).Count();
                int counter2 = list.Where(x => x.Value == c.Key).Count();
                if (counter == 2 && counter2 == 0)
                {
                    if(chave.Key != 0 )
                    {
                        throw new Exception("E3 Raízes múltiplas");
                    }
                    chave = c;
                }
                if (counter > 2 || counter2 > 2)
                {
                    throw new Exception("E2 Ciclo presente");
                }
            }
            return chave;

        }
        #endregion

        #region toListOfKeyValuePair
        /// <summary>
        /// Converte uma string no formato [B,D] [D,E] [Z,B] [C,F] [E,G] [Z,C]
        /// em uma lista List<KeyValuePair<char, char>>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        List<KeyValuePair<char, char>> toListOfKeyValuePair(string s)
        {
            char[] sep = { ',' };
            s = s.Replace("[", "").Replace("]", ",");
            List<KeyValuePair<char, char>> list = new List<KeyValuePair<char, char>>();
            string[] pares = s.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            if (pares.Length % 2 != 0)
            {
                throw new Exception("Número inválido de argumentos");
            }
            for (int i = 0; i < pares.Length; i += 2)
            {
                list.Add(new KeyValuePair<char, char>(pares[i].Trim()[0], pares[i + 1].Trim()[0]));
            }
            return list;
        }
        #endregion

        #region Print
        /// <summary>
        /// Imprime a árvore
        /// </summary>
        /// <returns></returns>
        public string Print()
        {
            return _tree.Print();
        }
        #endregion

    }
}
