using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GRootPlugins.FlagChecker
{
    public static class FlagDataFactory
    {
        public static FlagData Create(int inSize, bool inUseArray = false)
        {
            if (inSize > 64 || inUseArray)
            {
                return new FlagDataArray(inSize);
            }
            else if (inSize > 32)
            {
                return new FlagData64(inSize);
            }
            else if (inSize > 16)
            {
                return new FlagData32(inSize);
            }
            else if (inSize > 8)
            {
                return new FlagData16(inSize);
            }

            return new FlagData8(inSize);
        }
    }
    
    // Consider using BitArray
    public abstract class FlagData
    {
        public Guid UniqueId { private set; get; }
        public int FlagSize { private set; get; } = 0;
        
        public FlagData(int inSize)
        {
            UniqueId = Guid.NewGuid();
            FlagSize = inSize;
        }

        public abstract void SetFlag(int inIndex, bool inValue);

        public abstract bool GetFlag(int inIndex);

        public bool All(bool inValue)
        {
            for (int i = 0; i < FlagSize; i++)
            {
                if (GetFlag(i) != inValue) return false;
            }

            return true;
        }

        public bool Any(bool inValue)
        {
            for (int i = 0; i < FlagSize; i++)
            {
                if (GetFlag(i) == inValue) return true;
            }

            return false; 
        }
    }

    public sealed class FlagDataArray : FlagData
    {
        private BitArray _flags;

        public FlagDataArray(int inSize) : base(inSize)
        {
            _flags = new BitArray(inSize, false);
        }

        public override void SetFlag(int inIndex, bool inValue)
        {
            _flags[inIndex] = inValue;
        }

        public override bool GetFlag(int inIndex)
        {
            return _flags[inIndex];
        }

        public new bool All(bool inValue)
        {
            foreach (bool val in _flags)
            {
                if (val != inValue) return false;
            }

            return true;
        }

        public new bool Any(bool inValue)
        {
            foreach (bool val in _flags)
            {
                if (val == inValue) return true;
            }

            return false;
        }
    }
    
    public sealed class FlagData8 : FlagData
    {
        private byte _flags;

        public Type DataType => _flags.GetType();
        public FlagData8(int inSize) : base(inSize)
        {
        }

        public override void SetFlag(int inIndex, bool inValue)
        {
            if (inIndex >= FlagSize) throw new Exception("Invalid Index");
            
            byte mask = (byte)(1 << inIndex);
            if (inValue) _flags |= mask;
            else _flags &= (byte)(~mask);
        }

        public override bool GetFlag(int inIndex)
        {
            if (inIndex >= FlagSize) throw new Exception("Invalid Index");

            byte mask = (byte) (1 << inIndex);
            return (_flags & mask) != 0;
        }
    }

    public sealed class FlagData16 : FlagData
    {
        private ushort _flags;
        public FlagData16(int inSize) : base(inSize)
        {
        }
        
        public override void SetFlag(int inIndex, bool inValue)
        {
            if (inIndex >= FlagSize) throw new Exception("Invalid Index");
            
            ushort mask = (ushort)(1 << inIndex);
            if (inValue) _flags |= mask;
            else _flags &= (ushort)(~mask);
        }

        public override bool GetFlag(int inIndex)
        {
            if (inIndex >= FlagSize) throw new Exception("Invalid Index");

            ushort mask = (ushort) (1 << inIndex);
            return (_flags & mask) != 0;
        }
    }

    public sealed class FlagData32 : FlagData
    {
        private uint _flags;
        public FlagData32(int inSize) : base(inSize)
        {
        }
        
        public override void SetFlag(int inIndex, bool inValue)
        {
            if (inIndex >= FlagSize) throw new Exception("Invalid Index");
            
            uint mask = (uint)(1 << inIndex);
            if (inValue) _flags |= mask;
            else _flags &= (uint)(~mask);
        }

        public override bool GetFlag(int inIndex)
        {
            if (inIndex >= FlagSize) throw new Exception("Invalid Index");

            uint mask = (uint) (1 << inIndex);
            return (_flags & mask) != 0;
        }
    }

    public sealed class FlagData64 : FlagData
    {
        private ulong _flags;
        public FlagData64(int inSize) : base(inSize)
        {
        }
        
        public override void SetFlag(int inIndex, bool inValue)
        {
            if (inIndex >= FlagSize) throw new Exception("Invalid Index");
            
            ulong mask = (ulong)(1 << inIndex);
            if (inValue) _flags |= mask;
            else _flags &= (ulong)(~mask);
        }

        public override bool GetFlag(int inIndex)
        {
            if (inIndex >= FlagSize) throw new Exception("Invalid Index");

            ulong mask = (ulong) (1 << inIndex);
            return (_flags & mask) != 0;
        }
    }
}

