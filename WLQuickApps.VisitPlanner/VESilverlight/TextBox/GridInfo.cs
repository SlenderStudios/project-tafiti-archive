//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace VESilverlight
{
    internal class GridInfo
    {
        public GridInfo(string spec)
        {
            string[] specs = spec.Split(',');

            _specs = new CellInfo[specs.Length];

            for (int index = 0; index < specs.Length; ++index)
            {
                _specs[index] = new CellInfo(specs[index]);
                if (_specs[index].Type == CellType.Fraction)
                {
                    FractionalSum += _specs[index].Fraction;
                    IsDynamic = true;
                }
                else if (_specs[index].Type == CellType.Auto)
                    IsDynamic = true;
            }
        }

        public CellInfo[] Cells
        {
            get { return _specs; }
        }

        public double SpecifiedSum
        {
            get { return _specifiedSum; }
            set { _specifiedSum = value; }
        }

        public double FractionalSum
        {
            get { return _fractionalSum; }
            set { _fractionalSum = value; }
        }

        public double Total
        {
            get { return _total; }
        }

        public bool IsDynamic
        {
            get { return _isDynamic; }
            set { _isDynamic = value; }
        }

        public void Reset()
        {
            for (int index = 0; index < Cells.Length; ++index)
            {
                if (Cells[index].Type != CellType.Specified)
                    Cells[index].Value = 0;
            }
        }

        public override string ToString()
        {
            string[] result = new string[Cells.Length];

            for (int index = 0; index < Cells.Length; ++index)
            {
                result[index] = Cells[index].ToString();
            }

            return string.Join(",", result);
        }

        public void CalculateOffsets(double available)
        {
            // Get total specified dimension
            SpecifiedSum = _total = 0;
            for (int index = 0; index < Cells.Length; ++index)
            {
                if (Cells[index].Type != CellType.Fraction)
                    SpecifiedSum += Cells[index].Value;
            }

            // Fix up fractions and offsets
            double remaining = available - SpecifiedSum;
            for (int index = 0; index < Cells.Length; ++index)
            {
                if (Cells[index].Type == CellType.Fraction)
                {
                    Cells[index].Value = (remaining > 0) ?
                        (Cells[index].Fraction / FractionalSum) * remaining : 0;
                }

                if (index > 0)
                {
                    Cells[index].Offset = Cells[index - 1].Offset + Cells[index - 1].Value;
                }

                _total += Cells[index].Value;
            }
        }

        CellInfo[] _specs;
        double _specifiedSum;
        double _fractionalSum;
        double _total;
        bool _isDynamic;    
    }

    internal enum CellType
    {
        Auto,
        Specified,
        Fraction
    }

    internal class CellInfo
    {
        public CellInfo(string spec)
        {
            if (spec.Equals("auto", StringComparison.OrdinalIgnoreCase))
            {
                Type = CellType.Auto;
            }
            else if (spec.EndsWith("*"))
            {
                Type = CellType.Fraction;
                Fraction = (spec.Length > 1) ? Value = double.Parse(spec.Substring(0, spec.Length - 1)) : 1;
            }
            else
            {
                Type = CellType.Specified;
                Value = double.Parse(spec);
            }
        }

        public CellType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public double Fraction
        {
            get { return _fraction; }
            set { _fraction = value; }
        }

        public double Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case CellType.Auto:
                    return "auto";
                case CellType.Fraction:
                    if (Fraction == 1)
                        return "*";
                    else
                        return Fraction.ToString() + "*";
                case CellType.Specified:
                    return Value.ToString();
            }

            return "?";
        }

        CellType _type;
        private double _value, _offset, _fraction;
    }
}
