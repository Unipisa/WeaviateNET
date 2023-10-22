using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query
{
    public class FieldList<P> : QueryBlock where P : class, new() 
    {
        private static List<string> classFields = Schema.PersistentFieldNames<P>().ToList();
        private List<string> _fields;
        private AdditionalFields _additional;

        public AdditionalFields Additional { get { return _additional; }  }

        internal FieldList(bool selectall=true)
        {
            if (selectall)
                _fields = Schema.PersistentFieldNames<P>().ToList();
            else
                _fields = new List<string>();

            _additional = new AdditionalFields();
        }

        public int AddFields(params string[] names)
        {
            int count = 0;
            foreach (var name in names)
            {
                if (AddField(name))
                    count++;
            }

            return count;
        }

        public bool AddField(string name)
        {
            if (_fields.Contains(name))
                return false;
            var f = classFields.Where(n => n == name).FirstOrDefault();
            if (f != null)
            {
                _fields.Add(f);
                return true;
            }

            throw new Exception($"Invalid field name: {name}");
        }

        public bool RemoveField(string name)
        {
            if (_fields.Contains(name))
            {
                var f = _fields.Where(n => n == name).First();
                _fields.Remove(f);
                return true;
            }

            if (!classFields.Contains(name))
                throw new Exception($"Invalid field name: {name}");

            return false;
        }

        public void SelectAll()
        {
            _fields = Schema.PersistentFieldNames<P>().ToList();
        }

        public void ClearFields()
        {
            _fields.Clear();
        }

        protected override void Render()
        {
            if (_fields.Count == 0)
                throw new Exception("No fields selected");
            foreach (var f in _fields)
            {
                var t = typeof(P).GetField(f)!.FieldType;
                if (t == typeof(GeoCoordinates))
                {
                    AppendLineStartBlock($"{f} {{");
                    AppendLine("latitude");
                    AppendLine("longitude");
                    AppendLineEndBlock("}");
                } else
                {
                    AppendLine(f);
                }
            }
            _additional.Render(Output!, IndentationLevel);
        }
    }
}
