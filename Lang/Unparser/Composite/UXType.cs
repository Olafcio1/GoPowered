using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Type;
using GoPowered.Lang.Parser.Type.Go;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleType(IType type)
        {
            if (type is AnyType)
                return "any";
            else if (type is ArrayType)
                return "---";
            else if (type is ChannelType chan)
                return "chan " + HandleType(chan.Type);
            else if (type is FunctionType func)
                return "func" + (func.Generics == null
                                    ? null
                                    : HandleFuncGenerics(func.Generics))
                              + HandleFuncArguments(func.Args)
                              + (func.Returns == null
                                    ? null
                                    : HandleFuncReturns(func.Returns));
            else if (type is InterfaceType @interface)
                return "interface{" + HandleInherits(@interface.Inherits)
                                    + HandleMethods(@interface.Methods)
                                    + "\n}";
            else if (type is ListType list)
                return "[]" + HandleType(list.ElementType);
            else if (type is MapType map)
                return "map[" + HandleType(map.Key) + "]" + HandleType(map.Value);
            else if (type is PointerType ptr)
                return "*" + HandleType(ptr.Type);
            else if (type is PrimitiveType prim)
                return prim.Primitive.ToString().ToLower();
            else if (type is StructType @struct)
                return "struct{" + HandleInherits(@struct.Inherits)
                                 + HandleFields(@struct.Fields) + "\n}";
            else if (type is UniqueType uniq)
                return string.Join('.', uniq.Location) + (uniq.Generics == null
                                                            ? ""
                                                            : HandleTypeGenerics(uniq.Generics));
            else throw new UnparserError("Unrecognized type '" + type + "'");
        }

        protected string HandleTypeGenerics(List<IType> generics)
        {
            var value = "[";
            var first = true;

            foreach (var type in generics)
            {
                if (first)
                    first = false;
                else value += ", ";

                value += HandleType(type);
            }

            value += "]";

            return value;
        }
    }
}
