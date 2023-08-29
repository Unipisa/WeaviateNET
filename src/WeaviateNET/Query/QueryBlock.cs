using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query
{
    public abstract class QueryBlock
    {
        public static int IndentationSpaces { get; set; } = 4;
        internal int IndentationLevel { get; set; } = 0;

        internal StringBuilder? Output { get; set; }

        protected QueryBlock()
        {
        }

        protected void AppendLine(string line, bool indent=true)
        {
            if (indent)
            {
                Output!.Append(new string(' ', IndentationLevel * IndentationSpaces));
            }
            Output!.AppendLine(line);
        }

        protected void AppendText(string text, bool indent)
        {
            if (indent)
            {
                Output!.Append(new string(' ', IndentationLevel * IndentationSpaces));
            }
            Output!.Append(text);
        }

        protected void AppendLineStartBlock(string line, bool indent=true)
        {
            AppendLine(line, indent);
            IndentationLevel++;
        }

        protected void AppendLineEndBlock(string line, bool indent = true)
        {
            IndentationLevel--;
            AppendLine(line, indent);
        }

        protected void AppendTextEndBlock(string line, bool indent=true)
        {
            IndentationLevel--;
            AppendText(line, indent);
        }

        protected void AppendLineBetweenBlocks(string line)
        {
            IndentationLevel--;
            AppendLine(line);
            IndentationLevel++;
        }

        internal void Render(StringBuilder output, int indentationLevel)
        {
            Output = output;
            IndentationLevel = indentationLevel;
            Render();
        }

        internal string[]? CheckPropertyNames<P>(string[] properties, bool canBoost=false) where P : class, new()
        {
            var flds = Schema.PersistentFields<P>().Select(p => p.Name);
            var q = properties.Where(p => {
                var txt = canBoost ? p.Split('^')[0] : p;
                return flds.Contains(txt);
            });
            if (!q.Any())
            {
               return q.ToArray();
            }

            return null;
        }

        abstract protected void Render();

        public override string ToString()
        {
            Output!.Clear();
            Render();
            return Output.ToString();
        }
    }
}
