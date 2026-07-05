namespace GoPowered.Lang.Parser.Type.Go
{
    public record PrimitiveType : IType
    {
        public Primitive Primitive { get; }

        public static readonly PrimitiveType STRING = new(Primitive.STRING);
        public static readonly PrimitiveType RUNE = new(Primitive.RUNE);
        public static readonly PrimitiveType BOOL = new(Primitive.BOOL);
        public static readonly PrimitiveType INT = new(Primitive.INT);
        public static readonly PrimitiveType INT64 = new(Primitive.INT64);
        public static readonly PrimitiveType INT32 = new(Primitive.INT32);
        public static readonly PrimitiveType INT16 = new(Primitive.INT16);
        public static readonly PrimitiveType INT8 = new(Primitive.INT8);
        public static readonly PrimitiveType UINT = new(Primitive.UINT);
        public static readonly PrimitiveType UINT64 = new(Primitive.UINT64);
        public static readonly PrimitiveType UINT32 = new(Primitive.UINT32);
        public static readonly PrimitiveType UINT16 = new(Primitive.UINT16);
        public static readonly PrimitiveType UINT8 = new(Primitive.UINT8);
        public static readonly PrimitiveType FLOAT = new(Primitive.FLOAT);
        public static readonly PrimitiveType FLOAT64 = new(Primitive.FLOAT64);
        public static readonly PrimitiveType FLOAT32 = new(Primitive.FLOAT32);
        public static readonly PrimitiveType ERROR = new(Primitive.ERROR);

        internal PrimitiveType(Primitive primitive)
        {
            this.Primitive = primitive;
        }
    }

    public enum Primitive
    {
        STRING,
        RUNE,
        BOOL,
        INT,
        INT64,
        INT32,
        INT16,
        INT8,
        UINT,
        UINT64,
        UINT32,
        UINT16,
        UINT8,
        FLOAT,
        FLOAT64,
        FLOAT32,
        ERROR,
    }
}
